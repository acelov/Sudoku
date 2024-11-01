namespace Sudoku.Drawing.Ocr;

/// <summary>
/// Indicates the exception that throws when the recognition tools hasn't been initialized
/// before using a function.
/// </summary>
public sealed class RecognizerNotInitializedException : Exception
{
	/// <inheritdoc/>
	public override string Message => SR.Get("Message_RecognizerNotInitializedException");

	/// <inheritdoc/>
	public override string? HelpLink => null;
}
