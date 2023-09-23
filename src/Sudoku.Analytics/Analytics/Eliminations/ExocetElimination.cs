using System.Runtime.CompilerServices;
using Sudoku.Text.Coordinate;
using static Sudoku.Analytics.Strings.StringsAccessor;

namespace Sudoku.Analytics.Eliminations;

/// <summary>
/// Represents a data pattern that describes the eliminations that are created and proved by the exocet technique.
/// </summary>
/// <param name="Conclusions">Indicates the conclusions.</param>
/// <param name="Reason">Indicates the reason why these candidates can be eliminated.</param>
public readonly record struct ExocetElimination(Conclusion[] Conclusions, ExocetEliminatedReason Reason) :
	IPhasedConclusionProvider<ExocetElimination, ExocetEliminatedReason>
{
	/// <inheritdoc cref="object.ToString"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString()
	{
		var header = GetString($"Exocet{Reason}EliminationName")!;
		var snippet = GetString("ExocetElimination")!;
		var elim = new RxCyConverter().ConclusionConverter(Conclusions);
		return $"* {header}{snippet}{elim}";
	}
}
