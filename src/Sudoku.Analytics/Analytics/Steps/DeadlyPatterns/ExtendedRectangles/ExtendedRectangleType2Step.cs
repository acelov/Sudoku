namespace Sudoku.Analytics.Steps;

/// <summary>
/// Provides with a step that is an <b>Extended Rectangle Type 2</b> technique.
/// </summary>
/// <param name="conclusions"><inheritdoc/></param>
/// <param name="views"><inheritdoc/></param>
/// <param name="options"><inheritdoc/></param>
/// <param name="cells"><inheritdoc/></param>
/// <param name="digitsMask"><inheritdoc/></param>
/// <param name="extraDigit">Indicates the extra digit used.</param>
public sealed partial class ExtendedRectangleType2Step(
	Conclusion[] conclusions,
	View[]? views,
	StepSearcherOptions options,
	scoped ref readonly CellMap cells,
	Mask digitsMask,
	[RecordParameter] Digit extraDigit
) : ExtendedRectangleStep(conclusions, views, options, in cells, digitsMask)
{
	/// <inheritdoc/>
	public override int Type => 2;

	/// <inheritdoc/>
	public override ExtraDifficultyFactor[] ExtraDifficultyFactors
		=> [.. base.ExtraDifficultyFactors, new(ExtraDifficultyFactorNames.ExtraDigit, .1M)];

	/// <inheritdoc/>
	public override FormatInterpolation[] FormatInterpolationParts
		=> [new(EnglishLanguage, [DigitsStr, CellsStr, ExtraDigitStr]), new(ChineseLanguage, [DigitsStr, CellsStr, ExtraDigitStr])];

	private string ExtraDigitStr => Options.Converter.DigitConverter((Mask)(1 << ExtraDigit));
}
