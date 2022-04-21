﻿namespace Sudoku.CommandLine.CommandOptions;

/// <summary>
/// Represents a solve command.
/// </summary>
public sealed class Solve : IRootCommand
{
	/// <summary>
	/// Indicates the method to be used.
	/// </summary>
	[Command('m', "method", "Indicates the method to be used for solving a sudoku.")]
	[CommandConverter(typeof(EnumTypeConverter<SolveAlgorithm>))]
	public SolveAlgorithm SolveMethod { get; set; } = SolveAlgorithm.Bitwise;

	/// <summary>
	/// Indicates the grid used.
	/// </summary>
	[Command('g', "grid", "Indicates the grid used for being solved.")]
	[CommandConverter(typeof(GridConverter))]
	public Grid Grid { get; set; }

	/// <inheritdoc/>
	public static string Name => "solve";

	/// <inheritdoc/>
	public static string Description => "To solve a sudoku grid using the specified algorithm.";

	/// <inheritdoc/>
	public static string[] SupportedCommands => new[] { "solve" };

	/// <inheritdoc/>
	public static IEnumerable<(string CommandLine, string Meaning)>? UsageCommands =>
		new[]
		{
			(
				"""
				solve -g "...892.....2...3..75.....69.359.814...........713.659.96.....21..4...6.....621..." -m bitwise
				""",
				"Solves a sudoku puzzle, using the bitwise algorithm."
			)
		};


	/// <inheritdoc/>
	public void Execute()
	{
		if (Grid.Solution is not { IsUndefined: false } solution)
		{
			throw new CommandLineException((int)ErrorCode.ArgGridValueIsNotUnique);
		}

		string? methodNameUsed = null;
		foreach (var type in
			from type in typeof(ISimpleSolver).Assembly.GetTypes()
			where type.IsClass && type.IsAssignableTo(typeof(ISimpleSolver))
			let parameterlessConstructor = type.GetConstructor(Array.Empty<Type>())
			where parameterlessConstructor is not null
			select type)
		{
			var fieldInfo = typeof(SolveAlgorithm).GetField(SolveMethod.ToString())!;
			var actualType = fieldInfo.GetCustomAttribute<RouteToTypeAttribute>()!.TypeToRoute;
			if (actualType != type)
			{
				continue;
			}

			string? uriLink = (string?)type.GetProperty(nameof(ISimpleSolver.UriLink))!.GetValue(null);
			string name = fieldInfo.GetCustomAttribute<DescriptionAttribute>()!.Description;
			switch (Activator.CreateInstance(type))
			{
				case ISimpleSolver simpleSolver:
				{
					if (simpleSolver.Solve(Grid, out _) is not true)
					{
						throw new CommandLineException((int)ErrorCode.ArgGridValueIsNotUnique);
					}

					// .NET Runtime issue: If the type does not implement 'IFormattable',
					// the format string is meaningless to be used in the interpolated string holes.
					// In this invocation, type 'Grid' does not implement the type 'IFormattable',
					// therefore, we cannot use the interpolated string syntax like '$"{grid:#}"'
					// to get the same result as 'grid.ToString("#")'; on contrast, 'grid.ToString("#")'
					// as expected will be replaced with 'grid.ToString()'.
					// Same reason for the below output case.
					string uriLinkRealStr = uriLink is null ? string.Empty : $"\r\nURI link: '{uriLink}'";
					Terminal.WriteLine(
						$"""
						Puzzle: {Grid:#}
						Method name used: '{name}'{uriLinkRealStr}
						---
						Solution: {solution:!}
						"""
					);

					break;
				}
				case IComplexSolver<ManualSolverResult> puzzleSolver:
				{
					if (puzzleSolver.Solve(Grid) is not { IsSolved: true } solverResult)
					{
						throw new CommandLineException((int)ErrorCode.ArgGridValueIsNotUnique);
					}

					Terminal.WriteLine(
						$"""
						Puzzle: {Grid:#}
						Method name used: '{methodNameUsed}'
						---
						Solution: {solution:!}
						Solving details:
						{solverResult}
						"""
					);

					break;
				}
			}
		}

		if (methodNameUsed is null)
		{
			throw new CommandLineException((int)ErrorCode.ArgMethodIsInvalid);
		}
	}
}
