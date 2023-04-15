namespace SudokuStudio.Views.Pages.Operation;

/// <summary>
/// Indicates the basic operation command bar.
/// </summary>
[DependencyProperty<string>("SucceedFilePath", IsNullable = true, Accessibility = GeneralizedAccessibility.Internal, DocSummary = "Indicates the path of the saved file.")]
public sealed partial class BasicOperation : Page, IOperationProviderPage
{
	/// <summary>
	/// Defines a default puzzle generator.
	/// </summary>
	private static readonly PatternBasedPuzzleGenerator Generator = new();


	/// <summary>
	/// Initializes a <see cref="BasicOperation"/> instance.
	/// </summary>
	public BasicOperation() => InitializeComponent();


	/// <inheritdoc/>
	public AnalyzePage BasePage { get; set; } = null!;


	/// <summary>
	/// The method called by <see cref="Page_Loaded(object, RoutedEventArgs)"/> and <see cref="Page_Unloaded(object, RoutedEventArgs)"/>.
	/// </summary>
	private void OnSaveFileFailed(AnalyzePage _, SaveFileFailedEventArgs e)
		=> (e.Reason switch { SaveFileFailedReason.UnsnappingFailed => ErrorDialog_ProgramIsSnapped }).IsOpen = true;

	/// <summary>
	/// The method called by <see cref="Page_Loaded(object, RoutedEventArgs)"/> and <see cref="Page_Unloaded(object, RoutedEventArgs)"/>.
	/// </summary>
	private void OnOpenFileFailed(AnalyzePage _, OpenFileFailedEventArgs e)
		=> (
			e.Reason switch
			{
				OpenFileFailedReason.UnsnappingFailed => ErrorDialog_ProgramIsSnapped,
				OpenFileFailedReason.FileIsEmpty => ErrorDialog_FileIsEmpty,
				OpenFileFailedReason.FileIsTooLarge => ErrorDialog_FileIsOversized,
				OpenFileFailedReason.FileCannotBeParsed => ErrorDialog_FileCannotBeParsed
			}
		).IsOpen = true;

	/// <summary>
	/// Try to get all possible format flags.
	/// </summary>
	/// <param name="panel">The panel.</param>
	/// <returns>All format flags.</returns>
	private SudokuFormatFlags GetFormatFlags(StackPanel panel)
		=> (
			from children in panel.Children
			where children is CheckBox { Tag: int and not 0, IsChecked: true }
			select (SudokuFormatFlags)(int)((CheckBox)children).Tag!
		).Aggregate(SudokuFormatFlags.None, static (interim, next) => interim | next);


	private void Page_Loaded(object sender, RoutedEventArgs e)
	{
		BasePage.OpenFileFailed += OnOpenFileFailed;
		BasePage.SaveFileFailed += OnSaveFileFailed;
	}

	private void Page_Unloaded(object sender, RoutedEventArgs e)
	{
		BasePage.OpenFileFailed -= OnOpenFileFailed;
		BasePage.SaveFileFailed -= OnSaveFileFailed;
	}

	private async void NewPuzzleButton_ClickAsync(object sender, RoutedEventArgs e)
	{
		BasePage.GeneratorIsNotRunning = false;

		var grid = await Generator.GenerateAsync();

		BasePage.GeneratorIsNotRunning = true;

		if (((App)Application.Current).Preference.UIPreferences.SavePuzzleGeneratingHistory)
		{
			((App)Application.Current).PuzzleGeneratingHistory.Puzzles.Add(new() { GridString = grid.ToString() });
		}

		BasePage.SudokuPane.Puzzle = grid;
	}

	private void SaveAsButton_Click(object sender, RoutedEventArgs e) => Dialog_FormatChoosing.IsOpen = true;

	private void CopyButton_Click(object sender, RoutedEventArgs e) => BasePage.CopySudokuGridText();

