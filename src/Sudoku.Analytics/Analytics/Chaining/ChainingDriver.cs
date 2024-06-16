namespace Sudoku.Analytics.Chaining;

/// <summary>
/// Provides a driver module on chaining.
/// </summary>
internal static class ChainingDriver
{
	/// <summary>
	/// Collect all <see cref="ChainOrLoop"/> instances appears in a grid.
	/// </summary>
	/// <param name="grid">The grid.</param>
	/// <param name="rules">
	/// Indicates the rule instances that will create strong and weak links by their own represented concept.
	/// </param>
	/// <returns>All possible <see cref="ChainOrLoop"/> instances.</returns>
	/// <remarks>
	/// <include file="../../global-doc-comments.xml" path="/g/developer-notes" />
	/// A valid chain can only belong to the following three cases:
	/// <list type="number">
	/// <item>
	/// <para><b>Discontinuous Nice Loop</b></para>
	/// <para>
	/// Start with weak link, alternating strong and weak links and return to itself of a weak link
	/// (with an even number of nodes, odd number of links).
	/// </para>
	/// </item>
	/// <item>
	/// <para><b>Discontinuous Nice Loop</b></para>
	/// <para>
	/// Start with strong link, alternating strong and weak links and return to itself of a strong link
	/// (with an even number of nodes, even number of links).
	/// </para>
	/// </item>
	/// <item>
	/// <para><b>Continuous Nice Loop</b></para>
	/// <para>
	/// Start with strong link, alternating strong and weak links and return to itself of a weak link
	/// (with an even number of nodes, even number of links).
	/// </para>
	/// </item>
	/// </list>
	/// </remarks>
	public static ReadOnlySpan<ChainOrLoop> CollectChainPatterns(ref readonly Grid grid, ReadOnlySpan<ChainingRule> rules)
	{
		// Step 1: Collect for all strong and weak links appeared in the grid.
		var (strongLinks, weakLinks) = (new LinkDictionary(), new LinkDictionary());
		foreach (var chainingRule in rules)
		{
			chainingRule.CollectLinks(in grid, strongLinks, weakLinks, LinkOption.House, LinkOption.House);
		}

		// Step 2: Iterate on dictionary to get chains.
		var foundPatterns = new HashSet<ChainOrLoop>(LocalComparer.ChainPatternComparer);
		foreach (var cell in EmptyCells)
		{
			foreach (var digit in (Mask)(grid.GetCandidates(cell) & (Mask)~(1 << Solution.GetDigit(cell))))
			{
				var node = new Node(cell, digit, true, false);
				bfs(node, foundPatterns);
				bfs(~node, foundPatterns);
			}
		}

		// Step 3: Check eliminations. If a chain doesn't contain any possible conclusions,
		// it will be removed from the result collection.
		var finalCollection = new List<ChainOrLoop>();
		foreach (var pattern in foundPatterns)
		{
			if (pattern.GetConclusions(in grid))
			{
				finalCollection.Add(pattern);
			}
		}

		// Step 4: Sort found patterns and return.
		switch (finalCollection.Count)
		{
			case 0:
			{
				return [];
			}
			case > 1:
			{
				finalCollection.Sort();
				goto default;
			}
			default:
			{
				return finalCollection.AsReadOnlySpan();
			}
		}


		void bfs(Node startNode, HashSet<ChainOrLoop> result)
		{
			var pendingStrong = new LinkedList<Node>();
			var pendingWeak = new LinkedList<Node>();
			(startNode.IsOn ? pendingWeak : pendingStrong).AddLast(startNode);

			var visitedStrong = new HashSet<Node>(LocalComparer.NodeMapComparer);
			var visitedWeak = new HashSet<Node>(LocalComparer.NodeMapComparer);
			_ = (visitedStrong.Add(startNode), visitedWeak.Add(startNode));

			while (pendingStrong.Count != 0 || pendingWeak.Count != 0)
			{
				while (pendingStrong.Count != 0)
				{
					var currentNode = pendingStrong.First!.Value;
					pendingStrong.RemoveFirst();
					if (weakLinks.TryGetValue(currentNode, out var nodes))
					{
						foreach (var node in nodes)
						{
							var resultNode = new Node(node, currentNode);

							////////////////////////////////////////////
							// Continuous Nice Loop 3) Strong -> Weak //
							////////////////////////////////////////////
							if (node == startNode && resultNode.AncestorsLength >= 4)
							{
								result.Add(new Loop(resultNode, strongLinks, weakLinks));
							}

							/////////////////////////////////////////////////
							// Discontinuous Nice Loop 2) Strong -> Strong //
							/////////////////////////////////////////////////
							if (node == ~startNode)
							{
								result.Add(new Chain(resultNode, strongLinks, weakLinks));
							}

							// This step will filter duplicate nodes in order not to make a internal loop on chains.
							if (!node.IsAncestorOf(currentNode, NodeComparison.IncludeIsOn) && visitedWeak.Add(node))
							{
								pendingWeak.AddLast(resultNode);
							}
						}
					}
				}
				while (pendingWeak.Count != 0)
				{
					var currentNode = pendingWeak.First!.Value;
					pendingWeak.RemoveFirst();
					if (strongLinks.TryGetValue(currentNode, out var nodes))
					{
						foreach (var node in nodes)
						{
							var resultNode = new Node(node, currentNode);

							/////////////////////////////////////////////
							// Discontinuous Nice Loop 1) Weak -> Weak //
							/////////////////////////////////////////////
							if (node == ~startNode)
							{
								result.Add(new Chain(resultNode, strongLinks, weakLinks));
							}

							if (!node.IsAncestorOf(currentNode, NodeComparison.IncludeIsOn) && visitedStrong.Add(node))
							{
								pendingStrong.AddLast(resultNode);
							}
						}
					}
				}
			}
		}
	}

