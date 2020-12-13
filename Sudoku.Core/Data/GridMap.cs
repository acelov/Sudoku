﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Extensions;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Sudoku.DocComments;
using static Sudoku.Constants.Processings;

namespace Sudoku.Data
{
	/// <summary>
	/// Encapsulates a binary series of cell status table.
	/// </summary>
	/// <remarks>
	/// The instance stores two <see cref="long"/> values, consisting of 81 bits,
	/// where <see langword="true"/> bit (1) is for the cell having that digit,
	/// and the <see langword="false"/> bit (0) is for the cell not containing
	/// the digit.
	/// </remarks>
	public partial struct GridMap : IEnumerable<int>, IValueEquatable<GridMap>, IFormattable
	{
		/// <summary>
		/// <para>Indicates an empty instance (all bits are 0).</para>
		/// <para>
		/// I strongly recommend you <b>should</b> use this instance instead of default constructor
		/// <see cref="GridMap()"/> and <see langword="default"/>(<see cref="GridMap"/>).
		/// </para>
		/// </summary>
		/// <seealso cref="GridMap()"/>
		public static readonly GridMap Empty;


		/// <summary>
		/// The value used for shifting.
		/// </summary>
		private const int Shifting = 41;

		/// <summary>
		/// The value of offsets.
		/// </summary>
		private const int BlockOffset = 0, RowOffset = 9, ColumnOffset = 18, Limit = 27;


		/// <summary>
		/// Indicates the internal two <see cref="long"/> values,
		/// which represents 81 bits. <see cref="_high"/> represent the higher
		/// 40 bits and <see cref="_low"/> represents the lower 41 bits.
		/// </summary>
		private long _high, _low;


		/// <summary>
		/// Initializes an instance with the specified cell offset
		/// (Sets itself and all peers).
		/// </summary>
		/// <param name="offset">The cell offset.</param>
		public GridMap(int offset) : this(offset, true)
		{
		}

		/// <summary>
		/// Same behavior of the constructor as <see cref="GridMap(IEnumerable{int})"/>.
		/// </summary>
		/// <param name="offsets">All offsets.</param>
		/// <remarks>
		/// This constructor is defined after another constructor with
		/// <see cref="ReadOnlySpan{T}"/> had defined. Although this constructor
		/// doesn't initialize something (use the other one instead),
		/// while initializing with the type <see cref="int"/>[], the complier
		/// gives me an error without this constructor (ambiguity of two
		/// constructors). However, unfortunately, <see cref="ReadOnlySpan{T}"/>
		/// doesn't implemented the interface <see cref="IEnumerable{T}"/>.
		/// </remarks>
		/// <seealso cref="GridMap(IEnumerable{int})"/>
		public GridMap(int[] offsets) : this(offsets.AsEnumerable())
		{
		}

		/// <summary>
		/// Initializes an instance with a series of cell offsets.
		/// </summary>
		/// <param name="cells">(<see langword="in"/> parameter) cell offsets.</param>
		/// <remarks>
		/// <para>
		/// Note that all offsets will be set <see langword="true"/>, but their own peers
		/// won't be set <see langword="true"/>.
		/// </para>
		/// <para>
		/// In some case, you can use object initializer instead.
		/// You can use the code
		/// <code>
		/// var map = new GridMap { 0, 3, 5 };
		/// </code>
		/// instead of the code
		/// <code>
		/// var map = new GridMap(stackalloc[] { 0, 3, 5 });
		/// </code>
		/// </para>
		/// </remarks>
		public GridMap(in ReadOnlySpan<int> cells) : this()
		{
			foreach (int offset in cells)
			{
				(offset / Shifting == 0 ? ref _low : ref _high) |= 1L << offset % Shifting;
				Count++;
			}
		}

		/// <summary>
		/// (Copy constructor) To copy an instance with the specified information.
		/// </summary>
		/// <param name="another">(<see langword="in"/> parameter) Another instance.</param>
		/// <remarks>
		/// <para>
		/// This constructor is only used for adding or removing some extra cells like:
		/// <code>
		/// var y = new GridMap(x) { [i] = true };
		/// </code>
		/// or
		/// <code>
		/// var y = new GridMap(x) { i };
		/// </code>
		/// </para>
		/// <para>
		/// Similarly, the following code is also okay:
		/// <code>
		/// var y = new GridMap(x) { [i] = false };
		/// </code>
		/// or
		/// <code>
		/// var y = new GridMap(x) { ~i };
		/// </code>
		/// where <c>~i</c> means assigning <see langword="false"/> value to the position
		/// whose the corresponding value is <c>i</c>.
		/// </para>
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GridMap(in GridMap another) => this = another;

