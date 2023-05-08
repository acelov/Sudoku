namespace SudokuStudio.Views.Controls;

/// <summary>
/// Defines a cell displayed in a <see cref="SudokuPane"/>.
/// </summary>
/// <seealso cref="SudokuPane"/>
[DependencyProperty<Mask>("CandidatesMask", DefaultValue = Grid.MaxCandidatesMask)]
[DependencyProperty<CellStatus>("Status", DefaultValue = CellStatus.Empty)]
internal sealed partial class SudokuPaneCell : UserControl
{
	/// <summary>
	/// Initializes a <see cref="SudokuPaneCell"/> instance.
	/// </summary>
	public SudokuPaneCell() => InitializeComponent();


#nullable disable
	/// <summary>
	/// Indicates the base pane.
	/// </summary>
	public SudokuPane BasePane { get; set; }
#nullable restore

	/// <summary>
	/// Indicates the cell index.
	/// </summary>
	internal Cell CellIndex { get; init; }


	private void UserControl_PointerEntered(object sender, PointerRoutedEventArgs e) => BasePane.SelectedCell = CellIndex;

	private void UserControl_PointerExited(object sender, PointerRoutedEventArgs e) => BasePane.SelectedCell = -1;

	private void TextBlock_PointerEntered(object sender, PointerRoutedEventArgs e) => BasePane.SelectedCell = CellIndex;

	private void TextBlock_PointerExited(object sender, PointerRoutedEventArgs e) => BasePane.SelectedCell = CellIndex;

	private void TextBlock_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
	{
		if (!BasePane.EnableDoubleTapFilling)
		{
			return;
		}

		if ((this, sender) is not ({ BasePane: { Puzzle: var modified, SelectedCell: var cell and not -1 } }, TextBlock { Text: var text }))
		{
			return;
		}

		if (!Digit.TryParse(text, out var originalDigit)
			|| originalDigit - 1 is not (var digit and >= 0 and < 9)
			|| modified.GetStatus(cell) != CellStatus.Empty
			|| (modified.GetCandidates(cell) >> digit & 1) == 0)
		{
			return;
		}

		modified[cell] = digit;
		BasePane.SetPuzzleInternal(modified);

		BasePane.TriggerGridUpdated(GridUpdatedBehavior.Assignment, cell * 9 + digit);
	}

	private void TextBlock_RightTapped(object sender, RightTappedRoutedEventArgs e)
	{
		if ((this, sender) is not ({ BasePane: { Puzzle: var modified, SelectedCell: var cell and not -1 } }, TextBlock { Text: var text }))
		{
			return;
		}

		if (!Digit.TryParse(text, out var originalDigit) || originalDigit - 1 is not (var digit and >= 0 and < 9))
		{
			return;
		}

		BasePane.TriggerClicked(MouseButton.Right, cell * 9 + digit);

		if (!BasePane.EnableRightTapRemoving
			|| modified.GetStatus(cell) != CellStatus.Empty || (modified.GetCandidates(cell) >> digit & 1) == 0)
		{
			return;
		}

		modified[cell, digit] = false;
		BasePane.SetPuzzleInternal(modified);

		BasePane.TriggerGridUpdated(GridUpdatedBehavior.Elimination, cell * 9 + digit);
	}

	private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
	{
		if ((this, sender) is not ({ BasePane.SelectedCell: var cell and not -1 }, TextBlock { Text: var text }))
		{
			return;
		}

		if (!Digit.TryParse(text, out var originalDigit) || originalDigit - 1 is not (var digit and >= 0 and < 9))
		{
			return;
		}

		BasePane.TriggerClicked(MouseButton.Left, cell * 9 + digit);
	}
}
