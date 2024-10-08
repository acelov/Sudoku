namespace Sudoku.Analytics.Steps;

/// <summary>
/// Provides with a step that is a <b>Complex Junior Exocet (Locked Member)</b> or <b>Complex Senior Exocet (Locked Member)</b> technique.
/// </summary>
/// <param name="conclusions"><inheritdoc/></param>
/// <param name="views"><inheritdoc/></param>
/// <param name="options"><inheritdoc/></param>
/// <param name="digitsMask"><inheritdoc/></param>
/// <param name="baseCells"><inheritdoc/></param>
/// <param name="targetCells"><inheritdoc/></param>
/// <param name="endoTargetCells"><inheritdoc/></param>
/// <param name="crosslineCells"><inheritdoc/></param>
/// <param name="crosslineHousesMask">Indicates the mask holding a list of houses spanned for cross-line cells.</param>
/// <param name="extraHousesMask">Indicates the mask holding a list of extra houses.</param>
public sealed partial class ComplexExocetLockedMemberStep(
	ReadOnlyMemory<Conclusion> conclusions,
	View[]? views,
	StepGathererOptions options,
	Mask digitsMask,
	ref readonly CellMap baseCells,
	ref readonly CellMap targetCells,
	ref readonly CellMap endoTargetCells,
	ref readonly CellMap crosslineCells,
	[Property] HouseMask crosslineHousesMask,
	[Property] HouseMask extraHousesMask
) :
	ExocetStep(conclusions, views, options, digitsMask, in baseCells, in targetCells, in endoTargetCells, in crosslineCells),
	IComplexSeniorExocet
{
	/// <inheritdoc/>
	public override int BaseDifficulty
		=> base.BaseDifficulty + 2 + this.GetShapeKind() switch
		{
			ExocetShapeKind.Franken => 4,
			ExocetShapeKind.Mutant => 6,
			ExocetShapeKind.Basic => 0
		};

	/// <inheritdoc/>
	public override Technique Code
		=> (EndoTargetCells, this.GetShapeKind()) switch
		{
			([], ExocetShapeKind.Franken) => Technique.FrankenJuniorExocetLockedMember,
			(_, ExocetShapeKind.Franken) => Technique.FrankenSeniorExocetLockedMember,
			([], ExocetShapeKind.Mutant) => Technique.MutantJuniorExocetLockedMember,
			(_, ExocetShapeKind.Mutant) => Technique.MutantSeniorExocetLockedMember
		};
}
