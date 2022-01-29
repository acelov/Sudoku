﻿using Sudoku.Data;

namespace Sudoku.Presentation;

/// <summary>
/// Defines a coordinate used in a sudoku grid.
/// </summary>
/// <param name="Cell">
/// Indicates the cell value used. The possible values are between 0 and 80. You can't assign a value
/// out of this range; otherwise, an <see cref="InvalidOperationException"/> or
/// <see cref="ArgumentOutOfRangeException"/> will be thrown.
/// </param>
public readonly record struct Coordinate(byte Cell) :
	IAdditionOperators<Coordinate, byte, Coordinate>,
	IComparable<Coordinate>,
	IComparisonOperators<Coordinate, Coordinate>,
	IDefaultable<Coordinate>,
	IEqualityOperators<Coordinate, Coordinate>,
	IEquatable<Coordinate>,
	IMinMaxValue<Coordinate>,
	ISimpleFormattable,
	ISimpleParseable<Coordinate>,
	ISubtractionOperators<Coordinate, byte, Coordinate>
{
	/// <summary>
	/// Indicates the undefined <see cref="Coordinate"/> instance that stands
	/// for an invalid <see cref="Coordinate"/> value.
	/// </summary>
	public static readonly Coordinate Undefined = new(byte.MaxValue);

	/// <inheritdoc cref="IMinMaxValue{TSelf}.MinValue"/>
	public static readonly Coordinate MinValue;

	/// <inheritdoc cref="IMinMaxValue{TSelf}.MaxValue"/>
	public static readonly Coordinate MaxValue = new(80);


	/// <summary>
	/// Indicates the current row lying in.
	/// </summary>
	public int Row
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => Cell.ToRegionIndex(Region.Row);
	}

	/// <summary>
	/// Indicates the current column lying in.
	/// </summary>
	public int Column
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => Cell.ToRegionIndex(Region.Column);
	}

	/// <summary>
	/// Indicates the current block lying in.
	/// </summary>
	public int Block
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => Cell.ToRegionIndex(Region.Block);
	}

	/// <inheritdoc/>
	bool IDefaultable<Coordinate>.IsDefault
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => this == Undefined;
	}

	/// <inheritdoc/>
	static Coordinate IMinMaxValue<Coordinate>.MinValue
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => MinValue;
	}

	/// <inheritdoc/>
	static Coordinate IMinMaxValue<Coordinate>.MaxValue
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => MaxValue;
	}

	/// <inheritdoc/>
	static Coordinate IDefaultable<Coordinate>.Default
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => Undefined;
	}


	/// <summary>
	/// Determine whether the specified <see cref="Coordinate"/> instance holds the same cell
	/// as the current instance.
	/// </summary>
	/// <param name="other">The instance to compare.</param>
	/// <returns>A <see cref="bool"/> result.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(Coordinate other) => Cell == other.Cell;

	/// <inheritdoc cref="object.GetHashCode"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode() => Cell;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int CompareTo(Coordinate other) => Cell - other.Cell;

	/// <inheritdoc cref="object.ToString"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString() => ToString(null);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(string? format) =>
		format switch
		{
			null or "rc" => $"r{Row + 1}c{Column + 1}",
			"RC" => $"R{Row + 1}C{Column + 1}",
			"RCB" => $"R{Row + 1}C{Column + 1}B{Block + 1}",
			"rcb" => $"r{Row + 1}c{Column + 1}b{Block + 1}",
			[var formatChar] => formatChar switch
			{
				'N' => $"R{Row + 1}C{Column + 1}",
				'n' => $"r{Row + 1}c{Column + 1}",
				'R' => $"R{Row + 1}",
				'r' => $"r{Row + 1}",
				'C' => $"C{Column + 1}",
				'c' => $"c{Column + 1}",
				'B' => $"B{Block + 1}",
				'b' => $"b{Block + 1}",
				_ => throw new FormatException($"The specified format '{formatChar}' is invalid or not supported.")
			},
			_ => throw new FormatException($"The specified format '{format}' is invalid or not supported.")
		};

	/// <inheritdoc cref="operator +(Coordinate, byte)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Coordinate MoveForwardly(byte offset) => this + offset;

	/// <inheritdoc cref="operator -(Coordinate, byte)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Coordinate MoveBackwardly(byte offset) => this - offset;

	/// <summary>
	/// Moves the current <see cref="Coordinate"/> to skip cells to the first cell in its next block forwardly.
	/// </summary>
	/// <returns>The result <see cref="Coordinate"/></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Coordinate MoveForwardlyThroughBlock() =>
		new(
			(byte)(Row * 9 + Column / 3 * 3 + 3) is var newResult && newResult == 81
				? (byte)(newResult - 81)
				: newResult
		);

	/// <summary>
	/// Moves the current <see cref="Coordinate"/> to skip cells to the first cell in its next block backwardly.
	/// </summary>
	/// <returns>The result <see cref="Coordinate"/></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Coordinate MoveBackwardlyThroughBlock() =>
		new(
			(byte)(Row * 9 + Column / 3 * 3 - 1) is var newResult && newResult == byte.MaxValue
				? (byte)80
				: newResult
		);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	int IComparable.CompareTo(object? obj) =>
		obj is not Coordinate { Cell: var cell }
			? throw new ArgumentException("The specified argument type is invalid.", nameof(obj))
			: Cell.CompareTo(cell);


	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Coordinate Parse(string str)
	{
		if (string.IsNullOrWhiteSpace(str))
		{
			goto ThrowFormatException;
		}

		string resultStr = str.Trim();
		if (rcb(resultStr, out byte c))
		{
			return new(c);
		}
		if (k9(resultStr, out c))
		{
			return new(c);
		}

	ThrowFormatException:
		throw new FormatException("The specified string is invalid to parse.");


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static bool rcb(string str, out byte cell)
		{
			if (str is not ['R' or 'r', var rowChar, 'C' or 'c', var columnChar])
			{
				goto DefaultReturn;
			}

			if (!char.IsDigit(rowChar) || !char.IsDigit(columnChar))
			{
				goto DefaultReturn;
			}

			cell = (byte)((rowChar - '1') * 9 + (columnChar - '1'));
			return true;

		DefaultReturn:
			cell = default;
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static bool k9(string str, out byte cell)
		{
			if (str is not [var rowChar and (>= 'a' and <= 'i' or >= 'A' and <= 'I'), var columnChar])
			{
				goto DefaultReturn;
			}

			if (!char.IsDigit(columnChar))
			{
				goto DefaultReturn;
			}

			char start = rowChar is >= 'A' and <= 'I' ? 'A' : 'a';
			cell = (byte)((rowChar - start) * 9 + columnChar - '1');
			return true;

		DefaultReturn:
			cell = default;
			return false;
		}
	}

	/// <inheritdoc/>
	public static bool TryParse(string str, out Coordinate result)
	{
		try
		{
			result = Parse(str);
			return true;
		}
		catch (Exception ex) when (ex is FormatException or ArgumentNullException)
		{
			result = Undefined;
			return false;
		}
	}


	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator >=(Coordinate left, Coordinate right) => left.Cell >= right.Cell;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator <=(Coordinate left, Coordinate right) => left.Cell <= right.Cell;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator >(Coordinate left, Coordinate right) => left.Cell > right.Cell;

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator <(Coordinate left, Coordinate right) => left.Cell < right.Cell;

	/// <summary>
	/// Moves the specified <see cref="Coordinate"/> forwardly.
	/// </summary>
	/// <param name="coordinate">The <see cref="Coordinate"/> instance to be moved.</param>
	/// <param name="offset">The offset that the <see cref="Coordinate"/> instance moves.</param>
	/// <returns>The result <see cref="Coordinate"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Coordinate operator +(Coordinate coordinate, byte offset) =>
		new(
			(byte)(coordinate.Cell + offset) is var newResult && newResult >= 81
				? (byte)(newResult % 81)
				: newResult
		);

	/// <summary>
	/// Moves the specified <see cref="Coordinate"/> backwardly.
	/// </summary>
	/// <param name="coordinate">The <see cref="Coordinate"/> instance to be moved.</param>
	/// <param name="offset">The offset that the <see cref="Coordinate"/> instance moves.</param>
	/// <returns>The result <see cref="Coordinate"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Coordinate operator -(Coordinate coordinate, byte offset) =>
		new(
			(byte)(coordinate.Cell - offset) is var newResult && newResult < 0
				? (byte)(-newResult % 81)
				: newResult
		);


	/// <summary>
	/// Implicit conversion from <see cref="Coordinate"/> to <see cref="byte"/>.
	/// </summary>
	/// <param name="coordinate">The <see cref="Coordinate"/> instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator byte(Coordinate coordinate) => coordinate.Cell;

	/// <summary>
	/// Explicit conversion from <see cref="byte"/> to <see cref="Coordinate"/>.
	/// </summary>
	/// <param name="cell">The cell value.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator Coordinate(byte cell) => new(cell);
}
