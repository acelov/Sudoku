namespace Sudoku.Measuring.Factors;

/// <summary>
/// Represents a factor that describes whether the external unique rectangle is incomplete.
/// </summary>
public sealed partial class UniqueRectangleExternalType4IsIncompleteFactor : Factor
{
	/// <inheritdoc/>
	public override string[] ParameterNames => [nameof(UniqueRectangleExternalType4Step.IsIncomplete)];

	/// <inheritdoc/>
	public override Type ReflectedStepType => typeof(UniqueRectangleExternalType4Step);

	/// <inheritdoc/>
	public override ParameterizedFormula Formula => static args => (bool)args![0]! ? 1 : 0;
}