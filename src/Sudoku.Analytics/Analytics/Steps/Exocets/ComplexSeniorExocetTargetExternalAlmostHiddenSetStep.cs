using System.SourceGeneration;
using Sudoku.Analytics.Categorization;
using Sudoku.Analytics.Configuration;
using Sudoku.Concepts;
using Sudoku.Rendering;

namespace Sudoku.Analytics.Steps;

/// <summary>
/// Provides with a step that is a <b>Complex Senior Exocet (Target Sync)</b> technique.
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
/// <param name="almostHiddenSetMask">The mask that holds a list of digits forming an external AHS.</param>
public sealed partial class ComplexSeniorExocetTargetExternalAlmostHiddenSetStep(
	Conclusion[] conclusions,
	View[]? views,
	StepSearcherOptions options,
	Mask digitsMask,
	scoped ref readonly CellMap baseCells,
	scoped ref readonly CellMap targetCells,
	scoped ref readonly CellMap endoTargetCells,
	scoped ref readonly CellMap crosslineCells,
	[DataMember] HouseMask crosslineHousesMask,
	[DataMember] HouseMask extraHousesMask,
	[DataMember] Mask almostHiddenSetMask
) :
	ExocetStep(conclusions, views, options, digitsMask, in baseCells, in targetCells, in endoTargetCells, in crosslineCells),
	IComplexSeniorExocetStepBaseOverrides
{
	/// <inheritdoc/>
	public override decimal BaseDifficulty
		=> base.BaseDifficulty + this.GetShapeKind() switch
		{
			ExocetShapeKind.Franken => .4M,
			ExocetShapeKind.Mutant => .7M,
			ExocetShapeKind.Basic => 0
		};

	/// <inheritdoc/>
	public override Technique Code
		=> this.GetShapeKind() switch
		{
			ExocetShapeKind.Franken => Technique.FrankenSeniorExocetTargetExternalAlmostHiddenSet,
			ExocetShapeKind.Mutant => Technique.MutantSeniorExocetTargetExternalAlmostHiddenSet
		};
}