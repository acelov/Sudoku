using System.SourceGeneration;
using Sudoku.Analytics.Categorization;
using Sudoku.Analytics.Configuration;
using Sudoku.Analytics.Rating;
using Sudoku.Concepts;
using Sudoku.Rendering;

namespace Sudoku.Analytics.Steps;

/// <summary>
/// Provides with a step that is a <b>Complex Junior Exocet (Adjacent Target)</b> technique.
/// </summary>
/// <param name="conclusions"><inheritdoc/></param>
/// <param name="views"><inheritdoc/></param>
/// <param name="options"><inheritdoc/></param>
/// <param name="digitsMask"><inheritdoc/></param>
/// <param name="baseCells"><inheritdoc/></param>
/// <param name="targetCells"><inheritdoc/></param>
/// <param name="crosslineCells"><inheritdoc/></param>
/// <param name="crosslineHousesMask">Indicates the mask holding a list of houses spanned for cross-line cells.</param>
/// <param name="extraHousesMask">Indicates the mask holding a list of extra houses.</param>
/// <param name="singleMirrors">Indicates the single mirror cells. The value should be used one-by-one.</param>
public sealed partial class ComplexJuniorExocetAdjacentTargetStep(
	Conclusion[] conclusions,
	View[]? views,
	StepSearcherOptions options,
	Mask digitsMask,
	scoped ref readonly CellMap baseCells,
	scoped ref readonly CellMap targetCells,
	scoped ref readonly CellMap crosslineCells,
	[Data] HouseMask crosslineHousesMask,
	[Data] HouseMask extraHousesMask,
	[Data] scoped ref readonly CellMap singleMirrors
) :
	ExocetStep(conclusions, views, options, digitsMask, in baseCells, in targetCells, [], in crosslineCells),
	IComplexSeniorExocetStepBaseOverrides
{
	/// <inheritdoc/>
	public override decimal BaseDifficulty
		=> base.BaseDifficulty + this.GetShapeKind() switch
		{
			ExocetShapeKind.Franken => .4M,
			ExocetShapeKind.Mutant => .6M
		};

	/// <inheritdoc/>
	public override Technique Code
		=> this.GetShapeKind() switch
		{
			ExocetShapeKind.Franken => Technique.FrankenJuniorExocetAdjacentTarget,
			ExocetShapeKind.Mutant => Technique.MutantJuniorExocetAdjacentTarget
		};

	/// <inheritdoc/>
	public override ExtraDifficultyCase[] ExtraDifficultyCases => [new(ExtraDifficultyCaseNames.Mirror, .1M)];
}
