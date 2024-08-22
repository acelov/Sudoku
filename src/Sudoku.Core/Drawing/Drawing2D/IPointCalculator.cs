namespace Sudoku.Drawing.Drawing2D;

using LocalPoint = (float X, float Y);
using LocalSize = (float Width, float Height);

/// <summary>
/// Represents a base type of pointer calculator.
/// </summary>
public interface IPointCalculator
{
	/// <summary>
	/// Indicates the width of the picture to draw.
	/// </summary>
	public float Width { get; }

	/// <summary>
	/// Indicates the height of the picture to draw.
	/// </summary>
	public float Height { get; }

	/// <summary>
	/// Indicates the padding of the gap between the picture box outline and the sudoku grid outline.
	/// </summary>
	public float Padding { get; }

	/// <summary>
	/// Indicates the control size.
	/// </summary>
	public LocalSize ControlSize { get; }

	/// <summary>
	/// Indicates the grid size.
	/// </summary>
	public LocalSize GridSize { get; }

	/// <summary>
	/// Indicates the cell size.
	/// </summary>
	public LocalSize CellSize { get; }

	/// <summary>
	/// Indicates the candidate size.
	/// </summary>
	public LocalSize CandidateSize { get; }

	/// <summary>
	/// Indicates the absolutely points in grid cross-lines.
	/// This property will be assigned later (and not <see langword="null"/>).
	/// </summary>
	/// <remarks>Note that the size of this 2D array is always 28 by 28.</remarks>
	public LocalPoint[,] GridPoints { get; }
}
