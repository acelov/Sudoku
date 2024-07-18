namespace Sudoku.Analytics.Chaining;

/// <summary>
/// Represents the properties used while chaining.
/// </summary>
/// <param name="grid">The grid to be checked.</param>
/// <param name="strongLinks">The dictionary that stores a list of strong links.</param>
/// <param name="weakLinks">The dictionary that stores a list of weak links.</param>
/// <param name="options">Indicates the step searcher options to be used.</param>
[TypeImpl(TypeImplFlag.AllObjectMethods)]
[SuppressMessage("Style", "IDE0250:Make struct 'readonly'", Justification = "<Pending>")]
public ref partial struct ChainingRuleLinkContext(
	[PrimaryConstructorParameter(MemberKinds.Field, Accessibility = "public", NamingRule = ">@")] ref readonly Grid grid,
	[PrimaryConstructorParameter(MemberKinds.Field, Accessibility = "public", NamingRule = ">@")] LinkDictionary strongLinks,
	[PrimaryConstructorParameter(MemberKinds.Field, Accessibility = "public", NamingRule = ">@")] LinkDictionary weakLinks,
	[PrimaryConstructorParameter(MemberKinds.Field, Accessibility = "public", NamingRule = ">@")] StepSearcherOptions options
) : IContext
{
	/// <inheritdoc/>
	readonly ref readonly Grid IContext.Grid => ref Grid;


	/// <summary>
	/// Try to get the link option for the specified link type.
	/// </summary>
	/// <param name="linkType">The link type.</param>
	/// <returns>
	/// The link option returned.
	/// If there's no overridden link option, return <see cref="StepSearcherOptions.DefaultLinkOption"/>.
	/// </returns>
	/// <seealso cref="StepSearcherOptions.DefaultLinkOption"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly LinkOption GetLinkOption(LinkType linkType)
		=> Options.OverriddenLinkOptions.TryGetValue(linkType, out var lo) ? lo : Options.DefaultLinkOption;
}