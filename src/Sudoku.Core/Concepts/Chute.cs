namespace Sudoku.Concepts;

/// <summary>
/// Defines a chute.
/// </summary>
/// <param name="Cells">The cells used.</param>
/// <param name="IsRow">Indicates whether the chute is in a mega-row.</param>
/// <param name="HousesMask">Indicates the houses used.</param>
public readonly record struct Chute(scoped ref readonly CellMap Cells, bool IsRow, Mask HousesMask);
