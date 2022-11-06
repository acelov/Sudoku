﻿namespace Sudoku.Diagnostics.CodeGen.Generators;

using Data = ValueTuple<
	INamedTypeSymbol /*ContainingType*/,
	IMethodSymbol /*Method*/,
	ImmutableArray<IParameterSymbol> /*Parameters*/,
	SyntaxTokenList /*Modifiers*/
>;

/// <summary>
/// Defines a source generator that generates the source code for deconstruction methods.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class GeneratedDeconstructionGenerator : IIncrementalGenerator
{
	/// <inheritdoc/>
	public void Initialize(IncrementalGeneratorInitializationContext context)
		=> context.RegisterSourceOutput(
			context.SyntaxProvider
				.ForAttributeWithMetadataName("System.Diagnostics.CodeGen.GeneratedDeconstructionAttribute", NodePredicate, Transform)
				.Where(static d => d is not null)
				.Collect(),
			OutputAction
		);

	/// <summary>
	/// Output action.
	/// </summary>
	private void OutputAction(SourceProductionContext spc, ImmutableArray<Data?> data)
	{
		foreach (var tuple in data.CastToNotNull())
		{
#pragma warning disable format
			if (tuple is not (
					{ ContainingNamespace: var @namespace, Name: var typeName } containingType,
					{ DeclaredAccessibility: var methodAccessibility } method,
					{ Length: var parameterLength } parameters,
					var modifiers
				))
#pragma warning restore format
			{
				continue;
			}

			var membersData =
				from m in containingType.GetAllMembers()
				where m switch
				{
					IFieldSymbol { RefKind: RefKind.None } => true,
					IPropertySymbol { ReturnsByRef: false, ReturnsByRefReadonly: false } => true,
					IMethodSymbol { ReturnsVoid: false, Parameters: [] } => true,
					_ => false
				}
				let name = StandardizeIdentifierName(m.Name)
				select (CheckId: true, Member: m, Name: name);

			var selection = (
				from parameter in parameters
				let correspondingData = membersData.FirstOrDefault(member => member.Name == StandardizeIdentifierName(parameter.Name))
				where correspondingData.CheckId // If none found, this field will be set 'false' by default because of 'default(T)'.
				select (correspondingData.Member, correspondingData.Member.Name, ParameterName: parameter.Name)
			).ToArray();

			if (selection.Length != parameterLength)
			{
				// The method is invalid to generate source code, because some parameters are invalid to be matched.
				continue;
			}

			var assignmentsCode = string.Join(
				"\r\n\t\t",
				from t in selection
				select t.Member switch
				{
					IFieldSymbol => $"{t.ParameterName} = {t.Name};",
					IPropertySymbol => $"{t.ParameterName} = {t.Name};",
					IMethodSymbol => $"{t.ParameterName} = {t.Name}();"
				}
			);

			var argsStr = string.Join(
				", ",
				from parameter in parameters
				let parameterType = parameter.Type
				let name = parameterType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
				let annotation = parameterType.NullableAnnotation == NullableAnnotation.Annotated ? "?" : string.Empty
				select $"out {name}{annotation} {parameter.Name}"
			);

			var namespaceStr = @namespace?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) switch
			{
				{ } s => $"namespace {s["global::".Length..]};\r\n\r\n",
				_ => string.Empty
			};

			spc.AddSource(
				$"{containingType.ToFileName()}_p{parameters.Length}.g.{Shortcuts.GeneratedDeconstruction}.cs",
				$$"""
				// <auto-generated/>

				#nullable enable

				{{namespaceStr}}partial {{containingType.GetTypeKindModifier()}} {{typeName}}
				{
					/// <include file="../../global-doc-comments.xml" path="g/csharp7/feature[@name='deconstruction-method']/target[@name='method']"/>
					[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
					[global::System.CodeDom.Compiler.GeneratedCodeAttribute("{{GetType().FullName}}", "{{VersionValue}}")]
					[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
					{{modifiers}} void Deconstruct({{argsStr}})
					{
						{{assignmentsCode}}
					}
				}
				"""
			);
		}
	}


	/// <summary>
	/// Node predicate.
	/// </summary>
	private static bool NodePredicate(SyntaxNode node, CancellationToken _) => node is MethodDeclarationSyntax;

	/// <summary>
	/// To standardize the identifier name, converting it into <c>PascalCase</c>.
	/// </summary>
	/// <param name="name">The identifier name.</param>
	/// <returns>The converted name. The return value must be <c>PascalCase</c>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static string StandardizeIdentifierName(string name)
		=> name switch
		{
			['_', .. var slice] => StandardizeIdentifierName(slice),
			[>= 'A' and <= 'Z', ..] => name,
			[var ch and >= 'a' and <= 'z', .. var slice] => $"{char.ToUpper(ch)}{slice}",
			_ => name
		};

	/// <summary>
	/// The transforming method.
	/// </summary>
	private static Data? Transform(GeneratorAttributeSyntaxContext gasc, CancellationToken _)
		=> gasc switch
		{
			{
				Attributes.Length: 1,
				TargetNode: MethodDeclarationSyntax { Modifiers: var modifiers } node,
				TargetSymbol: IMethodSymbol
				{
					Name: "Deconstruct",
					TypeParameters: [],
					Parameters: var parameters and not [],
					IsStatic: false,
					ReturnsVoid: true,
					ContainingType: { ContainingType: null, IsFileLocal: false } type
				} symbol
			} when parameters.All(static p => p.RefKind == RefKind.Out) => new(type, symbol, parameters, modifiers),
			_ => null
		};
}
