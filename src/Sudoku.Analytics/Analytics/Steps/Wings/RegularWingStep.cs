namespace Sudoku.Analytics.Steps;

/// <summary>
/// Provides with a step that is a <b>Regular Wing</b> technique.
/// </summary>
/// <param name="conclusions"><inheritdoc/></param>
/// <param name="views"><inheritdoc/></param>
/// <param name="options"><inheritdoc/></param>
/// <param name="pivot">Indicates the cell that blossomed its petals.</param>
/// <param name="pivotCandidatesCount">Indicates the number of digits in the pivot cell.</param>
/// <param name="digitsMask">Indicates a mask that contains all digits used.</param>
/// <param name="petals">Indicates the petals used.</param>
public sealed partial class RegularWingStep(
	Conclusion[] conclusions,
	View[]? views,
	StepSearcherOptions options,
	[PrimaryConstructorParameter] Cell pivot,
	[PrimaryConstructorParameter] int pivotCandidatesCount,
	[PrimaryConstructorParameter] Mask digitsMask,
	[PrimaryConstructorParameter] scoped ref readonly CellMap petals
) : WingStep(conclusions, views, options), ISizeTrait
{
	/// <summary>
	/// Indicates whether the pattern is incomplete.
	/// </summary>
	public bool IsIncomplete => Size == PivotCandidatesCount + 1;

	/// <inheritdoc/>
	public override decimal BaseDifficulty => 42;

	/// <inheritdoc/>
	/// <remarks>
	/// The size indicates the number of candidates that the pivot cell holds. All names are:
	/// <list type="table">
	/// <item>
	/// <term>3</term>
	/// <description>XY-Wing or XYZ-Wing</description>
	/// </item>
	/// <item>
	/// <term>4</term>
	/// <description>WXYZ-Wing</description>
	/// </item>
	/// <item>
	/// <term>5</term>
	/// <description>VWXYZ-Wing</description>
	/// </item>
	/// <item>
	/// <term>6</term>
	/// <description>UVWXYZ-Wing</description>
	/// </item>
	/// <item>
	/// <term>7</term>
	/// <description>TUVWXYZ-Wing</description>
	/// </item>
	/// <item>
	/// <term>8</term>
	/// <description>STUVWXYZ-Wing</description>
	/// </item>
	/// <item>
	/// <term>9</term>
	/// <description>RSTUVWXYZ-Wing</description>
	/// </item>
	/// </list>
	/// </remarks>
	public int Size => PopCount((uint)DigitsMask);

	/// <inheritdoc/>
	public override Technique Code => TechniqueMarshal.MakeRegularWingTechniqueCode(TechniqueMarshal.GetRegularWingEnglishName(Size, IsIncomplete));

	/// <inheritdoc/>
	public override FormatInterpolation[] FormatInterpolationParts
		=> [new(EnglishLanguage, [DigitsStr, PivotCellStr, CellsStr]), new(ChineseLanguage, [DigitsStr, PivotCellStr, CellsStr])];

	/// <inheritdoc/>
	public override FactorCollection Factors => [new RegularWingSizeFactor(), new RegularWingIncompletenessFactor()];

	private string DigitsStr => Options.Converter.DigitConverter(DigitsMask);

	private string PivotCellStr => Options.Converter.CellConverter([Pivot]);

	private string CellsStr => Options.Converter.CellConverter(Petals);
}
