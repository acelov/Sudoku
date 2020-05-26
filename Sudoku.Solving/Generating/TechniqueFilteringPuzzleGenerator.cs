﻿using System;
using Sudoku.Data;
using Sudoku.Extensions;
using Sudoku.Solving.Manual;
using static Sudoku.Solving.Manual.TechniqueCode;

namespace Sudoku.Solving.Generating
{
	/// <summary>
	/// Provides a puzzle generator with the technique filter.
	/// </summary>
	public sealed class TechniqueFilteringPuzzleGenerator : HardPatternPuzzleGenerator
	{
		/// <summary>
		/// Indicates the default filter.
		/// </summary>
		private static readonly TechniqueCode[] DefaultFilter =
		{
			LastDigit, FullHouse, HiddenSingle, NakedSingle,
			NakedPair, NakedPairPlus, HiddenPair, LockedPair,
			NakedTriple, NakedTriplePlus, HiddenTriple, LockedTriple,
			NakedQuadruple, NakedQuadruplePlus, HiddenQuadruple,
		};

		/// <summary>
		/// The default manual solver.
		/// </summary>
		private static readonly ManualSolver ManualSolver = new ManualSolver();


		/// <inheritdoc/>
		public override IReadOnlyGrid Generate() => Generate(DefaultFilter);


		/// <summary>
		/// To generate a puzzle that contains the specified technique code.
		/// </summary>
		/// <param name="techniqueCodes">
		/// The technique codes to filter. If the parameter is <see langword="null"/>,
		/// the process will use the default filter.
		/// </param>
		/// <returns>The puzzle.</returns>
		public IReadOnlyGrid Generate(TechniqueCode[]? techniqueCodes)
		{
			techniqueCodes ??= DefaultFilter;
			while (true)
			{
				var puzzle = base.Generate();
				var analysisResult = ManualSolver.Solve(puzzle);
				foreach (var step in analysisResult)
				{
					if (techniqueCodes.Contains(step.TechniqueCode))
					{
						return puzzle;
					}
				}
			}
		}
	}
}
