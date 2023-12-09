using Sudoku.Analytics;

namespace Sudoku.ComponentModel;

/// <summary>
/// Represents a delegate type that executes a list of methods that will be triggered when analyzer has detected all found steps,
/// and applied them.
/// </summary>
/// <param name="sender">The object that triggers the execution.</param>
/// <param name="e">The event argument.</param>
/// <inheritdoc cref="ExceptionThrownEventHandler{TSelf, TResult}"/>
public delegate void FullAppliedEventHandler<in TSelf, out TResult>(TSelf sender, FullAppliedEventArgs e)
	where TSelf : IAnalyzer<TSelf, TResult>
	where TResult : IAnalyzerResult<TSelf, TResult>;
