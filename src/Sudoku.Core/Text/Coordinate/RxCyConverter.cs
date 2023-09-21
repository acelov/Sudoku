using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using Sudoku.Analytics;
using Sudoku.Concepts;
using static System.Numerics.BitOperations;

namespace Sudoku.Text.Coordinate;

/// <summary>
/// Represents a coordinate converter using RxCy notation.
/// </summary>
/// <param name="MakeLettersUpperCase">
/// <para>Indicates whether we make the letters <c>'r'</c>, <c>'c'</c> and <c>'b'</c> to be upper-casing.</para>
/// <para>The value is <see langword="false"/> by default.</para>
/// </param>
/// <param name="MakeDigitBeforeCell">
/// <para>Indicates whether digits will be displayed before the cell coordinates.</para>
/// <para>The value is <see langword="false"/> by default.</para>
/// </param>
/// <param name="HouseNotationOnlyDisplayCapitals">
/// <para>Indicates whether the houses will be displayed its capitals only.</para>
/// <para>The value is <see langword="false"/> by default.</para>
/// </param>
/// <param name="DefaultSeparator">
/// <para>Indicates the default separator. The value will be inserted into two non-digit-kind instances.</para>
/// <para>The value is <c>", "</c> by default.</para>
/// </param>
/// <param name="DigitsSeprarator">
/// <para>DigitsSeprarator</para>
/// <para>The value is <see langword="null"/> by default, meaning no separators will be inserted between 2 digits.</para>
/// </param>
public sealed record RxCyConverter(
	bool MakeLettersUpperCase = false,
	bool MakeDigitBeforeCell = false,
	bool HouseNotationOnlyDisplayCapitals = false,
	string DefaultSeparator = ", ",
	string? DigitsSeprarator = null
) : CoordinateConverter
{
	/// <inheritdoc/>
	public override CellNotationConverter CellNotationConverter
		=> (scoped ref readonly CellMap cells) =>
		{
			return cells switch
			{
				[] => string.Empty,
				[var p] => MakeLettersUpperCase switch { true => $"R{p / 9 + 1}C{p % 9 + 1}", _ => $"r{p / 9 + 1}c{p % 9 + 1}" },
				_ => r(in cells) is var a && c(in cells) is var b && a.Length <= b.Length ? a : b
			};


			string r(scoped ref readonly CellMap cells)
			{
				scoped var sbRow = new StringHandler(50);
				var dic = new Dictionary<int, List<int>>(9);
				foreach (var cell in cells)
				{
					if (!dic.ContainsKey(cell / 9))
					{
						dic.Add(cell / 9, new(9));
					}

					dic[cell / 9].Add(cell % 9);
				}
				foreach (var row in dic.Keys)
				{
					sbRow.Append(MakeLettersUpperCase ? 'R' : 'r');
					sbRow.Append(row + 1);
					sbRow.Append(MakeLettersUpperCase ? 'C' : 'c');
					sbRow.AppendRange(dic[row], d => DigitNotationConverter((Mask)(1 << d)));
					sbRow.Append(DefaultSeparator);
				}
				sbRow.RemoveFromEnd(DefaultSeparator.Length);

				return sbRow.ToStringAndClear();
			}

			string c(scoped ref readonly CellMap cells)
			{
				var dic = new Dictionary<int, List<int>>(9);
				scoped var sbColumn = new StringHandler(50);
				foreach (var cell in cells)
				{
					if (!dic.ContainsKey(cell % 9))
					{
						dic.Add(cell % 9, new(9));
					}

					dic[cell % 9].Add(cell / 9);
				}

				foreach (var column in dic.Keys)
				{
					sbColumn.Append(MakeLettersUpperCase ? 'R' : 'r');
					sbColumn.AppendRange(dic[column], d => DigitNotationConverter((Mask)(1 << d)));
					sbColumn.Append(MakeLettersUpperCase ? 'C' : 'c');
					sbColumn.Append(column + 1);
					sbColumn.Append(DefaultSeparator);
				}
				sbColumn.RemoveFromEnd(DefaultSeparator.Length);

				return sbColumn.ToStringAndClear();
			}
		};

	/// <inheritdoc/>
	public override CandidateNotationConverter CandidateNotationConverter
		=> (scoped ref readonly CandidateMap candidates) =>
		{
			scoped var sb = new StringHandler(50);
			foreach (var digitGroup in
				from candidate in candidates
				group candidate by candidate % 9 into digitGroups
				orderby digitGroups.Key
				select digitGroups)
			{
				var cells = CellMap.Empty;
				foreach (var candidate in digitGroup)
				{
					cells.Add(candidate / 9);
				}

				if (MakeDigitBeforeCell)
				{
					sb.Append(digitGroup.Key + 1);
					sb.Append(CellNotationConverter(in cells));
				}
				else
				{
					sb.Append(CellNotationConverter(in cells));
					sb.Append('(');
					sb.Append(digitGroup.Key + 1);
					sb.Append(')');
				}

				sb.Append(DefaultSeparator);
			}

			sb.RemoveFromEnd(DefaultSeparator.Length);
			return sb.ToStringAndClear();
		};

	/// <inheritdoc/>
	public override HouseNotationConverter HouseNotationConverter
		=> housesMask =>
		{
			if (housesMask == 0)
			{
				return string.Empty;
			}

			if (HouseNotationOnlyDisplayCapitals)
			{
				scoped var sb = new StringHandler(27);
				for (var (houseIndex, i) = (9, 0); i < 27; i++, houseIndex = (houseIndex + 1) % 27)
				{
					if ((housesMask >> houseIndex & 1) != 0)
					{
						sb.Append(getChar(houseIndex / 9));
					}
				}

				return sb.ToStringAndClear();
			}

			if (IsPow2((uint)housesMask))
			{
				var house = Log2((uint)housesMask);
				return $"{getChar(house)}{house % 9 + 1}";
			}

			{
				var dic = new Dictionary<HouseType, List<int>>(3);
				foreach (var house in housesMask)
				{
					var houseType = house.ToHouseType();
					if (!dic.TryAdd(houseType, [house]))
					{
						dic[houseType].Add(house);
					}
				}

				scoped var sb = new StringHandler(30);
				foreach (var (houseType, h) in from kvp in dic orderby kvp.Key.GetProgramOrder() select kvp)
				{
					sb.Append(houseType.GetLabel());
					sb.AppendRange(from house in h select house % 9 + 1);
				}

				return sb.ToStringAndClear();
			}


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static char getChar(House house)
				=> house switch
				{
					>= 0 and < 9 => 'b',
					>= 9 and < 18 => 'r',
					>= 18 and < 27 => 'c',
					_ => throw new ArgumentOutOfRangeException(nameof(house))
				};
		};

	/// <inheritdoc/>
	public override ConclusionNotationConverter ConclusionNotationConverter
		=> (scoped ReadOnlySpan<Conclusion> conclusions) =>
		{
			return conclusions switch
			{
				[] => string.Empty,
				[(var t, var c, var d)] => $"{CellNotationConverter([c])}{t.Notation()}{DigitNotationConverter((Mask)(1 << d))}",
				_ => toString(conclusions)
			};


			static int cmp(scoped ref readonly Conclusion left, scoped ref readonly Conclusion right) => left.CompareTo(right);

			unsafe string toString(scoped ReadOnlySpan<Conclusion> c)
			{
				var conclusions = new Conclusion[c.Length];
				Unsafe.CopyBlock(
					ref Unsafe.As<Conclusion, byte>(ref conclusions[0]),
					in Unsafe.As<Conclusion, byte>(ref Unsafe.AsRef(in c[0])),
					(uint)(sizeof(Conclusion) * c.Length)
				);

				scoped var sb = new StringHandler(50);
				conclusions.Sort(&cmp);

				var selection = from conclusion in conclusions orderby conclusion.Digit group conclusion by conclusion.ConclusionType;
				var hasOnlyOneType = selection.HasOnlyOneElement();
				foreach (var typeGroup in selection)
				{
					var op = typeGroup.Key.Notation();
					foreach (var digitGroup in from conclusion in typeGroup group conclusion by conclusion.Digit)
					{
						sb.Append(CellMap.Empty + from conclusion in digitGroup select conclusion.Cell);
						sb.Append(op);
						sb.Append(digitGroup.Key + 1);
						sb.Append(DefaultSeparator);
					}

					sb.RemoveFromEnd(DefaultSeparator.Length);
					if (!hasOnlyOneType)
					{
						sb.Append(DefaultSeparator);
					}
				}

				if (!hasOnlyOneType)
				{
					sb.RemoveFromEnd(DefaultSeparator.Length);
				}

				return sb.ToStringAndClear();
			}
		};

	/// <inheritdoc/>
	public override DigitNotationConverter DigitNotationConverter
		=> new LiteralCoordinateConverter { DefaultSeparator = DefaultSeparator }.DigitNotationConverter;

	/// <inheritdoc/>
	public override IntersectionNotationConverter IntersectionNotationConverter
		=> (scoped ReadOnlySpan<(IntersectionBase Base, IntersectionResult Result)> intersections) => DefaultSeparator switch
		{
			null or [] => string.Concat([
				..
				from intersection in intersections
				let baseSet = intersection.Base.Line
				let coverSet = intersection.Base.Block
				select $"{GetLabel((byte)(baseSet / 9))}{baseSet % 9 + 1}{GetLabel((byte)(coverSet / 9))}{coverSet % 9 + 1}"
			]),
			_ => string.Join(
				DefaultSeparator,
				[
					..
					from intersection in intersections
					let baseSet = intersection.Base.Line
					let coverSet = intersection.Base.Block
					select $"{GetLabel((byte)(baseSet / 9))}{baseSet % 9 + 1}{GetLabel((byte)(coverSet / 9))}{coverSet % 9 + 1}"
				]
			)
		};

	/// <inheritdoc/>
	public override ChuteNotationConverter ChuteNotationConverter
		=> (scoped ReadOnlySpan<Chute> chutes) =>
		{
			var megalines = new Dictionary<bool, byte>(2);
			foreach (var (index, _, isRow, _) in chutes)
			{
				if (!megalines.TryAdd(isRow, (byte)(1 << index % 3)))
				{
					megalines[isRow] |= (byte)(1 << index % 3);
				}
			}

			var sb = new StringHandler(12);
			if (megalines.TryGetValue(true, out var megaRows))
			{
				sb.Append(MakeLettersUpperCase ? "MR" : "mr");
				foreach (var megaRow in megaRows)
				{
					sb.Append(megaRow);
				}

				sb.Append(DefaultSeparator);
			}
			if (megalines.TryGetValue(false, out var megaColumns))
			{
				sb.Append(MakeLettersUpperCase ? "MC" : "mc");
				foreach (var megaColumn in megaColumns)
				{
					sb.Append(megaColumn);
				}
			}

			return sb.ToStringAndClear();
		};

	/// <inheritdoc/>
	public override ConjugateNotationConverter ConjugateNotationConverter
		=> (scoped ReadOnlySpan<Conjugate> conjugatePairs) =>
		{
			if (conjugatePairs.Length == 0)
			{
				return string.Empty;
			}

			var sb = new StringHandler(20);
			foreach (var conjugatePair in conjugatePairs)
			{
				var fromCellString = CellNotationConverter([conjugatePair.From]);
				var toCellString = CellNotationConverter([conjugatePair.To]);
				sb.Append(
					MakeDigitBeforeCell
						? $"{DigitNotationConverter((Mask)(1 << conjugatePair.Digit))}{fromCellString} == {toCellString}"
						: $"{fromCellString} == {toCellString}({DigitNotationConverter((Mask)(1 << conjugatePair.Digit))})"
				);

				sb.Append(DefaultSeparator);
			}
			sb.RemoveFromEnd(DefaultSeparator.Length);

			return sb.ToStringAndClear();
		};


	/// <summary>
	/// Get the label of each house.
	/// </summary>
	/// <param name="houseIndex">The house index.</param>
	/// <returns>The label.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private char GetLabel(byte houseIndex)
		=> (houseIndex, MakeLettersUpperCase) switch
		{
			(0, true) => 'B',
			(0, _) => 'b',
			(1, true) => 'R',
			(1, _) => 'r',
			(2, true) => 'C',
			_ => 'c'
		};
}