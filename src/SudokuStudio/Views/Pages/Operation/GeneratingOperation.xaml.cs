namespace SudokuStudio.Views.Pages.Operation;

/// <summary>
/// Indicates the generating operation command bar.
/// </summary>
public sealed partial class GeneratingOperation : Page, IOperationProviderPage
{
	/// <summary>
	/// The fields for core usages on counting puzzles.
	/// </summary>
	private int _generatingCount, _generatingFilteredCount;


	/// <summary>
	/// Initializes a <see cref="GeneratingOperation"/> instance.
	/// </summary>
	public GeneratingOperation() => InitializeComponent();


	/// <inheritdoc/>
	public AnalyzePage BasePage { get; set; } = null!;


	/// <inheritdoc/>
	protected override void OnNavigatedTo(NavigationEventArgs e)
	{
		if (e.Parameter is AnalyzePage p)
		{
			SetConfiguredOptions(p);
		}
	}

	/// <summary>
	/// Update control selection via user's configuration.
	/// </summary>
	/// <param name="basePage">The base page.</param>
	private void SetConfiguredOptions(AnalyzePage basePage)
	{
		var uiPref = ((App)Application.Current).Preference.UIPreferences;
		var comma = ResourceDictionary.Get("_Token_Comma", App.CurrentCulture);
		var openBrace = ResourceDictionary.Get("_Token_OpenBrace", App.CurrentCulture);
		var closedBrace = ResourceDictionary.Get("_Token_ClosedBrace", App.CurrentCulture);
		TextBlockBindable.SetInlines(
			GeneratorStrategyTooltip,
			[
				new Run().WithText($"{null:AnalyzePage_GeneratingStrategySelected}"),
				new LineBreak(),
				new Run().WithText($"{(uiPref.CanRestrictGeneratingGivensCount, uiPref.GeneratedPuzzleGivensCount) switch
				{
					(false, _) or (_, -1) => (ResourceDictionary.Get("AnalyzePage_GeneratedPuzzleGivensNoRestriction", App.CurrentCulture), string.Empty),
					_ => (uiPref.GeneratedPuzzleGivensCount, ResourceDictionary.Get("AnalyzePage_NumberOfGivens", App.CurrentCulture))
				}:AnalyzePage_GeneratedPuzzleGivensIs}"),
				new LineBreak(),
				new Run().WithText($"{DifficultyLevelConversion.GetNameWithDefault(
					uiPref.GeneratorDifficultyLevel,
					ResourceDictionary.Get("DifficultyLevel_None", App.CurrentCulture)
				):AnalyzePage_SelectedDifficultyLevelIs}"),
				new LineBreak(),
				new Run().WithText($"{ResourceDictionary.Get($"SymmetricType_{uiPref.GeneratorSymmetricPattern}", App.CurrentCulture):AnalyzePage_SelectedSymmetricTypeIs}"),
				new LineBreak(),
				new Run().WithText($"{uiPref.GeneratorSelectedTechniques switch
				{
					[var f] => string.Format(ResourceDictionary.Get("AnalyzePage_SingleTechniquesSelected", App.CurrentCulture), f.GetName(App.CurrentCulture)),
					[var f, ..] t and { Count: var fc } => string.Format(ResourceDictionary.Get("AnalyzePage_MultipleTechniquesSelected", App.CurrentCulture), f.GetName(App.CurrentCulture), fc),
					_ => ResourceDictionary.Get("TechniqueSelector_NoTechniqueSelected", App.CurrentCulture),
				}:AnalyzePage_SelectedTechniqueIs}"),
				new LineBreak(),
				new Run().WithText($"{(
				uiPref.GeneratedPuzzleShouldBeMinimal
					? ResourceDictionary.Get("AnalyzePage_IsAMinimal", App.CurrentCulture)
					: ResourceDictionary.Get("AnalyzePage_IsNotMinimal", App.CurrentCulture)
				):AnalyzePage_SelectedMinimalRuleIs}"),
				new LineBreak(),
				new Run().WithText($"{uiPref.GeneratedPuzzleShouldBePearl switch
				{
					true => ResourceDictionary.Get("GeneratingStrategyPage_PearlPuzzle", App.CurrentCulture),
					false => ResourceDictionary.Get("GeneratingStrategyPage_NormalPuzzle", App.CurrentCulture),
					//_ => ResourceDictionary.Get("GeneratingStrategyPage_DiamondPuzzle", App.CurrentCulture)
				}:AnalyzePage_SelectedDiamondRuleIs}"),
				new LineBreak(),
				new Run().WithText($"{uiPref.GeneratorDifficultyLevel switch
				{
					DifficultyLevel.Easy => string.Format(ResourceDictionary.Get("AnalyzePage_IttoryuLength", App.CurrentCulture), uiPref.IttoryuLength),
					_ => ResourceDictionary.Get("AnalyzePage_IttoryuPathIsNotLimited", App.CurrentCulture)
				}:AnalyzePage_SelectedIttoryuIs}")
			]
		);
	}

	/// <summary>
	/// Handle generating operation.
	/// </summary>
	/// <typeparam name="T">The type of the progress data provider.</typeparam>
	/// <param name="onlyGenerateOne">Indicates whether the generator engine only generates for one puzzle.</param>
	/// <param name="gridStateChanger">
	/// The method that can change the state of the target grid. This callback method will be used for specify the grid state
	/// when a user has set the techniques that must be appeared.
	/// </param>
	/// <param name="gridHandler">The grid handler.</param>
	/// <returns>The task that holds the asynchronous operation.</returns>
	private async Task HandleGeneratingAsync<T>(
		bool onlyGenerateOne,
		GridStateChanger<(Analyzer Analyzer, TechniqueSet Techniques)>? gridStateChanger = null,
		ActionRefReadOnly<Grid>? gridHandler = null
	) where T : struct, IEquatable<T>, IProgressDataProvider<T>
	{
		BasePage.IsGeneratorLaunched = true;
		BasePage.ClearAnalyzeTabsData();

		var processingText = ResourceDictionary.Get("AnalyzePage_GeneratorIsProcessing", App.CurrentCulture);
		var preferences = ((App)Application.Current).Preference.UIPreferences;
		var difficultyLevel = preferences.GeneratorDifficultyLevel;
		var symmetry = preferences.GeneratorSymmetricPattern;
		var minimal = preferences.GeneratedPuzzleShouldBeMinimal;
		var pearl = preferences.GeneratedPuzzleShouldBePearl;
		var techniques = preferences.GeneratorSelectedTechniques;
		var givensCount = preferences switch
		{
			{ CanRestrictGeneratingGivensCount: true, GeneratedPuzzleGivensCount: var count and not -1 } => count,
			_ => HodokuPuzzleGenerator.AutoClues
		};
		var ittoryuLength = preferences.IttoryuLength;
		var analyzer = ((App)Application.Current).GetAnalyzerConfigured(BasePage.SudokuPane, preferences.GeneratorDifficultyLevel);
		var ittoryuFinder = new DisorderedIttoryuFinder([.. ((App)Application.Current).Preference.AnalysisPreferences.IttoryuSupportedTechniques]);

		using var cts = new CancellationTokenSource();
		BasePage._ctsForAnalyzingRelatedOperations = cts;

		try
		{
			(_generatingCount, _generatingFilteredCount) = (0, 0);
			var details = new GeneratingDetails(difficultyLevel, symmetry, minimal, pearl, techniques, givensCount, ittoryuLength);
			if (onlyGenerateOne)
			{
				if (await Task.Run(task) is { IsUndefined: false } grid)
				{
					gridStateChanger?.Invoke(ref grid, (analyzer, techniques));
					gridHandler?.Invoke(ref grid);
				}
			}
			else
			{
				while (true)
				{
					if (await Task.Run(task) is { IsUndefined: false } grid)
					{
						gridStateChanger?.Invoke(ref grid, (analyzer, techniques));
						gridHandler?.Invoke(ref grid);

						_generatingFilteredCount++;
						continue;
					}

					break;
				}
			}


			Grid task() => gridCreator(analyzer, ittoryuFinder, details);
		}
		catch (TaskCanceledException)
		{
		}
		finally
		{
			BasePage._ctsForAnalyzingRelatedOperations = null;
			BasePage.IsGeneratorLaunched = false;
		}


		Grid gridCreator(Analyzer analyzer, DisorderedIttoryuFinder finder, GeneratingDetails details)
		{
			try
			{
				return generatePuzzleCore(
					progress => DispatcherQueue.TryEnqueue(
						() =>
						{
							BasePage.AnalyzeProgressLabel.Text = processingText;
							BasePage.AnalyzeStepSearcherNameLabel.Text = progress.ToDisplayString();
						}
					), details, cts.Token, analyzer, finder
				) ?? Grid.Undefined;
			}
			catch (TaskCanceledException)
			{
				return Grid.Undefined;
			}
		}

		Grid? generatePuzzleCore(
			Action<T> reportingAction,
			GeneratingDetails details,
			CancellationToken cancellationToken,
			Analyzer analyzer,
			DisorderedIttoryuFinder finder
		)
		{
			try
			{
				for (
					var (progress, (difficultyLevel, symmetry, minimal, pearl, techniques, givensCount, ittoryuLength)) = (
						new SelfReportingProgress<T>(reportingAction),
						details
					); ; _generatingCount++, progress.Report(T.Create(_generatingCount, _generatingFilteredCount)), cancellationToken.ThrowIfCancellationRequested()
				)
				{
					switch (difficultyLevel)
					{
						case DifficultyLevel.Easy:
						{
							// Optimize: transform the grid if worth.
							var grid = new HodokuPuzzleGenerator().Generate(givensCount, symmetry, cancellationToken);
							var foundIttoryu = finder.FindPath(in grid);
							if (ittoryuLength >= 5 && foundIttoryu.Digits.Length >= 5)
							{
								grid.MakeIttoryu(foundIttoryu);
							}

							if (basicCondition(in grid) && (ittoryuLength != -1 && foundIttoryu.Digits.Length >= ittoryuLength || ittoryuLength == -1))
							{
								return grid;
							}
							break;
						}
						default:
						{
							scoped ref readonly var grid = ref new HodokuPuzzleGenerator().Generate(givensCount, symmetry, cancellationToken);
							if (basicCondition(in grid))
							{
								return grid;
							}
							break;
						}
					}


					bool basicCondition(scoped ref readonly Grid grid)
						=> (givensCount != -1 && grid.GivensCount == givensCount || givensCount == -1)
						&& analyzer.Analyze(in grid) is
						{
							IsSolved: true,
							IsPearl: var isPearl,
							DifficultyLevel: var puzzleDifficultyLevel,
							SolvingPath: var p
						}
						&& (difficultyLevel == 0 || puzzleDifficultyLevel == difficultyLevel)
						&& (minimal && grid.IsMinimal || !minimal)
						&& (pearl && isPearl is true || !pearl)
						&& (!!(techniques && p & techniques) || !techniques);
				}
			}
			catch (OperationCanceledException)
			{
				return null;
			}
		}
	}


	private async void NewPuzzleButton_ClickAsync(object sender, RoutedEventArgs e)
		=> await HandleGeneratingAsync<GeneratorProgress>(
			true,
			gridHandler: (scoped ref readonly Grid grid) =>
			{
				if (((App)Application.Current).Preference.UIPreferences.SavePuzzleGeneratingHistory)
				{
					((App)Application.Current).PuzzleGeneratingHistory.Puzzles.Add(new() { BaseGrid = grid });
				}

				BasePage.SudokuPane.Puzzle = grid;
			}
		);

	private async void BatchGeneratingButton_ClickAsync(object sender, RoutedEventArgs e)
	{
		if (!BasePage.EnsureUnsnapped(true))
		{
			return;
		}

		var fsp = new FileSavePicker();
		fsp.Initialize(this);
		fsp.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
		fsp.SuggestedFileName = ResourceDictionary.Get("Sudoku", App.CurrentCulture);
		fsp.AddFileFormat(FileFormats.PlainText);

		if (await fsp.PickSaveFileAsync() is not { Path: var filePath })
		{
			return;
		}

		await HandleGeneratingAsync<FilteredGeneratorProgress>(
			false,
			static (scoped ref Grid grid, (Analyzer Analyzer, TechniqueSet Techniques) pair) =>
			{
				var (analyzer, techniques) = pair;
				var analyzerResult = analyzer.Analyze(in grid);
				if (!analyzerResult.IsSolved)
				{
					return;
				}

				foreach (var (g, s) in analyzerResult.SolvingPath)
				{
					if (techniques.Contains(s.Code))
					{
						grid = g;
						break;
					}
				}
			},
			(scoped ref readonly Grid grid) =>
			{
				File.AppendAllText(filePath, $"{grid:#}{Environment.NewLine}");

				if (((App)Application.Current).Preference.UIPreferences.AlsoSaveBatchGeneratedPuzzlesIntoHistory
					&& ((App)Application.Current).Preference.UIPreferences.SavePuzzleGeneratingHistory)
				{
					((App)Application.Current).PuzzleGeneratingHistory.Puzzles.Add(new() { BaseGrid = grid });
				}
			}
		);
	}
}

