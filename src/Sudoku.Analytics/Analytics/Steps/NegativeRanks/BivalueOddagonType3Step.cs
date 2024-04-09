namespace Sudoku.Analytics.Steps;

/// <summary>
/// Provides with a step that is a <b>Bivalue Oddagon Type 3</b> technique.
/// </summary>
/// <param name="conclusions"><inheritdoc/></param>
/// <param name="views"><inheritdoc/></param>
/// <param name="options"><inheritdoc/></param>
/// <param name="loopCells"><inheritdoc/></param>
/// <param name="digit1"><inheritdoc/></param>
/// <param name="digit2"><inheritdoc/></param>
/// <param name="extraCells">Indicates the extra cells used.</param>
/// <param name="extraDigitsMask">Indicates the mask that contains all extra digits used.</param>
public sealed partial class BivalueOddagonType3Step(
	Conclusion[] conclusions,
	View[]? views,
	StepSearcherOptions options,
	scoped ref readonly CellMap loopCells,
	Digit digit1,
	Digit digit2,
	[PrimaryConstructorParameter] scoped ref readonly CellMap extraCells,
	[PrimaryConstructorParameter] Mask extraDigitsMask
) : BivalueOddagonStep(conclusions, views, options, in loopCells, digit1, digit2)
{
	/// <inheritdoc/>
	public override int Type => 3;

	/// <inheritdoc/>
	public override FormatInterpolation[] FormatInterpolationParts
		=> [
			new(EnglishLanguage, [LoopStr, Digit1Str, Digit2Str, DigitsStr, ExtraCellsStr]),
			new(ChineseLanguage, [Digit1Str, Digit2Str, LoopStr, ExtraCellsStr, DigitsStr])
		];

	/// <inheritdoc/>
	public override FactorCollection Factors => [.. base.Factors, new BivalueOddagonSubsetSizeFactor()];

	private string Digit1Str => Options.Converter.DigitConverter((Mask)(1 << Digit1));

	private string Digit2Str => Options.Converter.DigitConverter((Mask)(1 << Digit2));

	private string DigitsStr => Options.Converter.DigitConverter(ExtraDigitsMask);

	private string ExtraCellsStr => Options.Converter.CellConverter(ExtraCells);
}
