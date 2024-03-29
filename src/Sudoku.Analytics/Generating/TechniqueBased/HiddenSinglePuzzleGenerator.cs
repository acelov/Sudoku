namespace Sudoku.Generating.TechniqueBased;

/// <summary>
/// Represents a puzzle generator that uses hidden single.
/// </summary>
public sealed class HiddenSinglePuzzleGenerator : SinglePuzzleGenerator<HiddenSingleStep>
{
	/// <summary>
	/// Indicates whether the generator will create for block excluders.
	/// This option will only be used if the generator generates for hidden single in lines.
	/// </summary>
	public bool AllowsBlockExcluders { get; set; }

	/// <inheritdoc/>
	public override TechniqueSet SupportedTechniques
		=> [
			Technique.HiddenSingleBlock, Technique.HiddenSingleRow, Technique.HiddenSingleColumn,
			Technique.CrosshatchingBlock, Technique.CrosshatchingRow, Technique.CrosshatchingColumn
		];


	/// <inheritdoc/>
	public override JustOneCellPuzzle GenerateJustOneCell()
	{
		var house = RandomlySelectHouse();
		var subtype = RandomlySelectSubtype(house, s => AllowsBlockExcluders || s.GetExcludersCount(HouseType.Block) == 0);
		var a = GenerateForBlock;
		var b = GenerateForLine;
		return (house < 9 ? a : b)(house, subtype);
	}

	/// <inheritdoc/>
	public override FullPuzzle GenerateUnique(CancellationToken cancellationToken = default)
		=> new FullPuzzleFailed(GeneratingFailedReason.Unnecessary);

