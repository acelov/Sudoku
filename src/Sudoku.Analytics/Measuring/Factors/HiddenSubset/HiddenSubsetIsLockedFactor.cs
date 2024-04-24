namespace Sudoku.Measuring.Factors;

/// <summary>
/// Represents a factor that describes which locked case a hidden subset is.
/// </summary>
public sealed class HiddenSubsetIsLockedFactor : Factor
{
	/// <inheritdoc/>
	public override string FormulaString
		=> """
		({0}, {1}) switch
		{{
			(true, 2) => -12,
			(true, 3) => -13,
			_ => 0
		}}
		""";

	/// <inheritdoc/>
	public override string[] ParameterNames => [nameof(HiddenSubsetStep.IsLocked), nameof(HiddenSubsetStep.Size)];

	/// <inheritdoc/>
	public override Type ReflectedStepType => typeof(HiddenSubsetStep);

	/// <inheritdoc/>
	public override Func<Step, int?> Formula
		=> static step => step switch
		{
			HiddenSubsetStep { IsLocked: var isLocked, Size: var size } => isLocked switch
			{
				true => size switch { 2 => -12, 3 => -13 },
				_ => 0
			},
			_ => null
		};
}