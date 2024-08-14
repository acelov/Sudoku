namespace Sudoku.Analytics.StepSearchers;

using unsafe SearcherSubtypeCheckerFuncPtr = delegate*<
	List<UniqueLoopStep>,
	ref readonly Grid,
	ref StepAnalysisContext,
	Digit,
	Digit,
	ref readonly CellMap,
	ref readonly CellMap,
	Mask,
	Cell[],
	UniqueLoopStep?
>;

/// <summary>
/// Provides with a <b>Unique Loop</b> step searcher.
/// The step searcher will include the following techniques:
/// <list type="bullet">
/// <item>Unique Loop Type 1</item>
/// <item>Unique Loop Type 2</item>
/// <item>Unique Loop Type 3</item>
/// <item>Unique Loop Type 4</item>
/// <item>Unique Loop Strong Link Type</item>
/// </list>
/// </summary>
[StepSearcher(
	"StepSearcherName_UniqueLoopStepSearcher",
	Technique.UniqueLoopType1, Technique.UniqueLoopType2, Technique.UniqueLoopType3, Technique.UniqueLoopType4,
	Technique.UniqueLoopStrongLinkType,
	SupportedSudokuTypes = SudokuType.Standard,
	RuntimeFlags = StepSearcherRuntimeFlags.TimeComplexity,
	SupportAnalyzingMultipleSolutionsPuzzle = false)]
public sealed partial class UniqueLoopStepSearcher : StepSearcher
{
	/// <summary>
	/// Indicates the type checkers.
	/// </summary>
	private static readonly unsafe SearcherSubtypeCheckerFuncPtr[] TypeCheckers = [
		&CheckType1,
		&CheckType2,
		&CheckType3,
		&CheckType4,
		&CheckStrongLinkType
	];


	/// <inheritdoc/>
	protected internal override unsafe Step? Collect(ref StepAnalysisContext context)
	{
		// Now iterate on each bi-value cells as the start cell to get all possible unique loops,
		// making it the start point to execute the recursion.
		ref readonly var grid = ref context.Grid;
		var tempAccumulator = new List<UniqueLoopStep>();
		foreach (ref readonly var pattern in FindLoops(in grid))
		{
			var (loop, path, comparer) = pattern;
			if ((loop & ~BivalueCells) is not (var extraCellsMap and not []))
			{
				// The current puzzle has multiple solutions.
				throw new PuzzleInvalidException(in grid, typeof(UniqueLoopStepSearcher));
			}

			var d1 = Mask.TrailingZeroCount(comparer);
			var d2 = comparer.GetNextSet(d1);
			for (var i = 0; i < TypeCheckers.Length; i++)
			{
				if (TypeCheckers[i](tempAccumulator, in grid, ref context, d1, d2, in loop, in extraCellsMap, comparer, path) is { } step)
				{
					return step;
				}
			}
		}
		if (tempAccumulator.Count == 0)
		{
			return null;
		}

		// Maybe it is unnecessary because loop pattern has already been filtered.
		var resultList = StepMarshal.RemoveDuplicateItems(tempAccumulator).ToList();
		StepMarshal.SortItems(resultList);

		if (context.OnlyFindOne)
		{
			return resultList[0];
		}

		context.Accumulator.AddRange(resultList);
		return null;
	}

