namespace Sudoku.Recognition.Imaging;

/// <summary>
/// Indicates the exception that throws when the value is failed to fill into a cell.
/// </summary>
/// <param name="cell">Indicates the wrong cell.</param>
/// <param name="digit">Indicates the wrong digit.</param>
public sealed partial class FailedToFillValueException([RecordParameter] Cell cell, [RecordParameter] Digit digit) : Exception
{
	/// <inheritdoc/>
	public override string Message => $"Can't fill the cell {CellsMap[Cell]} with the digit {Digit + 1}.";

	/// <inheritdoc/>
	public override string? HelpLink => null;
}