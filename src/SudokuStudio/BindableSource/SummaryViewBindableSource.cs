namespace SudokuStudio.BindableSource;

/// <summary>
/// Represents a type that can be used for binding as source, for the table-like grid controls to display techniques used,
/// using technique name to distinct them.
/// </summary>
/// <param name="techniqueName">Indicates the name of the technique.</param>
/// <param name="difficultyLevel">Indicates the difficulty level of the technique.</param>
/// <param name="totalDifficulty">Indicates the total difficulty of the group of steps.</param>
/// <param name="maximumDifficulty">Indicates the maximum difficulty of the group of steps.</param>
/// <param name="countOfSteps">Indicates the number of steps in this group.</param>
/// <seealso cref="AnalysisResult"/>
[method: SetsRequiredMembers]
internal sealed partial class SummaryViewBindableSource(
	[PrimaryConstructorParameter(Accessibility = "public required", SetterExpression = "set")] string techniqueName,
	[PrimaryConstructorParameter(Accessibility = "public required", SetterExpression = "set")] DifficultyLevel difficultyLevel,
	[PrimaryConstructorParameter(Accessibility = "public required", SetterExpression = "set")] decimal totalDifficulty,
	[PrimaryConstructorParameter(Accessibility = "public required", SetterExpression = "set")] decimal maximumDifficulty,
	[PrimaryConstructorParameter(Accessibility = "public required", SetterExpression = "set")] int countOfSteps
)
{
	/// <summary>
	/// Creates the list of <see cref="SummaryViewBindableSource"/> as the result value,
	/// via the specified <paramref name="analysisResult"/> instance of <see cref="AnalysisResult"/> type.
	/// </summary>
	/// <param name="analysisResult">
	/// The <see cref="AnalysisResult"/> instance that is used for creating the result value.
	/// </param>
	/// <returns>The result list of <see cref="SummaryViewBindableSource"/>-typed elements.</returns>
	/// <exception cref="InvalidOperationException">Throws when the puzzle hasn't been solved.</exception>
	public static unsafe ObservableCollection<SummaryViewBindableSource> CreateListFrom(AnalysisResult analysisResult)
	{
		var pref = ((App)Application.Current).Preference.TechniqueInfoPreferences;
		return analysisResult switch
		{
			{ IsSolved: true, InterimSteps: var steps } => [.. g(steps, pref)],
			{ IsPartiallySolved: true, InterimSteps: var steps } => [.. g(steps, pref)],
			_ => throw new InvalidOperationException(SR.ExceptionMessage("GridMustBeSolvedOrNotBad"))
		};


		static decimal r(Step step)
		{
			var pref = ((App)Application.Current).Preference.TechniqueInfoPreferences;
			return pref.GetRating(step.Code) switch { { } v => v, _ => step.Difficulty } / pref.RatingScale;
		}

		static SummaryViewBindableSource[] g(Step[] steps, TechniqueInfoPreferenceGroup pref)
			=>
			from step in steps
			orderby step.DifficultyLevel, step.Code
			group step by step.GetName(App.CurrentCulture) into stepGroup
			let stepGroupArray = (Step[])[.. stepGroup]
			let difficultyLevels =
				from step in stepGroupArray
				let code = step.Code
				group step by pref.GetDifficultyLevelOrDefault(code) into stepGroupedByDifficultyLevel
				select stepGroupedByDifficultyLevel.Key into targetDifficultyLevel
				orderby targetDifficultyLevel
				select targetDifficultyLevel
			select new SummaryViewBindableSource(
				stepGroup.Key,
				difficultyLevels.Aggregate(@delegate.EnumFlagMerger),
				stepGroupArray.SumUnsafe(&r),
				stepGroupArray.MaxUnsafe(&r),
				stepGroupArray.Length
			);
	}
}
