namespace Sudoku.Strategying;

/// <summary>
/// Represents a rule that checks whether a grid or its relied analysis information is passed the constraint.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$typeid", UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization)]
[JsonDerivedType(typeof(BottleneckStepRatingConstraint), nameof(BottleneckStepRatingConstraint))]
[JsonDerivedType(typeof(BottleneckTechniqueConstraint), nameof(BottleneckTechniqueConstraint))]
[JsonDerivedType(typeof(ConclusionConstraint), nameof(ConclusionConstraint))]
[JsonDerivedType(typeof(CountBetweenConstraint), nameof(CountBetweenConstraint))]
[JsonDerivedType(typeof(DiamondConstraint), nameof(DiamondConstraint))]
[JsonDerivedType(typeof(DifficultyLevelConstraint), nameof(DifficultyLevelConstraint))]
[JsonDerivedType(typeof(EliminationCountConstraint), nameof(EliminationCountConstraint))]
[JsonDerivedType(typeof(IttoryuConstraint), nameof(IttoryuConstraint))]
[JsonDerivedType(typeof(IttoryuLengthConstraint), nameof(IttoryuLengthConstraint))]
[JsonDerivedType(typeof(LastingConstraint), nameof(LastingConstraint))]
[JsonDerivedType(typeof(MinimalConstraint), nameof(MinimalConstraint))]
[JsonDerivedType(typeof(PearlConstraint), nameof(PearlConstraint))]
[JsonDerivedType(typeof(PrimarySingleConstraint), nameof(PrimarySingleConstraint))]
[JsonDerivedType(typeof(SymmetryConstraint), nameof(SymmetryConstraint))]
[JsonDerivedType(typeof(TechniqueConstraint), nameof(TechniqueConstraint))]
[JsonDerivedType(typeof(TechniqueCountConstraint), nameof(TechniqueCountConstraint))]
[JsonDerivedType(typeof(TechniqueSetConstraint), nameof(TechniqueSetConstraint))]
[TypeImpl(
	TypeImplFlag.AllObjectMethods | TypeImplFlag.EqualityOperators,
	OtherModifiersOnEquals = "sealed",
	GetHashCodeBehavior = GetHashCodeBehavior.MakeAbstract,
	ToStringBehavior = ToStringBehavior.MakeAbstract)]
public abstract partial class Constraint : IEquatable<Constraint>, IEqualityOperators<Constraint, Constraint, bool>, IFormattable
{
	/// <summary>
	/// Indicates whether the constraint should be negated.
	/// </summary>
	public bool IsNegated { get; set; }


	/// <summary>
	/// Determine whether the specified grid is passed the constraint.
	/// </summary>
	/// <param name="context">Indicates the context used.</param>
	/// <returns>A <see cref="bool"/> result indicating that.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Check(ConstraintCheckingContext context) => CheckCore(context) is var result && (IsNegated ? !result : result);

	/// <inheritdoc/>
	public abstract bool Equals([NotNullWhen(true)] Constraint? other);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public abstract string ToString(IFormatProvider? formatProvider);

	/// <summary>
	/// Creates a new instance that contains same data with the current instance.
	/// </summary>
	/// <returns>A new instance that contains same data with the current instance.</returns>
	public abstract Constraint Clone();

	/// <summary>
	/// Indicates the checking <see cref="Expression{TDelegate}"/> instance of the constraint.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This property will be reserved, in order to combine multiple constraint checking rules by using custom boolean logic.
	/// For example, a user may want either one of two constraints satisfy rather than checking for both two constraints.
	/// By using this property, use <see cref="Expression.OrElse(Expression, Expression)"/> to achieve the goal.
	/// </para>
	/// <para>
	/// Due to implement this property and limitation of <see cref="Expression{TDelegate}"/>, we cannot apply the type
	/// <see cref="ConstraintCheckingContext"/> as a <see langword="ref struct"/> because <see cref="Expression{TDelegate}"/>
	/// doesn't support for <see langword="ref struct"/> types.
	/// </para>
	/// </remarks>
	/// <seealso cref="Expression{TDelegate}"/>
	/// <seealso cref="Expression.OrElse(Expression, Expression)"/>
	/// <seealso cref="ConstraintCheckingContext"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Expression<Func<Constraint, ConstraintCheckingContext, bool>> CreateCheckingQueryExpression()
		=> static (constraint, context) => constraint.CheckCore(context);

	/// <summary>
	/// Returns a <see cref="ConstraintOptionsAttribute"/> instance that represents the metadata of the constraint configured.
	/// </summary>
	/// <returns>A <see cref="ConstraintOptionsAttribute"/> instance or <see langword="null"/> if not configured.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ConstraintOptionsAttribute? GetMetadata() => GetType().GetCustomAttribute<ConstraintOptionsAttribute>();

	/// <inheritdoc cref="Check(ConstraintCheckingContext)"/>
	protected abstract bool CheckCore(ConstraintCheckingContext context);

	/// <inheritdoc/>
	string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString(formatProvider);
}