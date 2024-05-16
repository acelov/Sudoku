namespace System.Linq.Providers;

/// <summary>
/// Represents a type that supports method group <c>DefaultIfEmpty</c>.
/// </summary>
/// <inheritdoc/>
public interface IDefaultIfEmptyProvider<TSelf, TSource> : IAnyAllProvider<TSelf, TSource>, ILinqMethodProvider<TSelf, TSource>
	where TSelf : IDefaultIfEmptyProvider<TSelf, TSource>
{
	/// <inheritdoc cref="Enumerable.DefaultIfEmpty{TSource}(IEnumerable{TSource})"/>
	public virtual IEnumerable<TSource?> DefaultIfEmpty() => Any() ? this : [default];

	/// <inheritdoc cref="Enumerable.DefaultIfEmpty{TSource}(IEnumerable{TSource}, TSource)"/>
	public virtual IEnumerable<TSource> DefaultIfEmpty(TSource defaultValue) => Any() ? this : [defaultValue];
}
