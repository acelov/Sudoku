﻿namespace Sudoku.Solving.Manual.Searchers;

/// <summary>
/// Defines a step searcher that searches for alternating inference chain steps.
/// </summary>
public interface IAlternatingInferenceChainStepSearcher : IChainStepSearcher
{
	/// <summary>
	/// Indicates the maximum capacity used for the allocation on shared memory.
	/// </summary>
	public abstract int MaxCapacity { get; set; }

	/// <summary>
	/// <para>
	/// Indicates the extended nodes to be searched for. Please note that the type of the property
	/// is an enumeration type with bit-fields attribute, which means you can add multiple choices
	/// into the value.
	/// </para>
	/// <para>
	/// You can set the value as a bit-field mask to define your own types to be searched for, where:
	/// <list type="table">
	/// <listheader>
	/// <term>Field name</term>
	/// <description>Description (What kind of nodes can be searched)</description>
	/// </listheader>
	/// <item>
	/// <term><see cref="SearcherNodeTypes.SoleDigit"/></term>
	/// <description>
	/// The strong and weak inferences between 2 sole candidate nodes of a same digit
	/// (i.e. X-Chain).
	/// </description>
	/// </item>
	/// <item>
	/// <term><see cref="SearcherNodeTypes.SoleCell"/></term>
	/// <description>
	/// The strong and weak inferences between 2 sole candidate nodes of a same cell
	/// (i.e. Y-Chain).
	/// </description>
	/// </item>
	/// <item>
	/// <term><see cref="SearcherNodeTypes.LockedCandidates"/></term>
	/// <description>
	/// The strong and weak inferences between 2 nodes, where at least one node is a locked candidates node.
	/// </description>
	/// </item>
	/// <item>
	/// <term><see cref="SearcherNodeTypes.LockedSet"/></term>
	/// <description>
	/// The strong inferences between 2 nodes, where at least one node is an almost locked set node.
	/// </description>
	/// </item>
	/// <item>
	/// <term><see cref="SearcherNodeTypes.HiddenSet"/></term>
	/// <description>
	/// The weak inferences between 2 nodes, where at least one node is an almost hidden set node.
	/// </description>
	/// </item>
	/// <item>
	/// <term><see cref="SearcherNodeTypes.UniqueRectangle"/></term>
	/// <description>
	/// The strong and weak inferences between 2 nodes, where at least one node
	/// is an almost unique rectangle node.
	/// </description>
	/// </item>
	/// <item>
	/// <term><see cref="SearcherNodeTypes.AvoidableRectangle"/></term>
	/// <description>
	/// The strong and weak inferences between 2 nodes, where at least one node
	/// is an almost avoidable rectangle node.
	/// </description>
	/// </item>
	/// <item>
	/// <term><see cref="SearcherNodeTypes.XyWing"/></term>
	/// <description>
	/// The strong and weak inferences between 2 nodes, where at least one node
	/// is an XY-Wing node.
	/// </description>
	/// </item>
	/// <item>
	/// <term><see cref="SearcherNodeTypes.XyzWing"/></term>
	/// <description>
	/// The strong and weak inferences between 2 nodes, where at least one node
	/// is an XYZ-Wing node.
	/// </description>
	/// </item>
	/// <item>
	/// <term><see cref="SearcherNodeTypes.Kraken"/></term>
	/// <description>
	/// The strong and weak inferences between 2 nodes, where at least one node
	/// is a kraken fish node.
	/// </description>
	/// </item>
	/// </list>
	/// Other typed inferences are being considered, such as an XYZ-Wing node, etc.
	/// </para>
	/// </summary>
	public abstract SearcherNodeTypes NodeTypes { get; init; }


	/// <summary>
	/// Checks whether the node list is redundant, which means the list contains duplicate link node IDs
	/// in the non- endpoint nodes.
	/// </summary>
	/// <param name="ids">The list of node IDs.</param>
	/// <returns>A <see cref="bool"/> result.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static sealed bool IsNodesRedundant(int[] ids) => ids[0] == ids[^1] && ids[1] == ids[^2];
}
