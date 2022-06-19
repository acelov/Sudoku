﻿namespace Sudoku.UI.Drawing.Shapes;

/// <summary>
/// Defines a sudoku grid.
/// </summary>
public sealed class SudokuGrid : DrawingElement
{
	/// <summary>
	/// Indicates the inner grid layout control.
	/// </summary>
	private readonly GridLayout _gridLayout;

	/// <summary>
	/// Indicates the cell digits.
	/// </summary>
	private readonly CellDigit[] _cellDigits = new CellDigit[81];

	/// <summary>
	/// Indicates the candidate digits.
	/// </summary>
	private readonly CandidateDigit[] _candidateDigits = new CandidateDigit[81];

#if AUTHOR_DEFINED
	/// <summary>
	/// Indicates the cell marks.
	/// </summary>
	private readonly CellMark[] _cellMarks = new CellMark[81];
#endif

	/// <summary>
	/// Indicates the stacks to store the undoing and redoing steps.
	/// </summary>
	private readonly Stack<Grid> _undoSteps = new(), _redoSteps = new();

	/// <summary>
	/// Indicates the focused cell rectangle.
	/// </summary>
	private readonly Rectangle _focusedRectangle;

	/// <summary>
	/// Indicates the rectangles displaying for peers of the focused cell.
	/// </summary>
	private readonly Rectangle[] _peerFocusedRectangle = new Rectangle[20];

	/// <summary>
	/// Indicates the callback method that invokes when the undoing and redoing steps are updated.
	/// </summary>
	private readonly Action? _undoRedoStepsUpdatedCallback;

	/// <summary>
	/// Indicates the user preference used.
	/// </summary>
	private readonly IDrawingPreference _preference;

	/// <summary>
	/// Indicates whether the current mode is mask mode.
	/// </summary>
	private bool _isMaskMode;

	/// <summary>
	/// Indicates whether the current grid pane shows candidates regardless of the value in the preference file.
	/// </summary>
	private bool _showsCandidates;

	/// <summary>
	/// Indicates the pane size.
	/// </summary>
	private double _paneSize;

	/// <summary>
	/// Indicates the outside offset.
	/// </summary>
	private double _outsideOffset;

	/// <summary>
	/// Indicates the focused cell.
	/// </summary>
	private int _focusedCell;

	/// <summary>
	/// Indicates the inner grid.
	/// </summary>
	private Grid _grid;


	/// <summary>
	/// Initializes a <see cref="SudokuGrid"/> instance via the details.
	/// </summary>
	/// <param name="preference">The user preference instance.</param>
	/// <param name="paneSize">The pane size.</param>
	/// <param name="outsideOffset">The outside offset.</param>
	/// <param name="elementUpdatedCallback">
	/// The callback method that triggers when the inner undo-redo steps are updated.
	/// </param>
	public SudokuGrid(
		IDrawingPreference preference, double paneSize, double outsideOffset, Action? elementUpdatedCallback) :
		this(Grid.Empty, preference, paneSize, outsideOffset, elementUpdatedCallback)
	{
	}

