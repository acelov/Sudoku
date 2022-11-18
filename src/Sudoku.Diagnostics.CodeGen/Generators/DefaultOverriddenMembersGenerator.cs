﻿namespace Sudoku.Diagnostics.CodeGen.Generators;

using EqualsData = ValueTuple<int /*GeneratedMode*/, SyntaxTokenList /*MethodModifiers*/, INamedTypeSymbol /*Type*/, string /*ParameterName*/>;
using GetHashCodeData = ValueTuple<int /*GeneratedMode*/, SyntaxTokenList /*MethodModifiers*/, INamedTypeSymbol /*Type*/, IEnumerable<string> /*ExpressionValueNames*/>;

/// <summary>
/// Defines a source generator that generates the source code for default-overridden members
/// from type <see cref="object"/> or <see cref="ValueType"/>.
/// </summary>
/// <seealso cref="object"/>
/// <seealso cref="ValueType"/>
[Generator(LanguageNames.CSharp)]
public sealed class DefaultOverriddenMembersGenerator : IIncrementalGenerator
{
	/// <inheritdoc/>
	public void Initialize(IncrementalGeneratorInitializationContext context)
		=> context
			.WithRegisteredSourceOutput(TransformEqualsData, OutputEquals)
			.WithRegisteredSourceOutput(TransformGetHashCodeData, OutputGetHashCode);


	/// <summary>
	/// Transforms the data from current context into a tuple of values as generated data.
	/// </summary>
	private static GetHashCodeData? TransformGetHashCodeData(GeneratorAttributeSyntaxContext gasc, CancellationToken ct)
	{
		if (gasc is not
			{
				Attributes: [{ ConstructorArguments: [{ Value: int rawMode }, { Values: var extraArguments }] }],
				TargetNode: MethodDeclarationSyntax { Modifiers: var modifiers },
				TargetSymbol: IMethodSymbol
				{
					OverriddenMethod: var overridenMethod,
					ContainingType: { } type,
					Name: nameof(object.GetHashCode),
					IsOverride: true,
					IsStatic: false,
					ReturnType.SpecialType: SpecialType.System_Int32,
					IsGenericMethod: false,
					Parameters: []
				} method
			})
		{
			return null;
		}

		// Check whether the method is overridden from object.Equals(object?).
		var rootMethod = overridenMethod;
		var currentMethod = method;
		for (; rootMethod is not null; rootMethod = rootMethod.OverriddenMethod, currentMethod = currentMethod!.OverriddenMethod) ;
		if (currentMethod!.ContainingType.SpecialType is not (SpecialType.System_Object or SpecialType.System_ValueType))
		{
			return null;
		}

		if ((rawMode, type) switch { (0, { TypeKind: TypeKind.Struct, IsRefLikeType: true }) => false, (1 or 2, _) => false, _ => true })
		{
			return null;
		}

		return new(rawMode, modifiers, type, from extraArgument in extraArguments select (string)extraArgument.Value!);
	}

	/// <summary>
	/// Transforms the data from current context into a tuple of values as generated data.
	/// </summary>
	private static EqualsData? TransformEqualsData(GeneratorAttributeSyntaxContext gasc, CancellationToken ct)
	{
#pragma warning disable format
		if (gasc is not
			{
				Attributes: [{ ConstructorArguments: [{ Value: int rawMode }] }],
				TargetNode: MethodDeclarationSyntax { Modifiers: var modifiers },
				TargetSymbol: IMethodSymbol
				{
					OverriddenMethod: var overridenMethod,
					ContainingType: { } type,
					Name: nameof(object.Equals),
					IsOverride: true,
					IsStatic: false,
					ReturnType.SpecialType: SpecialType.System_Boolean,
					IsGenericMethod: false,
					Parameters:
					[
						{
							Name: var parameterName,
							Type: { SpecialType: SpecialType.System_Object, NullableAnnotation: NullableAnnotation.Annotated }
						}
					]
				} method
			})
#pragma warning restore format
		{
			return null;
		}

		// Check whether the method is overridden from object.Equals(object?).
		var rootMethod = overridenMethod;
		var currentMethod = method;
		for (; rootMethod is not null; rootMethod = rootMethod.OverriddenMethod, currentMethod = currentMethod!.OverriddenMethod) ;
		if (currentMethod!.ContainingType.SpecialType is not (SpecialType.System_Object or SpecialType.System_ValueType))
		{
			return null;
		}

#pragma warning disable format
		if ((rawMode, type) switch
			{
				(0, { TypeKind: TypeKind.Struct, IsRefLikeType: true }) => false,
				(1, _) => false,
				(2, { TypeKind: TypeKind.Class }) => false,
				_ => true
			})
#pragma warning restore format
		{
			return null;
		}

		return new(rawMode, modifiers, type, parameterName);
	}

	/// <summary>
	/// Generates the source code.
	/// </summary>
	private static void OutputEquals(SourceProductionContext spc, ImmutableArray<EqualsData?> data, Type sourceGeneratorType)
	{
		var codeSnippets = new List<string>();

		foreach (var tuple in data.CastToNotNull())
		{
			if (tuple is not (var mode, var modifiers, { Name: var typeName, ContainingNamespace: var @namespace } type, var paramName))
			{
				continue;
			}

			var extraAttributeStr = mode switch
			{
				0 => """
				[global::System.Obsolete(global::System.Runtime.Messages.RefStructDefaultImplementationMessage.OverriddenEqualsMethod, false, DiagnosticId = "SCA0104", UrlFormat = "https://sunnieshine.github.io/Sudoku/code-analysis/sca0104")]
						
				""",
				_ => string.Empty
			};
			var targetExpression = mode switch
			{
				0 => "false",
				1 => $"{paramName} is {type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} comparer && Equals(comparer)",
				2 => $"Equals({paramName} as {type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)})"
			};
			var attributeStr = mode switch
			{
				0 => string.Empty,
				1 or 2 => "[global::System.Diagnostics.CodeAnalysis.NotNullWhenAttribute(true)] "
			};

			var namespaceStr = @namespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)["global::".Length..];
			codeSnippets.Add(
				$$"""
				namespace {{namespaceStr}}
				{
					partial {{type.GetTypeKindModifier()}} {{typeName}}
					{
						/// <inheritdoc cref="object.Equals(object?)"/>
						[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
						[global::System.CodeDom.Compiler.GeneratedCodeAttribute("{{sourceGeneratorType.FullName}}", "{{VersionValue}}")]
						[global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
						{{extraAttributeStr}}{{modifiers}} bool Equals({{attributeStr}}object? {{paramName}})
							=> {{targetExpression}};
					}
				}
				"""
			);
		}

