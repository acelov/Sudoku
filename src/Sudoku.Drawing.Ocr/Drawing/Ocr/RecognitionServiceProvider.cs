namespace Sudoku.Drawing.Ocr;

/// <summary>
/// Define a sudoku recognition service provider.
/// </summary>
[TypeImpl(TypeImplFlags.Disposable)]
public sealed partial class RecognitionServiceProvider : IDisposable
{
	/// <summary>
	/// Indicates the internal recognition service provider.
	/// </summary>
	[DisposableMember]
	private readonly InternalServiceProvider _recognizingServiceProvider;


	/// <summary>
	/// Initializes a default <see cref="RecognitionServiceProvider"/> instance.
	/// </summary>
	public RecognitionServiceProvider()
		=> (_recognizingServiceProvider = new()).InitTesseract($@"{Directory.GetCurrentDirectory()}\tessdata");


	/// <summary>
	/// Indicates whether the OCR tool has already initialized.
	/// </summary>
	public bool IsInitialized => _recognizingServiceProvider.Initialized;


	/// <summary>
	/// Recognize the image.
	/// </summary>
	/// <param name="image">The image.</param>
	/// <returns>The grid.</returns>
	/// <exception cref="RecognizerNotInitializedException">Throws when the tool has not initialized yet.</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Grid Recognize(Bitmap image)
	{
		if (IsInitialized)
		{
			using var gridRecognizer = new GridRecognizer(image);
			return _recognizingServiceProvider.RecognizeDigits(gridRecognizer.Recognize());
		}

		throw new RecognizerNotInitializedException();
	}
}
