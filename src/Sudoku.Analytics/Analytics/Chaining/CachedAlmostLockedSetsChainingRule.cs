namespace Sudoku.Analytics.Chaining;

/// <summary>
/// Represents a chaining rule on ALS rule (i.e. <see cref="LinkType.AlmostLockedSet"/>).
/// </summary>
/// <seealso cref="LinkType.AlmostLockedSet"/>
internal sealed class CachedAlmostLockedSetsChainingRule : ChainingRule
{
	/// <inheritdoc/>
	internal override void CollectLinks(ref readonly Grid grid, LinkDictionary strongLinks, LinkDictionary weakLinks)
	{
		var alses = AlmostLockedSetsModule.CollectAlmostLockedSets(in grid);

		// Strong.
		var maskTempList = (stackalloc Mask[81]);
		foreach (var als in alses)
		{
			if (als is not (_, var cells) { IsBivalueCell: false, StrongLinks: var links, House: var house })
			{
				// This ALS is special case - it only uses 2 digits in a cell.
				// This will be handled as a normal bi-value strong link (Y rule).
				continue;
			}

			// Avoid the ALS chosen contains a sub-subset, meaning some cells held by ALS forms a subset.
			// If so, the ALS can be reduced.
			var isAlsCanBeReduced = false;
			maskTempList.Clear();
			foreach (var cell in cells)
			{
				maskTempList[cell] = grid.GetCandidates(cell);
			}
			foreach (var subsetCells in cells | cells.Count - 1)
			{
				var mask = (Mask)0;
				foreach (var cell in subsetCells)
				{
					mask |= maskTempList[cell];
				}

				if (PopCount((uint)mask) == subsetCells.Count)
				{
					isAlsCanBeReduced = true;
					break;
				}
			}
			if (isAlsCanBeReduced)
			{
				continue;
			}

			foreach (var digitsPair in links)
			{
				var node1ExtraMap = CandidateMap.Empty;
				foreach (var cell in cells)
				{
					node1ExtraMap.AddRange(from digit in grid.GetCandidates(cell) select cell * 9 + digit);
				}
				var node2ExtraMap = CandidateMap.Empty;
				foreach (var cell in cells)
				{
					node2ExtraMap.AddRange(from digit in grid.GetCandidates(cell) select cell * 9 + digit);
				}

				var digit1 = TrailingZeroCount(digitsPair);
				var digit2 = digitsPair.GetNextSet(digit1);
				var node1Cells = HousesMap[house] & cells & CandidatesMap[digit1];
				var node2Cells = HousesMap[house] & cells & CandidatesMap[digit2];
				var node1 = new Node(Subview.ExpandedCellFromDigit(in node1Cells, digit1), false, true);
				var node2 = new Node(Subview.ExpandedCellFromDigit(in node2Cells, digit2), true, true);
				strongLinks.AddEntry(node1, node2, true, als);
			}
		}

		// Weak.
		foreach (var als in alses)
		{
			if (als is not (var digitsMask, var cells) { IsBivalueCell: false, House: var house })
			{
				continue;
			}

			foreach (var digit in digitsMask)
			{
				var cells1 = HousesMap[house] & cells;
				var possibleCells2 = HousesMap[house] & CandidatesMap[digit] & ~cells;
				if (!possibleCells2)
				{
					// Cannot link to the other node.
					continue;
				}

				var node1 = new Node(Subview.ExpandedCellFromDigit(in cells1, digit), true, true);
				foreach (ref readonly var cells2 in possibleCells2 | 3)
				{
					if (!cells2.IsInIntersection)
					{
						continue;
					}

					var node2 = new Node(Subview.ExpandedCellFromDigit(in cells2, digit), false, true);
					weakLinks.AddEntry(node1, node2, false, als);
				}
			}
		}
	}

	/// <inheritdoc/>
	internal override void CollectExtraViewNodes(ref readonly Grid grid, ChainPattern pattern, ref View[] views)
	{
		var (alsIndex, view) = (0, views[0]);
		foreach (var link in pattern.Links)
		{
			if (link.GroupedLinkPattern is not AlmostLockedSet { Cells: var cells })
			{
				continue;
			}

			var linkMap = link.FirstNode.Map | link.SecondNode.Map;
			var id = (ColorIdentifier)(alsIndex + WellKnownColorIdentifierKind.AlmostLockedSet1);
			foreach (var cell in cells)
			{
				view.Add(new CellViewNode(id, cell));
				foreach (var digit in grid.GetCandidates(cell))
				{
					var candidate = cell * 9 + digit;
					if (!linkMap.Contains(candidate))
					{
						view.Add(new CandidateViewNode(id, cell * 9 + digit));
					}
				}
			}

			alsIndex = (alsIndex + 1) % 5;
		}
	}

	/// <inheritdoc/>
	internal override ConclusionSet CollectLoopConclusions(Loop loop, ref readonly Grid grid)
	{
		// An example with 19 eliminations:
		// .2.1...7...5..31..6.+1..7..8+2....59..5.3.1...2+1.93.+2.5..1...6...9..2.......2.4...7:821 448 648 848 449 649 388

		// A valid ALS can be eliminated as a real naked subset.
		var result = ConclusionSet.Empty;
		foreach (var element in loop.Links)
		{
			if (element is
				{
					IsStrong: true,
					FirstNode.Map.Digits: var digitsMask1,
					SecondNode.Map.Digits: var digitsMask2,
					GroupedLinkPattern: AlmostLockedSet(var digitsMask, var alsCells)
				})
			{
				var elimDigitsMask = (Mask)(digitsMask & (Mask)~(Mask)(digitsMask1 | digitsMask2));
				foreach (var digit in elimDigitsMask)
				{
					foreach (var cell in alsCells % CandidatesMap[digit])
					{
						result.Add(new Conclusion(Elimination, cell, digit));
					}
				}
			}
		}
		return result;
	}
}
