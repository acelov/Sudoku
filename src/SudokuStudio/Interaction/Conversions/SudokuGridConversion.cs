namespace SudokuStudio.Interaction.Conversions;

/// <summary>
/// Provides with conversion methods used by XAML designer, about puzzle details displayed.
/// </summary>
internal static class SudokuGridConversion
{
	/// <summary>
	/// Defines a solver.
	/// </summary>
	private static readonly ThreadLocal<BitwiseSolver> Solver = new(static () => new());


	public static bool GetFixedButtonAvailability(Grid grid) => grid.ModifiablesCount != 0;

	public static bool GetUnfixedButtonAvailability(Grid grid) => grid.GivensCount != 0;

	public static string GetPuzzleHintsCount(Grid grid)
		=> grid switch
		{
			{ IsUndefined: true } => GetString("AnalyzePage_UndefinedGrid"),
			{ IsEmpty: true } => GetString("AnalyzePage_EmptyGrid"),
			{ GivensCount: var givens } => givens.ToString()
		};

	public static string GetPuzzleCode(Grid grid)
	{
		var character = ((App)Application.Current).Preference.UIPreferences.EmptyCellCharacter;
		return grid switch
		{
			{ IsUndefined: true } => GetString("AnalyzePage_UndefinedGrid"),
			{ IsEmpty: true } => GetString("AnalyzePage_EmptyGrid"),
			_ => grid.ToString($"#{character}")
		};
	}

	public static unsafe string GetPuzzleUniqueness(Grid grid)
	{
		if (grid.IsUndefined)
		{
			return GetString("AnalyzePage_PuzzleHasNoSolution");
		}

		if (grid.IsEmpty)
		{
			return GetString("AnalyzePage_PuzzleHasMultipleSolutions");
		}

		var character = ((App)Application.Current).Preference.UIPreferences.EmptyCellCharacter;
		var hasNoGivenCells = grid.GivensCount == 0;
		var str = hasNoGivenCells ? grid.ToString($"!{character}") : grid.ToString();
		return GetString(
			Solver.Value!.Solve(str, null, 2) switch
			{
				0 => "AnalyzePage_PuzzleHasNoSolution",
				1 => hasNoGivenCells ? "AnalyzePage_PuzzleHasUniqueSolutionButUnfixed" : "AnalyzePage_PuzzleHasUniqueSolution",
				_ => "AnalyzePage_PuzzleHasMultipleSolutions"
			}
		);
	}

	public static string GetIsMinimal(Grid grid)
	{
		if (grid is { IsUndefined: true } or { IsEmpty: true })
		{
			return GetString("AnalyzePage_MinimalResult_NotUniquePuzzle");
		}

		if (!Solver.Value!.CheckValidity(grid.ToString()))
		{
			return GetString("AnalyzePage_MinimalResult_NotUniquePuzzle");
		}

		if (!grid.CheckMinimal(out var firstCandidateMakePuzzleNotMinimal))
		{
			return string.Format(
				GetString("AnalyzePage_MinimalResult_AtLeastOneHintCanBeRemoved"),
				new RxCyConverter().CandidateConverter([firstCandidateMakePuzzleNotMinimal])
			);
		}

		return GetString("AnalyzePage_MinimalResult_Yes");
	}

	public static FontFamily GetFont(FontFamily givenFont, FontFamily modifiable, CellState state)
		=> state == CellState.Modifiable ? modifiable : givenFont;
}
