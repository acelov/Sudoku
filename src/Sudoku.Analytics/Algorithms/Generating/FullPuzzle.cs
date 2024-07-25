namespace Sudoku.Runtime.GeneratingServices;

/// <summary>
/// Represents a full puzzle.
/// </summary>
[TypeImpl(TypeImplFlag.Object_GetHashCode)]
public abstract partial class FullPuzzle : PuzzleBase
{
	/// <summary>
	/// Indicates the solution to the puzzle.
	/// </summary>
	public Grid Solution => Puzzle.GetSolutionGrid();


	/// <inheritdoc/>
	public override bool Equals([NotNullWhen(true)] PuzzleBase? other)
		=> other is FullPuzzle comparer && (FailedReason, Puzzle) == (comparer.FailedReason, comparer.Puzzle);
}
