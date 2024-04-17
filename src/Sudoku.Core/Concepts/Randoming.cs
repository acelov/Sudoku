namespace Sudoku.Concepts;

/// <summary>
/// Provides with extension methods for <see cref="Random"/>.
/// </summary>
/// <seealso cref="Random"/>
public static class Randoming
{
	/// <summary>
	/// Returns a random integer that is within valid digit range (0..9).
	/// </summary>
	/// <param name="random">The random instance.</param>
	/// <returns>
	/// An integer that represents a valid <see cref="Digit"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Digit NextDigit(this Random random) => random.Next(0, 9);

	/// <summary>
	/// Returns a random integer that is within valid cell range (0..81).
	/// </summary>
	/// <param name="random">The random instance.</param>
	/// <returns>
	/// An integer that represents a valid <see cref="Cell"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Cell NextCell(this Random random) => random.Next(0, 81);

	/// <summary>
	/// Returns a random integer that is within valid house range (0..27).
	/// </summary>
	/// <param name="random">The random instance.</param>
	/// <returns>
	/// An integer that represents a valid <see cref="House"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static House NextHouse(this Random random) => random.Next(0, 27);

	/// <summary>
	/// Randomly select the specified number of elements from the current collection.
	/// </summary>
	/// <param name="random">The random instance.</param>
	/// <param name="cells">The cells to be chosen.</param>
	/// <param name="count">The desired number of elements.</param>
	/// <returns>The specified number of elements returned, represented as a <see cref="CellMap"/> instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static CellMap RandomlySelect(this Random random, scoped ref readonly CellMap cells, int count)
	{
		var result = cells.Offsets[..];
		(random ?? Random.Shared).Shuffle(result);
		return [.. result[..count]];
	}

	/// <summary>
	/// Randomly select the specified number of elements from the current collection.
	/// </summary>
	/// <param name="random">The random instance.</param>
	/// <param name="cells">The cells to be chosen.</param>
	/// <param name="count">The desired number of elements.</param>
	/// <returns>The specified number of elements returned, represented as a <see cref="CandidateMap"/> instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static CandidateMap RandomlySelect(this Random random, scoped ref readonly CandidateMap cells, int count)
	{
		var result = cells.Offsets[..];
		(random ?? Random.Shared).Shuffle(result);
		return [.. result[..count]];
	}
}
