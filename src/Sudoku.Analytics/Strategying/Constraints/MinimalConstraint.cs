namespace Sudoku.Strategying.Constraints;

/// <summary>
/// Represents minimal constraint.
/// </summary>
[GetHashCode]
[ToString]
public sealed partial class MinimalConstraint : Constraint
{
	/// <summary>
	/// Indicates whether the puzzle shsould be minimal.
	/// </summary>
	[HashCodeMember]
	public bool ShouldBeMinimal { get; set; }

	[StringMember]
	private string ShouldBeMinimalString => ShouldBeMinimal.ToString();


	/// <inheritdoc/>
	public override bool Equals([NotNullWhen(true)] Constraint? other)
		=> other is MinimalConstraint comparer && ShouldBeMinimal == comparer.ShouldBeMinimal;

	/// <inheritdoc/>
	public override string ToString(IFormatProvider? formatProvider)
	{
		var culture = formatProvider as CultureInfo;
		return string.Format(
			ResourceDictionary.Get("MinimalConstraint", culture),
			ShouldBeMinimal ? string.Empty : ResourceDictionary.Get("NoString", culture)
		);
	}

	/// <inheritdoc/>
	public override MinimalConstraint Clone() => new() { IsNegated = IsNegated, ShouldBeMinimal = ShouldBeMinimal };

	/// <inheritdoc/>
	protected override bool CheckCore(ConstraintCheckingContext context) => context.Grid.IsMinimal == ShouldBeMinimal;
}