/// <summary>
/// Defines a self-reporting progress type.
/// </summary>
/// <param name="handler"><inheritdoc cref="Progress{T}.Progress(Action{T})" path="/param[@name='handler']"/></param>
/// <typeparam name="T"><inheritdoc cref="Progress{T}" path="/typeparam[@name='T']"/></typeparam>
file sealed class SelfReportingProgress<T>(Action<T> handler) : Progress<T>(handler) where T : struct, IEquatable<T>, IProgressDataProvider<T>
{
	/// <inheritdoc cref="Progress{T}.OnReport(T)"/>
	public void Report(T value) => OnReport(value);
}

/// <summary>
/// The encapsulated type to describe the details for generating puzzles.
/// </summary>
/// <param name="DifficultyLevel">Indicates the difficulty level selected.</param>
/// <param name="SymmetricPattern">Indicates the symmetric pattern selected.</param>
/// <param name="ShouldBeMinimal">Indicates whether generated puzzles should be minimal.</param>
/// <param name="ShouldBePearl">Indicates whether generated puzzles should be pearl.</param>
/// <param name="SelectedTechniques">Indicates the selected technique that you want it to be appeared in generated puzzles.</param>
/// <param name="CountOfGivens">Indicates the limit of givens count.</param>
/// <param name="IttoryuLength">Indicates the ittoryu length.</param>
file sealed record GeneratingDetails(
	DifficultyLevel DifficultyLevel,
	SymmetricType SymmetricPattern,
	bool ShouldBeMinimal,
	bool ShouldBePearl,
	TechniqueSet SelectedTechniques,
	int CountOfGivens,
	int IttoryuLength
);

