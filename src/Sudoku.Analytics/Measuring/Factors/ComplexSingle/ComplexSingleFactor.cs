namespace Sudoku.Measuring.Factors;

/// <summary>
/// Represents a factor that describes the factor for <see cref="ComplexSingleStep"/>.
/// </summary>
/// <seealso cref="ComplexSingleStep"/>
public sealed partial class ComplexSingleFactor : Factor
{
	/// <inheritdoc/>
	public override string[] ParameterNames => [nameof(ComplexSingleStep.IndirectTechniques)];

	/// <inheritdoc/>
	public override Type ReflectedStepType => typeof(ComplexSingleStep);

	/// <inheritdoc/>
	public override ParameterizedFormula Formula => static args => ComplexTechniqueUsages.GetComplexityDifficulty((Technique[][])args![0]!);
}
