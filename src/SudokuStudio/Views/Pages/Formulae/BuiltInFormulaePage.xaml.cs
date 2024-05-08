namespace SudokuStudio.Views.Pages.Formulae;

/// <summary>
/// Represents built-in formulae page.
/// </summary>
public sealed partial class BuiltInFormulaePage : Page
{
	/// <summary>
	/// Indicates the property flags.
	/// </summary>
	private const BindingFlags PropertyFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;


	/// <summary>
	/// Indicates the tab ('<c>\t</c>').
	/// </summary>
	private static readonly string Tab = "\t";

	/// <summary>
	/// Indicates the quad spaces.
	/// </summary>
	private static readonly string QuadSpaces = new(' ', 4);


	/// <summary>
	/// Initializes a <see cref="BuiltInFormulaePage"/> instance.
	/// </summary>
	public BuiltInFormulaePage() => InitializeComponent();


	private async void Page_LoadedAsync(object sender, RoutedEventArgs e)
	{
		var factorType = typeof(Factor);
		foreach (var type in factorType.Assembly.GetTypes())
		{
			if (type.BaseType != factorType)
			{
				continue;
			}

			var parameterFormat = ResourceDictionary.Get("ParameterName", App.CurrentCulture);
			var factor = (Factor)Activator.CreateInstance(type)!;
			var stepType = factor.ReflectedStepType;
			FormulaeDisplayer.Children.Add(
				new SettingsCard
				{
					HeaderIcon = new FontIcon { Glyph = "\uE734" },
					Header = factor.GetName(App.CurrentCulture),
					Description = new TextBlock
					{
						Text = string.Join(Environment.NewLine, factor.ParameterNames.Select(parameterTypeIdentifier))
					},
					Content = new TextBlock
					{
						Text = createExpressionString(factor.FormulaExpressionString),
						FontFamily = new("Cascadia Code")
					}
				}
			);

			await Task.Delay(100);


			static string matchItself(Match match) => match.Groups[1].Value;

			string createExpressionString(string expr)
			{
				var interim = ArgsPattern().Replace(expr, match => string.Format(parameterFormat, int.Parse(matchItself(match)) + 1));
				interim = interim.Replace(Tab, QuadSpaces);
				interim = BracePattern().Replace(interim, matchItself);
				interim = TechniqueDotNamePattern().Replace(interim, matchItself);
				interim = StaticMethodNamePattern().Replace(interim, static match => $"${matchItself(match)}");
				return interim;
			}

			string parameterTypeIdentifier(string parameterName, int index)
			{
				var parameterIndex = string.Format(parameterFormat, index + 1);
				var typeName = TypeReflecting.GetFriendlyTypeName(
					(
						from propertyInfo in stepType.GetProperties(PropertyFlags)
						where propertyInfo.Name.EndsWith(parameterName)
						select propertyInfo
					).First().PropertyType
				);
				return $"{parameterIndex}: {typeName}";
			}
		}
	}


	[GeneratedRegex("""\([\w\[\]\?]+\)args!?\[(\d+)\]!?""", RegexOptions.Compiled)]
	private static partial Regex ArgsPattern();

	[GeneratedRegex("""\(([^\)]+)\)(?=\.)""", RegexOptions.Compiled)]
	private static partial Regex BracePattern();

	[GeneratedRegex("""\bTechnique\.(\w+)""", RegexOptions.Compiled)]
	private static partial Regex TechniqueDotNamePattern();

	[GeneratedRegex("""\w+\.(\w+)(?=\([^\)]+\))""", RegexOptions.Compiled)]
	private static partial Regex StaticMethodNamePattern();
}

/// <summary>
/// Represents a list of types to be reflected with its short name.
/// </summary>
file static class TypeReflecting
{
	/// <summary>
	/// Gets friendly type name.
	/// </summary>
	/// <param name="type">Indicates the type.</param>
	/// <returns>The name.</returns>
	public static string GetFriendlyTypeName(Type type)
		=> type == typeof(Technique[][])
			? $"{TypeSystem.GetFriendlyTypeName(type, App.CurrentCulture)}[][]"
			: TypeSystem.GetFriendlyTypeName(type, App.CurrentCulture);
}
