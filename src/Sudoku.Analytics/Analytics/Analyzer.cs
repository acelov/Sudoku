namespace Sudoku.Analytics;

/// <summary>
/// Provides an analyzer that solves a sudoku puzzle using the human-friendly logics, and creates an <see cref="AnalyzerResult"/> instance
/// indicating the analytics data.
/// </summary>
/// <remarks>
/// Please note that this type has no accessible constructors,
/// you can just use type <see cref="PredefinedAnalyzers"/> to get <see cref="Analyzer"/>s you want to get.
/// In addition, you can also use <see cref="AnalyzerFactory"/> to create some extra configuration.
/// </remarks>
/// <seealso cref="AnalyzerResult"/>
/// <seealso cref="PredefinedAnalyzers"/>
/// <seealso cref="AnalyzerFactory"/>
/// <completionlist cref="PredefinedAnalyzers"/>
public sealed class Analyzer : IAnalyzer<Analyzer, AnalyzerResult>, IAnalyzerOrCollector
{
	/// <summary>
	/// Indicates the backing field of property <see cref="StepSearchers"/>.
	/// </summary>
	/// <seealso cref="StepSearchers"/>
	private StepSearcher[]? _stepSearchers;


	/// <summary>
	/// Indicates whether the solver will apply all found steps in a step searcher,
	/// in order to solve a puzzle faster. If the value is <see langword="true"/>,
	/// the third argument of <see cref="StepSearcher.Collect(ref AnalysisContext)"/>
	/// will be set <see langword="false"/> value, in order to find all possible steps in a step searcher,
	/// and all steps will be applied at the same time.
	/// </summary>
	/// <remarks>
	/// The default value is <see langword="false"/>.
	/// </remarks>
	/// <seealso cref="StepSearcher.Collect(ref AnalysisContext)"/>
	public bool IsFullApplying { get; set; }

	/// <summary>
	/// Indicates whether the solver will ignore slow step searchers being configured <see cref="ConditionalCase.UnlimitedTimeComplexity"/>.
	/// </summary>
	/// <remarks>
	/// The default value is <see langword="false"/>.
	/// </remarks>
	/// <seealso cref="ConditionalCase.UnlimitedTimeComplexity"/>
	public bool IgnoreSlowAlgorithms { get; internal set; }

	/// <summary>
	/// Indicates whether the solver will ignore slow step searchers being configured
	/// <see cref="ConditionalCase.UnlimitedSpaceComplexity"/>.
	/// </summary>
	/// <remarks>
	/// The default value is <see langword="false"/>.
	/// </remarks>
	/// <seealso cref="ConditionalCase.UnlimitedSpaceComplexity"/>
	public bool IgnoreHighAllocationAlgorithms { get; internal set; }

	/// <inheritdoc/>
	[DisallowNull]
	public StepSearcher[]? StepSearchers
	{
		get => _stepSearchers;

		internal set => ResultStepSearchers = IAnalyzerOrCollector.FilterStepSearchers(_stepSearchers = value, StepSearcherRunningArea.Searching);
	}

	/// <inheritdoc/>
	public StepSearcher[] ResultStepSearchers { get; private set; } =
	(
		from searcher in StepSearcherPool.Default(true)
		where searcher.RunningArea.Flags(StepSearcherRunningArea.Searching)
		select searcher
	).ToArray();


	/// <summary>
	/// Initializes an <see cref="Analyzer"/> instance.
	/// </summary>
	[Obsolete($"This constructor may not produce some extra options. Please visit type '{nameof(PredefinedAnalyzers)}' to get a suitable instance.", false)]
	public Analyzer()
	{
	}


