namespace Sudoku.Concepts;

using static IGrid;

/// <summary>
/// Represents a sudoku grid that can be only used by users to append pencilmarks.
/// </summary>
[InlineArray(CellsCount)]
public struct PencilmarkGrid// : IGrid<PencilmarkGrid>
{
	/// <inheritdoc cref="IGrid{TSelf}.FirstMaskRef"/>
	[SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "<Pending>")]
	[SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
	private Mask _values;
}
