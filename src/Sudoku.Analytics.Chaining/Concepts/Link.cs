namespace Sudoku.Concepts;

/// <summary>
/// <para>Represents a link that describes a relation between two <see cref="Node"/> instances.</para>
/// <para><b>
/// Please note that two <see cref="Link"/> instances will be considered as equal
/// only if they holds same node values, regardless of what link type two <see cref="Link"/> instances use.
/// </b></para>
/// </summary>
/// <param name="firstNode">Indicates the first node to be used.</param>
/// <param name="secondNode">Indicates the second node to be used.</param>
/// <param name="type">Indicates the type of the link.</param>
/// <param name="inference">Indicates the inference between two nodes <paramref name="firstNode"/> and <paramref name="secondNode"/>.</param>
/// <seealso cref="Node"/>
[TypeImpl(TypeImplFlag.Object_Equals | TypeImplFlag.EqualityOperators)]
public sealed partial class Link(
	[PrimaryConstructorParameter(MemberKinds.Field)] Node firstNode,
	[PrimaryConstructorParameter(MemberKinds.Field)] Node secondNode,
	[PrimaryConstructorParameter] LinkType type,
	[PrimaryConstructorParameter] Inference inference
) : IEquatable<Link>, IEqualityOperators<Link, Link, bool>
{
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals([NotNullWhen(true)] Link? other) => other is not null && Equals(other, LinkComparison.Undirected);

	/// <summary>
	/// Determine whether two <see cref="Link"/> are considered equal on the specified comparison rule.
	/// </summary>
	/// <param name="other">The other object to be compared.</param>
	/// <param name="comparison">The comparison rule to be used.</param>
	/// <returns>A <see cref="bool"/> result indicating that.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Throws when the argument <paramref name="comparison"/> is not defined.</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(Link other, LinkComparison comparison)
		=> Enum.IsDefined(comparison)
			? Inference == other.Inference && comparison switch
			{
				LinkComparison.Undirected
					=> _firstNode == other._firstNode && _secondNode == other._secondNode
					|| _firstNode == other._secondNode && _secondNode == other._firstNode,
				_ => _firstNode == other._firstNode && _secondNode == other._secondNode
			}
			: throw new ArgumentOutOfRangeException(nameof(comparison));

	/// <inheritdoc/>
	public override int GetHashCode() => GetHashCode(LinkComparison.Undirected);

	/// <summary>
	/// Serves as hash code functions, with consideration on the specified comparison rule.
	/// </summary>
	/// <param name="comparison">The comparison rule.</param>
	/// <returns>An <see cref="int"/> indicating the hash code.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Throws when the argument <paramref name="comparison"/> is not defined.</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetHashCode(LinkComparison comparison)
		=> Enum.IsDefined(comparison)
			? comparison switch
			{
				LinkComparison.Undirected
					=> HashCode.Combine(HashCodeNativeConstants.Prime3(), _firstNode.GetHashCode() ^ _secondNode.GetHashCode()),
				_ => HashCode.Combine(_firstNode, _secondNode)
			}
			: throw new ArgumentOutOfRangeException(nameof(comparison));

	/// <inheritdoc/>
	public override string ToString() => $"{_firstNode}{Inference.ConnectingNotation()}{_secondNode}";
}