	/// <summary>
	/// Initializes a <see cref="SudokuGrid"/> instance via the details.
	/// </summary>
	/// <param name="grid">The <see cref="Grid"/> instance.</param>
	/// <param name="preference">The user preference.</param>
	/// <param name="paneSize">The pane size.</param>
	/// <param name="outsideOffset">The outside offset.</param>
	/// <param name="elementUpdatedCallback">
	/// The callback method that triggers when the inner undo-redo steps are updated.
	/// </param>
	public SudokuGrid(
		in Grid grid, IDrawingPreference preference, double paneSize, double outsideOffset,
		Action? elementUpdatedCallback)
	{
		(
			_preference,
			_grid,
			_paneSize,
			_outsideOffset,
			_gridLayout,
			_undoRedoStepsUpdatedCallback,
			_showsCandidates,
			_focusedRectangle
		) = (
			preference,
			grid,
			paneSize,
			outsideOffset,
			initializeGridLayout(paneSize, outsideOffset),
			elementUpdatedCallback,
			preference.ShowCandidates,
			new() { Fill = new SolidColorBrush(preference.FocusedCellColor), Visibility = Visibility.Collapsed }
		);

		// Initializes the field '_peerFocusedRectangle'.
		foreach (ref var rectangle in _peerFocusedRectangle.EnumerateRef())
		{
			rectangle = new()
			{
				Fill = new SolidColorBrush(preference.PeersFocusedCellColor),
				Visibility = Visibility.Collapsed
			};

			GridLayout.SetRow(rectangle, 4);
			GridLayout.SetRow(rectangle, 4);
			Canvas.SetZIndex(rectangle, -1);
		}

		// Sets the Z-Index.
		GridLayout.SetRow(_focusedRectangle, 4);
		GridLayout.SetColumn(_focusedRectangle, 4);
		Canvas.SetZIndex(_focusedRectangle, -1);

#if AUTHOR_DEFINED
		// Initializes cell marks.
		foreach (ref var cellMark in _cellMarks.EnumerateRef())
		{
			cellMark = new(preference);
		}
#endif

		// Initializes values.
		initializeValues();

		// Then initialize other items.
		UpdateView();

		// Last, set the focused cell to -1, to hide the highlight cell by default.
		FocusedCell = -1;


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static GridLayout initializeGridLayout(double paneSize, double outsideOffset)
		{
			var result = new GridLayout
			{
				Width = paneSize,
				Height = paneSize,
				Padding = new(outsideOffset),
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center
			};
			for (int i = 0; i < 9; i++)
			{
				result.RowDefinitions.Add(new());
				result.ColumnDefinitions.Add(new());
			}

			return result;
		}

		void initializeValues()
		{
			for (int i = 0; i < 81; i++)
			{
				ref var p = ref _cellDigits[i];
				p = new(preference);
				var control1 = p.GetControl();
				GridLayout.SetRow(control1, i / 9);
				GridLayout.SetColumn(control1, i % 9);
				_gridLayout.Children.Add(control1);
				var maskEllipse1 = p.GetMaskEllipseControl();
				GridLayout.SetRow(maskEllipse1, i / 9);
				GridLayout.SetColumn(maskEllipse1, i % 9);
				_gridLayout.Children.Add(maskEllipse1);

				ref var q = ref _candidateDigits[i];
				q = new(preference);
				var control2 = q.GetControl();
				GridLayout.SetRow(control2, i / 9);
				GridLayout.SetColumn(control2, i % 9);
				_gridLayout.Children.Add(control2);

#if AUTHOR_DEFINED
				// Initializes for the cell marks.
				for (int cellIndex = 0; cellIndex < _cellMarks.Length; cellIndex++)
				{
					var cellMark = _cellMarks[cellIndex];
					var control = cellMark.GetControl();
					GridLayout.SetRow(control, cellIndex / 9);
					GridLayout.SetColumn(control, cellIndex % 9);
					_gridLayout.Children.Add(control);
				}
#endif

				// Initializes for the items to render the focusing elements.
				if (_focusedCell == i)
				{
					GridLayout.SetRow(_focusedRectangle, _focusedCell / 9);
					GridLayout.SetColumn(_focusedRectangle, _focusedCell % 9);
					_gridLayout.Children.Add(_focusedRectangle);

					for (int peerIndex = 0; peerIndex < 20; peerIndex++)
					{
						var rectangle = _peerFocusedRectangle[peerIndex];
						int peerCell = Peers[_focusedCell][peerIndex];
						GridLayout.SetRow(rectangle, peerCell / 9);
						GridLayout.SetColumn(rectangle, peerCell % 9);
						_gridLayout.Children.Add(rectangle);
					}
				}
			}
		}
	}


	/// <summary>
	/// <para>Indicates whether the grid displays for candidates.</para>
	/// <para>
	/// This property will also change the value in the user preference. If you want to temporarily change
	/// the value, use <see cref="UserShowCandidates"/> instead.
	/// </para>
	/// </summary>
	/// <seealso cref="UserShowCandidates"/>
	public bool ShowCandidates
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => _preference.ShowCandidates;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			if (_preference.ShowCandidates == value || _isMaskMode)
			{
				return;
			}

