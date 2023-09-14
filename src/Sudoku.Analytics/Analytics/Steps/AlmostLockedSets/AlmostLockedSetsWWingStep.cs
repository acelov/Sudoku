using System.SourceGeneration;
using Sudoku.Analytics.Categorization;
using Sudoku.Concepts;
using Sudoku.Rendering;
using Sudoku.Text;
using Sudoku.Text.Notation;
using static Sudoku.Analytics.Strings.StringsAccessor;

namespace Sudoku.Analytics.Steps;

/// <summary>
/// Provides with a step that is an <b>Almost Locked Sets W-Wing</b> technique.
/// </summary>
/// <param name="conclusions"><inheritdoc/></param>
/// <param name="views"><inheritdoc/></param>
/// <param name="als1">Indicates the first ALS used.</param>
/// <param name="als2">Indicates the second ALS used.</param>
/// <param name="conjugatePair">Indicates the conjugate pair used as a bridge.</param>
/// <param name="wDigitsMask">Indicates the mask of W digits used.</param>
/// <param name="xDigit">Indicates the digit X.</param>
public sealed partial class AlmostLockedSetsWWingStep(
	Conclusion[] conclusions,
	View[]? views,
	[DataMember(GeneratedMemberName = "FirstAls")] AlmostLockedSet als1,
	[DataMember(GeneratedMemberName = "SecondAls")] AlmostLockedSet als2,
	[DataMember] Conjugate conjugatePair,
	[DataMember] Mask wDigitsMask,
	[DataMember] Digit xDigit
) : AlmostLockedSetsStep(conclusions, views)
{
	/// <inheritdoc/>
	public override decimal BaseDifficulty => 6.2M;

	/// <inheritdoc/>
	public override Technique Code => Technique.AlmostLockedSetsWWing;

	/// <inheritdoc/>
	public override FormatInterpolation[] FormatInterpolationParts
		=> [new(EnglishLanguage, [Als1Str, Als2Str, ConjStr, WStr, XStr]), new(ChineseLanguage, [Als1Str, Als2Str, ConjStr, WStr, XStr])];

	private string Als1Str => FirstAls.ToString();

	private string Als2Str => SecondAls.ToString();

	private string ConjStr => ConjugatePair.ToString();

	private string WStr => DigitNotation.ToString(WDigitsMask);

	private string XStr => DigitNotation.ToString(XDigit);
}
