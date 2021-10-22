﻿namespace Sudoku.Diagnostics.CodeGen;

/// <summary>
/// Indicates an attribute that is used for a <see langword="class"/> or <see langword="struct"/>
/// as a mark that interacts with the source generator, to tell the source generator that
/// it'll generate the source code for <c>GetHashCode</c> overriden.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
public sealed class AutoGetHashCodeAttribute : Attribute
{
	/// <summary>
	/// Initializes an instance with the specified member list.
	/// </summary>
	/// <param name="dataMembers">The data members.</param>
	public AutoGetHashCodeAttribute(params string[] dataMembers) => DataMembers = dataMembers;


	/// <summary>
	/// All members to generate.
	/// </summary>
	public string[] DataMembers { get; }
}
