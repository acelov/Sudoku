namespace Sudoku.Analytics;

/// <summary>
/// Represents a searcher that can creates <see cref="Step"/> instances for the specified technique.
/// </summary>
/// <param name="priority">
/// <para>
/// Indicates the priority value of the current step searcher.
/// This property is used for sorting multiple <see cref="StepSearcher"/> instances.
/// </para>
/// <para>
/// Please note that the set value cannot be same for different <see cref="StepSearcher"/> types;
/// otherwise, <see cref="InvalidOperationException"/> will be thrown while comparing with two <see cref="StepSearcher"/>s.
/// </para>
/// <para>
/// This property may be automatically generated by source generator. Therefore, you may not care about implementation of this property.
/// </para>
/// </param>
/// <param name="level">
/// <para>Indicates the level that the current step searcher belongs to.</para>
/// <para>
/// This property indicates how difficult the step searcher can be enabled.
/// </para>
/// </param>
/// <param name="runningArea">
/// <para>Indicates the running area which describes a function where the current step searcher can be invoked.</para>
/// <para>
/// By default, the step searcher will support
/// both <see cref="StepSearcherRunningArea.Searching"/> and <see cref="StepSearcherRunningArea.Collecting"/>.
/// </para>
/// </param>
/// <seealso cref="Step"/>
[TypeImpl(
	TypeImplFlags.AllObjectMethods | TypeImplFlags.AllEqualityComparisonOperators,
	OtherModifiersOnEquals = "sealed",
	OtherModifiersOnGetHashCode = "sealed",
	OtherModifiersOnToString = "sealed")]
public abstract partial class StepSearcher(
	[Property, HashCodeMember] int priority,
	[Property] int level,
	[Property] StepSearcherRunningArea runningArea = StepSearcherRunningArea.Searching | StepSearcherRunningArea.Collecting
) :
	IComparable<StepSearcher>,
	IComparisonOperators<StepSearcher, StepSearcher, bool>,
	IEquatable<StepSearcher>,
	IEqualityOperators<StepSearcher, StepSearcher, bool>,
	IFormattable
{
	/// <summary>
	/// Indicates the implementation details of the current step searcher instance.
	/// </summary>
	public StepSearcherMetadataInfo Metadata => new(this, GetType().GetCustomAttribute<StepSearcherAttribute>()!);


	/// <summary>
	/// Determines whether two <see cref="StepSearcher"/> instances hold a same priority value.
	/// </summary>
	/// <param name="other">The other object to be compared.</param>
	/// <returns>A <see cref="bool"/> result indicating that.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals([NotNullWhen(true)] StepSearcher? other) => other is not null && Priority == other.Priority;

	/// <summary>
	/// Compares priority value of two <see cref="StepSearcher"/> instances, and returns the less one.
	/// </summary>
	/// <param name="other">The other object to be compared.</param>
	/// <returns>An <see cref="int"/> indicating which one is greater.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int CompareTo(StepSearcher? other) => other is null ? -1 : Priority.CompareTo(other.Priority);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(IFormatProvider? formatProvider) => Metadata.GetName(formatProvider as CultureInfo);

	/// <summary>
	/// Try to collect all available <see cref="Step"/>s using the current technique rule.
	/// </summary>
	/// <param name="context">
	/// <para>
	/// The analysis context. This argument offers you some elementary data configured or assigned, for the current loop of step searching.
	/// </para>
	/// <para>
	/// All available <see cref="Step"/> results will be stored in property <see cref="StepAnalysisContext.Accumulator"/>
	/// of this argument, if property <see cref="StepAnalysisContext.OnlyFindOne"/> returns <see langword="false"/>;
	/// otherwise, the property won't be used, and this method will return the first found step.
	/// </para>
	/// </param>
	/// <returns>
	/// Returns the first found step. The nullability of the return value is described as follow:
	/// <list type="bullet">
	/// <item>
	/// <see langword="null"/>:
	/// <list type="bullet">
	/// <item><c><paramref name="context"/>.OnlyFindOne == <see langword="false"/></c>.</item>
	/// <item><c><paramref name="context"/>.OnlyFindOne == <see langword="true"/></c>, but nothing found.</item>
	/// </list>
	/// </item>
	/// <item>
	/// Not <see langword="null"/>:
	/// <list type="bullet">
	/// <item>
	/// <c><paramref name="context"/>.OnlyFindOne == <see langword="true"/></c>,
	/// and found <b>at least one step</b>. In this case the return value is the first found step.
	/// </item>
	/// </list>
	/// </item>
	/// </list>
	/// </returns>
	/// <seealso cref="Step"/>
	/// <seealso cref="StepAnalysisContext"/>
	protected internal abstract Step? Collect(ref StepAnalysisContext context);

	/// <inheritdoc/>
	string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString(formatProvider);
}
