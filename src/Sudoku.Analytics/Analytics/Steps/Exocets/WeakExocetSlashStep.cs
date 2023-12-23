namespace Sudoku.Analytics.Steps;

/// <summary>
/// Provides with a step that is a <b>Weak Exocet (Slash)</b> technique.
/// </summary>
/// <param name="conclusions"><inheritdoc/></param>
/// <param name="views"><inheritdoc/></param>
/// <param name="options"><inheritdoc/></param>
/// <param name="digitsMask"><inheritdoc/></param>
/// <param name="stabilityBalancer">Indicates the value cell that makes the exocet pattern to be stable.</param>
/// <param name="missingValueCell">Indicates the missing value cell in cross-line.</param>
/// <param name="baseCells"><inheritdoc/></param>
/// <param name="targetCells"><inheritdoc/></param>
/// <param name="crosslineCells"><inheritdoc/></param>
public sealed partial class WeakExocetSlashStep(
	Conclusion[] conclusions,
	View[]? views,
	StepSearcherOptions options,
	Mask digitsMask,
	[Data] Cell stabilityBalancer,
	[Data] Cell missingValueCell,
	scoped ref readonly CellMap baseCells,
	scoped ref readonly CellMap targetCells,
	scoped ref readonly CellMap crosslineCells
) : ExocetStep(conclusions, views, options, digitsMask, in baseCells, in targetCells, [], in crosslineCells)
{
	/// <inheritdoc/>
	public override decimal BaseDifficulty => base.BaseDifficulty + .3M;

	/// <inheritdoc/>
	public override Technique Code => Technique.WeakExocetSlash;

	/// <inheritdoc/>
	public override ExtraDifficultyFactor[] ExtraDifficultyFactors => [new(ExtraDifficultyFactorNames.SlashElimination, .5M)];
}