	/// <summary>
	/// Collect all multiple forcing chain instances appears in a grid.
	/// </summary>
	/// <param name="grid">The grid.</param>
	/// <param name="rules">
	/// Indicates the rule instances that will create strong and weak links by their own represented concept.
	/// </param>
	/// <returns>All possible multiple forcing chain instances.</returns>
	public static ReadOnlySpan<MultipleForcingChains> CollectMultipleChainPatterns(ref readonly Grid grid, ReadOnlySpan<ChainingRule> rules)
	{
		// Step 1: Collect for all strong and weak links appeared in the grid.
		var (strongLinks, weakLinks) = (new LinkDictionary(), new LinkDictionary());
		foreach (var chainingRule in rules)
		{
			chainingRule.CollectLinks(in grid, strongLinks, weakLinks, LinkOption.House, LinkOption.House);
		}

		// Step 2: Iterate on dictionary to get all forcing chains.
		var foundPatterns = new HashSet<MultipleForcingChains>(LocalComparer.MultipleForcingChainsPatternComparer);
		foreach (var cell in EmptyCells & ~BivalueCells)
		{
			var digitsMask = grid.GetCandidates(cell);
			var digitToStrong = new Dictionary<Candidate, HashSet<Node>>();
			var digitToWeak = new Dictionary<Candidate, HashSet<Node>>();
			var cellToStrong = default(HashSet<Node>);
			var cellToWeak = default(HashSet<Node>);
			foreach (var digit in digitsMask)
			{
				bfs(new(cell, digit, true, false), out var onToStrong, out var onToWeak);

				// Iterate on three house types, to collect with region forcing chains.
				foreach (var houseType in HouseTypes)
				{
					var house = cell.ToHouseIndex(houseType);
					var cellsInHouse = HousesMap[house] & CandidatesMap[digit];
					var firstCellInHouse = HousesCells[house][0];
					if (cellsInHouse.Count <= 2)
					{
						// There's no need iterating on such house because the chain only contains 2 branches,
						// which means it can be combined into one normal chain.
						continue;
					}

					if (firstCellInHouse != cell)
					{
						// We should skip the other cells in the house, in order to avoid duplicate forcing chains.
						continue;
					}

					var houseCellToStrong = new Dictionary<Candidate, HashSet<Node>>();
					var houseCellToWeak = new Dictionary<Candidate, HashSet<Node>>();
					var regionToStrong = new HashSet<Node>(LocalComparer.NodeMapComparer);
					var regionToWeak = new HashSet<Node>(LocalComparer.NodeMapComparer);
					foreach (var otherCell in cellsInHouse)
					{
						if (otherCell == cell)
						{
							houseCellToStrong.Add(otherCell * 9 + digit, onToStrong);
							houseCellToWeak.Add(otherCell * 9 + digit, onToWeak);
							regionToStrong.UnionWith(onToStrong);
							regionToWeak.UnionWith(onToWeak);
						}
						else
						{
							var other = new Node(otherCell, digit, true, false);
							bfs(other, out var otherToStrong, out var otherToWeak);
							houseCellToStrong.Add(otherCell * 9 + digit, otherToStrong);
							houseCellToWeak.Add(otherCell * 9 + digit, otherToWeak);
							regionToStrong.IntersectWith(otherToStrong);
							regionToWeak.IntersectWith(otherToWeak);
						}
					}

					////////////////////////////////////////
					// Collect with region forcing chains //
					////////////////////////////////////////
					foreach (var node in regionToStrong)
					{
						var conclusion = new Conclusion(node.IsOn ? Assignment : Elimination, node.Map[0]);
						var regionForcingChains = new MultipleForcingChains(conclusion);
						foreach (var c in cellsInHouse)
						{
							var branchNode = houseCellToStrong[c * 9 + digit].First(n => n.Equals(node, NodeComparison.IncludeIsOn));
							var chain = new WeakChain(branchNode, strongLinks, weakLinks);
							regionForcingChains.Add(c * 9 + digit, chain);
						}
						foundPatterns.Add(regionForcingChains);
					}
					foreach (var node in regionToWeak)
					{
						var conclusion = new Conclusion(node.IsOn ? Assignment : Elimination, node.Map[0]);
						var regionForcingChains = new MultipleForcingChains(conclusion);
						foreach (var c in cellsInHouse)
						{
							var branchNode = houseCellToWeak[c * 9 + digit].First(n => n.Equals(node, NodeComparison.IncludeIsOn));
							var chain = new WeakChain(branchNode, strongLinks, weakLinks);
							regionForcingChains.Add(c * 9 + digit, chain);
						}
						foundPatterns.Add(regionForcingChains);
					}
				}

				digitToStrong.Add(cell * 9 + digit, onToStrong);
				digitToWeak.Add(cell * 9 + digit, onToWeak);
				if (cellToStrong is null)
				{
					cellToStrong = new(LocalComparer.NodeMapComparer);
					cellToWeak = new(LocalComparer.NodeMapComparer);
					cellToStrong.UnionWith(onToStrong);
					cellToWeak.UnionWith(onToWeak);
				}
				else
				{
					Debug.Assert(cellToWeak is not null);
					cellToStrong.IntersectWith(onToStrong);
					cellToWeak.IntersectWith(onToWeak);
				}
			}

			//////////////////////////////////////
			// Collect with cell forcing chains //
			//////////////////////////////////////
			foreach (var node in cellToStrong ?? [])
			{
				var conclusion = new Conclusion(node.IsOn ? Assignment : Elimination, node.Map[0]);
				var cellForcingChains = new MultipleForcingChains(conclusion);
				foreach (var d in digitsMask)
				{
					var branchNode = digitToStrong[cell * 9 + d].First(n => n.Equals(node, NodeComparison.IncludeIsOn));
					var chain = new WeakChain(branchNode, strongLinks, weakLinks);
					cellForcingChains.Add(cell * 9 + d, chain);
				}
				foundPatterns.Add(cellForcingChains);
			}
			foreach (var node in cellToWeak ?? [])
			{
				var conclusion = new Conclusion(node.IsOn ? Assignment : Elimination, node.Map[0]);
				var cellForcingChains = new MultipleForcingChains(conclusion);
				foreach (var d in digitsMask)
				{
					var branchNode = digitToWeak[cell * 9 + d].First(n => n.Equals(node, NodeComparison.IncludeIsOn));
					var chain = new WeakChain(branchNode, strongLinks, weakLinks);
					cellForcingChains.Add(cell * 9 + d, chain);
				}
				foundPatterns.Add(cellForcingChains);
			}
		}

		// Step 3: Return the values.
		return foundPatterns.ToArray();


		void bfs(Node startNode, out HashSet<Node> toStrong, out HashSet<Node> toWeak)
		{
			var (pendingStrong, pendingWeak) = (new LinkedList<Node>(), new LinkedList<Node>());
			(startNode.IsOn ? pendingStrong : pendingWeak).AddLast(startNode);
			(toStrong, toWeak) = (new(LocalComparer.NodeMapComparer), new(LocalComparer.NodeMapComparer));

			while (pendingStrong.Count != 0 || pendingWeak.Count != 0)
			{
				if (pendingStrong.Count != 0)
				{
					var currentNode = pendingStrong.First!.Value;
					pendingStrong.RemoveFirst();
					if (weakLinks.TryGetValue(currentNode, out var nodes))
					{
						foreach (var node in nodes)
						{
							var resultNode = new Node(node, currentNode);
							if (toStrong.Contains(~resultNode))
							{
								// Contradiction is found.
								return;
							}

							if (toWeak.Add(resultNode))
							{
								pendingWeak.AddLast(resultNode);
							}
						}
					}
				}
				else
				{
					var currentNode = pendingWeak.First!.Value;
					pendingWeak.RemoveFirst();
					if (strongLinks.TryGetValue(currentNode, out var nodes))
					{
						foreach (var node in nodes)
						{
							var resultNode = new Node(node, currentNode);
							if (toWeak.Contains(~resultNode))
							{
								// Contradiction is found.
								return;
							}

							if (toStrong.Add(resultNode))
							{
								pendingStrong.AddLast(resultNode);
							}
						}
					}
				}
			}
		}
	}
}

