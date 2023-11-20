using System.Algorithm;
using System.SourceGeneration;
using Sudoku.Analytics.Categorization;
using Sudoku.Analytics.Configuration;
using Sudoku.Analytics.Rating;
using Sudoku.Concepts;
using Sudoku.Rendering;
using static Sudoku.Analytics.Strings.StringsAccessor;

namespace Sudoku.Analytics.Steps;

/// <summary>
/// Provides with a step that is a <b>Unique Rectangle External Type 1/2</b> or <b>Avoidable Rectangle External Type 1/2</b> technique.
/// </summary>
/// <param name="conclusions"><inheritdoc/></param>
/// <param name="views"><inheritdoc/></param>
/// <param name="options"><inheritdoc/></param>
/// <param name="digit1"><inheritdoc/></param>
/// <param name="digit2"><inheritdoc/></param>
/// <param name="cells"><inheritdoc/></param>
/// <param name="guardianCells">Indicates the cells that the guardians lie in.</param>
/// <param name="guardianDigit">Indicates the digit that the guardians are used.</param>
/// <param name="isIncomplete">Indicates whether the rectangle is incomplete.</param>
/// <param name="isAvoidable"><inheritdoc/></param>
/// <param name="absoluteOffset"><inheritdoc/></param>
public sealed partial class UniqueRectangleExternalType1Or2Step(
	Conclusion[] conclusions,
	View[]? views,
	StepSearcherOptions options,
	Digit digit1,
	Digit digit2,
	scoped ref readonly CellMap cells,
	[Data] scoped ref readonly CellMap guardianCells,
	[Data] Digit guardianDigit,
	[Data] bool isIncomplete,
	bool isAvoidable,
	int absoluteOffset
) : UniqueRectangleStep(
	conclusions,
	views,
	options,
	(isAvoidable, guardianCells.Count == 1) switch
	{
		(true, true) => Technique.AvoidableRectangleExternalType1,
		(true, false) => Technique.AvoidableRectangleExternalType2,
		(false, true) => Technique.UniqueRectangleExternalType1,
		_ => Technique.UniqueRectangleExternalType2
	},
	digit1,
	digit2,
	in cells,
	false,
	absoluteOffset
)
{
	/// <inheritdoc/>
	public override ExtraDifficultyCase[] ExtraDifficultyCases
		=> [
			new(ExtraDifficultyCaseNames.Guardian, Sequences.A004526(GuardianCells.Count) * .1M),
			new(ExtraDifficultyCaseNames.Avoidable, IsAvoidable ? .1M : 0),
			new(ExtraDifficultyCaseNames.Incompleteness, IsIncomplete ? .1M : 0)
		];

	/// <inheritdoc/>
	public override FormatInterpolation[] FormatInterpolationParts
		=> [
			new(EnglishLanguage, [D1Str, D2Str, CellsStr, GuardianDigitStr, GuardianCellsStr]),
			new(ChineseLanguage, [D1Str, D2Str, CellsStr, GuardianDigitStr, GuardianCellsStr])
		];

	private string GuardianDigitStr => Options.Converter.DigitConverter((Mask)(1 << GuardianDigit));

	private string GuardianCellsStr => Options.Converter.CellConverter(GuardianCells);
}
