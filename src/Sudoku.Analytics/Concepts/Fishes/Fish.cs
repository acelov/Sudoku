namespace Sudoku.Concepts;

/// <summary>
/// Represents a fish pattern.
/// </summary>
/// <param name="digit">Indicates the digit to be used.</param>
/// <param name="baseSets">Indicates the base sets.</param>
/// <param name="coverSets">Indicates the cover sets.</param>
/// <param name="exofins">Indicates the exo-fins.</param>
/// <param name="endofins">Indicates the endo-fins.</param>
[StructLayout(LayoutKind.Auto)]
[TypeImpl(TypeImplFlag.AllObjectMethods | TypeImplFlag.EqualityOperators, IsLargeStructure = true)]
public readonly partial struct Fish(
	[PrimaryConstructorParameter, HashCodeMember] Digit digit,
	[PrimaryConstructorParameter, HashCodeMember] HouseMask baseSets,
	[PrimaryConstructorParameter, HashCodeMember] HouseMask coverSets,
	[PrimaryConstructorParameter, HashCodeMember] ref readonly CellMap exofins,
	[PrimaryConstructorParameter, HashCodeMember] ref readonly CellMap endofins
) : IEquatable<Fish>, IEqualityOperators<Fish, Fish, bool>, IFormattable
{
	/// <summary>
	/// Indicates whether the pattern is complex fish.
	/// </summary>
	public bool IsComplex => Endofins is not [];

	/// <summary>
	/// Indicates all fins.
	/// </summary>
	public CellMap Fins => Exofins | Endofins;

	/// <summary>
	/// Indicates the shape kind of the current fish.
	/// </summary>
	public FishShapeKind ShapeKind
	{
		get
		{
			return this switch
			{
				{ IsComplex: true, BaseSets: var baseSets, CoverSets: var coverSets } => (k(baseSets), k(coverSets)) switch
				{
					(FishShapeKind.Mutant, _) or (_, FishShapeKind.Mutant) => FishShapeKind.Mutant,
					(FishShapeKind.Franken, _) or (_, FishShapeKind.Franken) => FishShapeKind.Franken,
					_ => FishShapeKind.Basic
				},
				_ => FishShapeKind.Basic
			};


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static FishShapeKind k(HouseMask mask)
			{
				var (blockMask, rowMask, columnMask) = mask.SplitMask();
				return rowMask != 0 && columnMask != 0
					? FishShapeKind.Mutant
					: (Mask)(rowMask | columnMask) != 0 && blockMask != 0 ? FishShapeKind.Franken : FishShapeKind.Basic;
			}
		}
	}


	/// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(ref readonly Fish other)
		=> (Digit, BaseSets, CoverSets, Exofins, Endofins) == (other.Digit, other.BaseSets, other.CoverSets, other.Exofins, other.Endofins);

	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(IFormatProvider? formatProvider)
	{
		switch (CoordinateConverter.GetInstance(formatProvider))
		{
			case RxCyConverter c:
			{
				// Special optimization.
				var bs = c.HouseConverter(BaseSets);
				var cs = c.HouseConverter(CoverSets);
				var exofins = this switch
				{
					{ Exofins: var f and not [] } => $" f{c.CellConverter(in f)}",
					_ => string.Empty
				};
				var endofins = this switch
				{
					{ Endofins: var e and not [] } => $"ef{c.CellConverter(in e)}",
					_ => string.Empty
				};
				return $@"{c.DigitConverter((Mask)(1 << Digit))} {bs}\{cs}{(string.IsNullOrEmpty(endofins) ? exofins : $"{exofins} ")}{endofins}";
			}
			case var c:
			{
				var exofinsAre = SR.Get("ExofinsAre", c.CurrentCulture);
				var comma = SR.Get("Comma", c.CurrentCulture);
				var digitString = c.DigitConverter((Mask)(1 << Digit));
				var bs = c.HouseConverter(BaseSets);
				var cs = c.HouseConverter(CoverSets);
				var exofins = this switch
				{
					{ Exofins: var f and not [] } => $"{comma}{string.Format(exofinsAre, c.CellConverter(in f))}",
					_ => string.Empty
				};
				var endofins = this switch
				{
					{ Endofins: var e and not [] } => $"{comma}{string.Format(exofinsAre, c.CellConverter(in e))}",
					_ => string.Empty
				};
				return $@"{c.DigitConverter((Mask)(1 << Digit))}{comma}{bs}\{cs}{exofins}{endofins}";
			}
		}
	}

	/// <summary>
	/// Try to get the fin kind using the specified grid as candidate references.
	/// </summary>
	/// <param name="grid">The grid to be used.</param>
	/// <returns>The fin kind.</returns>
	public FishFinKind GetFinKind(ref readonly Grid grid)
	{
		var fins = Fins;
		if (!fins)
		{
			return FishFinKind.Normal;
		}

		var candidatesMap = grid.CandidatesMap;
		foreach (var baseSet in BaseSets)
		{
			if ((HousesMap[baseSet] & ~fins & candidatesMap[Digit]).Count == 1)
			{
				return FishFinKind.Sashimi;
			}
		}
		return FishFinKind.Finned;
	}

	/// <inheritdoc/>
	bool IEquatable<Fish>.Equals(Fish other) => Equals(in other);

	/// <inheritdoc/>
	string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString(formatProvider);
}
