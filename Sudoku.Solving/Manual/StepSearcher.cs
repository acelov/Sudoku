﻿using System;
using System.Collections.Generic;
using System.Extensions;
using System.Linq;
using System.Reflection;
using Sudoku.Data;
using Sudoku.Solving.Manual.Tracing;
using static Sudoku.Resources.TextResources;

namespace Sudoku.Solving.Manual
{
	/// <summary>
	/// Encapsulates a step finder that used in solving in <see cref="ManualSolver"/>.
	/// </summary>
	/// <seealso cref="ManualSolver"/>
	public abstract class StepSearcher
	{
		/// <summary>
		/// Indicates all step searchers and their type info used in the current solution.
		/// </summary>
		/// <remarks>
		/// Please note that the return value is a list of elements that contain its type and its
		/// searcher properties.
		/// </remarks>
		public static IEnumerable<(Type CurrentType, string SearcherName, TechniqueProperties Properties)> AllStepSearchers =>
			from type in Assembly.GetExecutingAssembly().GetTypes()
			where !type.IsAbstract && type.IsSubclassOf<StepSearcher>() && !type.IsDefined<ObsoleteAttribute>()
			let prior = TechniqueProperties.FromType(type)!.Priority
			orderby prior
			let v = type.GetProperty("Properties", BindingFlags.Public | BindingFlags.Static)?.GetValue(null)
			let casted = v as TechniqueProperties
			where casted is not null && !casted.DisabledReason.Flags(DisabledReason.HasBugs)
			select (type, GetValue($"Progress{casted.DisplayLabel}"), casted);


		/// <summary>
		/// Take a technique step after searched all solving steps.
		/// </summary>
		/// <param name="grid">(<see langword="in"/> parameter) The grid to search steps.</param>
		/// <returns>A technique information.</returns>
		public StepInfo? GetOne(in SudokuGrid grid)
		{
			var bag = new List<StepInfo>();
			GetAll(bag, grid);
			return bag.FirstOrDefault();
		}

		/// <summary>
		/// Accumulate all technique information instances into the specified accumulator.
		/// </summary>
		/// <param name="accumulator">The accumulator to store technique information.</param>
		/// <param name="grid">(<see langword="in"/> parameter) The grid to search for techniques.</param>
		public abstract void GetAll(IList<StepInfo> accumulator, in SudokuGrid grid);


		/// <summary>
		/// To bind a step.
		/// </summary>
		/// <param name="grid">The grid.</param>
		public void BindOne(TraceableGrid grid) => grid.BindStep(InternalOnBinding(grid)[0]);

		/// <summary>
		/// To bind all steps.
		/// </summary>
		/// <param name="grid">The grid.</param>
		public void BindAll(TraceableGrid grid) => ((ITraceable)grid).BindSteps(InternalOnBinding(grid));

		/// <summary>
		/// To bind all steps that satisfy the specified condition.
		/// </summary>
		/// <param name="grid">The grid.</param>
		/// <param name="condition">The condition.</param>
		public void BindWhen(TraceableGrid grid, Predicate<StepInfo> condition)
		{
			foreach (var step in InternalOnBinding(grid))
			{
				if (condition(step))
				{
					grid.BindStep(step);
				}
			}
		}

		/// <summary>
		/// The method that is called by binding ones.
		/// </summary>
		/// <param name="grid">The grid.</param>
		/// <returns>All possible steps that may be bound.</returns>
		private IReadOnlyList<StepInfo> InternalOnBinding(TraceableGrid grid)
		{
			var bag = new List<StepInfo>();
			GetAll(bag, (SudokuGrid)grid);

			return bag;
		}
	}
}
