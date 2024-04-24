namespace Sudoku.Measuring;

/// <summary>
/// Represents a factor that describes a rule for calculating difficulty rating for a step in one factor.
/// </summary>
public abstract class Factor : ICultureFormattable
{
	/// <summary>
	/// Indicates the name of the factor that can be used by telling with multple <see cref="Factor"/>
	/// instances with different types.
	/// </summary>
	public string DistinctKey => GetType().Name;

	/// <summary>
	/// Represents the string representation of formula.
	/// </summary>
	/// <remarks>
	/// The text can contain placeholders such like <c>{0}</c>, <c>{1}</c>, <c>{2}</c> and so on. The values will be replaced
	/// with the fact parameter name from property <see cref="Parameters"/> in runtime.
	/// </remarks>
	/// <seealso cref="Parameters"/>
	[StringSyntax(StringSyntaxAttribute.CompositeFormat)]
	public abstract string FormulaString { get; }

	/// <summary>
	/// Indicates a list of <see cref="string"/> instances that binds with real instance properties
	/// stored inside a <see cref="Step"/> instance, representing the target step type is compatible
	/// with the current factor and can be calculated its rating.
	/// </summary>
	public abstract string[] ParameterNames { get; }

	/// <summary>
	/// Indicates the relied <see cref="Type"/> instance.
	/// </summary>
	public abstract Type ReflectedStepType { get; }

	/// <summary>
	/// Indicates a <see cref="PropertyInfo"/> instance that creates from property <see cref="ParameterNames"/>.
	/// </summary>
	/// <seealso cref="ParameterNames"/>
	/// <seealso cref="PropertyInfo"/>
	public PropertyInfo[] Parameters => from parameterName in ParameterNames select ReflectedStepType.GetProperty(parameterName)!;

	/// <summary>
	/// Provides with a formula that calculates for the result, unscaled.
	/// </summary>
	public abstract Func<Step, int?> Formula { get; }


	/// <summary>
	/// Try to fetch the name stored in resource dictionary.
	/// </summary>
	/// <param name="culture">The culture.</param>
	/// <returns>The name of the factor.</returns>
	public string GetName(CultureInfo? culture = null) => ResourceDictionary.Get($"Factor_{DistinctKey}", culture);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public sealed override string ToString() => ToString(null);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ToString(CultureInfo? culture = null)
	{
		var colonCharacter = ResourceDictionary.Get("_Token_Colon", culture);
		return $"{GetName(culture)}{colonCharacter}{Environment.NewLine}{FormulaString}";
	}
}