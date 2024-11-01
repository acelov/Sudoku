namespace Sudoku.Analytics.Steps;

/// <summary>
/// Provides with a step that is a <b>Rectangle Forcing Chains</b> technique.
/// </summary>
/// <param name="conclusions"><inheritdoc/></param>
/// <param name="views"><inheritdoc/></param>
/// <param name="options"><inheritdoc/></param>
/// <param name="pattern">The pattern to be used.</param>
public sealed partial class RectangleForcingChainsStep(
	StepConclusions conclusions,
	View[]? views,
	StepGathererOptions options,
	[Property] RectangleForcingChains pattern
) : ChainStep(conclusions, views, options)
{
	/// <inheritdoc/>
	public override bool IsMultiple => true;

	/// <inheritdoc/>
	public override bool IsDynamic => false;

	/// <summary>
	/// Indicates whether the pattern uses grouped nodes.
	/// </summary>
	public bool IsGrouped => Pattern.Exists(static chain => chain.IsGrouped);

	/// <inheritdoc/>
	public override int BaseDifficulty => 70;

	/// <inheritdoc/>
	public override int Complexity => Pattern.Complexity;

	/// <inheritdoc/>
	public override Technique Code => Technique.RectangleForcingChains;

	/// <inheritdoc/>
	public override Mask DigitsUsed => Pattern.DigitsMask;

	/// <inheritdoc/>
	public override InterpolationArray Interpolations => [new(SR.EnglishLanguage, [ChainsStr]), new(SR.ChineseLanguage, [ChainsStr])];

	/// <inheritdoc/>
	public override FactorArray Factors
		=> [
			Factor.Create(
				"Factor_MultipleForcingChainsGroupedFactor",
				[nameof(IsGrouped)],
				GetType(),
				static args => (bool)args![0]! ? 2 : 0
			),
			Factor.Create(
				"Factor_MultipleForcingChainsGroupedNodeFactor",
				[nameof(Pattern)],
				GetType(),
				static args =>
				{
					var result = 0;
					foreach (var branch in ((RectangleForcingChains)args![0]!).Values)
					{
						foreach (var link in branch.Links)
						{
							result += link.GroupedLinkPattern switch
							{
								AlmostLockedSetPattern => 2,
								AlmostHiddenSetPattern => 3,
								UniqueRectanglePattern => 4,
								FishPattern => 6,
								XyzWingPattern => 8,
								null when link.FirstNode.IsGroupedNode || link.SecondNode.IsGroupedNode => 1,
								_ => 0
							};
						}
					}
					return result;
				}
			),
			Factor.Create(
				"Factor_MultipleForcingChainsLengthFactor",
				[nameof(Complexity)],
				GetType(),
				static args => ChainingLength.GetLengthDifficulty((int)args![0]!)
			)
		];

	private string ChainsStr => Pattern.ToString(new ChainFormatInfo(Options.Converter));


	/// <inheritdoc/>
	public override bool Equals([NotNullWhen(true)] Step? other)
		=> other is RectangleForcingChainsStep comparer && Pattern.Equals(comparer.Pattern);

	/// <inheritdoc/>
	public override int CompareTo(Step? other)
		=> other is RectangleForcingChainsStep comparer
			? Conclusions.Length.CompareTo(comparer.Conclusions.Length) is var r and not 0
				? r
				: Pattern.CompareTo(comparer.Pattern)
			: -1;
}
