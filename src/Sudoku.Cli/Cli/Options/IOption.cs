namespace Sudoku.Cli.Options;

/// <summary>
/// Represents an option to be used by a <see cref="Command"/>.
/// </summary>
/// <typeparam name="TSelf">The type of option instance itself.</typeparam>
/// <typeparam name="T">The type of the default value of the current option.</typeparam>
/// <seealso cref="Command"/>
public interface IOption<TSelf, out T> where TSelf : class, IOption<TSelf, T>, new()
{
	/// <summary>
	/// <inheritdoc cref="Option{T}.Option(string, ParseArgument{T}, bool, string?)" path="/param[@name='isDefault']"/>
	/// </summary>
	/// <remarks>This property is <see langword="false"/> by default.</remarks>
	static virtual bool IsDefault { get; } = false;

	/// <summary>
	/// <para><inheritdoc cref="Option.IsRequired" path="/summary"/></para>
	/// <para><inheritdoc cref="Option.IsRequired" path="/remarks"/></para>
	/// </summary>
	/// <remarks>This property is <see langword="false"/> by default.</remarks>
	static virtual bool IsRequired { get; } = false;

	/// <summary>
	/// Indicates the description to be used.
	/// </summary>
	static abstract string Description { get; }

	/// <summary>
	/// Indicates the aliases to be used.
	/// </summary>
	static abstract string[] Aliases { get; }

	/// <summary>
	/// Indicates the default value to be used.
	/// </summary>
	static abstract T DefaultValue { get; }


	/// <summary>
	/// Create an <see cref="Option{T}"/> instance.
	/// </summary>
	/// <param name="parseArgument">
	/// <inheritdoc cref="Option{T}(string, ParseArgument{T}, bool, string?)" path="/param[@name='parseArgument']"/>
	/// </param>
	/// <returns>An <see cref="Option{T}"/> instance.</returns>
	static Option<T> CreateOption(ParseArgument<T>? parseArgument = null)
	{
		var result = parseArgument is null
			? new Option<T>(TSelf.Aliases, TSelf.Description) { IsRequired = TSelf.IsRequired }
			: new Option<T>(TSelf.Aliases, parseArgument, TSelf.IsDefault, TSelf.Description) { IsRequired = TSelf.IsRequired };
		result.SetDefaultValue(TSelf.DefaultValue);

		return result;
	}
}
