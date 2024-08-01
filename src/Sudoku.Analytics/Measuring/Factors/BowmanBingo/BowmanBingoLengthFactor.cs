namespace Sudoku.Measuring.Factors;

/// <summary>
/// Represents a factor that describes the length of <see cref="BowmanBingoStep"/>.
/// </summary>
/// <seealso cref="BowmanBingoStep"/>
public sealed class BowmanBingoLengthFactor : Factor
{
	/// <inheritdoc/>
	public override string[] ParameterNames => [nameof(ISizeTrait.Size)];

	/// <inheritdoc/>
	public override Type ReflectedStepType => typeof(BowmanBingoStep);

	/// <inheritdoc/>
	public override Func<ReadOnlySpan<object?>, int> Formula => static args => ChainingLength.GetLengthDifficulty((int)args![0]!);
}
