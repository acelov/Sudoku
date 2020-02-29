﻿using System.Collections.Generic;
using Sudoku.Data;
using Sudoku.Drawing;
using Sudoku.Solving.Utils;

namespace Sudoku.Solving.Manual.Alses
{
	/// <summary>
	/// Provides a usage of sue de coq (SdC) technique.
	/// </summary>
	public sealed class SueDeCoqTechniqueInfo : AlmostLockedSetTechniqueInfo
	{
		/// <summary>
		/// Initializes an instance with the specified information.
		/// </summary>
		/// <param name="conclusions">All conclusions.</param>
		/// <param name="views">All views.</param>
		/// <param name="als1Cells">ALS 1 cells.</param>
		/// <param name="als1Digits">ALS 1 digits.</param>
		/// <param name="als2Cells">ALS 2 cells.</param>
		/// <param name="als2Digits">ALS 2 digits.</param>
		/// <param name="interCells">Intersection cells.</param>
		/// <param name="interDigits">Intersection digits.</param>
		public SueDeCoqTechniqueInfo(
			IReadOnlyList<Conclusion> conclusions, IReadOnlyList<View> views,
			IReadOnlyList<int> als1Cells, IReadOnlyList<int> als1Digits,
			IReadOnlyList<int> als2Cells, IReadOnlyList<int> als2Digits,
			IReadOnlyList<int> interCells, IReadOnlyList<int> interDigits)
			: base(conclusions, views) =>
			(Als1Cells, Als1Digits, Als2Cells, Als2Digits, IntersectionCells, IntersectionDigits) = (als1Cells, als1Digits, als2Cells, als2Digits, interCells, interDigits);


		/// <summary>
		/// Indicates all cells in ALS 1.
		/// </summary>
		public IReadOnlyList<int> Als1Cells { get; }

		/// <summary>
		/// Indicates all cells in ALS 2.
		/// </summary>
		public IReadOnlyList<int> Als2Cells { get; }

		/// <summary>
		/// Indicates all digits in ALS 1.
		/// </summary>
		public IReadOnlyList<int> Als1Digits { get; }

		/// <summary>
		/// Indicates all digits in ALS 2.
		/// </summary>
		public IReadOnlyList<int> Als2Digits { get; }

		/// <summary>
		/// Indicates all intersection cells.
		/// </summary>
		public IReadOnlyList<int> IntersectionCells { get; }

		/// <summary>
		/// Indicates all intersection digits.
		/// </summary>
		public IReadOnlyList<int> IntersectionDigits { get; }

		/// <inheritdoc/>
		public override string Name => "Sue de Coq";

		/// <inheritdoc/>
		public override decimal Difficulty => IntersectionCells.Count == 2 ? 5.0m : 5.1m;

		/// <inheritdoc/>
		public override DifficultyLevel DifficultyLevel => DifficultyLevel.VeryHard;


		/// <inheritdoc/>
		public override bool Equals(TechniqueInfo other)
		{
			if (!(other is SueDeCoqTechniqueInfo comparer))
			{
				return false;
			}

			var thisMap = GridMap.Empty;
			foreach (int cell in Als1Cells) thisMap[cell] = true;
			foreach (int cell in Als2Cells) thisMap[cell] = true;
			foreach (int cell in IntersectionCells) thisMap[cell] = true;

			var comparerMap = GridMap.Empty;
			foreach (int cell in comparer.Als1Cells) comparerMap[cell] = true;
			foreach (int cell in comparer.Als2Cells) comparerMap[cell] = true;
			foreach (int cell in comparer.IntersectionCells) comparerMap[cell] = true;

			return thisMap == comparerMap;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			string interCells = CellCollection.ToString(IntersectionCells);
			string digits = DigitCollection.ToSimpleString(IntersectionDigits);
			string elimStr = ConclusionCollection.ToString(Conclusions);
			string als1Cells = CellCollection.ToString(Als1Cells);
			string als1Digits = DigitCollection.ToSimpleString(Als1Digits);
			string als2Cells = CellCollection.ToString(Als2Cells);
			string als2Digits = DigitCollection.ToSimpleString(Als2Digits);
			return $"{Name}: {interCells}({digits}) - ({als1Cells}({als1Digits}) & {als2Cells}({als2Digits})) => {elimStr}";
		}
	}
}