		spc.AddSource(
			$"DefaultOverrides.g.{Shortcuts.GeneratedOverriddenMemberEquals}.cs",
			$"""
			// <auto-generated/>
			
			#nullable enable
			
			{string.Join("\r\n\r\n", codeSnippets)}
			"""
		);
	}

	/// <summary>
	/// Generates the source code.
	/// </summary>
	private static void OutputGetHashCode(SourceProductionContext spc, ImmutableArray<GetHashCodeData?> data, Type sourceGeneratorType)
	{
		var codeSnippets = new List<string>();

		foreach (var tuple in data.CastToNotNull())
		{
			if (tuple is not (var mode, var modifiers, { Name: var typeName, ContainingNamespace: var @namespace } type, var rawMemberNames))
			{
				continue;
			}

			var needCast = mode switch
			{
				1 when rawMemberNames.First() is var name => (from m in type.GetMembers() where m.Name == name select m).FirstOrDefault() switch
				{
					IFieldSymbol field => field switch
					{
						{ Type.SpecialType: SpecialType.System_Int32, RefKind: RefKind.None } => false,
						_ => true
					},
					IPropertySymbol property => property switch
					{
						{ Type.SpecialType: SpecialType.System_Int32, RefKind: RefKind.None } => false,
						_ => true
					},
					IMethodSymbol { Parameters: [] } method => method switch
					{
						{ ReturnType.SpecialType: SpecialType.System_Int32, RefKind: RefKind.None } => false,
						_ => true
					},
					_ => null
				},
				_ => (bool?)null
			};

			var extraAttributeStr = mode switch
			{
				0 => """
				[global::System.Obsolete(global::System.Runtime.Messages.RefStructDefaultImplementationMessage.OverriddenGetHashCodeMethod, false, DiagnosticId = "SCA0105", UrlFormat = "https://sunnieshine.github.io/Sudoku/code-analysis/sca0105")]
						
				""",
				_ => string.Empty
			};
			var targetExpression = (mode, rawMemberNames.ToArray(), needCast) switch
			{
				(0, [], _)
					=> $"\t=> throw new global::System.NotSupportedException(global::System.Runtime.Messages.RefStructDefaultImplementationMessage.OverriddenGetHashCodeMethod);",
				(1, [var memberName], true or null)
					=> $"\t=> (int){memberName};",
				(1, [var memberName], _)
					=> $"\t=> {memberName};",
				(2, { Length: <= 8 } memberNames, _) when string.Join(", ", from e in memberNames select e) is var a
					=> $"\t=> global::System.HashCode.Combine({a});",
				(2, { Length: > 8 } memberNames, _) when string.Join("\r\n\t\t\t", from e in memberNames select $"result.Add({e});") is var a
					=> $$"""
					{
								var result = new global::System.HashCode();
								{{a}}
								return result.ToHashCode();
							}
					""",
			};

			var namespaceStr = @namespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)["global::".Length..];
			codeSnippets.Add(
				$$"""
				namespace {{namespaceStr}}
				{
					partial {{type.GetTypeKindModifier()}} {{typeName}}
					{
						/// <inheritdoc cref="object.GetHashCode"/>
						[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
						[global::System.CodeDom.Compiler.GeneratedCodeAttribute("{{sourceGeneratorType.FullName}}", "{{VersionValue}}")]
						[global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
						{{extraAttributeStr}}{{modifiers}} int GetHashCode()
						{{targetExpression}}
					}
				}
				"""
			);
		}

		spc.AddSource(
			$"DefaultOverrides.g.{Shortcuts.GeneratedOverriddenMemberGetHashCode}.cs",
			$"""
			// <auto-generated/>
			
			#nullable enable
			
			{string.Join("\r\n\r\n", codeSnippets)}
			"""
		);
	}
}

file static class Extensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref readonly IncrementalGeneratorInitializationContext WithRegisteredSourceOutput<T>(
		this in IncrementalGeneratorInitializationContext @this,
		Func<GeneratorAttributeSyntaxContext, CancellationToken, T?> transformAction,
		Action<SourceProductionContext, ImmutableArray<T?>, Type> outputAction,
		Predicate<SyntaxNode>? nodePredicate = null)
		where T : struct
	{
		const string attributeFullName = "System.Diagnostics.CodeGen.GeneratedOverriddingMemberAttribute";

		nodePredicate ??= static n => n is MethodDeclarationSyntax { Modifiers: var m } && m.Any(SyntaxKind.PartialKeyword);

		@this.RegisterSourceOutput(
			@this.SyntaxProvider
				.ForAttributeWithMetadataName(attributeFullName, (node, _) => nodePredicate(node), transformAction)
				.Where(static d => d is not null)
				.Collect(),
			(spc, data) => outputAction(spc, data, typeof(DefaultOverriddenMembersGenerator))
		);

		return ref @this;
	}
}