		/// <summary>
		/// Initializes an instance with a series of cell offsets.
		/// </summary>
		/// <param name="offsets">cell offsets.</param>
		/// <remarks>
		/// Note that all offsets will be set <see langword="true"/>, but their own peers
		/// won't be set <see langword="true"/>.
		/// </remarks>
		public GridMap(IEnumerable<int> offsets) : this()
		{
			foreach (int offset in offsets)
			{
				(offset / Shifting == 0 ? ref _low : ref _high) |= 1L << offset % Shifting;
				Count++;
			}
		}

		/// <summary>
		/// Initializes an instance with three binary values.
		/// </summary>
		/// <param name="high">Higher 27 bits.</param>
		/// <param name="mid">Medium 27 bits.</param>
		/// <param name="low">Lower 27 bits.</param>
		public GridMap(int high, int mid, int low)
			: this((high & 0x7FFFFFFL) << 13 | (mid >> 14 & 0x1FFFL), (mid & 0x3FFFL) << 27 | (low & 0x7FFFFFFL))
		{
		}

		/// <summary>
		/// Initializes an instance with the specified cell offset.
		/// This will set all bits of all peers of this cell. Another
		/// <see cref="bool"/> value indicates whether this initialization
		/// will set the bit of itself.
		/// </summary>
		/// <param name="offset">The cell offset.</param>
		/// <param name="setItself">
		/// A <see cref="bool"/> value indicating whether this initialization
		/// will set the bit of itself.
		/// If the value is <see langword="false"/>, it will be equivalent
		/// to below:
		/// <code>
		/// var map = new GridMap(offset) { [offset] = false };
		/// </code>
		/// </param>
		/// <remarks>
		/// If you want to use this constructor, please use <see cref="PeerMaps"/> instead.
		/// </remarks>
		/// <seealso cref="PeerMaps"/>
		private GridMap(int offset, bool setItself)
		{
			this = PeerMaps[offset];
			this[offset] = setItself;
		}

		/// <summary>
		/// Initializes an instance with two binary values.
		/// </summary>
		/// <param name="high">Higher 40 bits.</param>
		/// <param name="low">Lower 41 bits.</param>
		private GridMap(long high, long low) => Count = (_high = high).PopCount() + (_low = low).PopCount();


		/// <summary>
		/// Indicates whether the map has no set bits.
		/// This property is equivalent to code '<c>!this.IsNotEmpty</c>'.
		/// </summary>
		/// <seealso cref="IsNotEmpty"/>
		public readonly bool IsEmpty => (_high, _low) is (0, 0);

		/// <summary>
		/// Indicates whether the map has at least one set bit.
		/// This property is equivalent to code '<c>!this.IsEmpty</c>'.
		/// </summary>
		/// <seealso cref="IsEmpty"/>
		public readonly bool IsNotEmpty => (_high, _low) is not (0, 0);

		/// <summary>
		/// Same as <see cref="AllSetsAreInOneRegion(out int)"/>, but only contains
		/// the <see cref="bool"/> result.
		/// </summary>
		/// <seealso cref="AllSetsAreInOneRegion(out int)"/>
		public readonly bool InOneRegion
		{
			get
			{
				for (int i = BlockOffset; i < Limit; i++)
				{
					if ((_high & ~CoverTable[i, 0]) == 0 && (_low & ~CoverTable[i, 1]) == 0)
					{
						return true;
					}
				}

				return false;
			}
		}

		/// <summary>
		/// Indicates the mask of block.
		/// </summary>
		public readonly short BlockMask => GetFirst(BlockOffset, RowOffset);

		/// <summary>
		/// Indicates the mask of row.
		/// </summary>
		public readonly short RowMask => GetFirst(RowOffset, ColumnOffset);

