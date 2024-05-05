namespace Sudoku.Measuring.Factors;

/// <summary>
/// Represents a factor that describes the length of <see cref="BowmanBingoStep"/>.
/// </summary>
/// <seealso cref="BowmanBingoStep"/>
public sealed partial class BowmanBingoLengthFactor : Factor
{
	/// <inheritdoc/>
	public override string[] ParameterNames => [nameof(BowmanBingoStep.ContradictionLinks)];

	/// <inheritdoc/>
	public override Type ReflectedStepType => typeof(BowmanBingoStep);

	/// <inheritdoc/>
	public override ParameterizedFormula Formula => static args => ChainingLength.GetLengthDifficulty(((Conclusion[])args![0]!).Length);
}
