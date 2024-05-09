namespace Sudoku.Concepts;

/// <summary>
/// Represents a fish pattern.
/// </summary>
/// <param name="digit">Indicates the digit to be used.</param>
/// <param name="baseSets">Indicates the base sets.</param>
/// <param name="coverSets">Indicates the cover sets.</param>
/// <param name="exofins">Indicates the exo-fins.</param>
/// <param name="endofins">Indicates the endo-fins.</param>
[LargeStructure]
[Equals]
[GetHashCode]
[EqualityOperators]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct Fish(
	[PrimaryConstructorParameter, HashCodeMember] Digit digit,
	[PrimaryConstructorParameter, HashCodeMember] HouseMask baseSets,
	[PrimaryConstructorParameter, HashCodeMember] HouseMask coverSets,
	[PrimaryConstructorParameter, HashCodeMember] ref readonly CellMap exofins,
	[PrimaryConstructorParameter, HashCodeMember] ref readonly CellMap endofins
) : IEquatable<Fish>, IEqualityOperators<Fish, Fish, bool>, ISudokuConceptConvertible<Fish>
{
	/// <summary>
	/// Indicates whether the pattern is complex fish.
	/// </summary>
	public bool IsComplex => Endofins is not [];


	/// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(ref readonly Fish other)
		=> (Digit, BaseSets, CoverSets, Exofins, Endofins) == (other.Digit, other.BaseSets, other.CoverSets, other.Exofins, other.Endofins);

	/// <inheritdoc cref="object.ToString"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString() => ToString(new RxCyConverter());

	/// <inheritdoc/>
	public string ToString<T>(T converter) where T : CoordinateConverter
	{
		switch (converter)
		{
			case RxCyConverter c:
			{
				// Special optimization.
				var bs = c.HouseConverter(BaseSets);
				var cs = c.HouseConverter(CoverSets);
				var exofins = this switch
				{
					{ Exofins: var f and not [] } => $" f{c.CellConverter(f)} ",
					_ => string.Empty
				};
				var endofins = this switch
				{
					{ Endofins: var e and not [] } => $"ef{c.CellConverter(e)}",
					_ => string.Empty
				};
				return $@"{c.DigitConverter((Mask)(1 << Digit))} {bs}\{cs}{exofins}{endofins}";
			}
			case var c:
			{
				var exofinsAre = ResourceDictionary.Get("ExofinsAre", converter.CurrentCulture);
				var comma = ResourceDictionary.Get("Comma", converter.CurrentCulture);
				var digitString = c.DigitConverter((Mask)(1 << Digit));
				var bs = c.HouseConverter(BaseSets);
				var cs = c.HouseConverter(CoverSets);
				var exofins = this switch
				{
					{ Exofins: var f and not [] } => $"{comma}{string.Format(exofinsAre, c.CellConverter(f))}",
					_ => string.Empty
				};
				var endofins = this switch
				{
					{ Endofins: var e and not [] } => $"{comma}{string.Format(exofinsAre, c.CellConverter(e))}",
					_ => string.Empty
				};
				return $@"{c.DigitConverter((Mask)(1 << Digit))}{comma}{bs}\{cs}{exofins}{endofins}";
			}
		}
	}

	/// <inheritdoc/>
	bool IEquatable<Fish>.Equals(Fish other) => Equals(in other);
}