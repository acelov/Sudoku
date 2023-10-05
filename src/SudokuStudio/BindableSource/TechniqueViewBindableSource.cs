using System.SourceGeneration;
using Sudoku.Analytics.Categorization;

namespace SudokuStudio.BindableSource;

/// <summary>
/// Represents a bindable source for a <see cref="Technique"/>.
/// </summary>
/// <param name="techniqueField">Indicates the technique field.</param>
/// <seealso cref="Technique"/>
public sealed partial class TechniqueViewBindableSource([DataMember(SetterExpression = "init")] Technique techniqueField)
{
	/// <summary>
	/// Indicates the name of the technique.
	/// </summary>
	public string TechniqueName => TechniqueField.GetName();

	/// <summary>
	/// Indicates the containing group for the current technique.
	/// </summary>
	public TechniqueGroup ContainingGroup => TechniqueField.GetGroup();

	/// <summary>
	/// Indicates the difficulty level of the technique.
	/// </summary>
	public DifficultyLevel DifficultyLevel => TechniqueField.GetDifficultyLevel();
}