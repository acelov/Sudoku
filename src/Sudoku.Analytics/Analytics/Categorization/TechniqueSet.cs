namespace Sudoku.Analytics.Categorization;

/// <summary>
/// Represents a set of <see cref="Technique"/> fields.
/// </summary>
/// <remarks>
/// This type uses a <see cref="BitArray"/> to construct the data structure. Because <see cref="BitArray"/> is a reference type,
/// we cannot directly copy a <see cref="TechniqueSet"/> value. If you want to clone a <see cref="TechniqueSet"/>,
/// please use <c>[..]</c> syntax:
/// <code><![CDATA[
/// var techniques = TechniqueSets.All;
/// var another = techniques[..]; // Copy all elements.
/// ]]></code>
/// </remarks>
/// <seealso cref="Technique"/>
/// <seealso cref="BitArray"/>
/// <completionlist cref="TechniqueSets"/>
[JsonConverter(typeof(Converter))]
[Equals]
[EqualityOperators]
public sealed partial class TechniqueSet :
	IAdditionOperators<TechniqueSet, TechniqueGroup, TechniqueSet>,
	IBitwiseOperators<TechniqueSet, TechniqueSet, TechniqueSet>,
	ICollection<Technique>,
	ICultureFormattable,
	IEnumerable<Technique>,
	IEquatable<TechniqueSet>,
	IEqualityOperators<TechniqueSet, TechniqueSet, bool>,
	ILogicalOperators<TechniqueSet>,
	IReadOnlyCollection<Technique>,
	IReadOnlySet<Technique>,
	ISet<Technique>,
	ISubtractionOperators<TechniqueSet, TechniqueSet, TechniqueSet>
{
	/// <summary>
	/// Indicates the information for the techniques, can lookup the relation via its belonging technique group.
	/// This field will be used in extension method
	/// <see cref="TechniqueGroupExtensions.GetTechniques(TechniqueGroup, Func{Technique, bool}?)"/>.
	/// </summary>
	/// <seealso cref="TechniqueGroupExtensions.GetTechniques(TechniqueGroup, Func{Technique, bool}?)"/>
	public static readonly FrozenDictionary<TechniqueGroup, TechniqueSet> TechniqueRelationGroups;

	/// <summary>
	/// Indicates the technique groups and its containing techniques that supports customization on difficulty rating and level.
	/// Call <see cref="TechniqueExtensions.SupportsCustomizingDifficulty(Technique)"/>
	/// to check whether a technique supports configuration.
	/// </summary>
	/// <seealso cref="TechniqueExtensions.SupportsCustomizingDifficulty(Technique)"/>
	public static readonly FrozenDictionary<TechniqueGroup, TechniqueSet> ConfigurableTechniqueRelationGroups;

	/// <summary>
	/// Indicates the number of techniques included in this solution.
	/// </summary>
	private static readonly int TechniquesCount = Enum.GetValues<Technique>().Length - 1;


	/// <summary>
	/// The internal bits to store techniques.
	/// </summary>
	private readonly BitArray _techniqueBits;


	/// <summary>
	/// Initializes a <see cref="TechniqueSet"/> instance.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public TechniqueSet() => _techniqueBits = new(TechniquesCount);

	/// <summary>
	/// Copies a <see cref="TechniqueSet"/> instance, and adds it to the current collection.
	/// </summary>
	/// <param name="other">The other collection to be added.</param>
	public TechniqueSet(TechniqueSet other) : this()
	{
		foreach (var technique in other)
		{
			Add(technique);
		}
	}


	/// <include file='../../global-doc-comments.xml' path='g/static-constructor' />
	static TechniqueSet()
	{
		var dic = new Dictionary<TechniqueGroup, TechniqueSet>();
		foreach (var technique in Enum.GetValues<Technique>())
		{
			if (technique != Technique.None && technique.TryGetGroup() is { } group && !dic.TryAdd(group, [technique]))
			{
				dic[group].Add(technique);
			}
		}
		TechniqueRelationGroups = dic.ToFrozenDictionary();

		var configurableDic = new Dictionary<TechniqueGroup, TechniqueSet>();
		foreach (var technique in Enum.GetValues<Technique>())
		{
			if (technique.SupportsCustomizingDifficulty()
				&& technique.TryGetGroup() is { } group && !configurableDic.TryAdd(group, [technique]))
			{
				configurableDic[group].Add(technique);
			}
		}
		ConfigurableTechniqueRelationGroups = configurableDic.ToFrozenDictionary();
	}


	/// <summary>
	/// Indicates the length of the technique.
	/// </summary>
	public int Count => _techniqueBits.GetCardinality();

	/// <summary>
	/// Indicates the range of difficulty that the current collection containss.
	/// </summary>
	/// <remarks>
	/// This property returns a list of <see cref="DifficultyLevel"/> flags, merged into one instance.
	/// If you want to get the internal fields of flags the return value contains, use <see langword="foreach"/> loop to iterate them,
	/// or use method <see cref="EnumExtensions.GetAllFlags{T}(T)"/>.
	/// </remarks>
	/// <seealso cref="EnumExtensions.GetAllFlags{T}(T)"/>
	public DifficultyLevel DifficultyRange
	{
		get
		{
			var result = DifficultyLevel.Unknown;
			if (Count == 0)
			{
				return result;
			}

			foreach (var technique in this)
			{
				result |= technique.GetDifficultyLevel();
			}

			return result;
		}
	}

	/// <inheritdoc/>
	bool ICollection<Technique>.IsReadOnly => false;


	/// <summary>
	/// Try to get the <see cref="Technique"/> at the specified index.
	/// </summary>
	/// <param name="index">The index to be checked.</param>
	/// <returns>The found <see cref="Technique"/> instance.</returns>
	/// <exception cref="IndexOutOfRangeException">Throws when the index is out of range.</exception>
	public Technique this[int index]
	{
		get
		{
			var i = -1;
			foreach (var technique in this)
			{
				if (++i == index)
				{
					return technique;
				}
			}

			throw new IndexOutOfRangeException();
		}
	}

	/// <summary>
	/// Checks the index of the specified technique.
	/// </summary>
	/// <param name="technique">The technique to be checked.</param>
	/// <returns>The index that the technique is at. If none found, -1.</returns>
	public int this[Technique technique]
	{
		get
		{
			var result = 0;
			foreach (var currentTechnique in this)
			{
				if (currentTechnique == technique)
				{
					return result;
				}

				result++;
			}

			return -1;
		}
	}


	/// <summary>
	/// Clears the collection, making all techniques to be removed.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Clear() => _techniqueBits.SetAll(false);

	/// <inheritdoc/>
	public bool Equals([NotNullWhen(true)] TechniqueSet? other)
	{
		if (other is null)
		{
			return false;
		}

		if (Count != other.Count)
		{
			return false;
		}

		foreach (var technique in this)
		{
			if (!other.Contains(technique))
			{
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Determines whether the specified technique is in the collection.
	/// </summary>
	/// <param name="item">The technique.</param>
	/// <returns>A <see cref="bool"/> result indicating that.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(Technique item) => _techniqueBits[TechniqueProjection(item)];

	/// <summary>
	/// Try to add a new technique.
	/// </summary>
	/// <param name="item">A technique to be added.</param>
	/// <returns>A <see cref="bool"/> result indicating whether the current technique is successfully added.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Add(Technique item)
	{
		if (_techniqueBits[TechniqueProjection(item)])
		{
			return false;
		}

		_techniqueBits.Set(TechniqueProjection(item), true);
		return true;
	}

	/// <summary>
	/// Try to remove a technique from the collection.
	/// </summary>
	/// <param name="item">A technique to be removed.</param>
	/// <returns>A <see cref="bool"/> result indicating whether the current technique is successfully removed.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Remove(Technique item)
	{
		if (!_techniqueBits[TechniqueProjection(item)])
		{
			return false;
		}

		_techniqueBits.Set(TechniqueProjection(item), false);
		return true;
	}

	/// <inheritdoc/>
	public override int GetHashCode()
	{
		var result = 0;
		var flag = false;
		foreach (var technique in this)
		{
			var target = (int)technique.GetGroup() * 1000000 + (int)technique;
			result |= flag ? target >> 10 : target;
			result += flag ? 7 : 31;

			flag = !flag;
		}

		return result;
	}

	/// <inheritdoc cref="object.ToString"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString() => ToString(null);

	/// <inheritdoc/>
	public string ToString(CultureInfo? culture = null)
	{
		var currentCountryOrRegionName = (culture ?? CultureInfo.CurrentUICulture).Parent.Name;
		var isCurrentCountryOrRegionUseEnglish = currentCountryOrRegionName.Equals(EnglishLanguage, StringComparison.OrdinalIgnoreCase);
		return string.Join(
			ResourceDictionary.Get("Comma", culture),
			[
				..
				from technique in this
				let name = technique.GetName(culture)
				select isCurrentCountryOrRegionUseEnglish ? $"{name}" : $"{name} ({technique.GetEnglishName()})"
			]
		);
	}

	/// <summary>
	/// Converts the current collection into an array.
	/// </summary>
	/// <returns>The final array converted.</returns>
	public Technique[] ToArray() => [.. this];

	/// <summary>
	/// Forms a slice out of the current collection starting at a specified index for a specified length.
	/// </summary>
	/// <param name="start"><inheritdoc cref="ReadOnlySpan{T}.Slice(int)" path="/param[@name='start']"/></param>
	/// <param name="count"><inheritdoc cref="ReadOnlySpan{T}.Slice(int, int)" path="/param[@name='length']"/></param>
	/// <returns>
	/// A new <see cref="TechniqueSet"/> that consists of all elements of the current collection
	/// from <paramref name="start"/> to the end of the slicing, given by <paramref name="count"/>.
	/// </returns>
	public TechniqueSet Slice(int start, int count)
	{
		var result = TechniqueSets.None;
		var i = start - 1;
		foreach (var technique in this)
		{
			if (++i < start + count)
			{
				result.Add(technique);
			}
		}

		return result;
	}

	/// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Enumerator GetEnumerator() => new(_techniqueBits);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	void ICollection<Technique>.CopyTo(Technique[] array, int arrayIndex)
		=> Array.Copy(this[arrayIndex..].ToArray(), array, Count - arrayIndex);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	bool ISet<Technique>.IsProperSubsetOf(IEnumerable<Technique> other) => ((IReadOnlySet<Technique>)this).IsProperSubsetOf(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	bool ISet<Technique>.IsProperSupersetOf(IEnumerable<Technique> other) => ((IReadOnlySet<Technique>)this).IsProperSupersetOf(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	bool ISet<Technique>.IsSubsetOf(IEnumerable<Technique> other) => ((IReadOnlySet<Technique>)this).IsSubsetOf(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	bool ISet<Technique>.IsSupersetOf(IEnumerable<Technique> other) => ((IReadOnlySet<Technique>)this).IsSupersetOf(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	bool ISet<Technique>.Overlaps(IEnumerable<Technique> other) => ((IReadOnlySet<Technique>)this).Overlaps(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	bool ISet<Technique>.SetEquals(IEnumerable<Technique> other) => ((IReadOnlySet<Technique>)this).SetEquals(other);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	bool IReadOnlySet<Technique>.IsProperSubsetOf(IEnumerable<Technique> other)
	{
		var otherSet = (TechniqueSet)([.. other]);
		return (otherSet & this) == this && this != otherSet;
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	bool IReadOnlySet<Technique>.IsProperSupersetOf(IEnumerable<Technique> other)
	{
		var otherSet = (TechniqueSet)([.. other]);
		return (this & otherSet) == otherSet && this != otherSet;
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	bool IReadOnlySet<Technique>.IsSubsetOf(IEnumerable<Technique> other) => ([.. other] & this) == this;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	bool IReadOnlySet<Technique>.IsSupersetOf(IEnumerable<Technique> other)
	{
		var otherSet = (TechniqueSet)([.. other]);
		return (this & otherSet) == otherSet;
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	bool IReadOnlySet<Technique>.Overlaps(IEnumerable<Technique> other) => (this & [.. other]).Count != 0;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	bool IReadOnlySet<Technique>.SetEquals(IEnumerable<Technique> other) => this == [.. other];

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	IEnumerator IEnumerable.GetEnumerator() => _techniqueBits.GetEnumerator();

	/// <inheritdoc/>
	IEnumerator<Technique> IEnumerable<Technique>.GetEnumerator()
	{
		var index = 0;
		foreach (bool techniqueBit in _techniqueBits)
		{
			if (techniqueBit)
			{
				yield return TechniqueProjectionBack(index);
			}
			index++;
		}
	}

	/// <inheritdoc/>
	void ISet<Technique>.ExceptWith(IEnumerable<Technique> other)
	{
		foreach (var technique in other)
		{
			Remove(technique);
		}
	}

	/// <inheritdoc/>
	void ISet<Technique>.IntersectWith(IEnumerable<Technique> other)
	{
		foreach (var technique in other)
		{
			if (!Contains(technique))
			{
				Remove(technique);
			}
		}
	}

	/// <inheritdoc/>
	void ISet<Technique>.SymmetricExceptWith(IEnumerable<Technique> other)
	{
		var otherSet = (TechniqueSet)([.. other]);

		Clear();
		foreach (var technique in (this - otherSet) | (otherSet - this))
		{
			Add(technique);
		}
	}

	/// <inheritdoc/>
	void ISet<Technique>.UnionWith(IEnumerable<Technique> other)
	{
		foreach (var technique in other)
		{
			Add(technique);
		}
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	void ICollection<Technique>.Add(Technique item) => Add(item);


	/// <summary>
	/// Project the <see cref="Technique"/> instance into an <see cref="int"/> value as an index of <see cref="BitArray"/> field.
	/// </summary>
	/// <param name="technique">The techniuqe.</param>
	/// <returns>The index value.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int TechniqueProjection(Technique technique) => (int)technique - 1;

	/// <summary>
	/// Projects the <see cref="int"/> value into a <see cref="Technique"/> field back.
	/// </summary>
	/// <param name="index">The index.</param>
	/// <returns>The technique field.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Technique TechniqueProjectionBack(int index) => (Technique)index + 1;


	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !(TechniqueSet techniques) => techniques.Count == 0;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator true(TechniqueSet techniques) => techniques.Count != 0;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator false(TechniqueSet techniques) => techniques.Count == 0;

	/// <summary>
	/// Adds a new technique into the specified collection.
	/// </summary>
	/// <param name="left">The technique set.</param>
	/// <param name="right">The technique to be added.</param>
	/// <returns>
	/// A new collection that contains the values from the collection <paramref name="left"/>,
	/// with a new value <paramref name="right"/> added.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TechniqueSet operator +(TechniqueSet left, Technique right) => [.. left, right];

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TechniqueSet operator +(TechniqueSet left, TechniqueGroup right) => [.. left, .. TechniqueRelationGroups[right]];

	/// <summary>
	/// Removes a technique from the specified collection.
	/// </summary>
	/// <param name="left">The technique set.</param>
	/// <param name="right">A technique to be removed.</param>
	/// <returns>
	/// A new collection that contains the values from the collection <paramref name="left"/>,
	/// with a technique <paramref name="right"/> removed.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TechniqueSet operator -(TechniqueSet left, Technique right)
	{
		var result = left[..];
		result.Remove(right);
		return result;
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TechniqueSet operator -(TechniqueSet left, TechniqueSet right) => left & ~right;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TechniqueSet operator ~(TechniqueSet value)
	{
		var result = value[..];
		result._techniqueBits.Not();
		return result;
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TechniqueSet operator &(TechniqueSet left, TechniqueSet right)
	{
		var result = left[..];
		result._techniqueBits.And(right._techniqueBits);
		return result;
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TechniqueSet operator |(TechniqueSet left, TechniqueSet right)
	{
		var result = left[..];
		result._techniqueBits.Or(right._techniqueBits);
		return result;
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TechniqueSet operator ^(TechniqueSet left, TechniqueSet right)
	{
		var result = left[..];
		result._techniqueBits.Xor(right._techniqueBits);
		return result;
	}
}

/// <summary>
/// Defines a JSON converter for the current type.
/// </summary>
file sealed class Converter : JsonConverter<TechniqueSet>
{
	/// <inheritdoc/>
	public override TechniqueSet Read(scoped ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var (result, isInitialized) = (TechniqueSets.None, false);
		while (reader.Read())
		{
			switch (reader.TokenType)
			{
				case JsonTokenType.StartArray when !isInitialized:
				{
					isInitialized = true;
					break;
				}
				case JsonTokenType.String when reader.GetString() is { } s && Enum.TryParse(s, out Technique technique):
				{
					result.Add(technique);
					break;
				}
				case JsonTokenType.EndArray:
				{
					return result;
				}
				default:
				{
					throw new JsonException();
				}
			}
		}

		throw new JsonException();
	}

	/// <inheritdoc/>
	public override void Write(Utf8JsonWriter writer, TechniqueSet value, JsonSerializerOptions options)
	{
		writer.WriteStartArray();
		foreach (var technique in value)
		{
			writer.WriteStringValue(technique.ToString());
		}
		writer.WriteEndArray();
	}
}
