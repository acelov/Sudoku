using System.SourceGeneration;
using Sudoku.Concepts;
using Sudoku.Rendering;
using Sudoku.Text;
using Sudoku.Text.Notation;
using static Sudoku.Analytics.Strings.StringsAccessor;

namespace Sudoku.Analytics.Steps;

/// <summary>
/// Provides with a step that is a <b>Reverse Bi-value Universal Grave Type 3</b> technique.
/// </summary>
/// <param name="conclusions"><inheritdoc/></param>
/// <param name="views"><inheritdoc/></param>
/// <param name="digit1"><inheritdoc/></param>
/// <param name="digit2"><inheritdoc/></param>
/// <param name="subsetHouse">Indicates the subset house used.</param>
/// <param name="subsetMask">Indicates the subset digits mask.</param>
/// <param name="pattern"><inheritdoc/></param>
/// <param name="emptyCells"><inheritdoc/></param>
public sealed partial class ReverseBivalueUniversalGraveType3Step(
	Conclusion[] conclusions,
	View[]? views,
	Digit digit1,
	Digit digit2,
	[DataMember] House subsetHouse,
	[DataMember] Mask subsetMask,
	scoped in CellMap pattern,
	scoped in CellMap emptyCells
) : ReverseBivalueUniversalGraveStep(conclusions, views, digit1, digit2, pattern, emptyCells)
{
	/// <inheritdoc/>
	public override int Type => 3;

	/// <inheritdoc/>
	public override FormatInterpolation[] FormatInterpolationParts
		=> [new(EnglishLanguage, [ExtraHouseStr, ExtraDigitsStr]), new(ChineseLanguage, [ExtraHouseStr, ExtraDigitsStr])];

	private string ExtraHouseStr => HouseNotation.ToString(SubsetHouse);

	private string ExtraDigitsStr => DigitNotation.ToString(SubsetMask);
}
