namespace Sudoku.Solving.Dlx;

/// <summary>
/// Represents a dancing link node.
/// </summary>
[DebuggerDisplay($$"""{{{nameof(ToString)}}(),nq}""")]
[TypeImpl(TypeImplFlag.Object_ToString)]
public partial class DancingLinkNode : IFormattable
{
	/// <summary>
	/// Initializes a <see cref="DancingLinkNode"/> instance via the specified ID value and the column node.
	/// </summary>
	/// <param name="candidate">The candidate.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public DancingLinkNode(Candidate candidate)
		=> (Candidate, Column, Left, Right, Up, Down) = (candidate, null, this, this, this, this);

	/// <summary>
	/// Initializes a <see cref="DancingLinkNode"/> instance via the specified ID value and the column node.
	/// </summary>
	/// <param name="candidate">The candidate.</param>
	/// <param name="column">The column node.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public DancingLinkNode(Candidate candidate, ColumnNode? column)
		=> (Candidate, Column, Left, Right, Up, Down) = (candidate, column, this, this, this, this);


	/// <summary>
	/// Indicates the candidate of the node.
	/// </summary>
	public Candidate Candidate { get; set; }

	/// <summary>
	/// Indicates the current column node.
	/// </summary>
	public ColumnNode? Column { get; set; }

	/// <summary>
	/// Indicates the pointer that points to the left node.
	/// </summary>
	public DancingLinkNode Left { get; set; }

	/// <summary>
	/// Indicates the pointer that points to the right node.
	/// </summary>
	public DancingLinkNode Right { get; set; }

	/// <summary>
	/// Indicates the pointer that points to the up node.
	/// </summary>
	public DancingLinkNode Up { get; set; }

	/// <summary>
	/// Indicates the pointer that points to the down node.
	/// </summary>
	public DancingLinkNode Down { get; set; }


	/// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
	public string ToString(IFormatProvider? formatProvider)
	{
		return $"{nameof(Candidate)} = {f(Candidate)}, {nameof(Up)} = {f(Up.Candidate)}, {nameof(Down)} = {f(Down.Candidate)}, {nameof(Left)} = {f(Left.Candidate)}, {nameof(Right)} = {f(Right.Candidate)}";


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		string f(Candidate candidate)
			=> candidate == -1
				? "<invalid>"
				: candidate.AsCandidateMap() switch { var map => map.ToString(formatProvider) };
	}

	/// <inheritdoc/>
	string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString(formatProvider);
}
