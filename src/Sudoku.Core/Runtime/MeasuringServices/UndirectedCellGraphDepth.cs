namespace Sudoku.Runtime.MeasuringServices;

/// <summary>
/// Represents a node that describes for a graph node inside a <see cref="UndirectedCellGraph"/>, with depth from a node.
/// </summary>
/// <param name="Depth">Indicates the depth.</param>
/// <param name="Cell">The cell.</param>
public readonly record struct UndirectedCellGraphDepth(int Depth, Cell Cell)
{
	/// <include
	///     file="../../global-doc-comments.xml"
	///     path="/g/csharp9/feature[@name='records']/target[@name='method' and @cref='PrintMembers']"/>
	private bool PrintMembers(StringBuilder builder)
	{
		builder.Append($"{nameof(Depth)} = ");
		builder.Append(Depth);
		builder.Append($", {nameof(Cell)} = ");
		builder.Append($@"""{CoordinateConverter.GetInstance(null).CellConverter(in Cell.AsCellMap())}""");
		return true;
	}
}