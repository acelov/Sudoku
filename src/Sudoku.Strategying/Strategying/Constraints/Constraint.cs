namespace Sudoku.Strategying.Constraints;

/// <summary>
/// Represents a rule that checks whether a grid or its relied analysis information is passed the constraint.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$typeid", UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization)]
[JsonDerivedType(typeof(CountBetweenConstraint), nameof(CountBetweenConstraint))]
[JsonDerivedType(typeof(DiamondConstraint), nameof(DiamondConstraint))]
[JsonDerivedType(typeof(DifficultyLevelConstraint), nameof(DifficultyLevelConstraint))]
[JsonDerivedType(typeof(IttoryuConstraint), nameof(IttoryuConstraint))]
[JsonDerivedType(typeof(IttoryuLengthConstraint), nameof(IttoryuLengthConstraint))]
[JsonDerivedType(typeof(MinimalConstraint), nameof(MinimalConstraint))]
[JsonDerivedType(typeof(PearlConstraint), nameof(PearlConstraint))]
[JsonDerivedType(typeof(SymmetryConstraint), nameof(SymmetryConstraint))]
[JsonDerivedType(typeof(TechniqueConstraint), nameof(TechniqueConstraint))]
[Equals(OtherModifiers = "sealed")]
[GetHashCode(GetHashCodeBehavior.MakeAbstract)]
[ToString(ToStringBehavior.MakeAbstract)]
[EqualityOperators]
public abstract partial class Constraint : IEquatable<Constraint>, IEqualityOperators<Constraint, Constraint, bool>
{
	/// <summary>
	/// Determine whether the specified grid is passed the constraint.
	/// </summary>
	/// <param name="context">Indicates the context used.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Check(scoped ConstraintCheckingContext context) => ValidateCore().IsSuccess && CheckCore(context);

	/// <inheritdoc/>
	public abstract bool Equals([NotNullWhen(true)] Constraint? other);

	/// <summary>
	/// Determine whether the current constraint will raise a confliction with the specified constraint.
	/// By default, the method always return <see cref="ConflictionResult.Successful"/>.
	/// </summary>
	/// <param name="other">The constraint to be checked.</param>
	/// <returns>A <see cref="bool"/> indicating whether the current constraint will conflict with the specified one.</returns>
	/// <seealso cref="ConflictionResult.Successful"/>
	public virtual ConflictionResult VerifyConfliction(Constraint other) => ConflictionResult.Successful;

	/// <inheritdoc cref="Check"/>
	/// <remarks><i>
	/// This method only handles for the core rule of the type, which means we should suppose the values are valid.
	/// </i></remarks>
	protected internal abstract bool CheckCore(scoped ConstraintCheckingContext context);

	/// <summary>
	/// Verifies the validity of properties set.
	/// </summary>
	/// <returns>A <see cref="ValidationResult"/> instance describing the final result on validation.</returns>
	protected internal abstract ValidationResult ValidateCore();
}
