namespace Sudoku.Strategying.Constraints;

/// <summary>
/// Represents a lasting constraint.
/// </summary>
[TypeImpl(TypeImplFlag.Object_GetHashCode | TypeImplFlag.Object_ToString)]
public sealed partial class LastingConstraint : Constraint, ILimitCountConstraint<int>, IComparisonOperatorConstraint
{
	/// <inheritdoc/>
	[HashCodeMember]
	[StringMember]
	public int LimitCount { get; set; }

	/// <summary>
	/// Indicates the technique used.
	/// </summary>
	[HashCodeMember]
	[StringMember]
	public SingleTechniqueFlag Technique { get; set; }

	/// <inheritdoc/>
	[HashCodeMember]
	[StringMember]
	public ComparisonOperator Operator { get; set; }

	/// <inheritdoc/>
	static int ILimitCountConstraint<int>.Minimum => 1;

	/// <inheritdoc/>
	static int ILimitCountConstraint<int>.Maximum => 8;


	/// <inheritdoc/>
	public override bool Equals([NotNullWhen(true)] Constraint? other)
		=> other is LastingConstraint comparer
		&& (LimitCount, Technique, Operator) == (comparer.LimitCount, comparer.Technique, comparer.Operator);

	/// <inheritdoc/>
	public override string ToString(IFormatProvider? formatProvider)
	{
		var culture = formatProvider as CultureInfo;
		return string.Format(
			ResourceDictionary.Get("LastingConstraint", culture),
#if NET9_0_OR_GREATER
			[
#endif
			Technique.GetName(culture),
			Operator.GetOperatorString(),
			LimitCount.ToString()
#if NET9_0_OR_GREATER
			]
#endif
		);
	}

	/// <inheritdoc/>
	public override LastingConstraint Clone()
		=> new() { LimitCount = LimitCount, Technique = Technique, Operator = Operator, IsNegated = IsNegated };

	/// <inheritdoc/>
	protected override bool CheckCore(ConstraintCheckingContext context)
	{
		return context.AnalyzerResult.Any(predicate);


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		bool predicate(Step s)
			=> s is SingleStep { Subtype: var st } and ILastingTrait { Lasting: var l }
			&& st.GetSingleTechnique() == Technique
			&& Operator.GetOperator<int>()(LimitCount, l);
	}
}
