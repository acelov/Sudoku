﻿namespace Sudoku.Solving.Data.Representation;

/// <summary>
/// Represents for a data structure that describes for a technique structure.
/// </summary>
/// <typeparam name="TTechniqueDataInfo">The type of the implemented data structure.</typeparam>
public interface ITechniqueDataInfo<TTechniqueDataInfo>
	where TTechniqueDataInfo : struct, ITechniqueDataInfo<TTechniqueDataInfo>
{
	/// <summary>
	/// Indicates the cells used in this whole technique structure.
	/// </summary>
	public abstract Cells Map { get; }
}