		/// <summary>
		/// Indicates the mask of column.
		/// </summary>
		public readonly short ColumnMask => GetFirst(ColumnOffset, Limit);

		/// <summary>
		/// Indicates the covered line.
		/// </summary>
		public readonly int CoveredLine
		{
			get
			{
				for (int i = RowOffset; i < Limit; i++)
				{
					if ((_high & ~CoverTable[i, 0]) == 0 && (_low & ~CoverTable[i, 1]) == 0)
					{
						return i;
					}
				}

				return -1;
			}
		}

		/// <summary>
		/// Indicates the total number of cells where the corresponding
		/// value are set <see langword="true"/>.
		/// </summary>
		public int Count { readonly get; private set; }

		/// <summary>
		/// Gets the first set bit position. If the current map is empty,
		/// the return value will be <c>-1</c>.
		/// </summary>
		/// <remarks>
		/// The property will use the same process with <see cref="Offsets"/>,
		/// but the <see langword="yield"/> clause will be replaced with normal <see langword="return"/>s.
		/// </remarks>
		/// <seealso cref="Offsets"/>
		public readonly int First
		{
			get
			{
				if (IsEmpty)
				{
					return -1;
				}

				long value;
				int i;
				if (_low != 0)
				{
					for (value = _low, i = 0; i < Shifting; i++, value >>= 1)
					{
						if ((value & 1) != 0)
						{
							return i;
						}
					}
				}
				if (_high != 0)
				{
					for (value = _high, i = Shifting; i < 81; i++, value >>= 1)
					{
						if ((value & 1) != 0)
						{
							return i;
						}
					}
				}

				return default; // Here is only used for a placeholder.
			}
		}

		/// <summary>
		/// Indicates the map of cells, which is the peer intersections.
		/// </summary>
		public readonly GridMap PeerIntersection
		{
			get
			{
				long lowerBits = 0, higherBits = 0;
				int i = 0;
				foreach (int offset in Offsets)
				{
					long low = 0, high = 0;
					foreach (int peer in Peers[offset])
					{
						(peer / Shifting == 0 ? ref low : ref high) |= 1L << peer % Shifting;
					}

					if (i++ == 0)
					{
						lowerBits = low;
						higherBits = high;
					}
					else
					{
						lowerBits &= low;
						higherBits &= high;
					}
				}

				return new(higherBits, lowerBits);
			}
		}

		/// <summary>
		/// Indicates all regions covered. This property is used to check all regions that all cells
		/// of this instance covered. For example, if the cells are { 0, 1 }, the property
		/// <see cref="CoveredRegions"/> will return the region 0 (block 1) and region 9 (row 1);
		/// however, if cells spanned two regions or more (e.g. cells { 0, 1, 27 }), this property won't contain
		/// any regions.
		/// </summary>
		public readonly IEnumerable<int> CoveredRegions
		{
			get
			{
				for (int i = BlockOffset; i < Limit; i++)
				{
					if ((_high & ~CoverTable[i, 0]) == 0 && (_low & ~CoverTable[i, 1]) == 0)
					{
						yield return i;
					}
				}
			}
		}

		/// <summary>
		/// All regions that the map spanned. This property is used to check all regions that all cells of
		/// this instance spanned. For example, if the cells are { 0, 1 }, the property
		/// <see cref="Regions"/> will return the region 0 (block 1), region 9 (row 1), region 18 (column 1)
		/// and the region 19 (column 2).
		/// </summary>
		public readonly IEnumerable<int> Regions =>
			((int)BlockMask | RowMask << RowOffset | ColumnMask << ColumnOffset).GetAllSets();

		/// <summary>
		/// Indicates all cell offsets whose corresponding value are set <see langword="true"/>.
		/// </summary>
		private readonly IEnumerable<int> Offsets
		{
			get
			{
				if (IsEmpty)
				{
					yield break;
				}

				long value;
				int i;
				if (_low != 0)
				{
					for (value = _low, i = 0; i < Shifting; i++, value >>= 1)
					{
						if ((value & 1) != 0)
						{
							yield return i;
						}
					}
				}
				if (_high != 0)
				{
					for (value = _high, i = Shifting; i < 81; i++, value >>= 1)
					{
						if ((value & 1) != 0)
						{
							yield return i;
						}
					}
				}
			}
		}


