namespace System.Resources;

/// <summary>
/// Indicates an exception that will be thrown if target resource is not found.
/// </summary>
/// <param name="assembly"><inheritdoc/></param>
/// <param name="resourceKey">The resource key.</param>
/// <param name="culture">The culture information.</param>
public sealed partial class ResourceNotFoundException(
	Assembly? assembly,
	[PrimaryConstructorParameter(MemberKinds.Field, Accessibility = "private readonly")] string resourceKey,
	[PrimaryConstructorParameter(MemberKinds.Field, Accessibility = "private readonly")] CultureInfo? culture
) : ResourceException(assembly)
{
	/// <summary>
	/// The "unspecified" text.
	/// </summary>
	private const string CultureNotSpecifiedDefaultText = "<Unspecified>";


	/// <inheritdoc/>
	public override string Message
		=> $"""
		Specified resource not found.
		* Resource key: '{_resourceKey}',
		* Assembly: '{_assembly}',
		* Culture: '{_culture?.EnglishName ?? CultureNotSpecifiedDefaultText}'
		""";

	/// <inheritdoc/>
	public override IDictionary Data
		=> new Dictionary<string, object?>
		{
			{ nameof(assembly), _assembly },
			{ nameof(resourceKey), _resourceKey },
			{ nameof(culture), _culture }
		};
}