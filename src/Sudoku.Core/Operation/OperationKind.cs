namespace Sudoku.Operation;

/// <summary>
/// Provides with a kind of difference of candidates.
/// </summary>
public enum OperationKind
{
	/// <summary>
	/// Indicates the difference kind is none.
	/// </summary>
	None,

	/// <summary>
	/// Indicates the difference kind is assignment.
	/// </summary>
	Assignment,

	/// <summary>
	/// Indicates the difference kind is initialize appending candidates.
	/// </summary>
	/// <remarks>
	/// This operation is reserved one, to describe a user desires to start marking candidates.
	/// From the algorithm, if a user append a candidate at the first time, we should initialize the environment,
	/// removing all candidates that this user doesn't add.
	/// </remarks>
	InitialCandidatesMarking,

	/// <summary>
	/// Indicates the difference kind is appending candidates.
	/// </summary>
	Appending,

	/// <summary>
	/// Indicates the difference kind is elimination.
	/// </summary>
	Elimination,

	/// <summary>
	/// Indicates the difference kind is unknown.
	/// </summary>
	Unknown
}