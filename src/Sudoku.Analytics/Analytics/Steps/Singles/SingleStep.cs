namespace Sudoku.Analytics.Steps;

/// <summary>
/// Provides with a step that is a <b>Single</b> technique.
/// </summary>
/// <param name="conclusions"><inheritdoc/></param>
/// <param name="views"><inheritdoc/></param>
/// <param name="cell">Indicates the cell used.</param>
/// <param name="digit">Indicates the digit used.</param>
public abstract partial class SingleStep(Conclusion[] conclusions, View[]? views, [DataMember] Cell cell, [DataMember] Digit digit) :
	Step(conclusions, views),
	IEquatableStep<SingleStep>
{
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	static bool IEquatableStep<SingleStep>.operator ==(SingleStep left, SingleStep right)
		=> left.Cell == right.Cell && left.Digit == right.Digit && left.Code == right.Code;
}