	/// <summary>
	/// Generate for block.
	/// </summary>
	/// <param name="house">The target house.</param>
	/// <param name="subtype">The selected subtype.</param>
	/// <returns>The final result generated.</returns>
	private JustOneCellPuzzle GenerateForBlock(House house, SingleTechniqueSubtype subtype)
	{
		while (true)
		{
			// Placeholders may not necessary. Just check for excluders.
			var cellsInHouse = HousesMap[house];
			var (excluderRows, excluderColumns) = (0, 0);
			var (rows, columns) = (cellsInHouse.RowMask << 9, cellsInHouse.ColumnMask << 18);
			for (var i = 0; i < subtype.GetExcludersCount(HouseType.Row); i++)
			{
				House excluderHouse;
				do
				{
					excluderHouse = Rng.Next(9, 18);
				} while ((rows >> excluderHouse & 1) == 0);
				_ = (excluderRows |= 1 << excluderHouse, rows &= ~(1 << excluderHouse));
			}
			for (var i = 0; i < subtype.GetExcludersCount(HouseType.Column); i++)
			{
				House excluderHouse;
				do
				{
					excluderHouse = Rng.Next(18, 27);
				} while ((columns >> excluderHouse & 1) == 0);
				_ = (excluderColumns |= 1 << excluderHouse, columns &= ~(1 << excluderHouse));
			}
			var excluders = (CellMap)[];
			foreach (var r in excluderRows)
			{
				var lastCellsAvailable = HousesMap[r] - cellsInHouse - excluders.ExpandedPeers;
				excluders.Add(lastCellsAvailable[Rng.Next(0, lastCellsAvailable.Count)]);
			}
			foreach (var c in excluderColumns)
			{
				var lastCellsAvailable = HousesMap[c] - cellsInHouse - excluders.ExpandedPeers;
				excluders.Add(lastCellsAvailable[Rng.Next(0, lastCellsAvailable.Count)]);
			}
			if (!excluders)
			{
				// Try again if no excluders found.
				continue;
			}

			// Now checks for uncovered cells in the target house, one of the uncovered cells is the target cell,
			// and the others should be placeholders.
			ShuffleSequence(DigitSeed);
			var targetDigit = DigitSeed[Rng.Next(0, 9)];
			var puzzle = Grid.Empty;
			var uncoveredCells = cellsInHouse - excluders.ExpandedPeers;
			var targetCell = uncoveredCells[Rng.Next(0, uncoveredCells.Count)];
			var tempIndex = 0;
			foreach (var placeholderCell in uncoveredCells - targetCell)
			{
				if (DigitSeed[tempIndex] == targetDigit)
				{
					tempIndex++;
				}

				puzzle.SetDigit(placeholderCell, DigitSeed[tempIndex]);
				puzzle.SetState(placeholderCell, CellState.Given);
				tempIndex++;
			}
			foreach (var excluder in excluders)
			{
				puzzle.SetDigit(excluder, targetDigit);
				puzzle.SetState(excluder, CellState.Given);
			}

			// We should adjust the givens and target cells to the specified position.
			switch (Alignment)
			{
				case GridAlignment.CenterHouses or GridAlignment.CenterBlock when house != 4:
				{
					adjustToBlock5(ref house, ref targetCell, ref puzzle);
					break;
				}
				case GridAlignment.CenterCell when targetCell != 40:
				{
					adjustToBlock5(ref house, ref targetCell, ref puzzle);
					adjustToCenterCell(ref targetCell, ref puzzle);
					break;
				}
				default:
				{
					break;
				}
			}

			return new JustOneCellPuzzleSuccessful(
				in puzzle,
				targetCell,
				targetDigit,
				new HiddenSingleStep(null!, null, null!, targetCell, targetDigit, house, false, subtype)
			);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static void adjustToBlock5(scoped ref House house, scoped ref Cell targetCell, scoped ref Grid puzzle)
		{
			if (house == 4)
			{
				return;
			}

			var (c1s, c1e, c2s, c2e) = house switch
			{
				0 => (0, 1, 3, 4),
				1 => (0, 1, -1, -1),
				2 => (0, 1, 4, 5),
				3 => (3, 4, -1, -1),
				5 => (4, 5, -1, -1),
				6 => (1, 2, 3, 4),
				7 => (1, 2, -1, -1),
				8 => (1, 2, 4, 5)
			};

			puzzle.SwapChute(c1s, c1e);
			if (c2s != -1)
			{
				puzzle.SwapChute(c2s, c2e);
			}

			house = 4;
			targetCell = HousesCells[4][BlockPositionOf(targetCell)];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static void adjustToCenterCell(scoped ref Cell targetCell, scoped ref Grid puzzle)
		{
			var pos = BlockPositionOf(targetCell);
			if (targetCell.ToHouseIndex(HouseType.Block) == 4 && pos == 4)
			{
				return;
			}

			var (c1s, c1e, c2s, c2e) = pos switch
			{
				0 => (12, 13, 21, 22),
				1 => (12, 13, -1, -1),
				2 => (12, 13, 22, 23),
				3 => (21, 22, -1, -1),
				5 => (22, 23, -1, -1),
				6 => (13, 14, 21, 22),
				7 => (13, 14, -1, -1),
				8 => (13, 14, 22, 23)
			};

			puzzle.SwapTwoHouses(c1s, c1e);
			if (c2s != -1)
			{
				puzzle.SwapTwoHouses(c2s, c2e);
			}

			targetCell = 40;
		}
	}

	/// <summary>
	/// Generate for line.
	/// </summary>
	/// <param name="house">The target house.</param>
	/// <param name="subtype">The selected subtype.</param>
	/// <returns>The final result generated.</returns>
	private JustOneCellPuzzle GenerateForLine(House house, SingleTechniqueSubtype subtype)
	{
		// Adjust if values are invalid.
		house = (subtype.GetRelatedTechnique(), house) switch
		{
			(Technique.CrosshatchingRow, >= 18) => house - 9,
			(Technique.CrosshatchingColumn, < 18) => house + 9,
			_ => house
		};

		var cellsInHouse = HousesMap[house];
		while (true)
		{
			var (excluderBlocks, excluderLines) = (0, 0);
			var (blocks, lines) = ((HouseMask)cellsInHouse.BlockMask, house < 18 ? cellsInHouse.ColumnMask << 18 : cellsInHouse.RowMask << 9);
			for (var i = 0; i < subtype.GetExcludersCount(HouseType.Block); i++)
			{
				House excluderHouse;
				do
				{
					excluderHouse = Rng.Next(0, 9);
				} while ((blocks >> excluderHouse & 1) == 0);
				_ = (excluderBlocks |= 1 << excluderHouse, blocks &= ~(1 << excluderHouse));
			}
			for (var i = 0; i < subtype.GetExcludersCount(house < 18 ? HouseType.Column : HouseType.Row); i++)
			{
				House excluderHouse;
				do
				{
					excluderHouse = house < 18 ? Rng.Next(18, 27) : Rng.Next(9, 18);
				} while ((lines >> excluderHouse & 1) == 0);
				_ = (excluderLines |= 1 << excluderHouse, lines &= ~(1 << excluderHouse));
			}
			var excluders = (CellMap)[];
			foreach (var r in excluderBlocks)
			{
				var lastCellsAvailable = HousesMap[r] - cellsInHouse - excluders.ExpandedPeers;
				excluders.Add(lastCellsAvailable[Rng.Next(0, lastCellsAvailable.Count)]);
			}
			foreach (var c in excluderLines)
			{
				var lastCellsAvailable = HousesMap[c] - cellsInHouse - excluders.ExpandedPeers;
				excluders.Add(lastCellsAvailable[Rng.Next(0, lastCellsAvailable.Count)]);
			}
			if (!excluders)
			{
				// Try again if no excluders found.
				continue;
			}

			// Now checks for uncovered cells in the target house, one of the uncovered cells is the target cell,
			// and the others should be placeholders.
			ShuffleSequence(DigitSeed);
			var targetDigit = DigitSeed[Rng.Next(0, 9)];
			var puzzle = Grid.Empty;
			var uncoveredCells = cellsInHouse - excluders.ExpandedPeers;
			var targetCell = uncoveredCells[Rng.Next(0, uncoveredCells.Count)];
			var tempIndex = 0;
			foreach (var placeholderCell in uncoveredCells - targetCell)
			{
				if (DigitSeed[tempIndex] == targetDigit)
				{
					tempIndex++;
				}

				puzzle.SetDigit(placeholderCell, DigitSeed[tempIndex]);
				puzzle.SetState(placeholderCell, CellState.Given);
				tempIndex++;
			}
			if (!AllowsBlockExcluders)
			{
				var emptyCellsRelatedBlocksContainAnyExcluder = false;
				foreach (var block in (HousesMap[house] - puzzle.GivenCells).BlockMask)
				{
					if (HousesMap[block] & excluders)
					{
						emptyCellsRelatedBlocksContainAnyExcluder = true;
						break;
					}
				}
				if (emptyCellsRelatedBlocksContainAnyExcluder)
				{
					// Invalid case. Try again.
					continue;
				}
			}
			foreach (var excluder in excluders)
			{
				puzzle.SetDigit(excluder, targetDigit);
				puzzle.SetState(excluder, CellState.Given);
			}

			// We should adjust the givens and target cells to the specified position.
			switch (Alignment)
			{
				case GridAlignment.CenterHouses or GridAlignment.CenterBlock when targetCell.ToHouseIndex(HouseType.Block) is var b && b != 4:
				{
					adjustToBlock5(b, ref house, ref targetCell, ref puzzle);
					break;
				}
				case GridAlignment.CenterCell when targetCell != 40:
				{
					adjustToBlock5(targetCell.ToHouseIndex(HouseType.Block), ref house, ref targetCell, ref puzzle);
					adjustToCenterCell(ref house, ref targetCell, ref puzzle);
					break;
				}
				default:
				{
					break;
				}
			}

			return new JustOneCellPuzzleSuccessful(
				in puzzle,
				targetCell,
				targetDigit,
				new HiddenSingleStep(null!, null, null!, targetCell, targetDigit, house, false, subtype)
			);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static void adjustToBlock5(House cellHouse, scoped ref House house, scoped ref Cell targetCell, scoped ref Grid puzzle)
		{
			if (cellHouse == 4)
			{
				return;
			}

			var (c1s, c1e, c2s, c2e) = cellHouse switch
			{
				0 => (0, 1, 3, 4),
				1 => (0, 1, -1, -1),
				2 => (0, 1, 4, 5),
				3 => (3, 4, -1, -1),
				5 => (4, 5, -1, -1),
				6 => (1, 2, 3, 4),
				7 => (1, 2, -1, -1),
				8 => (1, 2, 4, 5)
			};

			puzzle.SwapChute(c1s, c1e);
			if (c2s != -1)
			{
				puzzle.SwapChute(c2s, c2e);
			}

			var isRow = house < 18;
			var blockPos = BlockPositionOf(targetCell);
			house = blockPos switch
			{
				0 => house + 3,
				1 => isRow ? house + 3 : house,
				2 => isRow ? house + 3 : house - 3,
				3 => isRow ? house : house + 3,
				4 => house,
				5 => isRow ? house : house - 3,
				6 => isRow ? house - 3 : house + 3,
				7 => isRow ? house - 3 : house,
				8 => house - 3
			};
			targetCell = HousesCells[4][blockPos];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static void adjustToCenterCell(scoped ref House house, scoped ref Cell targetCell, scoped ref Grid puzzle)
		{
			var pos = BlockPositionOf(targetCell);
			if (targetCell.ToHouseIndex(HouseType.Block) == 4 && pos == 4)
			{
				return;
			}

			var (c1s, c1e, c2s, c2e) = pos switch
			{
				0 => (12, 13, 21, 22),
				1 => (12, 13, -1, -1),
				2 => (12, 13, 22, 23),
				3 => (21, 22, -1, -1),
				5 => (22, 23, -1, -1),
				6 => (13, 14, 21, 22),
				7 => (13, 14, -1, -1),
				8 => (13, 14, 22, 23)
			};

			puzzle.SwapTwoHouses(c1s, c1e);
			if (c2s != -1)
			{
				puzzle.SwapTwoHouses(c2s, c2e);
			}

			var isRow = house < 18;
			targetCell = 40;
			house = pos switch
			{
				0 => house + 1,
				1 => isRow ? house + 1 : house,
				2 => isRow ? house + 1 : house - 1,
				3 => isRow ? house : house + 1,
				4 => house,
				5 => isRow ? house : house - 1,
				6 => isRow ? house - 1 : house + 1,
				7 => isRow ? house - 1 : house,
				8 => house - 1
			};
		}
	}
}
