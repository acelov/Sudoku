namespace Sudoku.Drawing.Nodes;

/// <summary>
/// Defines a view node that highlights for a chain link.
/// </summary>
/// <param name="identifier"><inheritdoc/></param>
/// <param name="start">Indicates the start point.</param>
/// <param name="end">Indicates the end point.</param>
/// <param name="isStrongLink">Indicates whether the link is a strong link.</param>
[TypeImpl(TypeImplFlags.Object_GetHashCode | TypeImplFlags.Object_ToString)]
[method: JsonConstructor]
public sealed partial class ChainLinkViewNode(
	ColorIdentifier identifier,
	[Property, HashCodeMember, StringMember] CandidateMap start,
	[Property, HashCodeMember, StringMember] CandidateMap end,
	[Property, StringMember] bool isStrongLink
) : BasicViewNode(identifier), ILinkViewNode
{
	/// <inheritdoc/>
	object ILinkViewNode.Start => Start;

	/// <inheritdoc/>
	object ILinkViewNode.End => End;

	/// <inheritdoc/>
	LinkShape ILinkViewNode.Shape => LinkShape.Chain;


	/// <include file="../../global-doc-comments.xml" path="g/csharp7/feature[@name='deconstruction-method']/target[@name='method']"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Deconstruct(out ColorIdentifier identifier, out CandidateMap start, out CandidateMap end, out bool isStrongLink)
		=> (identifier, start, end, isStrongLink) = (Identifier, Start, End, IsStrongLink);

	/// <inheritdoc/>
	/// <remarks><b>Chain links may not check for color identifiers by design.</b></remarks>
	public override bool Equals([NotNullWhen(true)] ViewNode? other)
		=> other is ChainLinkViewNode comparer && Start == comparer.Start && End == comparer.End
		&& IsStrongLink == comparer.IsStrongLink;

	/// <inheritdoc/>
	public override ChainLinkViewNode Clone() => new(Identifier, Start, End, IsStrongLink);
}
