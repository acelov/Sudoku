namespace Sudoku.Analytics.Construction.Components;

/// <summary>
/// Represents a node that supports parent linking.
/// </summary>
/// <typeparam name="TSelf"><include file="../../global-doc-comments.xml" path="/g/self-type-constraint"/></typeparam>
public interface IParentLinkedNode<TSelf> :
	IEquatable<TSelf>,
	IFormattable,
	IEqualityOperators<TSelf, TSelf, bool>,
	IShiftOperators<TSelf, TSelf, TSelf>
	where TSelf : IParentLinkedNode<TSelf>
{
	/// <summary>
	/// Indicates the length of ancestors.
	/// </summary>
	public sealed int AncestorsLength
	{
		get
		{
			var result = 0;
			for (var node = this; node is not null; node = node.Parent)
			{
				result++;
			}
			return result;
		}
	}

	/// <summary>
	/// Indicates the parent node.
	/// </summary>
	public abstract TSelf? Parent { get; }

	/// <summary>
	/// Indicates the root node.
	/// </summary>
	public abstract TSelf Root { get; }


	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public abstract string ToString(IFormatProvider? formatProvider);

	/// <inheritdoc/>
	string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString(formatProvider);


	/// <summary>
	/// Creates a <see cref="WhipNode"/> instance with parent node.
	/// </summary>
	/// <param name="current">The current node.</param>
	/// <param name="parent">The parent node.</param>
	/// <returns>The new node created.</returns>
	public static abstract TSelf operator >>(TSelf current, TSelf? parent);

	/// <inheritdoc cref="IShiftOperators{TSelf, TOther, TResult}.op_LeftShift(TSelf, TOther)"/>
	static TSelf IShiftOperators<TSelf, TSelf, TSelf>.operator <<(TSelf? parent, TSelf current) => current >> parent;

	/// <inheritdoc/>
	static TSelf IShiftOperators<TSelf, TSelf, TSelf>.operator >>>(TSelf value, TSelf shiftAmount) => value >> shiftAmount;
}
