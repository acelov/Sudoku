namespace System.Collections.Generic;

/// <summary>
/// Provides extension methods on <see cref="List{T}"/>.
/// </summary>
/// <seealso cref="List{T}"/>
public static class ListExtensions
{
#if NET9_0_OR_GREATER
	/// <summary>
	/// Adds an object to the end of the <see cref="List{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of elements in the list.</typeparam>
	/// <param name="this">The current instance.</param>
	/// <param name="item">The object to be added to the end of the <see cref="List{T}"/>.</param>
	/// <remarks>
	/// <include
	///     file="../../global-doc-comments.xml"
	///     path="//g/dotnet/version[@value='8']/feature[@name='unsafe-accessor']/target[@name='others']"/>
	/// <include
	///     file="../../global-doc-comments.xml"
	///     path="//g/dotnet/version[@value='9' and @preview-value='4']/feature[@name='unsafe-accessor']"/>
	/// <para>
	/// This method passes with a reference to an object, which is unnecessary to be called by a referece-typed object,
	/// or a value-typed object whose memory size is less than a pointer. <b>Always measure the necessity of the usage.</b>
	/// </para>
	/// </remarks>
	/// <seealso cref="UnsafeAccessorAttribute"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void AddRef<T>(this List<T> @this, scoped ref readonly T item)
	{
		@this.GetVersion()++;
		var array = @this.GetItems().AsSpan();
		var size = @this.GetSize();
		if ((uint)size < (uint)array.Length)
		{
			@this.GetSize()++;
			array[size] = item;
		}
		else
		{
			@this.AddWithResize(in item);
		}
	}

	/// <summary>
	/// Adds the elements of the specified <see cref="ReadOnlySpan{T}"/> to the end of the <see cref="List{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of elements in the list.</typeparam>
	/// <param name="this">The current instance.</param>
	/// <param name="items">The collection whose elements should be added to the end of the <see cref="List{T}"/>.</param>
	/// <remarks>
	/// <include
	///     file="../../global-doc-comments.xml"
	///     path="//g/dotnet/version[@value='8']/feature[@name='unsafe-accessor']/target[@name='others']"/>
	/// <include
	///     file="../../global-doc-comments.xml"
	///     path="//g/dotnet/version[@value='9' and @preview-value='4']/feature[@name='unsafe-accessor']"/>
	/// <para>
	/// This method passes with a <see cref="ReadOnlySpan{T}"/> instead of <see cref="IEnumerable{T}"/>,
	/// allowing iterating on read-only references of collection elements, which is unnecessary to be called
	/// by a reference-typed object, or a value-typed object whose memory size is less than a pointer.
	/// <b>Always measure the necessity of the usage.</b>
	/// </para>
	/// </remarks>
	/// <seealso cref="UnsafeAccessorAttribute"/>
	/// <seealso cref="ReadOnlySpan{T}"/>
	/// <seealso cref="IEnumerable{T}"/>
	public static void AddRangeRef<T>(this List<T> @this, params ReadOnlySpan<T> items)
	{
		foreach (ref readonly var item in items)
		{
			@this.AddRef(in item);
		}
	}
#endif

	/// <summary>
	/// Removes the last element stored in the current <see cref="List{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of each element.</typeparam>
	/// <param name="this">The list to be modified.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Remove<T>(this List<T> @this) => @this.RemoveAt(@this.Count - 1);

	/// <inheritdoc cref="List{T}.RemoveAt(int)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RemoveAt<T>(this List<T> @this, Index index) => @this.RemoveAt(index.GetOffset(@this.Count));

	/// <summary>
	/// Determines whether two sequences are equal by comparing the elements by using <see cref="IEquatable{T}.Equals(T)"/> for their type.
	/// </summary>
	/// <typeparam name="T">The type of each element.</typeparam>
	/// <param name="this">A <see cref="List{T}"/> to compare to <paramref name="other"/>.</param>
	/// <param name="other">A <see cref="List{T}"/> to compare to <paramref name="this"/>.</param>
	/// <returns>
	/// <see langword="true"/> if the two source sequences are of equal length and their correpsonding elements are equal according
	/// to <see cref="IEquatable{T}.Equals(T)"/> for their type; otherwise, <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool SequenceEqual<T>(this List<T> @this, List<T> other) where T : IEquatable<T>
		=> @this.AsReadOnlySpan().SequenceEqual(other.AsReadOnlySpan());

	/// <inheritdoc cref="CollectionsMarshal.AsSpan{T}(List{T}?)"/>
	/// <param name="this">The instance to be transformed.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Span<T> AsSpan<T>(this List<T> @this) => CollectionsMarshal.AsSpan(@this);

