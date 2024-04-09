namespace Sudoku.Measuring.Factors;

/// <summary>
/// Represents a factor that describes the factor 
/// </summary>
public sealed class RectangleDeathBlossomIsAvoidableFactor : Factor
{
	/// <inheritdoc/>
	public override string FormulaString => "{0} ? 1 : 0";

	/// <inheritdoc/>
	public override string[] ParameterNames => [nameof(RectangleDeathBlossomStep.IsAvoidable)];

	/// <inheritdoc/>
	public override Type ReflectedStepType => typeof(RectangleDeathBlossomStep);

	/// <inheritdoc/>
	public override Func<Step, int?> Formula
		=> static step => step switch
		{
			RectangleDeathBlossomStep { IsAvoidable: var isAvoidable } => isAvoidable ? 1 : 0,
			_ => null
		};
}