	/// <summary>
	/// Try to find all possible loops appeared in a grid.
	/// </summary>
	/// <param name="grid">The grid to be used.</param>
	/// <returns>A list of <see cref="UniqueLoop"/> instances.</returns>
	private ReadOnlySpan<UniqueLoop> FindLoops(ref readonly Grid grid)
	{
		var result = new HashSet<UniqueLoop>();
		foreach (var cell in BivalueCells)
		{
			var queue = LinkedList.Singleton(LinkedList.Singleton(cell));
			var comparer = grid.GetCandidates(cell);
			var d1 = Mask.TrailingZeroCount(comparer);
			var d2 = comparer.GetNextSet(d1);
			var pairMap = CandidatesMap[d1] & CandidatesMap[d2];
			while (queue.Count != 0)
			{
				var currentBranch = queue.RemoveFirstNode();

				// The node should be appended after the last node, and its start node is the first node of the linked list.
				var previousCell = currentBranch.LastValue();
				foreach (var currentCell in PeersMap[previousCell] & pairMap)
				{
					// Determine whether the current cell iterated is the first node.
					// If so, check whether the loop is of length greater than 6, and validity of the loop.
					if (currentCell == cell && currentBranch.Count is 6 or 8 or 10 or 12 or 14 && UniqueLoop.IsValid(currentBranch))
					{
						result.Add(new(currentBranch.AsCellMap(), [.. currentBranch], comparer));
						break;
					}

					if (!currentBranch.Contains(currentCell) && currentBranch.Count < 14
						&& (hasBivalueCell(previousCell, currentCell) || hasConjugatePair(previousCell, currentCell, d1, d2)))
					{
						// Create a new link with original value, and a new value at the last position.
						queue.AddLast(LinkedList.Create(currentBranch, currentCell));
					}
				}
			}
		}
		return result.ToArray();


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static bool hasBivalueCell(Cell previous, Cell current) => BivalueCells.Contains(previous) || BivalueCells.Contains(current);

		static bool hasConjugatePair(Cell previous, Cell current, Digit d1, Digit d2)
		{
			var twoCellsMap = previous.AsCellMap() + current;
			foreach (var house in twoCellsMap.SharedHouses)
			{
				if ((HousesMap[house] & CandidatesMap[d1]) == twoCellsMap || (HousesMap[house] & CandidatesMap[d2]) == twoCellsMap)
				{
					return true;
				}
			}
			return false;
		}
	}


	/// <summary>
	/// Gets all possible links of the loop.
	/// </summary>
	/// <param name="path">The loop, specified as its path.</param>
	/// <returns>A list of <see cref="CellLinkViewNode"/> instances.</returns>
	private static ReadOnlySpan<CellLinkViewNode> GetLoopLinks(Cell[] path)
	{
		var result = new List<CellLinkViewNode>();
		for (var i = 0; i < path.Length; i++)
		{
			result.Add(new(ColorIdentifier.Normal, path[i], path[i + 1 == path.Length ? 0 : i + 1]));
		}
		return result.AsReadOnlySpan();
	}


	private static partial UniqueLoopStep? CheckType1(List<UniqueLoopStep> accumulator, ref readonly Grid grid, ref StepAnalysisContext context, Digit d1, Digit d2, ref readonly CellMap loop, ref readonly CellMap extraCellsMap, Mask comparer, Cell[] path);
	private static partial UniqueLoopStep? CheckType2(List<UniqueLoopStep> accumulator, ref readonly Grid grid, ref StepAnalysisContext context, Digit d1, Digit d2, ref readonly CellMap loop, ref readonly CellMap extraCellsMap, Mask comparer, Cell[] path);
	private static partial UniqueLoopStep? CheckType3(List<UniqueLoopStep> accumulator, ref readonly Grid grid, ref StepAnalysisContext context, Digit d1, Digit d2, ref readonly CellMap loop, ref readonly CellMap extraCellsMap, Mask comparer, Cell[] path);
	private static partial UniqueLoopStep? CheckType4(List<UniqueLoopStep> accumulator, ref readonly Grid grid, ref StepAnalysisContext context, Digit d1, Digit d2, ref readonly CellMap loop, ref readonly CellMap extraCellsMap, Mask comparer, Cell[] path);
	private static partial UniqueLoopStep? CheckStrongLinkType(List<UniqueLoopStep> accumulator, ref readonly Grid grid, ref StepAnalysisContext context, Digit d1, Digit d2, ref readonly CellMap loop, ref readonly CellMap extraCellsMap, Mask comparer, Cell[] path);
}
