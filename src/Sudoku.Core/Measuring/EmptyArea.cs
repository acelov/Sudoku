namespace Sudoku.Measuring;

/// <summary>
/// Provides a way to calculate empty area of a <see cref="Grid"/> or a <see cref="CellMap"/>.
/// </summary>
public static class EmptyArea
{
	/// <summary>
	/// Try to get the maximum empty area exists in the specified grid.
	/// </summary>
	/// <param name="this">The grid to be checked.</param>
	/// <returns>An <see cref="int"/> value indicating the result.</returns>
	/// <remarks>
	/// <inheritdoc cref="GetMaxEmptyArea(ref readonly CellMap)" path="/remarks"/>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetMaxEmptyArea(this scoped ref readonly Grid @this) => @this.EmptyCells.GetMaxEmptyArea();

	/// <summary>
	/// Try to get the maximum empty square area exists in the specified grid.
	/// </summary>
	/// <param name="this">The grid to be checked.</param>
	/// <returns>An <see cref="int"/> value indicating the result.</returns>
	/// <remarks>
	/// <inheritdoc cref="GetMaxEmptySquareArea(ref readonly CellMap)" path="/remarks"/>
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetMaxEmptySquareArea(this scoped ref readonly Grid @this) => @this.EmptyCells.GetMaxEmptySquareArea();

	/// <summary>
	/// Try to get the maximum empty area exists in the specified cells.
	/// </summary>
	/// <param name="this">The cells to be checked.</param>
	/// <returns>An <see cref="int"/> value indicating the result.</returns>
	/// <remarks>
	/// This algorithm is from the puzzle called
	/// <see href="https://leetcode.com/problems/maximal-rectangle/"><i>Maximal Rectangle</i></see>.
	/// </remarks>
	public static int GetMaxEmptyArea(this scoped ref readonly CellMap @this)
	{
		var dp = (stackalloc int[9]);
		dp.Clear();

		var max = 0;
		for (var i = 0; i < 9; i++)
		{
			for (var j = 0; j < 9; j++)
			{
				dp[j] = @this.Contains(i * 9 + j) ? dp[j] + 1 : 0;
			}
			max = Math.Max(max, getMaxRow(dp));
		}
		return max;


		static int getMaxRow(ReadOnlySpan<int> height)
		{
			var stack = new Stack<int>();
			var max = 0;
			for (var i = 0; i <= 9; i++)
			{
				var h = i == 9 ? 0 : height[i];
				while (stack.Count != 0 && height[stack.Peek()] >= h)
				{
					var maxHeight = height[stack.Pop()];
					var area = stack.Count == 0 ? i * maxHeight : maxHeight * (i - 1 - stack.Peek());
					max = Math.Max(max, area);
				}
				stack.Push(i);
			}
			return max;
		}
	}

	/// <summary>
	/// Try to get the maximum empty square area exists in the specified cells.
	/// </summary>
	/// <param name="this">The cells to be checked.</param>
	/// <returns>An <see cref="int"/> value indicating the result.</returns>
	/// <remarks>
	/// This algorithm is from the puzzle called
	/// <see href="https://leetcode.com/problems/maximal-square/"><i>Maximal Square</i></see>.
	/// </remarks>
	public static int GetMaxEmptySquareArea(this scoped ref readonly CellMap @this)
	{
		var maxSide = 0;
		var dp = (stackalloc int[81]);
		for (var i = 0; i < 9; i++)
		{
			for (var j = 0; j < 9; j++)
			{
				if (@this.Contains(i * 9 + j))
				{
					dp[i * 9 + j] = (i, j) is not (not 0, not 0)
						? 1
						: MathExtensions.Min(dp[(i - 1) * 9 + j], dp[i * 9 + j - 1], dp[(i - 1) * 9 + j - 1]) + 1;
					maxSide = Math.Max(maxSide, dp[i * 9 + j]);
				}
			}
		}
		return maxSide * maxSide;
	}
}
