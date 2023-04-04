namespace Sudoku.Presentation.Nodes.Shapes;

/// <summary>
/// Defines an average bar view node.
/// </summary>
public sealed partial class AverageBarViewNode(Identifier identifier, int cell, bool isHorizontal) :
	SingleCellMarkViewNode(identifier, cell, Direction.None)
{
	/// <summary>
	/// Indicates whether the view node is for horizontal one.
	/// </summary>
	public bool IsHorizontal { get; } = isHorizontal;


	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals([NotNullWhen(true)] ViewNode? other)
		=> other is AverageBarViewNode comparer && Cell == comparer.Cell && IsHorizontal == comparer.IsHorizontal;

	[GeneratedOverriddingMember(GeneratedGetHashCodeBehavior.CallingHashCodeCombine, nameof(TypeIdentifier), nameof(Cell), nameof(IsHorizontal))]
	public override partial int GetHashCode();

	[GeneratedOverriddingMember(GeneratedToStringBehavior.RecordLike, nameof(Identifier), nameof(Cell))]
	public override partial string ToString();

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override AverageBarViewNode Clone() => new(Identifier, Cell, IsHorizontal);
}