		/// <summary>
		/// Gets or sets a <see cref="bool"/> value on the specified cell
		/// offset.
		/// </summary>
		/// <param name="cell">The cell offset.</param>
		/// <value>A <see cref="bool"/> value on assignment.</value>
		/// <returns>
		/// A <see cref="bool"/> value indicating whether the cell has digit.
		/// </returns>
		[IndexerName("Index")]
		public bool this[int cell]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get => ((stackalloc[] { _low, _high }[cell / Shifting] >> cell % Shifting) & 1L) != 0;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				if (cell is >= 0 and < 81)
				{
					ref long v = ref cell / Shifting == 0 ? ref _low : ref _high;
					bool older = this[cell];
					if (value)
					{
						v |= 1L << cell % Shifting;
						if (!older)
						{
							Count++;
						}
					}
					else
					{
						v &= ~(1L << cell % Shifting);
						if (older)
						{
							Count--;
						}
					}
				}
			}
		}


		/// <inheritdoc cref="DeconstructMethod"/>
		/// <param name="high">(<see langword="out"/> parameter) Higher 40 bits.</param>
		/// <param name="low">(<see langword="out"/> parameter) Lower 41 bits.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly void Deconstruct(out long high, out long low)
		{
			high = _high;
			low = _low;
		}

		/// <inheritdoc cref="object.Equals(object?)"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override readonly bool Equals(object? obj) => obj is GridMap comparer && Equals(comparer);

		/// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
		[CLSCompliant(false)]
		public readonly bool Equals(in GridMap other) => _high == other._high && _low == other._low;

		/// <summary>
		/// Indicates whether this map overlaps another one.
		/// </summary>
		/// <param name="other">(<see langword="in"/> parameter) The other map.</param>
		/// <returns>The <see cref="bool"/> value.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly bool Overlaps(in GridMap other) => (this & other).IsNotEmpty;

		/// <summary>
		/// Indicates whether all cells in this instance are in one region.
		/// </summary>
		/// <param name="region">
		/// (<see langword="out"/> parameter) The region covered. If the return value
		/// is false, this value will be the constant -1.
		/// </param>
		/// <returns>A <see cref="bool"/> result.</returns>
		/// <remarks>
		/// If you don't want to use the <see langword="out"/> parameter value, please
		/// use the property <see cref="InOneRegion"/> to improve the performance.
		/// </remarks>
		/// <seealso cref="InOneRegion"/>
		public readonly bool AllSetsAreInOneRegion(out int region)
		{
			for (int i = BlockOffset; i < Limit; i++)
			{
				if ((_high & ~CoverTable[i, 0]) == 0 && (_low & ~CoverTable[i, 1]) == 0)
				{
					region = i;
					return true;
				}
			}

			region = -1;
			return false;
		}

		/// <summary>
		/// Get a n-th index of the <see langword="true"/> bit in this instance.
		/// </summary>
		/// <param name="index">The true bit index order.</param>
		/// <returns>The real index.</returns>
		/// <remarks>
		/// If you want to select the first set bit, please use <see cref="First"/> instead.
		/// </remarks>
		/// <seealso cref="First"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly int SetAt(int index) => index == 0 ? First : Offsets.ElementAt(index);

		/// <summary>
		/// Get a n-th index of the <see langword="true"/> bit in this instance.
		/// </summary>
		/// <param name="index">(<see langword="in"/> parameter) The true bit index order.</param>
		/// <returns>The real index.</returns>
		/// <remarks>
		/// If you want to select the first set bit, please use <see cref="First"/> instead.
		/// </remarks>
		/// <seealso cref="First"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly int SetAt(in Index index) => SetAt(index.GetOffset(Count));

		/// <summary>
		/// Get all cell offsets whose bits are set <see langword="true"/>.
		/// </summary>
		/// <returns>An array of cell offsets.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly int[] ToArray() => Offsets.ToArray();

		/// <summary>
		/// Get the subview mask of this map.
		/// </summary>
		/// <param name="region">The region.</param>
		/// <returns>The mask.</returns>
		public readonly short GetSubviewMask(int region)
		{
			short p = 0, i = 0;
			foreach (int cell in RegionCells[region])
			{
				if (this[cell])
				{
					p |= (short)(1 << i);
				}

				i++;
			}

			return p;
		}

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly IEnumerator<int> GetEnumerator() => Offsets.GetEnumerator();

		/// <inheritdoc cref="object.GetHashCode"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override readonly int GetHashCode() =>
			GetType().GetHashCode() ^ (int)((_low ^ _high) & int.MaxValue);

		/// <inheritdoc cref="object.ToString"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override readonly string ToString() => ToString(null);

		/// <inheritdoc cref="Formattable.ToString(string?)"/>
		/// <remarks>
		/// The format can be <c><see langword="null"/></c>, <c>N</c>, <c>n</c>, <c>B</c> or <c>b</c>.
		/// If the former three, the return value will be a cell notation collection;
		/// otherwise, the binary representation.
		/// </remarks>
		/// <exception cref="FormatException">Throws when the format is invalid.</exception>
		public readonly string ToString(string? format)
		{
			return format switch
			{
				null or "N" or "n" => Count switch
				{
					0 => "{ }",
					1 when First is var cell => $"r{cell / 9 + 1}c{cell % 9 + 1}",
					_ => normalToString(this)
				},
				"B" or "b" => binaryToString(this),
				_ =>
					throw new FormatException(
						"The specified format is invalid.",
						new ArgumentException(null, nameof(format)))
			};

			static unsafe string normalToString(in GridMap @this)
			{
				const string leftCurlyBrace = "{ ", rightCurlyBrace = " }";
				static int converter(in int v) => v + 1;
				const string separator = ", ";
				var sbRow = new StringBuilder();
				var dic = new Dictionary<int, ICollection<int>>();
				foreach (int cell in @this)
				{
					if (!dic.ContainsKey(cell / 9))
					{
						dic.Add(cell / 9, new List<int>());
					}

					dic[cell / 9].Add(cell % 9);
				}
				bool addCurlyBraces = dic.Count > 1;
				if (addCurlyBraces)
				{
					sbRow.Append(leftCurlyBrace);
				}
				foreach (int row in dic.Keys)
				{
					sbRow
						.Append('r')
						.Append(row + 1)
						.Append('c')
						.AppendRange<int, int>(dic[row], &converter)
						.Append(separator);
				}
				sbRow.RemoveFromEnd(separator.Length);
				if (addCurlyBraces)
				{
					sbRow.Append(rightCurlyBrace);
				}

				dic.Clear();
				var sbColumn = new StringBuilder();
				foreach (int cell in @this)
				{
					if (!dic.ContainsKey(cell % 9))
					{
						dic.Add(cell % 9, new List<int>());
					}

					dic[cell % 9].Add(cell / 9);
				}
				addCurlyBraces = dic.Count > 1;
				if (addCurlyBraces)
				{
					sbColumn.Append(leftCurlyBrace);
				}

				foreach (int column in dic.Keys)
				{
					sbColumn
						.Append('r')
						.AppendRange<int, int>(dic[column], &converter)
						.Append('c')
						.Append(column + 1)
						.Append(separator);
				}
				sbColumn.RemoveFromEnd(separator.Length);
				if (addCurlyBraces)
				{
					sbColumn.Append(rightCurlyBrace);
				}

				return (sbRow.Length > sbColumn.Length ? sbColumn : sbRow).ToString();
			}

			static unsafe string binaryToString(in GridMap @this)
			{
				var sb = new StringBuilder();
				int i;
				long value = @this._low;
				for (i = 0; i < 27; i++, value >>= 1)
				{
					sb.Append(value & 1);
				}
				sb.Append(' ');
				for (; i < 41; i++, value >>= 1)
				{
					sb.Append(value & 1);
				}
				for (value = @this._high; i < 54; i++, value >>= 1)
				{
					sb.Append(value & 1);
				}
				sb.Append(' ');
				for (; i < 81; i++, value >>= 1)
				{
					sb.Append(value & 1);
				}

				return sb.Reverse().ToString();
			}
		}

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly string ToString(string? format, IFormatProvider? formatProvider) =>
			formatProvider.HasFormatted(this, format, out string? result) ? result : ToString(format);

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		/// <summary>
		/// Being called by <see cref="RowMask"/>, <see cref="ColumnMask"/> and <see cref="BlockMask"/>.
		/// </summary>
		/// <param name="start">The start index.</param>
		/// <param name="end">The end index.</param>
		/// <returns>The region mask.</returns>
		/// <seealso cref="RowMask"/>
		/// <seealso cref="ColumnMask"/>
		/// <seealso cref="BlockMask"/>
		private readonly short GetFirst(int start, int end)
		{
			short result = 0;
			for (int i = start; i < end; i++)
			{
				if (Overlaps(RegionMaps[i]))
				{
					result |= (short)(1 << i - start);
				}
			}

			return result;
		}

		/// <summary>
		/// Set the specified cell as <see langword="true"/> or <see langword="false"/> value.
		/// </summary>
		/// <param name="offset">
		/// The cell offset. This value can be positive and negative. If 
		/// negative, the offset will be assigned <see langword="false"/>
		/// into the corresponding bit position of its absolute value.
		/// </param>
		/// <remarks>
		/// <para>
		/// For example, if the offset is -2 (~1), the [1] will be assigned <see langword="false"/>:
		/// <code>
		/// var map = new GridMap(xxx) { ~1 };
		/// </code>
		/// which is equivalent to:
		/// <code>
		/// var map = new GridMap(xxx);
		/// map[1] = false;
		/// </code>
		/// </para>
		/// <para>
		/// Note: The argument <paramref name="offset"/> should be with the bit-complement operator <c>~</c>
		/// to describe the value is a negative one. As the belowing example, -2 is described as <c>~1</c>,
		/// so the offset is 1, rather than 2.
		/// </para>
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(int offset)
		{
			if (offset >= 0) // Positive or zero.
			{
				this[offset] = true;
			}
			else // Negative values.
			{
				this[~offset] = false;
			}
		}

		/// <summary>
		/// Set the specified cell as <see langword="true"/> value.
		/// </summary>
		/// <param name="offset">The cell offset.</param>
		/// <remarks>
		/// Different with <see cref="Add(int)"/>, the method will process negative values,
		/// but this won't.
		/// </remarks>
		/// <seealso cref="Add(int)"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddAnyway(int offset) => this[offset] = true;

		/// <summary>
		/// Set the specified cells as <see langword="true"/> value.
		/// </summary>
		/// <param name="offsets">(<see langword="in"/> parameter) The cells to add.</param>
		public void AddRange(in ReadOnlySpan<int> offsets)
		{
			foreach (int cell in offsets)
			{
				AddAnyway(cell);
			}
		}

		/// <summary>
		/// Set the specified cells as <see langword="true"/> value.
		/// </summary>
		/// <param name="offsets">The cells to add.</param>
		public void AddRange(IEnumerable<int> offsets)
		{
			foreach (int cell in offsets)
			{
				AddAnyway(cell);
			}
		}

		/// <summary>
		/// Set the specified cell as <see langword="false"/> value.
		/// </summary>
		/// <param name="offset">The cell offset.</param>
		/// <remarks>
		/// Different with <see cref="Add(int)"/>, this method <b>can't</b> receive
		/// the negative value as the parameter.
		/// </remarks>
		/// <seealso cref="Add(int)"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Remove(int offset) => this[offset] = false;

		/// <summary>
		/// Clear all bits.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear() => _low = _high = Count = 0;


		/// <inheritdoc cref="Operators.operator =="/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(in GridMap left, in GridMap right) => left.Equals(right);

		/// <inheritdoc cref="Operators.operator !="/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(in GridMap left, in GridMap right) => !(left == right);

		/// <summary>
		/// The syntactic sugar for <c>new GridMap(map) { cell }</c> (i.e. add a new cell into the current
		/// map, and return the new map).
		/// </summary>
		/// <param name="map">(<see langword="in"/> parameter) The map.</param>
		/// <param name="cell">The cell to add.</param>
		/// <returns>The result of the map.</returns>
		/// <remarks>
		/// I strongly recommend you should use <see cref="GridMap(in GridMap)"/> instead
		/// when more than one cell should be added into the map.
		/// </remarks>
		/// <seealso cref="GridMap(in GridMap)"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GridMap operator +(in GridMap map, int cell) => new(map) { cell };

		/// <summary>
		/// The syntactic sugar for <c>new GridMap(map) { ~cell }</c> (i.e. remove a cell from the current
		/// map, and return the new map).
		/// </summary>
		/// <param name="map">(<see langword="in"/> parameter) The map.</param>
		/// <param name="cell">The cell to remove.</param>
		/// <returns>The result of the map.</returns>
		/// <remarks>
		/// I strongly recommend you should use <see cref="GridMap(in GridMap)"/> instead
		/// when more than one cell should be removed from the map.
		/// </remarks>
		/// <seealso cref="GridMap(in GridMap)"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GridMap operator -(in GridMap map, int cell) => new(map) { ~cell };

		/// <summary>
		/// Reverse status for all cells, which means all <see langword="true"/> bits
		/// will be set <see langword="false"/>, and all <see langword="false"/> bits
		/// will be set <see langword="true"/>.
		/// </summary>
		/// <param name="gridMap">(<see langword="in"/> parameter) The instance to negate.</param>
		/// <returns>The negative result.</returns>
		/// <remarks>
		/// While reversing the higher 40 bits, the unused bits will be fixed and never be modified the state,
		/// that is why using the code "<c>higherBits &amp; 0xFFFFFFFFFFL</c>".
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GridMap operator ~(in GridMap gridMap) =>
			new(~gridMap._high & 0xFFFFFFFFFFL, ~gridMap._low);

		/// <summary>
		/// Get a <see cref="GridMap"/> that contains all <paramref name="left"/> cells
		/// but not in <paramref name="right"/> cells.
		/// </summary>
		/// <param name="left">(<see langword="in"/> parameter) The left instance.</param>
		/// <param name="right">(<see langword="in"/> parameter) The right instance.</param>
		/// <returns>The result.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GridMap operator -(in GridMap left, in GridMap right) => left & ~right;

		/// <summary>
		/// Get all cells that two <see cref="GridMap"/>s both contain.
		/// </summary>
		/// <param name="left">(<see langword="in"/> parameter) The left instance.</param>
		/// <param name="right">(<see langword="in"/> parameter) The right instance.</param>
		/// <returns>The intersection result.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GridMap operator &(in GridMap left, in GridMap right) =>
			new(left._high & right._high, left._low & right._low);

		/// <summary>
		/// Get all cells from two <see cref="GridMap"/>s.
		/// </summary>
		/// <param name="left">(<see langword="in"/> parameter) The left instance.</param>
		/// <param name="right">(<see langword="in"/> parameter) The right instance.</param>
		/// <returns>The union result.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GridMap operator |(in GridMap left, in GridMap right) =>
			new(left._high | right._high, left._low | right._low);

		/// <summary>
		/// Get all cells that only appears once in two <see cref="GridMap"/>s.
		/// </summary>
		/// <param name="left">(<see langword="in"/> parameter) The left instance.</param>
		/// <param name="right">(<see langword="in"/> parameter) The right instance.</param>
		/// <returns>The symmetrical difference result.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GridMap operator ^(in GridMap left, in GridMap right) =>
			new(left._high ^ right._high, left._low ^ right._low);


		/// <summary>
		/// Implicit cast from <see cref="int"/>[] to <see cref="GridMap"/>.
		/// </summary>
		/// <param name="cells">The cells.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator GridMap(int[] cells) => new(cells);

		/// <summary>
		/// Implicit cast from <see cref="Span{T}"/> to <see cref="GridMap"/>.
		/// </summary>
		/// <param name="cells">(<see langword="in"/> parameter) The cells.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator GridMap(in Span<int> cells) => new(cells);

		/// <summary>
		/// Implicit cast from <see cref="ReadOnlySpan{T}"/> to <see cref="GridMap"/>.
		/// </summary>
		/// <param name="cells">(<see langword="in"/> parameter) The cells.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator GridMap(in ReadOnlySpan<int> cells) => new(cells);

		/// <summary>
		/// Explicit cast from <see cref="GridMap"/> to <see cref="int"/>[].
		/// </summary>
		/// <param name="map">(<see langword="in"/> parameter) The map.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int[](in GridMap map) => map.ToArray();
	}
}
