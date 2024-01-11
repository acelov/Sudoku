namespace Sudoku.Analytics.Steps;

/// <summary>
/// Provides with a step that is a <b>Unique Rectangle Type 3</b> technique.
/// </summary>
/// <param name="conclusions"><inheritdoc/></param>
/// <param name="views"><inheritdoc/></param>
/// <param name="options"><inheritdoc/></param>
/// <param name="digit1"><inheritdoc/></param>
/// <param name="digit2"><inheritdoc/></param>
/// <param name="cells"><inheritdoc/></param>
/// <param name="extraCells">Indicates the extra cells used, forming the subset.</param>
/// <param name="extraDigitsMask">Indicates the mask that contains all extra digits used.</param>
/// <param name="house">Indicates the house used.</param>
/// <param name="isAvoidable"><inheritdoc/></param>
/// <param name="absoluteOffset"><inheritdoc/></param>
/// <param name="isNaked">
/// Indicates whether the subset is naked subset. If <see langword="true"/>, a naked subset; otherwise, a hidden subset.
/// </param>
public sealed partial class UniqueRectangleType3Step(
	Conclusion[] conclusions,
	View[]? views,
	StepSearcherOptions options,
	Digit digit1,
	Digit digit2,
	scoped ref readonly CellMap cells,
	[RecordParameter] scoped ref readonly CellMap extraCells,
	[RecordParameter] Mask extraDigitsMask,
	[RecordParameter] House house,
	bool isAvoidable,
	int absoluteOffset,
	[RecordParameter] bool isNaked = true
) : UniqueRectangleStep(
	conclusions,
	views,
	options,
	isAvoidable ? Technique.AvoidableRectangleType3 : Technique.UniqueRectangleType3,
	digit1,
	digit2,
	in cells,
	isAvoidable,
	absoluteOffset
)
{
	/// <inheritdoc/>
	public override ExtraDifficultyFactor[] ExtraDifficultyFactors
		=> [
			new(ExtraDifficultyFactorNames.Hidden, IsNaked ? 0 : .1M),
			new(ExtraDifficultyFactorNames.Size, PopCount((uint)ExtraDigitsMask) * .1M)
		];

	/// <inheritdoc/>
	public override FormatInterpolation[] FormatInterpolationParts
		=> [
			new(EnglishLanguage, [D1Str, D2Str, CellsStr, DigitsStr, OnlyKeyword, CellsStr, HouseStr]),
			new(ChineseLanguage, [D1Str, D2Str, CellsStr, DigitsStr, OnlyKeywordZhCn, HouseStr, CellsStr, AppearLimitKeyword])
		];

	private string DigitsStr => Options.Converter.DigitConverter(ExtraDigitsMask);

	private string OnlyKeyword => IsNaked ? string.Empty : "only ";

	private string OnlyKeywordZhCn => ResourceDictionary.Get("Only", new(2052))!;

	private string HouseStr => Options.Converter.HouseConverter(1 << House);

	private string AppearLimitKeyword => ResourceDictionary.Get("Appear", ResultCurrentCulture);
}
