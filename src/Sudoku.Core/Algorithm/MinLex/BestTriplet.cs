using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sudoku.Algorithm.MinLex;

/// <summary>
/// Represents for a best triplet permutation.
/// </summary>
[InlineArray(3)]
[CollectionBuilder(typeof(BestTriplet), nameof(Create))]
public struct BestTriplet
{
#pragma warning disable format
	/// <summary>
	/// The best triplet permutations.
	/// </summary>
	public static readonly BestTriplet[][] BestTripletPermutations = [
		[
			[65535, 0, 0], [0, 1, 1], [0, 2, 1], [0, 3, 2], [0, 4, 1], [0, 5, 2], [0, 6, 2], [0, 7, 3],
			[0, 8, 1], [0, 9, 2], [0, 10, 2], [0, 11, 3], [0, 12, 2], [0, 13, 3], [0, 14, 3], [0, 15, 4],
			[0, 16, 1], [0, 17, 2], [0, 18, 2], [0, 19, 3], [0, 20, 2], [0, 21, 3], [0, 22, 3], [0, 23, 4],
			[0, 24, 2], [0, 25, 3], [0, 26, 3], [0, 27, 4], [0, 28, 3], [0, 29, 4], [0, 30, 4], [0, 31, 5],
			[0, 32, 1], [0, 33, 2], [0, 34, 2], [0, 35, 3], [0, 36, 2], [0, 37, 3], [0, 38, 3], [0, 39, 4],
			[0, 40, 2], [0, 41, 3], [0, 42, 3], [0, 43, 4], [0, 44, 3], [0, 45, 4], [0, 46, 4], [0, 47, 5],
			[0, 48, 2], [0, 49, 3], [0, 50, 3], [0, 51, 4], [0, 52, 3], [0, 53, 4], [0, 54, 4], [0, 55, 5],
			[0, 56, 3], [0, 57, 4], [0, 58, 4], [0, 59, 5], [0, 60, 4], [0, 61, 5], [0, 62, 5], [0, 63, 6]
		],
		[
			[65535, 0, 0], [1, 1, 1], [2, 2, 1], [1, 1, 1], [1, 4, 1], [1, 5, 2], [1, 4, 1], [1, 5, 2],
			[4, 8, 1], [1, 1, 1], [2, 2, 1], [1, 1, 1], [1, 4, 1], [1, 5, 2], [1, 4, 1], [1, 5, 2],
			[2, 16, 1], [1, 1, 1], [2, 18, 2], [1, 1, 1], [1, 4, 1], [1, 5, 2], [1, 4, 1], [1, 5, 2],
			[2, 16, 1], [1, 1, 1], [2, 18, 2], [1, 1, 1], [1, 4, 1], [1, 5, 2], [1, 4, 1], [1, 5, 2],
			[4, 32, 1], [1, 1, 1], [2, 2, 1], [1, 1, 1], [1, 4, 1], [1, 5, 2], [1, 4, 1], [1, 5, 2],
			[4, 40, 2], [1, 1, 1], [2, 2, 1], [1, 1, 1], [1, 4, 1], [1, 5, 2], [1, 4, 1], [1, 5, 2],
			[2, 16, 1], [1, 1, 1], [2, 18, 2], [1, 1, 1], [1, 4, 1], [1, 5, 2], [1, 4, 1], [1, 5, 2],
			[2, 16, 1], [1, 1, 1], [2, 18, 2], [1, 1, 1], [1, 4, 1], [1, 5, 2], [1, 4, 1], [1, 5, 2]
		],
		[
			[65535, 0, 0], [2, 1, 1], [1, 2, 1], [1, 2, 1], [4, 4, 1], [2, 1, 1], [1, 2, 1], [1, 2, 1],
			[1, 8, 1], [1, 8, 1], [1, 10, 2], [1, 10, 2], [1, 8, 1], [1, 8, 1], [1, 10, 2], [1, 10, 2],
			[4, 16, 1], [2, 1, 1], [1, 2, 1], [1, 2, 1], [4, 20, 2], [2, 1, 1], [1, 2, 1], [1, 2, 1],
			[1, 8, 1], [1, 8, 1], [1, 10, 2], [1, 10, 2], [1, 8, 1], [1, 8, 1], [1, 10, 2], [1, 10, 2],
			[2, 32, 1], [2, 33, 2], [1, 2, 1], [1, 2, 1], [2, 32, 1], [2, 33, 2], [1, 2, 1], [1, 2, 1],
			[1, 8, 1], [1, 8, 1], [1, 10, 2], [1, 10, 2], [1, 8, 1], [1, 8, 1], [1, 10, 2], [1, 10, 2],
			[2, 32, 1], [2, 33, 2], [1, 2, 1], [1, 2, 1], [2, 32, 1], [2, 33, 2], [1, 2, 1], [1, 2, 1],
			[1, 8, 1], [1, 8, 1], [1, 10, 2], [1, 10, 2], [1, 8, 1], [1, 8, 1], [1, 10, 2], [1, 10, 2]
		],
		[
			[65535, 0, 0], [3, 1, 1], [3, 2, 1], [3, 3, 2], [5, 4, 1], [3, 1, 1], [3, 2, 1], [3, 3, 2],
			[5, 8, 1], [3, 1, 1], [3, 2, 1], [3, 3, 2], [5, 12, 2], [3, 1, 1], [3, 2, 1], [3, 3, 2],
			[6, 16, 1], [3, 1, 1], [3, 2, 1], [3, 3, 2], [5, 4, 1], [3, 1, 1], [3, 2, 1], [3, 3, 2],
			[5, 8, 1], [3, 1, 1], [3, 2, 1], [3, 3, 2], [5, 12, 2], [3, 1, 1], [3, 2, 1], [3, 3, 2],
			[6, 32, 1], [3, 1, 1], [3, 2, 1], [3, 3, 2], [5, 4, 1], [3, 1, 1], [3, 2, 1], [3, 3, 2],
			[5, 8, 1], [3, 1, 1], [3, 2, 1], [3, 3, 2], [5, 12, 2], [3, 1, 1], [3, 2, 1], [3, 3, 2],
			[6, 48, 2], [3, 1, 1], [3, 2, 1], [3, 3, 2], [5, 4, 1], [3, 1, 1], [3, 2, 1], [3, 3, 2],
			[5, 8, 1], [3, 1, 1], [3, 2, 1], [3, 3, 2], [5, 12, 2], [3, 1, 1], [3, 2, 1], [3, 3, 2]
		],
		[
			[65535, 0, 0], [4, 1, 1], [4, 2, 1], [4, 3, 2], [2, 4, 1], [2, 4, 1], [2, 4, 1], [2, 4, 1],
			[2, 8, 1], [2, 8, 1], [2, 8, 1], [2, 8, 1], [2, 12, 2], [2, 12, 2], [2, 12, 2], [2, 12, 2],
			[1, 16, 1], [1, 16, 1], [1, 16, 1], [1, 16, 1], [1, 16, 1], [1, 16, 1], [1, 16, 1], [1, 16, 1],
			[1, 16, 1], [1, 16, 1], [1, 16, 1], [1, 16, 1], [1, 16, 1], [1, 16, 1], [1, 16, 1], [1, 16, 1],
			[1, 32, 1], [1, 32, 1], [1, 32, 1], [1, 32, 1], [1, 32, 1], [1, 32, 1], [1, 32, 1], [1, 32, 1],
			[1, 32, 1], [1, 32, 1], [1, 32, 1], [1, 32, 1], [1, 32, 1], [1, 32, 1], [1, 32, 1], [1, 32, 1],
			[1, 48, 2], [1, 48, 2], [1, 48, 2], [1, 48, 2], [1, 48, 2], [1, 48, 2], [1, 48, 2], [1, 48, 2],
			[1, 48, 2], [1, 48, 2], [1, 48, 2], [1, 48, 2], [1, 48, 2], [1, 48, 2], [1, 48, 2], [1, 48, 2]
		],
		[
			[65535, 0, 0], [5, 1, 1], [6, 2, 1], [5, 1, 1], [3, 4, 1], [3, 4, 1], [3, 4, 1], [3, 4, 1],
			[6, 8, 1], [5, 1, 1], [6, 10, 2], [5, 1, 1], [3, 4, 1], [3, 4, 1], [3, 4, 1], [3, 4, 1],
			[3, 16, 1], [3, 16, 1], [3, 16, 1], [3, 16, 1], [3, 20, 2], [3, 20, 2], [3, 20, 2], [3, 20, 2],
			[3, 16, 1], [3, 16, 1], [3, 16, 1], [3, 16, 1], [3, 20, 2], [3, 20, 2], [3, 20, 2], [3, 20, 2],
			[5, 32, 1], [5, 33, 2], [5, 32, 1], [5, 33, 2], [3, 4, 1], [3, 4, 1], [3, 4, 1], [3, 4, 1],
			[5, 32, 1], [5, 33, 2], [5, 32, 1], [5, 33, 2], [3, 4, 1], [3, 4, 1], [3, 4, 1], [3, 4, 1],
			[3, 16, 1], [3, 16, 1], [3, 16, 1], [3, 16, 1], [3, 20, 2], [3, 20, 2], [3, 20, 2], [3, 20, 2],
			[3, 16, 1], [3, 16, 1], [3, 16, 1], [3, 16, 1], [3, 20, 2], [3, 20, 2], [3, 20, 2], [3, 20, 2]
		],
		[
			[65535, 0, 0], [6, 1, 1], [5, 2, 1], [5, 2, 1], [6, 4, 1], [6, 5, 2], [5, 2, 1], [5, 2, 1],
			[3, 8, 1], [3, 8, 1], [3, 8, 1], [3, 8, 1], [3, 8, 1], [3, 8, 1], [3, 8, 1], [3, 8, 1],
			[5, 16, 1], [5, 16, 1], [5, 18, 2], [5, 18, 2], [5, 16, 1], [5, 16, 1], [5, 18, 2], [5, 18, 2],
			[3, 8, 1], [3, 8, 1], [3, 8, 1], [3, 8, 1], [3, 8, 1], [3, 8, 1], [3, 8, 1], [3, 8, 1],
			[3, 32, 1], [3, 32, 1], [3, 32, 1], [3, 32, 1], [3, 32, 1], [3, 32, 1], [3, 32, 1], [3, 32, 1],
			[3, 40, 2], [3, 40, 2], [3, 40, 2], [3, 40, 2], [3, 40, 2], [3, 40, 2], [3, 40, 2], [3, 40, 2],
			[3, 32, 1], [3, 32, 1], [3, 32, 1], [3, 32, 1], [3, 32, 1], [3, 32, 1], [3, 32, 1], [3, 32, 1],
			[3, 40, 2], [3, 40, 2], [3, 40, 2], [3, 40, 2], [3, 40, 2], [3, 40, 2], [3, 40, 2], [3, 40, 2]
		],
		[
			[65535, 0, 0], [7, 1, 1], [7, 2, 1], [7, 3, 2], [7, 4, 1], [7, 5, 2], [7, 6, 2], [7, 7, 3],
			[7, 8, 1], [7, 9, 2], [7, 10, 2], [7, 11, 3], [7, 12, 2], [7, 13, 3], [7, 14, 3], [7, 15, 4],
			[7, 16, 1], [7, 17, 2], [7, 18, 2], [7, 19, 3], [7, 20, 2], [7, 21, 3], [7, 22, 3], [7, 23, 4],
			[7, 24, 2], [7, 25, 3], [7, 26, 3], [7, 27, 4], [7, 28, 3], [7, 29, 4], [7, 30, 4], [7, 31, 5],
			[7, 32, 1], [7, 33, 2], [7, 34, 2], [7, 35, 3], [7, 36, 2], [7, 37, 3], [7, 38, 3], [7, 39, 4],
			[7, 40, 2], [7, 41, 3], [7, 42, 3], [7, 43, 4], [7, 44, 3], [7, 45, 4], [7, 46, 4], [7, 47, 5],
			[7, 48, 2], [7, 49, 3], [7, 50, 3], [7, 51, 4], [7, 52, 3], [7, 53, 4], [7, 54, 4], [7, 55, 5],
			[7, 56, 3], [7, 57, 4], [7, 58, 4], [7, 59, 5], [7, 60, 4], [7, 61, 5], [7, 62, 5], [7, 63, 6]
		]
	];
#pragma warning restore format


