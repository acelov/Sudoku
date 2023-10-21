using System.Runtime.CompilerServices;
using System.SourceGeneration;

namespace Sudoku.Runtime.MaskServices;

/// <summary>
/// Represents a combination generator that iterations each combination of bits for the specified number of bits, and how many 1's in it.
/// </summary>
/// <param name="bitCount">Indicates the number of bits.</param>
/// <param name="oneCount">Indicates the number of bits set <see langword="true"/>.</param>
public readonly ref partial struct MaskCombinationsGenerator(
	[DataMember(MemberKinds.Field)] Count bitCount,
	[DataMember(MemberKinds.Field)] Count oneCount
)
{
	/// <summary>
	/// Gets the enumerator of the current instance in order to use <see langword="foreach"/> loop.
	/// </summary>
	/// <returns>The enumerator instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public MaskCombinationEnumerator GetEnumerator() => new(_bitCount, _oneCount);
}
