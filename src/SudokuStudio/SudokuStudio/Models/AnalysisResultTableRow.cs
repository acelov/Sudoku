namespace SudokuStudio.Models;

/// <summary>
/// Defines a row of analysis result table.
/// </summary>
internal sealed class AnalysisResultTableRow
{
	/// <summary>
	/// Indicates the total difficulty of all steps.
	/// </summary>
	public required decimal TotalDifficulty { get; set; }

	/// <summary>
	/// Indicates the maximum difficulty in the steps.
	/// </summary>
	public required decimal MaximumDifficulty { get; set; }

	/// <summary>
	/// Indicates the number of steps that uses logic of the current technique.
	/// </summary>
	public required int CountOfSteps { get; set; }

	/// <summary>
	/// Indicates the technique name.
	/// </summary>
	public required string TechniqueName { get; set; }

	/// <summary>
	/// Indicates the difficulty level of the technique belonging to.
	/// </summary>
	public required DifficultyLevel DifficultyLevel { get; set; }


	/// <summary>
	/// Creates the list of <see cref="AnalysisResultTableRow"/> as the result value,
	/// via the specified <paramref name="analyzerResult"/> instance of <see cref="AnalyzerResult"/> type.
	/// </summary>
	/// <param name="analyzerResult">
	/// The <see cref="AnalyzerResult"/> instance that is used for creating the result value.
	/// </param>
	/// <returns>The result list of <see cref="AnalysisResultTableRow"/>-typed elements.</returns>
	/// <exception cref="InvalidOperationException">Throws when the puzzle hasn't been solved.</exception>
	public static IEnumerable<AnalysisResultTableRow> CreateListFrom(AnalyzerResult analyzerResult)
	{
		if (analyzerResult is not { IsSolved: true, Steps: var steps })
		{
			throw new InvalidOperationException("This method requires the puzzle having been solved, and has a unique solution.");
		}

		return
			from step in steps
			orderby step.DifficultyLevel, step.Code
			group step by step.Name into stepGroup
			let stepGroupArray = stepGroup.ToArray()
			let difficultyLevels =
				from step in stepGroupArray
				group step by step.DifficultyLevel into stepGroupedByDifficultyLevel
				select stepGroupedByDifficultyLevel.Key into targetDifficultyLevel
				orderby targetDifficultyLevel
				select targetDifficultyLevel
			select new AnalysisResultTableRow
			{
				TechniqueName = stepGroup.Key,
				CountOfSteps = stepGroupArray.Length,
				DifficultyLevel = difficultyLevels.Aggregate(static (interim, next) => interim | next),
				TotalDifficulty = stepGroupArray.Sum(static step => step.Difficulty),
				MaximumDifficulty = stepGroupArray.Max(static step => step.Difficulty)
			};
	}
}
