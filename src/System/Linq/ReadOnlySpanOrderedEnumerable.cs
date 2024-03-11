namespace System.Linq;

/// <summary>
/// Represents an enumerable instance that is based on a <see cref="ReadOnlySpan{T}"/>.
/// </summary>
/// <typeparam name="T">Indicates the type of each element.</typeparam>
/// <param name="values">Indicates the values.</param>
/// <param name="selectors">
/// Indicates the selector functions that return <typeparamref name="T"/> instances, to be used as comparison.
/// </param>
[StructLayout(LayoutKind.Auto)]
[DebuggerStepThrough]
[Equals]
[GetHashCode]
[ToString]
[method: MethodImpl(MethodImplOptions.AggressiveInlining)]
public readonly ref partial struct ReadOnlySpanOrderedEnumerable<T>(
	[PrimaryConstructorParameter(MemberKinds.Field)] ReadOnlySpan<T> values,
	[PrimaryConstructorParameter(MemberKinds.Field)] params Func<T, T, int>[] selectors
)
{
	/// <summary>
	/// Indicates the number of elements stored in the collection.
	/// </summary>
	public int Length => _values.Length;

	/// <summary>
	/// Creates an ordered <see cref="Span{T}"/> instance.
	/// </summary>
	/// <returns>An ordered <see cref="Span{T}"/> instance, whose value is from the current enumerable instance.</returns>
	private Span<T> OrderedSpan
	{
		get
		{
			// Copy field in order to make the variable can be used inside lambda.
			var selectors = _selectors;

			// Sort the span of values.
			var result = new T[_values.Length].AsSpan();
			_values.CopyTo(result);
			result.Sort(
				(l, r) =>
				{
					foreach (var selector in selectors)
					{
						if (selector(l, r) is var tempResult and not 0)
						{
							return tempResult;
						}
					}
					return 0;
				}
			);

			return result;
		}
	}


	/// <inheritdoc cref="ReadOnlySpan{T}.this[int]"/>
	public ref readonly T this[int index] => ref _values[index];


	/// <summary>
	/// Projects each element into a new transform.
	/// </summary>
	/// <typeparam name="TResult">The type of the result values.</typeparam>
	/// <param name="selector">The selector to be used by transforming the <typeparamref name="T"/> instances.</param>
	/// <returns>A span of <typeparamref name="TResult"/> values.</returns>
	public ReadOnlySpan<TResult> Select<TResult>(Func<T, TResult> selector)
	{
		var result = new List<TResult>(_values.Length);
		foreach (var element in OrderedSpan)
		{
			result.Add(selector(element));
		}
		return result.AsReadOnlySpan();
	}

	/// <summary>
	/// Filters the collection using the specified condition.
	/// </summary>
	/// <param name="condition">The condition to be used.</param>
	/// <returns>A span of <typeparamref name="T"/> instances.</returns>
	public ReadOnlySpan<T> Where(Func<T, bool> condition)
	{
		var result = new List<T>(_values.Length);
		foreach (var element in OrderedSpan)
		{
			if (condition(element))
			{
				result.Add(element);
			}
		}
		return result.AsReadOnlySpan();
	}

	/// <inheritdoc cref="Enumerable.ThenBy{TSource, TKey}(IOrderedEnumerable{TSource}, Func{TSource, TKey})"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpanOrderedEnumerable<T> ThenBy<TKey>(Func<T, TKey> selector) where TKey : IComparable<TKey>
		=> new(_values, [.. _selectors, (l, r) => selector(l).CompareTo(selector(r))]);

	/// <inheritdoc cref="Enumerable.ThenByDescending{TSource, TKey}(IOrderedEnumerable{TSource}, Func{TSource, TKey})"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpanOrderedEnumerable<T> ThenByDescending<TKey>(Func<T, TKey> selector) where TKey : IComparable<TKey>
		=> new(_values, [.. _selectors, (l, r) => -selector(l).CompareTo(selector(r))]);

	/// <inheritdoc cref="ReadOnlySpan{T}.GetEnumerator"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Span<T>.Enumerator GetEnumerator() => OrderedSpan.GetEnumerator();
}