/// <summary>
/// The file-local comparer generator, lazily initialized.
/// </summary>
file static class LocalComparer
{
	/// <summary>
	/// Indicates the backing field of chain pattern comparer instance.
	/// </summary>
	private static IEqualityComparer<ChainOrLoop>? _chainPatternComparer;

	/// <summary>
	/// Indicates the backing field of node map comparer instance.
	/// </summary>
	private static IEqualityComparer<Node>? _nodeComparer;

	/// <summary>
	/// Indicates the backing field of multiple forcing chains comparer instance.
	/// </summary>
	private static IEqualityComparer<MultipleForcingChains>? _multipleForcingChainsPatternComparer;


	/// <summary>
	/// Creates an instance of type <see cref="EqualityComparer{T}"/> of <see cref="ChainOrLoop"/> on equality comparison
	/// in order to filter duplicate chains.
	/// </summary>
	public static IEqualityComparer<ChainOrLoop> ChainPatternComparer
		=> _chainPatternComparer ??= EqualityComparer<ChainOrLoop>.Create(
			static (left, right) => (left, right) switch
			{
				(null, null) => true,
				(Chain a, Chain b) => a.Equals(b),
				(Loop a, Loop b) => a.Equals(b),
				(not null, not null) => left.Equals(right, NodeComparison.IgnoreIsOn, ChainOrLoopComparison.Undirected),
				_ => false
			},
			static obj => obj switch
			{
				Chain c => c.GetHashCode(),
				Loop l => l.GetHashCode(),
				_ => obj.GetHashCode(NodeComparison.IgnoreIsOn, ChainOrLoopComparison.Undirected)
			}
		);

	/// <summary>
	/// Creates an instance of type <see cref="EqualityComparer{T}"/> of <see cref="MultipleForcingChains"/> on equality comparison
	/// in order to filter duplicate multiple forcing chains.
	/// </summary>
	public static IEqualityComparer<MultipleForcingChains> MultipleForcingChainsPatternComparer
		=> _multipleForcingChainsPatternComparer ??= EqualityComparer<MultipleForcingChains>.Create(
			static (left, right) => (left, right) switch
			{
				(null, null) => true,
				(not null, not null) => left.Equals(right, NodeComparison.IgnoreIsOn, ChainOrLoopComparison.Undirected),
				_ => false
			},
			static obj => obj.GetHashCode(NodeComparison.IgnoreIsOn, ChainOrLoopComparison.Undirected)
		);

	/// <summary>
	/// Creates an instance of type <see cref="EqualityComparer{T}"/> of <see cref="Node"/> on equality comparison
	/// in order to filter duplicate nodes on its containing map and "is on" property,
	/// guaranteeing same nodes won't be traversed multiple times.
	/// </summary>
	public static IEqualityComparer<Node> NodeComparer
		=> EqualityComparer<Node>.Create(
			static (left, right) => (left, right) switch
			{
				(not null, not null) => left == right,
				(null, null) => true,
				_ => false
			},
			static obj => obj.GetHashCode()
		);

	/// <summary>
	/// Creates an instance of type <see cref="EqualityComparer{T}"/> of <see cref="Node"/> on equality comparison
	/// in order to filter duplicate nodes on its containing map, guaranteeing same nodes won't be traversed multiple times.
	/// </summary>
	public static IEqualityComparer<Node> NodeMapComparer
		=> _nodeComparer ??= EqualityComparer<Node>.Create(
			static (left, right) => (left, right) switch
			{
				(not null, not null) => left.Equals(right, NodeComparison.IgnoreIsOn),
				(null, null) => true,
				_ => false
			},
			static obj => obj.GetHashCode(NodeComparison.IgnoreIsOn)
		);
}
