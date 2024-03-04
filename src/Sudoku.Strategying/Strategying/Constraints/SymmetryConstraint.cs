namespace Sudoku.Strategying.Constraints;

/// <summary>
/// Represents symmetry constraint. This constraint won't be used because <see cref="HodokuPuzzleGenerator"/> can controls this.
/// </summary>
/// <seealso cref="HodokuPuzzleGenerator"/>
[GetHashCode]
[ToString]
public sealed partial class SymmetryConstraint : Constraint
{
	/// <inheritdoc/>
	public override bool AllowDuplicate => false;

	/// <summary>
	/// Indicates the supported symmetry types to be used.
	/// </summary>
	[HashCodeMember]
	public SymmetricType SymmetricTypes { get; set; }

	[StringMember]
	private string SymmetricTypesString => SymmetricTypes.ToString();


	/// <inheritdoc/>
	public override bool Equals([NotNullWhen(true)] Constraint? other)
		=> other is SymmetryConstraint comparer && SymmetricTypes == comparer.SymmetricTypes;

	/// <inheritdoc/>
	public override bool Check(ConstraintCheckingContext context) => (SymmetricTypes & context.Grid.Symmetry) != 0;

	/// <inheritdoc/>
	public override string ToString(CultureInfo? culture = null)
		=> string.Format(
			ResourceDictionary.Get("SymmetryConstraint", culture),
			string.Join(ResourceDictionary.Get("_Token_Comma"), from type in SymmetricTypes.GetAllFlags() select type.GetName(culture))
		);
}