			_preference.ShowCandidates = value;
			Array.ForEach(_candidateDigits, candidateDigit => candidateDigit.ShowCandidates = value);
		}
	}

	/// <summary>
	/// <para>Indicates whether the grid displays for candidates.</para>
	/// <para>
	/// This property will temporarily change the state of the displaying candidates. The property doesn't
	/// modify the user preference. If you want to modify the user preference value, use <see cref="ShowCandidates"/>
	/// instead.
	/// </para>
	/// </summary>
	/// <seealso cref="ShowCandidates"/>
	public bool UserShowCandidates
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => _showsCandidates;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			if (_showsCandidates == value || _isMaskMode)
			{
				return;
			}

			_showsCandidates = value;
			Array.ForEach(_candidateDigits, candidateDigit => candidateDigit.UserShowCandidates = value);
		}
	}

	/// <summary>
	/// Indicates whether the current mode is mask mode.
	/// </summary>
	public bool IsMaskMode
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => _isMaskMode;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			if (_isMaskMode == value)
			{
				return;
			}

			var a = Mask;
			var b = Unmask;
			(value ? a : b)();
		}
	}

	/// <summary>
	/// Gets or sets the outside offset.
	/// </summary>
	public double OutsideOffset
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => _outsideOffset;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			if (_outsideOffset.NearlyEquals(value))
			{
				return;
			}

			_outsideOffset = value;
			_gridLayout.Padding = new(value);
		}
	}

	/// <summary>
	/// Gets or sets the pane size.
	/// </summary>
	public double PaneSize
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => _paneSize;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			if (_paneSize.NearlyEquals(value))
			{
				return;
			}

			_paneSize = value;
			_gridLayout.Width = value;
			_gridLayout.Height = value;
		}
	}

	/// <summary>
	/// Indicates the focused cell used.
	/// </summary>
	public int FocusedCell
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => _focusedCell;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			if (_focusedCell == value)
			{
				return;
			}

			if (!_preference.AllowFocusing)
			{
				return;
			}

			if ((_focusedCell = value) == -1)
			{
				_focusedRectangle.Visibility = Visibility.Collapsed;
				Array.ForEach(_peerFocusedRectangle, static rectangle => rectangle.Visibility = Visibility.Collapsed);

				return;
			}

			_focusedRectangle.Visibility = Visibility.Visible;
			GridLayout.SetRow(_focusedRectangle, value / 9);
			GridLayout.SetColumn(_focusedRectangle, value % 9);

			for (int i = 0; i < 20; i++)
			{
				var rectangle = _peerFocusedRectangle[i];
				int cell = Peers[value][i];

				rectangle.Visibility = Visibility.Visible;
				GridLayout.SetRow(rectangle, cell / 9);
				GridLayout.SetColumn(rectangle, cell % 9);
			}
		}
	}

	/// <summary>
	/// Gets or sets the grid. If you want to get the inner sudoku grid puzzle instance,
	/// we suggest you use the property <see cref="GridRef"/> instead of using the accessor
	/// <see cref="Grid"/>.<see langword="get"/> because that property (i.e. <see cref="GridRef"/>) copies by reference.
	/// </summary>
	/// <seealso cref="GridRef"/>
	public Grid Grid
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => _grid;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			// If the current mode is mask mode, we should skip the operation and do nothing.
			if (_isMaskMode)
			{
				return;
			}

			// Set the new grid and update the view.
			_grid = value;

			// Clear the wrong status for all cell digits and candidate digits.
			Array.ForEach(_cellDigits, static cellDigit => cellDigit.IsGiven = false);
			Array.ForEach(_candidateDigits, static candidateDigit => candidateDigit.WrongDigitMask = 0);

			// Update the view.
			UpdateView();

			// The operation must clear two stacks, and trigger the handler '_undoRedoStepsUpdatedCallback'.
			_undoSteps.Clear();
			_redoSteps.Clear();
			_undoRedoStepsUpdatedCallback?.Invoke();
		}
	}

	/// <summary>
	/// Indicates the number of available undoable steps.
	/// </summary>
	internal int UndoStepsCount => _undoSteps.Count;

	/// <summary>
	/// Indicates the number of available redoable steps.
	/// </summary>
	internal int RedoStepsCount => _redoSteps.Count;

	/// <summary>
	/// Gets the reference of the grid. The method is used for getting the grid instance by reference.
	/// </summary>
	internal ref readonly Grid GridRef
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref _grid;
	}

	/// <inheritdoc/>
	protected override string TypeIdentifier => nameof(SudokuGrid);


	/// <summary>
	/// To undo a step.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Undo()
	{
		if (_isMaskMode)
		{
			return;
		}

		_redoSteps.Push(_grid);

		var previousStep = _undoSteps.Pop();
		_grid = previousStep;

		_undoRedoStepsUpdatedCallback?.Invoke();

		UpdateView();
	}

	/// <summary>
	/// To redo a step.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Redo()
	{
		if (_isMaskMode)
		{
			return;
		}

		_undoSteps.Push(_grid);

		var nextStep = _redoSteps.Pop();
		_grid = nextStep;

		_undoRedoStepsUpdatedCallback?.Invoke();

		UpdateView();
	}

	/// <summary>
	/// To make the specified cell fill the specified digit.
	/// </summary>
	/// <param name="cell">The cell that the conclusion is from.</param>
	/// <param name="digit">The digit made.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void MakeDigit(int cell, int digit)
	{
		// If the current mode is mask mode, we should directly skip the operation.
		if (_isMaskMode)
		{
			return;
		}

		// Stores the previous grid status to the undo stack.
		AddStep(_grid);

		// To re-compute candidates if the current cell is modifiable.
		if (digit != -1 && _grid.GetStatus(cell) == CellStatus.Modifiable)
		{
			_grid[cell] = -1;
		}

		// Then set the new value.
		// Please note that the previous statement '_grid[cell] = -1' will re-compute candidates,
		// so it's not a redundant statement. For more information for the indexer,
		// please visit the member 'Grid.this[int].set'.
		// If you remove it, the candidates won't be re-calculated and cause a bug that the candidate
		// not being refreshed.
		_grid[cell] = digit;

		// To update the view.
		UpdateView();
	}

	/// <summary>
	/// To eliminate the specified digit from the specified cell.
	/// </summary>
	/// <param name="cell">The cell that the eliminated digit is from.</param>
	/// <param name="digit">The digit eliminated.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void EliminateDigit(int cell, int digit)
	{
		// If the current mode is mask mode, we should directly skip the operation.
		if (_isMaskMode)
		{
			return;
		}

		if (digit == -1)
		{
			// Skips the invalid data.
			return;
		}

		// Stores the previous grid status to the undo stack.
		AddStep(_grid);

		// Update the grid and view.
		_grid[cell, digit] = false;
		UpdateView();
	}

	/// <summary>
	/// To fix the grid, which means all modifiable digits will be changed their own status to given ones.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void FixGrid()
	{
		// If the current mode is mask mode, we should directly skip the operation.
		if (_isMaskMode)
		{
			return;
		}

		// Stores the previous grid status to the undo stack.
		AddStep(_grid);

		// Update the grid and view.
		_grid.Fix();
		UpdateView();
	}

	/// <summary>
	/// To unfix the grid, which means all given digits will be changed their own status to modifiable ones.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void UnfixGrid()
	{
		// If the current mode is mask mode, we should directly skip the operation.
		if (_isMaskMode)
		{
			return;
		}

		// Stores the previous grid status to the undo stack.
		AddStep(_grid);

		// Update the grid and view.
		_grid.Unfix();
		UpdateView();
	}

	/// <summary>
	/// To reset the grid, which means all value having been filled into the grid as modifiable ones
	/// will be cleared.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ResetGrid()
	{
		// If the current mode is mask mode, we should directly skip the operation.
		if (_isMaskMode)
		{
			return;
		}

		// Stores the previous grid status to the undo stack.
		AddStep(_grid);

		// Update the grid and view.
		_grid.Reset();
		UpdateView();
	}

	/// <summary>
	/// To replace with the new grid.
	/// </summary>
	/// <param name="grid">The grid to be replaced with.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ReplaceGrid(in Grid grid)
	{
		// If the current mode is mask mode, we should directly skip the operation.
		if (_isMaskMode)
		{
			return;
		}

		// Stores the previous grid status to the undo stack.
		AddStep(_grid);

		// Update the grid and view.
		_grid = grid;
		UpdateView();
	}

	/// <summary>
	/// To mask the grid.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Mask()
	{
		_isMaskMode = true;
		Array.ForEach(_cellDigits, static element => element.IsMaskMode = true);
		Array.ForEach(_candidateDigits, static element => element.IsMaskMode = true);
	}

	/// <summary>
	/// To unmask the grid.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Unmask()
	{
		_isMaskMode = false;
		Array.ForEach(_cellDigits, static element => element.IsMaskMode = false);
		Array.ForEach(_candidateDigits, static element => element.IsMaskMode = false);
	}

	/// <summary>
	/// Sets the mark shape at the specified cell index.
	/// </summary>
	/// <param name="cellIndex">The cell index.</param>
	/// <param name="shapeKind">
	/// The shape kind you want to assign. If the value is <see cref="ShapeKind.None"/>,
	/// the method will clear the displaying of the shape. In this case you can also call the method
	/// <see cref="ClearCellMark(int)"/>. They are same.
	/// </param>
	/// <seealso cref="ClearCellMark(int)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetCellMark(int cellIndex, ShapeKind shapeKind) => _cellMarks[cellIndex].ShapeKind = shapeKind;

	/// <summary>
	/// Clears the mark shape at the specified cell index.
	/// </summary>
	/// <param name="cellIndex">The cell index.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ClearCellMark(int cellIndex) => SetCellMark(cellIndex, ShapeKind.None);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals([NotNullWhen(true)] DrawingElement? other)
		=> other is SudokuGrid comparer && _grid == comparer._grid;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode() => HashCode.Combine(TypeIdentifier, _grid);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override GridLayout GetControl() => _gridLayout;

	/// <summary>
	/// Adds the specified step into the collection.
	/// </summary>
	/// <param name="grid">The step to be added.</param>
	private void AddStep(in Grid grid)
	{
		_undoSteps.Push(_grid);
		_grid = grid;

		_redoSteps.Clear();

		_undoRedoStepsUpdatedCallback?.Invoke();
	}

	/// <summary>
	/// To update the view via the current grid.
	/// </summary>
	private void UpdateView()
	{
		// Iterates on each cell.
		for (int i = 0; i < 81; i++)
		{
			// Checks the status of the cell.
			switch (_grid.GetStatus(i))
			{
				case CellStatus.Empty:
				{
					// Due to the empty cell, we should set the current cell value to byte.MaxValue
					// in order not to display the value in the view.
					_cellDigits[i].Digit = byte.MaxValue;

					// Gets the current candidate mask, and set the value in order to update the view.
					short candidateMask = _grid.GetCandidates(i);
					_candidateDigits[i].CandidateMask = candidateMask;

					// Checks the correctness of the candidates.
					// If a certain digit has been wrongly removed from the grid, we should display it
					// using a different color if enabled the delta view.
					if (_preference.EnableDeltaValuesDisplaying
						&& _grid.ResetGrid.Solution is { IsUndefined: false } solution)
					{
						// Checks the wrong digits.
						// Wrong digits are the correct digits in the solution but they have been eliminated.
						_candidateDigits[i].WrongDigitMask = (short)(511 & ~candidateMask & 1 << solution[i]);
					}

					break;
				}
				case var status and (CellStatus.Given or CellStatus.Modifiable):
				{
					// Gets the current digit input, and set the value in order to update the view.
					byte digit = (byte)_grid[i];
					_cellDigits[i].Digit = digit;

					// Due to the value cell, we should set the candidate mask to 0
					// in order not to display the value in the view.
					_cellDigits[i].IsGiven = status == CellStatus.Given;
					_candidateDigits[i].CandidateMask = 0;

					// Checks the correctness of the digit.
					// If the digit is wrong, we should display it using a different color
					// if enabled the delta view.
					if (_preference.EnableDeltaValuesDisplaying)
					{
						if (_grid.ResetGrid.Solution is { IsUndefined: false } solution)
						{
							// For unique-solution puzzle, we should check both duplicate digits
							// and wrong digits different with the solution.
							if (solution[i] != digit)
							{
								_cellDigits[i].IsGiven = null;
								_candidateDigits[i].WrongDigitMask = 0;
							}
						}
						else
						{
							// For multiple- and no- solution puzzle, we should check only duplicate digits.
							foreach (int cell in PeerMaps[i] - _grid.EmptyCells)
							{
								if (_grid[cell] == _grid[i] && _grid.GetStatus(i) != CellStatus.Given)
								{
									// Duplicate.
									// Here we should report the duplicate digit.
									_cellDigits[i].IsGiven = null;
									_candidateDigits[i].WrongDigitMask = 0;

									break;
								}
							}
						}
					}

					break;
				}
			}
		}
	}
}
