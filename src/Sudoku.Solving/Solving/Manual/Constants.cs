﻿namespace Sudoku.Solving.Manual
{
	/// <summary>
	/// Provides the constants for the operation handling on solving puzzles.
	/// </summary>
	internal static class Constants
	{
		/// <summary>
		/// Indicates the total number of Borescoper's Deadly Pattern possible templates with the size 3.
		/// </summary>
		public const int BdpTemplatesSize3Count = 14580;

		/// <summary>
		/// Indicates the total number of Borescoper's Deadly Pattern possible templates with the size 4.
		/// </summary>
		public const int BdpTemplatesSize4Count = 11664;

		/// <summary>
		/// Indicates the total number of Qiu's Deadly Pattern possible templates.
		/// </summary>
		public const int QdpTemplatesCount = 972;

		/// <summary>
		/// Indicates the total number of Unique Square possible templates.
		/// </summary>
		public const int UsTemplatesCount = 162;

		/// <summary>
		/// Indicates the total number of Exocet templates.
		/// </summary>
		public const int ExocetTemplatesCount = 1458;


		/// <summary>
		/// Indicates the mask that means all rows.
		/// </summary>
		public const int AllRowsMask = 0b111_111_111__000_000_000;

		/// <summary>
		/// Indicates the mask that means all columns.
		/// </summary>
		public const int AllColumnsMask = 0b111_111_111__000_000_000__000_000_000;

		/// <summary>
		/// Indicates the mask that means all regions.
		/// </summary>
		public const int AllRegionsMask = 0b111_111_111__111_111_111__111_111_111;
	}
}
