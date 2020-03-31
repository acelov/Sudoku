﻿using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Sudoku.Solving.Utils
{
	/// <summary>
	/// Provides extension methods for candidates.
	/// </summary>
	[DebuggerStepThrough]
	public static class CandidateUtils
	{
		/// <summary>
		/// Get a candidate text representation.
		/// </summary>
		/// <param name="candidateOffset">The candidate offset.</param>
		/// <returns>A string text.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string ToString(int candidateOffset)
		{
			int cell = candidateOffset / 9;
			return $"r{cell / 9 + 1}c{cell % 9 + 1}({candidateOffset % 9 + 1})";
		}
	}
}
