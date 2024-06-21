namespace Sudoku.Analytics.Steps;

/// <summary>
/// Provides with a step that is a <b>Gurth's Symmetrical Placement</b> technique.
/// </summary>
/// <param name="conclusions"><inheritdoc/></param>
/// <param name="views"><inheritdoc/></param>
/// <param name="options"><inheritdoc/></param>
/// <param name="symmetricType">
/// Indicates the symmetric type used. The supported value can only be:
/// <list type="bullet">
/// <item><see cref="SymmetricType.Central"/></item>
/// <item><see cref="SymmetricType.Diagonal"/></item>
/// <item><see cref="SymmetricType.AntiDiagonal"/></item>
/// </list>
/// </param>
/// <param name="mapping">
/// Indicates the mapping relations; in other words, this table shows what digits has symmetrical placement relation to what digits.
/// </param>
public partial class GurthSymmetricalPlacementStep(
	Conclusion[] conclusions,
	View[]? views,
	StepSearcherOptions options,
	[PrimaryConstructorParameter] SymmetricType symmetricType,
	[PrimaryConstructorParameter] Digit?[]? mapping
) : SymmetryStep(conclusions, views, options)
{
	/// <inheritdoc/>
	public override int BaseDifficulty
		=> SymmetricType switch
		{
			SymmetricType.Central => 70,
			SymmetricType.Diagonal or SymmetricType.AntiDiagonal => 71,
			SymmetricType.XAxis or SymmetricType.YAxis => 72 // This pattern will only be used by anti-GSP cases.
		};

	/// <inheritdoc/>
	public override Technique Code => Technique.GurthSymmetricalPlacement;

	/// <inheritdoc/>
	public override FormatInterpolation[] FormatInterpolationParts
		=> [new(EnglishLanguage, [SymmetryTypeStr, MappingStr]), new(ChineseLanguage, [SymmetryTypeStr, MappingStr])];

	private string SymmetryTypeStr => ResourceDictionary.Get($"{SymmetricType}Symmetry", GetCulture(null));

	private string MappingStr
	{
		get
		{
			var culture = GetCulture(null);
			var comma = ResourceDictionary.Get("Comma", culture);
			if (Mapping is not null)
			{
				var sb = new StringBuilder(10);
				for (var i = 0; i < 9; i++)
				{
					var currentMappingRelationDigit = Mapping[i];
					sb.Append(i + 1);
					sb.Append(currentMappingRelationDigit is { } c && c != i ? $" -> {c + 1}" : string.Empty);
					sb.Append(comma);
				}
				return sb.RemoveFrom(^comma.Length).ToString();
			}
			return ResourceDictionary.Get("NoMappingRelation", culture);
		}
	}
}
