namespace SudokuStudio.Views.Pages.Analyze;

/// <summary>
/// Defines a step collecting page.
/// </summary>
public sealed partial class StepCollecting : Page, IAnalyzerTab
{
	/// <summary>
	/// Indicates the found steps currently.
	/// </summary>
	internal Step[]? _currentFountSteps;


	/// <summary>
	/// Initializes a <see cref="StepCollecting"/> instance.
	/// </summary>
	public StepCollecting() => InitializeComponent();


	/// <inheritdoc/>
	public AnalyzePage BasePage { get; set; } = null!;

	/// <inheritdoc/>
	AnalyzerResult? IAnalyzerTab.AnalysisResult { get; set; }


	/// <summary>
	/// Converts the specified collection into the target view source collection.
	/// </summary>
	/// <param name="collection">The raw collection.</param>
	/// <param name="grid">The puzzle.</param>
	/// <returns>The collection that can be used as view source.</returns>
	private ObservableCollection<TechniqueGroupBindableSource> GetTechniqueGroups(Step[] collection, Grid grid)
	{
		var displayItems = ((App)Application.Current).Preference.UIPreferences.StepDisplayItems;
		return new(
			from step in collection
			let technique = step.Code
			orderby step.DifficultyLevel, technique.GetGroup(), technique
			group step by step.GetName(App.CurrentCulture) into stepsGroupedByName
			let name = stepsGroupedByName.Key
			let elements =
				from step in stepsGroupedByName
				orderby step.DifficultyLevel, step.Difficulty
				select new SolvingPathStepBindableSource { DisplayItems = displayItems, Step = step, StepGrid = grid }
			select new TechniqueGroupBindableSource(elements) { Key = name }
		);
	}


	private void TechniqueGroupView_StepChosen(TechniqueGroupView sender, TechniqueGroupViewStepChosenEventArgs e)
		=> BasePage.VisualUnit = e.ChosenStep;

	private void TechniqueGroupView_StepApplied(TechniqueGroupView sender, TechniqueGroupViewStepAppliedEventArgs e)
	{
		var appliedPuzzle = BasePage.SudokuPane.Puzzle;
		appliedPuzzle.Apply(e.ChosenStep);

		BasePage.SudokuPane.Puzzle = appliedPuzzle;
	}

	private async void CollectButton_ClickAsync(object sender, RoutedEventArgs e)
	{
		var grid = BasePage.SudokuPane.Puzzle;
		if (!grid.IsValid)
		{
			return;
		}

		CollectButton.IsEnabled = false;
		BasePage.IsGathererLaunched = true;
		TechniqueGroupView.ClearViewSource();

		var textFormat = ResourceDictionary.Get("AnalyzePage_AnalyzerProgress", App.CurrentCulture);
		using var cts = new CancellationTokenSource();
		var uiPref = ((App)Application.Current).Preference.UIPreferences;
		var analysisPref = ((App)Application.Current).Preference.AnalysisPreferences;
		var collector = ((App)Application.Current)
			.Collector
			.WithMaxSteps(analysisPref.StepGathererMaxStepsGathered)
			.WithCulture(App.CurrentCulture)
			.WithSameLevelConfigruation((CollectorDifficultyLevelMode)analysisPref.DifficultyLevelMode)
			.WithStepSearchers(((App)Application.Current).GetStepSearchers())
			.WithRuntimeIdentifierSetters(BasePage.SudokuPane)
			.WithUserDefinedOptions(App.CreateStepSearcherOptions());
		BasePage._ctsForAnalyzingRelatedOperations = cts;

		try
		{
			switch (await Task.Run(() =>
			{
				lock (AnalyzingRelatedSyncRoot)
				{
					return collector.Collect(in grid, new Progress<AnalyzerProgress>(progress => DispatcherQueue.TryEnqueue(() =>
					{
						var (stepSearcherName, percent) = progress;
						BasePage.ProgressPercent = percent * 100;
						BasePage.AnalyzeProgressLabel.Text = string.Format(textFormat, percent);
						BasePage.AnalyzeStepSearcherNameLabel.Text = stepSearcherName;
					})), cts.Token).ToArray();
				}
			}))
			{
				case { } result:
				{
					_currentFountSteps = result;
					TechniqueGroupView.TechniqueGroups.Source = GetTechniqueGroups(result, grid);
					break;
				}
			}
		}
		catch (TaskCanceledException)
		{
		}
		finally
		{
			BasePage._ctsForAnalyzingRelatedOperations = null;
			CollectButton.IsEnabled = true;
			BasePage.IsGathererLaunched = false;
		}
	}
}