	/// <summary>
	/// Gets a <see cref="ReadOnlySpan{T}"/> view over the data in a list. Items should not be added or removed from the <see cref="List{T}"/>
	/// while the <see cref="ReadOnlySpan{T}"/> is in use.
	/// </summary>
	/// <param name="this">The instance to be transformed.</param>
	/// <returns>A <see cref="ReadOnlySpan{T}"/> instance over the <see cref="List{T}"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ReadOnlySpan<T> AsReadOnlySpan<T>(this List<T> @this) => CollectionsMarshal.AsSpan(@this);

#if NET9_0_OR_GREATER
	/// <summary>
	/// Add an item and resize the <see cref="List{T}"/> of <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of the target value to be added.</typeparam>
	/// <param name="this">The list.</param>
	/// <param name="item">An instance of type <typeparamref name="T"/> to be added.</param>
	/// <remarks>
	/// <include
	///     file="../../global-doc-comments.xml"
	///     path="//g/dotnet/version[@value='8']/feature[@name='unsafe-accessor']/target[@name='others']"/>
	/// </remarks>
	private static void AddWithResize<T>(this List<T> @this, scoped ref readonly T item)
	{
		Debug.Assert(@this.GetSize() == @this.GetItems().Length);
		var size = @this.GetSize();
		@this.Capacity = @this.GetNewCapacity(size + 1);
		@this.GetSize() = size + 1;
		@this.GetItems()[size] = item;
	}

	/// <summary>
	/// Try to get a new capacity value by the desired capacity to be set.
	/// </summary>
	/// <typeparam name="T">The type of each element.</typeparam>
	/// <param name="this">The collection.</param>
	/// <param name="capacity">The desired capacity to be set.</param>
	/// <returns>The result value to be set.</returns>
	/// <remarks>
	/// <include
	///     file="../../global-doc-comments.xml"
	///     path="//g/dotnet/version[@value='8']/feature[@name='unsafe-accessor']/target[@name='others']"/>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int GetNewCapacity<T>(this List<T> @this, int capacity)
	{
		Debug.Assert(@this.GetItems().Length < capacity);
		return @this.GetItems().Length == 0 ? 4 : @this.GetItems().Length << 1;
	}

	/// <summary>
	/// Try to fetch the internal field <c>_size</c> in type <see cref="List{T}"/>.
	/// </summary>
	/// <param name="this">The list.</param>
	/// <returns>The reference to the internal field.</returns>
	/// <remarks>
	/// <include
	///     file="../../global-doc-comments.xml"
	///     path="//g/dotnet/version[@value='8']/feature[@name='unsafe-accessor']/target[@name='others']"/>
	/// <include
	///     file="../../global-doc-comments.xml"
	///     path="//g/dotnet/version[@value='8']/feature[@name='unsafe-accessor']/target[@name='field-related-method']"/>
	/// </remarks>
	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = LibraryIdentifiers.Size)]
	private static extern ref int GetSize<T>(this List<T> @this);

	/// <summary>
	/// Try to fetch the internal field <c>_version</c> in type <see cref="List{T}"/>.
	/// </summary>
	/// <param name="this">The list.</param>
	/// <returns>The reference to the internal field.</returns>
	/// <remarks>
	/// <include
	///     file="../../global-doc-comments.xml"
	///     path="//g/dotnet/version[@value='8']/feature[@name='unsafe-accessor']/target[@name='others']"/>
	/// <include
	///     file="../../global-doc-comments.xml"
	///     path="//g/dotnet/version[@value='8']/feature[@name='unsafe-accessor']/target[@name='field-related-method']"/>
	/// </remarks>
	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = LibraryIdentifiers.Version)]
	private static extern ref int GetVersion<T>(this List<T> @this);

	/// <summary>
	/// Try to fetch the internal reference to the first <typeparamref name="T"/> in a <see cref="List{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of each element.</typeparam>
	/// <param name="this">The list of <typeparamref name="T"/> instances.</param>
	/// <returns>The reference to the first <typeparamref name="T"/>.</returns>
	/// <remarks>
	/// <include
	///     file="../../global-doc-comments.xml"
	///     path="//g/dotnet/version[@value='8']/feature[@name='unsafe-accessor']/target[@name='others']"/>
	/// <include
	///     file="../../global-doc-comments.xml"
	///     path="//g/dotnet/version[@value='8']/feature[@name='unsafe-accessor']/target[@name='field-related-method']"/>
	/// </remarks>
	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = LibraryIdentifiers.Items)]
	private static extern ref T[] GetItems<T>(this List<T> @this);
#endif
}