	/// <inheritdoc/>
	public AnalyzerResult Analyze(scoped in Grid puzzle, IProgress<double>? progress = null, CancellationToken cancellationToken = default)
	{
		var result = new AnalyzerResult(puzzle) { IsSolved = false };
		if (puzzle.ExactlyValidate(out var solution, out var sukaku) && sukaku is { } isSukaku)
		{
			try
			{
				return analyzing(puzzle, solution, isSukaku, result, progress, cancellationToken);
			}
			catch (Exception ex)
			{
				return ex switch
				{
					NotImplementedException or NotSupportedException
						=> result with { IsSolved = false, FailedReason = AnalyzerFailedReason.NotImplemented },
					WrongStepException
						=> result with { IsSolved = false, FailedReason = AnalyzerFailedReason.WrongStep, UnhandledException = ex },
					OperationCanceledException { CancellationToken: var c } when c == cancellationToken
						=> result with { IsSolved = false, FailedReason = AnalyzerFailedReason.UserCancelled },
					_ when ex.GetType().IsGenericAssignableTo(typeof(StepSearcherProcessException<>))
						=> result with { IsSolved = false, FailedReason = AnalyzerFailedReason.PuzzleIsInvalid },
					_
						=> result with { IsSolved = false, FailedReason = AnalyzerFailedReason.ExceptionThrown, UnhandledException = ex }
				};
			}
		}
		else
		{
			return result with { IsSolved = false, FailedReason = AnalyzerFailedReason.PuzzleIsInvalid };
		}


		AnalyzerResult analyzing(
			scoped in Grid puzzle,
			scoped in Grid solution,
			bool isSukaku,
			AnalyzerResult resultBase,
			IProgress<double>? progress = null,
			CancellationToken cancellationToken = default
		)
		{
			var playground = puzzle;
			var totalCandidatesCount = playground.CandidatesCount;
			var recordedSteps = new List<Step>(100);
			var stepGrids = new List<Grid>(100);
			var stepSearchers = ResultStepSearchers;

			scoped var stopwatch = ValueStopwatch.StartNew();

		Again:
			Initialize(playground, solution);
			foreach (var searcher in stepSearchers)
			{
				switch (isSukaku, searcher, this)
				{
					case (true, { IsNotSupportedForSukaku: true }, _):
					case (_, { RunningArea: StepSearcherRunningArea.None }, _):
					case (_, { IsConfiguredSlow: true }, { IgnoreSlowAlgorithms: true }):
					case (_, { IsConfiguredHighAllocation: true }, { IgnoreHighAllocationAlgorithms: true }):
					{
						// Skips on those two cases:
						// 1. Sukaku puzzles can't use techniques that is marked as "not supported for sukaku".
						// 2. If the searcher is currently disabled, just skip it.
						// 3. If the searcher is configured as slow.
						// 4. If the searcher is configured as high-allocation.
						continue;
					}
					case (_, not BruteForceStepSearcher, { IsFullApplying: true }):
					{
						var accumulator = new List<Step>();
						scoped var context = new AnalysisContext(accumulator, playground, false);
						searcher.Collect(ref context);
						if (accumulator.Count == 0)
						{
							continue;
						}

						foreach (var foundStep in accumulator)
						{
							if (verifyConclusionValidity(solution, foundStep))
							{
								if (recordingStep(
									recordedSteps, foundStep, ref playground, ref stopwatch, stepGrids,
									resultBase, cancellationToken, out var result))
								{
									return result;
								}
							}
							else
							{
								throw new WrongStepException(playground, foundStep);
							}
						}

						// The puzzle has not been finished, we should turn to the first step finder
						// to continue solving puzzle.
						goto ReportStatusAndSkipToTryAgain;
					}
					default:
					{
						scoped var context = new AnalysisContext(null, playground, true);
						switch (searcher.Collect(ref context))
						{
							case null:
							{
								continue;
							}
							case var foundStep:
							{
								if (verifyConclusionValidity(solution, foundStep))
								{
									if (recordingStep(
										recordedSteps, foundStep, ref playground, ref stopwatch, stepGrids,
										resultBase, cancellationToken, out var result))
									{
										return result;
									}
								}
								else
								{
									throw new WrongStepException(playground, foundStep);
								}

								// The puzzle has not been finished, we should turn to the first step finder
								// to continue solving puzzle.
								goto ReportStatusAndSkipToTryAgain;
							}
						}
					}
				}
			}

			// All solver can't finish the puzzle...
			// :(

			return resultBase with
			{
				FailedReason = AnalyzerFailedReason.PuzzleIsTooHard,
				ElapsedTime = stopwatch.GetElapsedTime(),
				Steps = recordedSteps.ToArray(),
				StepGrids = stepGrids.ToArray()
			};

		ReportStatusAndSkipToTryAgain:
			progress?.Report((double)(totalCandidatesCount - playground.CandidatesCount) / totalCandidatesCount);
			goto Again;


			static bool verifyConclusionValidity(scoped in Grid solution, Step step)
			{
				foreach (var (t, c, d) in step.Conclusions)
				{
					var digit = solution[c];
					if (t == Assignment && digit != d || t == Elimination && digit == d)
					{
						return false;
					}
				}

				return true;
			}

			static bool recordingStep(
				List<Step> steps,
				Step step,
				scoped ref Grid playground,
				scoped ref ValueStopwatch stopwatch,
				List<Grid> stepGrids,
				AnalyzerResult resultBase,
				CancellationToken cancellationToken,
				[NotNullWhen(true)] out AnalyzerResult? result
			)
			{
				var atLeastOneConclusionIsWorth = false;
				foreach (var (t, c, d) in step.Conclusions)
				{
					switch (t)
					{
						case Assignment when playground.GetStatus(c) == CellStatus.Empty:
						case Elimination when playground.Exists(c, d) is true:
						{
							atLeastOneConclusionIsWorth = true;

							goto FinalCheck;
						}
					}
				}

			FinalCheck:
				if (atLeastOneConclusionIsWorth)
				{
					stepGrids.Add(playground);
					step.ApplyTo(ref playground);
					steps.Add(step);

					if (playground.IsSolved)
					{
						result = resultBase with
						{
							IsSolved = true,
							Solution = playground,
							ElapsedTime = stopwatch.GetElapsedTime(),
							Steps = steps.ToArray(),
							StepGrids = stepGrids.ToArray()
						};
						return true;
					}
				}
				else
				{
					// No steps are available.
					goto ReturnFalse;
				}

				cancellationToken.ThrowIfCancellationRequested();

			ReturnFalse:
				result = null;
				return false;
			}
		}
	}
}
