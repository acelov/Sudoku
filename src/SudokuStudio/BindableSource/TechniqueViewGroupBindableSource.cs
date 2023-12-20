using System.Diagnostics.CodeAnalysis;
using System.SourceGeneration;
using Sudoku.Analytics.Categorization;
using static SudokuStudio.Strings.StringsAccessor;

namespace SudokuStudio.BindableSource;

/// <summary>
/// Represents a bindable source for a group of <see cref="Technique"/> instances.
/// </summary>
/// <param name="group">Indicates the group of the techniques.</param>
/// <param name="items">Indicates the items belonging to the group.</param>
/// <seealso cref="Technique"/>
[method: SetsRequiredMembers]
public sealed partial class TechniqueViewGroupBindableSource(
	[Data(SetterExpression = "init", Accessibility = "public required")] TechniqueGroup group,
	[Data(SetterExpression = "init", Accessibility = "public required")] TechniqueViewBindableSource[] items
)
{
	/// <summary>
	/// Indicates the full name of the group.
	/// </summary>
	public string GroupFullName => (GroupName, ShortenedName) switch { var (a, b) => a == b ? a : $"{a} ({b})" };

	/// <summary>
	/// Indicates the group name.
	/// </summary>
	public string GroupName => Group.GetName(CurrentCultureInfo);

	/// <summary>
	/// Indicates the shortened name of the group.
	/// </summary>
	public string ShortenedName => Group.GetShortenedName(CurrentCultureInfo);
}
