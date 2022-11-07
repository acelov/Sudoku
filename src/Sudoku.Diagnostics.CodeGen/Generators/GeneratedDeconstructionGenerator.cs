﻿namespace Sudoku.Diagnostics.CodeGen.Generators;

using Data = ValueTuple<
	INamedTypeSymbol /*ContainingType*/,
	IMethodSymbol /*Method*/,
	ImmutableArray<IParameterSymbol> /*Parameters*/,
	SyntaxTokenList /*Modifiers*/,
	INamedTypeSymbol /*AttributeType*/
>;

/// <summary>
/// Defines a source generator that generates the source code for deconstruction methods.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class GeneratedDeconstructionGenerator : IIncrementalGenerator
{
	/// <inheritdoc/>
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterSourceOutput(
			context.SyntaxProvider
				.ForAttributeWithMetadataName("System.Diagnostics.CodeGen.GeneratedDeconstructionAttribute", nodePredicate, transform)
				.Where(static d => d is not null)
				.Collect(),
			OutputAction
		);


		static bool nodePredicate(SyntaxNode node, CancellationToken _) => node is MethodDeclarationSyntax;

		static Data? transform(GeneratorAttributeSyntaxContext gasc, CancellationToken _)
		{
			switch (gasc)
			{
				case
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
					} symbol,
					SemanticModel: { Compilation: var compilation } semanticModel
				}
				when parameters.All(static p => p.RefKind == RefKind.Out):
				{
					var attributeType = compilation.GetTypeByMetadataName("System.Diagnostics.CodeGen.GeneratedDeconstructionArgumentAttribute");
					if (attributeType is null)
					{
						goto default;
					}

					return new(type, symbol, parameters, modifiers, attributeType);
				}
				default:
				{
					return null;
				}
			}
		}
	}

	/// <summary>
	/// Output action.
	/// </summary>
	private void OutputAction(SourceProductionContext spc, ImmutableArray<Data?> data)
	{
		_ = spc is { CancellationToken: var ct };

		foreach (var tuple in data.CastToNotNull())
		{
#pragma warning disable format
			if (tuple is not (
					{ ContainingNamespace: var @namespace, Name: var typeName } containingType,
					{ DeclaredAccessibility: var methodAccessibility } method,
					{ Length: var parameterLength } parameters,
					var modifiers,
					var attributeType
				))
#pragma warning restore format
			{
				continue;
			}

			var membersData = (
				from m in containingType.GetAllMembers()
				where m switch
				{
					IFieldSymbol { RefKind: RefKind.None } => true,
					IPropertySymbol { ReturnsByRef: false, ReturnsByRefReadonly: false } => true,
					IMethodSymbol { ReturnsVoid: false, Parameters: [] } => true,
					_ => false
				}
				let name = standardizeIdentifierName(m.Name)
				select (CheckId: true, Member: m, Name: name)
			).ToArray();

			var selection = (
				from parameter in parameters
				let index = Array.FindIndex(membersData, member => memberDataSelector(member, parameter, attributeType))
				where index != -1
				let correspondingData = membersData[index]
				where correspondingData.CheckId // If none found, this field will be set 'false' by default because of 'default(T)'.
				let parameterName = parameter.Name
				let isDirect = standardizeIdentifierName(parameterName) == correspondingData.Name
				select (IsDirect: isDirect, correspondingData.Member, correspondingData.Member.Name, ParameterName: parameterName)
			).ToArray();

			if (selection.Length != parameterLength)
			{
				// The method is invalid to generate source code, because some parameters are invalid to be matched.
				continue;
			}

			var assignmentsCode = string.Join("\r\n\t\t", from t in selection select getAssignmentStatementCode(t, ct));

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


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static string standardizeIdentifierName(string name)
			=> name switch
			{
				['_', .. var slice] => standardizeIdentifierName(slice),
				[>= 'A' and <= 'Z', ..] => name,
				[var ch and >= 'a' and <= 'z', .. var slice] => $"{char.ToUpper(ch)}{slice}",
				_ => name
			};

		static bool memberDataSelector(
			(bool, ISymbol, string) memberData,
			IParameterSymbol parameter,
			INamedTypeSymbol attributeType)
		{
			if (memberData is not (_, { Name: var rawName }, var name))
			{
				return false;
			}

			if (name == standardizeIdentifierName(parameter.Name))
			{
				return true;
			}

			// If cannot corresponds to the target, route to indirect member using attributes.
			if (parameter.GetAttributes() is not (var attributes and not []))
			{
				return false;
			}

			if (attributes.FirstOrDefault(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, attributeType)) is not
				{
					ConstructorArguments: [{ Value: string targetPropertyExpression }]
				})
			{
				return false;
			}

			return targetPropertyExpression == rawName;
		}

		static string getAssignmentStatementCode((bool IsDirect, ISymbol Member, string Name, string ParameterName) t, CancellationToken ct)
			=> t switch
			{
				(_, IFieldSymbol, var name, var parameterName) => $"{t.ParameterName} = {t.Name};",
				(var isDirect, IPropertySymbol { DeclaringSyntaxReferences: [var syntaxRef] }, var name, var parameterName)
					when syntaxRef.GetSyntax(ct) is PropertyDeclarationSyntax node
					=> node switch
					{
						{ AccessorList.Accessors: [{ Keyword.RawKind: (int)SyntaxKind.GetKeyword, ExpressionBody.Expression: var expr }] }
							when !isDirect => $"{parameterName} = {expr};",
						{ ExpressionBody.Expression: var expr } when !isDirect => $"{parameterName} = {expr};",
						_ => $"{parameterName} = {name};"
					},
				(var isDirect, IMethodSymbol { DeclaringSyntaxReferences: [var syntaxRef] }, var name, var parameterName)
					when syntaxRef.GetSyntax(ct) is MethodDeclarationSyntax node
					=> node switch
					{
						{ ExpressionBody.Expression: var expr } when !isDirect => $"{parameterName} = {expr};",
						_ => $"{parameterName} = {name}();"
					}
			};
	}
}
