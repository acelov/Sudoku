using Sudoku.Concepts;
using Sudoku.Concepts.Primitive;

namespace Sudoku.Text.SudokuGrid;

/// <summary>
/// Represents a grid converter type that can converts into a <see cref="string"/> text representing the equivalent <see cref="Grid"/> instance.
/// </summary>
/// <seealso cref="Grid"/>
public abstract record GridConverter : ISpecifiedConceptConverter<Grid>
{
	/// <inheritdoc cref="ISpecifiedConceptConverter{T}.Converter"/>
	public abstract GridNotationConverter Converter { get; }

	/// <inheritdoc/>
	Func<Grid, string> ISpecifiedConceptConverter<Grid>.Converter => grid => Converter(in grid);
}