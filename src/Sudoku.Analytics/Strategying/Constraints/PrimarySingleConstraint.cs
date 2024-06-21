namespace Sudoku.Strategying.Constraints;

/// <summary>
/// Represents a primary single constraint.
/// </summary>
[TypeImpl(TypeImplFlag.Object_GetHashCode | TypeImplFlag.Object_ToString)]
public sealed partial class PrimarySingleConstraint : Constraint
{
	/// <summary>
	/// Indicates whether the constraint allows hidden singles on rows or columns.
	/// </summary>
	[HashCodeMember]
	[StringMember]
	public bool AllowsHiddenSingleInLines { get; set; }

	/// <summary>
	/// Indicates which technique a user likes to finish a grid.
	/// </summary>
	[HashCodeMember]
	public SingleTechniqueFlag Primary { get; set; }

	[StringMember(nameof(Primary))]
	private string SinglePreferString => Primary.GetName(null);


	/// <inheritdoc/>
	public override bool Equals([NotNullWhen(true)] Constraint? other)
		=> other is PrimarySingleConstraint comparer && Primary == comparer.Primary;

	/// <inheritdoc/>
	public override string ToString(IFormatProvider? formatProvider)
	{
		var culture = formatProvider as CultureInfo;
		return string.Format(
			SR.Get("PrimarySingleConstraint", culture),
#if NET9_0_OR_GREATER
			[
#endif
			Primary.GetName(culture),
			AllowsHiddenSingleInLines ? string.Empty : SR.Get("NoString", culture)
#if NET9_0_OR_GREATER
			]
#endif
		);
	}

	/// <inheritdoc/>
	public override PrimarySingleConstraint Clone()
		=> new() { IsNegated = IsNegated, AllowsHiddenSingleInLines = AllowsHiddenSingleInLines, Primary = Primary };

	/// <inheritdoc/>
	protected override bool CheckCore(ConstraintCheckingContext context)
		=> Primary switch
		{
			SingleTechniqueFlag.FullHouse => context.Grid.CanPrimaryFullHouse(),
			SingleTechniqueFlag.HiddenSingle => context.Grid.CanPrimaryHiddenSingle(AllowsHiddenSingleInLines),
			SingleTechniqueFlag.NakedSingle => context.Grid.CanPrimaryNakedSingle()
		};
}