/// <summary>
/// Provides with extension methods on <see cref="Run"/>.
/// </summary>
/// <seealso cref="Run"/>
file static class RunExtensions
{
	/// <summary>
	/// Sets with <see cref="Run.Text"/> property, reducing indenting.
	/// </summary>
	/// <param name="this">The <see cref="Run"/> instance.</param>
	/// <param name="text">The text to be initialized.</param>
	public static Run WithText(this Run @this, scoped ref ResourceFetcher text)
	{
		@this.Text = text.ToString();
		return @this;
	}
}

/// <summary>
/// Represents a mechanism that allows you using interpolated string syntax to fetch resource text.
/// </summary>
/// <param name="literalLength">Indicates the whole length of the interpolated string.</param>
/// <param name="formattedCount">Indicates the number of the interpolated holes.</param>
/// <remarks>
/// Usage:
/// <code><![CDATA[
/// scoped ResourceFetcher expr = $"{value:ResourceKeyName}";
/// string s = expr.ToString();
/// ]]></code>
/// Here <c>value</c> will be inserted into the resource, whose related key value is specified as <c>ResourceKeyName</c> after colon token.
/// Its equivalent value is
/// <code><![CDATA[
/// string s = string.Format(ResourceDictionary.Get("ResourceKeyName", App.CurrentCulture), value);
/// ]]></code>
/// This type is implemented via an interpolated string handler pattern, same as <see cref="DefaultInterpolatedStringHandler"/>,
/// marked with <see cref="InterpolatedStringHandlerAttribute"/>.
/// </remarks>
/// <seealso cref="DefaultInterpolatedStringHandler"/>
/// <seealso cref="InterpolatedStringHandlerAttribute"/>
[InterpolatedStringHandler]
[StructLayout(LayoutKind.Auto)]
file ref struct ResourceFetcher(int literalLength, int formattedCount)
{
	/// <summary>
	/// <inheritdoc cref="ResourceFetcher" path="/param[@name='literalLength']"/>
	/// </summary>
	private readonly int _literalLength = literalLength;

	/// <summary>
	/// <inheritdoc cref="ResourceFetcher" path="/param[@name='formattedCount']"/>
	/// </summary>
	private readonly int _formattedCount = formattedCount;

	/// <summary>
	/// The internal format.
	/// </summary>
	private string? _format;

	/// <summary>
	/// The internal content.
	/// </summary>
	private object? _content;


	/// <inheritdoc cref="object.ToString"/>
	/// <exception cref="InvalidOperationException">Throws when the value is not initialized.</exception>
	public override readonly string ToString()
		=> _format switch
		{
			not null => _content switch
			{
				var (a, b, c) => string.Format(ResourceDictionary.Get(_format, App.CurrentCulture), a, b, c),
				var (a, b) => string.Format(ResourceDictionary.Get(_format, App.CurrentCulture), a, b),
				ITuple tuple => string.Format(ResourceDictionary.Get(_format, App.CurrentCulture), tuple.ToArray()),
				not null => string.Format(ResourceDictionary.Get(_format, App.CurrentCulture), _content),
				_ => ResourceDictionary.Get(_format, App.CurrentCulture)
			},
			_ => throw new InvalidOperationException("The format cannot be null.")
		};

	/// <inheritdoc cref="DefaultInterpolatedStringHandler.AppendFormatted{T}(T, string?)"/>
	public void AppendFormatted(object? content, string format) => (_format, _content) = (format, content);
}
