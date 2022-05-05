﻿namespace Sudoku.Solving.Manual.Steps;

partial record class ComplexFishStep
{
	/// <summary>
	/// Indicates the shape modifiers.
	/// </summary>
	[Flags]
	private enum ShapeModifiers
	{
		/// <summary>
		/// Indicates the basic fish.
		/// </summary>
		Basic = 1,

		/// <summary>
		/// Indicates the franken fish.
		/// </summary>
		Franken = 1 << 1,

		/// <summary>
		/// Indicates the mutant fish.
		/// </summary>
		Mutant = 1 << 2
	}
}