	/// <summary>
	/// The field that points to the zero-indexed position.
	/// </summary>
	[SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "<Pending>")]
	[SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
	private int _field;


	/// <summary>
	/// The total score of the pattern.
	/// </summary>
	[UnscopedRef]
	[SuppressMessage("Style", "IDE0251:Make member 'readonly'", Justification = "<Pending>")]
	public ref int BestResult => ref this[0];

	/// <summary>
	/// The result mask.
	/// </summary>
	[UnscopedRef]
	[SuppressMessage("Style", "IDE0251:Make member 'readonly'", Justification = "<Pending>")]
	public ref int ResultMask => ref this[1];

	/// <summary>
	/// The result number bits.
	/// </summary>
	[UnscopedRef]
	[SuppressMessage("Style", "IDE0251:Make member 'readonly'", Justification = "<Pending>")]
	public ref int ResultNumBits => ref this[2];


	/// <summary>
	/// Creates a <see cref="BestTriplet"/> instance via collection expression.
	/// </summary>
	/// <param name="values">The values.</param>
	/// <returns>A valid <see cref="BestTriplet"/> result.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[DebuggerStepThrough]
	public static BestTriplet Create(scoped ReadOnlySpan<int> values)
		=> values switch
		{
			[] => new(),
			[var bestResult, var resultMask, var resultNumBits] => new() { BestResult = bestResult, ResultMask = resultMask, ResultNumBits = resultNumBits },
			_ => throw new InvalidOperationException($"The length of argument '{nameof(values)}' is mismatched.")
		};
}
