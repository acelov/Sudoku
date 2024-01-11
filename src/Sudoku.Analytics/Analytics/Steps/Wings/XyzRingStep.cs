namespace Sudoku.Analytics.Steps;

/// <summary>
/// Provides with a step that is an <b>XYZ-Ring</b> or <b>Grouped XYZ-Ring</b> technique.
/// </summary>
/// <param name="conclusions"><inheritdoc/></param>
/// <param name="views"><inheritdoc/></param>
/// <param name="options"><inheritdoc/></param>
/// <param name="pivot">Indicates the pivot cell.</param>
/// <param name="leafCell1">Indicates the leaf cell 1.</param>
/// <param name="leafCell2">Indicates the leaf cell 2.</param>
/// <param name="conjugateHouse">Indicates the conjugate house used.</param>
/// <param name="isType2">Indicates whether the type is type 2.</param>
/// <param name="isGrouped">Indicates whether the conjugate pair is grouped one.</param>
public sealed partial class XyzRingStep(
	Conclusion[] conclusions,
	View[]? views,
	StepSearcherOptions options,
	[RecordParameter] Cell pivot,
	[RecordParameter] Cell leafCell1,
	[RecordParameter] Cell leafCell2,
	[RecordParameter] House conjugateHouse,
	[RecordParameter] bool isType2,
	[RecordParameter] bool isGrouped
) : WingStep(conclusions, views, options)
{
	/// <inheritdoc/>
	public override decimal BaseDifficulty => IsType2 ? 5.0M : 5.2M;

	/// <inheritdoc/>
	public override Technique Code
		=> (IsGrouped, IsType2) switch
		{
			(true, true) => Technique.GroupedXyzNiceLoop,
			(true, _) => Technique.GroupedXyzLoop,
			(_, true) => Technique.XyzNiceLoop,
			_ => Technique.XyzLoop
		};
}
