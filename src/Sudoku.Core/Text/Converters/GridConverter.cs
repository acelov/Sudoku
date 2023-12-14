using Sudoku.Concepts;

namespace Sudoku.Text.Converters;

/// <summary>
/// Represents a grid converter type that can converts into a <see cref="string"/> text representing the equivalent <see cref="Grid"/> instance.
/// </summary>
/// <seealso cref="Grid"/>
public abstract record GridConverter : ISpecifiedConceptConverter<Grid>
{
	/// <inheritdoc cref="ISpecifiedConceptConverter{T}.Converter"/>
	public abstract GridNotationConverter Converter { get; }

	/// <inheritdoc/>
	FuncRefReadOnly<Grid, string> ISpecifiedConceptConverter<Grid>.Converter => (scoped ref readonly Grid grid) => Converter(in grid);
}
