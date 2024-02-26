namespace Sudoku.Strategying.Constraints;

/// <summary>
/// Represents a count between constraint.
/// </summary>
[GetHashCode]
[ToString]
public sealed partial class CountBetweenConstraint : Constraint
{
	/// <summary>
	/// Indicates the range of the numbers set.
	/// </summary>
	[HashCodeMember]
	[StringMember]
	public required Range Range { get; set; }

	/// <summary>
	/// Indicates the cell state to be checked.
	/// </summary>
	[HashCodeMember]
	[StringMember]
	public required CellState CellState { get; set; }

	/// <summary>
	/// Indicates the between rule.
	/// </summary>
	[HashCodeMember]
	[StringMember]
	public required BetweenRule BetweenRule { get; set; }


	/// <inheritdoc/>
	public override bool Equals([NotNullWhen(true)] Constraint? other)
		=> other is CountBetweenConstraint comparer
		&& Range.Equals(comparer.Range) && (CellState, BetweenRule) == (comparer.CellState, comparer.BetweenRule);

	/// <inheritdoc/>
	protected internal override bool CheckCore(scoped ConstraintCheckingContext context)
	{
		scoped ref readonly var grid = ref context.Grid;
		var factCount = CellState switch
		{
			CellState.Empty => grid.EmptiesCount,
			CellState.Modifiable => grid.ModifiablesCount,
			_ => grid.GivensCount
		};

		_ = Range is { Start.Value: var min, End.Value: var max };
		return BetweenRule switch
		{
			BetweenRule.BothOpen => factCount > min && factCount < max,
			BetweenRule.LeftOpen => factCount >= min && factCount <= max,
			BetweenRule.RightOpen => factCount >= min && factCount < max,
			BetweenRule.BothClosed => factCount >= min && factCount <= max
		};
	}

	/// <inheritdoc/>
	protected internal override ValidationResult ValidateCore() => throw new NotImplementedException();
}
