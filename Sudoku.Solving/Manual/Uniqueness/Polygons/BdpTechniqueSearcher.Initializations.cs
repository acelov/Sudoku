﻿using System.Collections.Generic;
using Sudoku.Constants;
using Sudoku.Data;
using static Sudoku.Constants.Processings;
using static Sudoku.Constants.RegionLabel;

namespace Sudoku.Solving.Manual.Uniqueness.Polygons
{
	partial class BdpTechniqueSearcher
	{
		/// <summary>
		/// All combinations in a block.
		/// </summary>
		private static readonly int[][] Quads =
		{
			new[] { 0, 1, 3, 4 }, new[] { 1, 2, 4, 5 }, new[] { 3, 4, 6, 7 }, new[] { 4, 5, 7, 8 },
			new[] { 0, 2, 3, 5 }, new[] { 3, 5, 6, 8 }, new[] { 0, 1, 6, 7 }, new[] { 1, 2, 7, 8 },
			new[] { 0, 2, 6, 8 }
		};


		/// <include file='....\GlobalDocComments.xml' path='comments/staticConstructor'/>
		static BdpTechniqueSearcher()
		{
			int count = 0;
			for (int block = 0; block < 9; block++)
			{
				for (int i = 0; i < 9; i++) // 9 cases.
				{
					int[] quad = Quads[i];
					int[] tempQuad = new int[4];
					for (int j = 0; j < 4; j++)
					{
						// Set all indices to cell offsets.
						tempQuad[j] = (block / 3 * 3 + quad[j] / 3) * 9 + block % 3 * 3 + quad[j] % 3;
					}

					c3(block, i, tempQuad);
					c4(block, i, tempQuad);
				}
			}

			void c3(int block, int i, int[] quad)
			{
				int[][] triplets = new int[4][]
				{
					new[] { quad[0], quad[1], quad[2] }, // (0, 1) and (0, 2) is same region.
					new[] { quad[1], quad[0], quad[3] }, // (0, 1) and (1, 3) is same region.
					new[] { quad[2], quad[0], quad[3] }, // (0, 2) and (2, 3) is same region.
					new[] { quad[3], quad[1], quad[2] }, // (1, 3) and (2, 3) is same region.
				};

				for (int j = 0; j < 4; j++)
				{
					int[] triplet = triplets[j];
					int region1 = new GridMap { triplet[0], triplet[1] }.CoveredLine;
					int region2 = new GridMap { triplet[0], triplet[2] }.CoveredLine;
					int[,] pair1 = new int[6, 2], pair2 = new int[6, 2];
					(int incre1, int incre2) = i switch
					{
						0 => (9, 1),
						1 => (9, 1),
						2 => (9, 1),
						3 => (9, 1),
						4 => (9, 2),
						5 => (9, 2),
						6 => (18, 1),
						7 => (18, 1),
						8 => (18, 2),
						_ => throw Throwings.ImpossibleCase
					};
					if (region1 is >= 9 and < 18)
					{
						// 'region1' is a row and 'region2' is a column.
						r(block, region1, pair1, incre1, j);
						r(block, region2, pair2, incre2, j);
					}
					else
					{
						// 'region1' is a column and 'region2' is a row.
						r(block, region1, pair1, incre2, j);
						r(block, region2, pair2, incre1, j);
					}

					for (int i1 = 0; i1 < 6; i1++)
					{
						for (int i2 = 0; i2 < 6; i2++)
						{
							// Now check extra digits.
							var allCells = new List<int>(triplet)
							{
								pair1[i1, 0], pair1[i1, 1], pair2[i2, 0], pair2[i2, 1]
							};

							long v = 0;
							for (int z = 0; z < allCells.Count; z++)
							{
								v |= (long)allCells[z];
								if (z != allCells.Count - 1)
								{
									v <<= 7;
								}

								if (z == 2)
								{
									v |= 127;
									v <<= 7;
								}
							}

							Patterns[count++] = new Pattern(v);
						}
					}
				}
			}

			void c4(int block, int i, int[] quad)
			{
				int region1 = new GridMap { quad[0], quad[1] }.CoveredLine;
				int region2 = new GridMap { quad[0], quad[2] }.CoveredLine;
				int[,] pair1 = new int[6, 2], pair2 = new int[6, 2];
				(int incre1, int incre2) = i switch
				{
					0 => (9, 1),
					1 => (9, 1),
					2 => (9, 1),
					3 => (9, 1),
					4 => (9, 2),
					5 => (9, 2),
					6 => (18, 1),
					7 => (18, 1),
					8 => (18, 2),
					_ => throw Throwings.ImpossibleCase
				};
				if (region1 is >= 9 and < 18)
				{
					// 'region1' is a row and 'region2' is a column.
					r(block, region1, pair1, incre1, 0);
					r(block, region2, pair2, incre2, 0);
				}
				else
				{
					// 'region1' is a column and 'region2' is a row.
					r(block, region1, pair1, incre2, 0);
					r(block, region2, pair2, incre1, 0);
				}

				for (int i1 = 0; i1 < 6; i1++)
				{
					for (int i2 = 0; i2 < 6; i2++)
					{
						// Now check extra digits.
						var allCells = new List<int>(quad) { pair1[i1, 0], pair1[i1, 1], pair2[i2, 0], pair2[i2, 1] };

						long v = 0;
						for (int z = 0; z < allCells.Count; z++)
						{
							int cell = allCells[z];
							v |= (long)cell;
							if (z != allCells.Count - 1)
							{
								v <<= 7;
							}
						}

						Patterns[count++] = new Pattern(v);
					}
				}
			}

			static void r(int block, int region, int[,] pair, int increment, int index)
			{
				for (int i = 0, cur = 0; i < 9; i++)
				{
					int cell = RegionCells[region][i];
					if (block == GetRegion(cell, Block))
					{
						continue;
					}

					(pair[cur, 0], pair[cur, 1]) = index switch
					{
						0 => (cell, cell + increment),
						1 => region is >= 18 and < 27 ? (cell - increment, cell) : (cell, cell + increment),
						2 => region is >= 9 and < 18 ? (cell - increment, cell) : (cell, cell + increment),
						3 => (cell - increment, cell),
						_ => throw Throwings.ImpossibleCase
					};
					cur++;
				}
			}
		}
	}
}