	private void CopySpecifedFormatsButton_Click(object sender, RoutedEventArgs e) => Dialog_FormatChoosingOnCopying.IsOpen = true;

	private async void CopyPictureButton_ClickAsync(object sender, RoutedEventArgs e) => await BasePage.CopySudokuGridControlAsSnapshotAsync();

	private async void PasteButton_ClickAsync(object sender, RoutedEventArgs e) => await BasePage.PasteCodeToSudokuGridAsync();

	private void ResetButton_Click(object sender, RoutedEventArgs e)
	{
		var puzzle = BasePage.SudokuPane.Puzzle;
		puzzle.Reset();

		BasePage.SudokuPane.Puzzle = puzzle;
	}

	private void ClearButton_Click(object sender, RoutedEventArgs e) => Dialog_AreYouSureToReturnToEmpty.IsOpen = true;

	private void UndoButton_Click(object sender, RoutedEventArgs e) => BasePage.SudokuPane.UndoStep();

	private void RedoButton_Click(object sender, RoutedEventArgs e) => BasePage.SudokuPane.RedoStep();

	private async void Dialog_FormatChoosing_ActionButtonClickAsync(TeachingTip sender, object args)
	{
		if (GetFormatFlags(FormatGroupPanel) is not (var flags and not 0))
		{
			return;
		}

		if (await BasePage.SaveFileInternalAsync(createFormatHandlers(flags)))
		{
			Dialog_FormatChoosing.IsOpen = false;
		}


		static ArrayList createFormatHandlers(SudokuFormatFlags flags)
		{
			var result = new ArrayList();
			foreach (var flag in flags.GetAllFlags()!)
			{
				result.Add(
					flag switch
					{
						SudokuFormatFlags.InitialFormat => SusserFormat.Default,
						SudokuFormatFlags.CurrentFormat => SusserFormat.Full,
						SudokuFormatFlags.CurrentFormatIgnoringValueKind => SusserFormatTreatingValuesAsGivens.Default,
						SudokuFormatFlags.HodokuCompatibleFormat => HodokuLibraryFormat.Default,
						SudokuFormatFlags.MultipleGridFormat => MultipleLineFormat.Default,
						SudokuFormatFlags.PencilMarkFormat => PencilMarkFormat.Default,
						SudokuFormatFlags.SukakuFormat => SukakuFormat.Default,
						SudokuFormatFlags.ExcelFormat => ExcelFormat.Default,
						SudokuFormatFlags.OpenSudokuFormat => OpenSudokuFormat.Default
					}
				);
			}

			return result;
		}
	}

	private void Dialog_FormatChoosingOnCopying_ActionButtonClick(TeachingTip sender, object args)
	{
		if (GetFormatFlags(FormatGroupPanelOnCopying) is not (var flags and not 0))
		{
			return;
		}

		if (!flags.IsFlag())
		{
			return;
		}

		var puzzle = BasePage.SudokuPane.Puzzle;
		var targetText = string.Join("\r\n\r\n", from flag in flags let formatter = flag.GetFormatter() select puzzle.ToString(formatter));

		// Copy into clipboard.
		var dataPackage = new DataPackage { RequestedOperation = DataPackageOperation.Copy };
		dataPackage.SetText(targetText);

		Clipboard.SetContent(dataPackage);

		Dialog_FormatChoosingOnCopying.IsOpen = false;
	}

	private void Dialog_AreYouSureToReturnToEmpty_ActionButtonClick(TeachingTip sender, object args)
	{
		BasePage.SudokuPane.Puzzle = Grid.Empty;

		Dialog_AreYouSureToReturnToEmpty.IsOpen = false;
	}

	private async void SaveFileButton_ClickAsync(object sender, RoutedEventArgs e) => await BasePage.SaveFileInternalAsync();

	private async void OpenFileButton_ClickAsync(object sender, RoutedEventArgs e) => await BasePage.OpenFileInternalAsync();
}
