namespace Sudoku.SourceGeneration.Handlers;

/// <summary>
/// The generator handler for primary constructor member parameters.
/// </summary>
internal static class PrimaryConstructorMemberHandler
{
	private const string FieldAttributeTypeName = "System.Diagnostics.CodeAnalysis.FieldAttribute";

	private const string PropertyAttributeTypeName = "System.Diagnostics.CodeAnalysis.PropertyAttribute";

	private const string IsReadOnlyByDefaultPropertyName = "IsReadOnlyByDefault";

	private const string RefKindPropertyName = "RefKind";

	private const string NamingRulePropertyName = "NamingRule";

	private const string AccessibilityPropertyName = "Accessibility";

	private const string EmitPropertyStylePropertyName = "EmitPropertyStyle";

	private const string SetterPropertyName = "Setter";


	/// <inheritdoc/>
	public static void Output(SourceProductionContext spc, ImmutableArray<string> values)
		=> spc.AddSource(
			"PrimaryConstructorParameters.g.cs",
			$$"""
			{{Banner.AutoGenerated}}

			#nullable enable

			{{string.Join("\r\n\r\n", values)}}
			"""
		);

	/// <inheritdoc/>
	public static string? Transform(GeneratorSyntaxContext context, CancellationToken cancellationToken)
	{
		// Deconstruct members.
		if (context is not { Node: TypeDeclarationSyntax node, SemanticModel: { Compilation: var compilation } semanticModel })
		{
			return null;
		}

		if (semanticModel.GetDeclaredSymbol(node, cancellationToken) is not INamedTypeSymbol
			{
				ContainingType: null,
				DeclaredAccessibility: var accessibility,
				TypeKind: var typeKind and (TypeKind.Class or TypeKind.Struct),
				IsRecord: var isRecord,
				IsReadOnly: var isReadOnlyStruct,
				IsRefLikeType: var isRefStruct,
				TypeParameters: var typeParameters,
				InstanceConstructors: var instanceCtors,
				Name: var typeName,
				ContainingNamespace: { } namespaceSymbol
			})
		{
			return null;
		}

		if (instanceCtors.FirstOrDefault(primaryConstructorPredicate) is null)
		{
			return null;
		}

		// Get parameters.
		var firstParameterNode = node.DescendantNodes().OfType<ParameterSyntax>().FirstOrDefault();
		var firstParameterSymbol = semanticModel.GetDeclaredSymbol(firstParameterNode, cancellationToken);
		if (firstParameterSymbol is not { ContainingSymbol: IMethodSymbol { Parameters: var parameters, MethodKind: MethodKind.Constructor } })
		{
			return null;
		}

		// Check existence of necessary APIs.
		var fieldAttribute = compilation.GetTypeByMetadataName(FieldAttributeTypeName);
		var propertyAttribute = compilation.GetTypeByMetadataName(PropertyAttributeTypeName);
		if ((fieldAttribute, propertyAttribute) is not (not null, not null))
		{
			return null;
		}

		// Retrieve all parameters that has already marked [Field] or [Property].
		var parametersMarked = new List<ParameterLocalData>();
		foreach (var parameter in parameters)
		{
			var p = parameter.GetAttributes().ToArray();
			var fieldAttributeData = p.FirstOrDefault(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, fieldAttribute));
			var propertyAttributeData = p.FirstOrDefault(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, propertyAttribute));
			if ((fieldAttributeData, propertyAttributeData) is not (null, null))
			{
				parametersMarked.Add(new(parameter, fieldAttributeData, propertyAttributeData));
			}
		}

		// Try to create source code here.
		var fields = new List<string>();
		foreach (var parameterData in parametersMarked)
		{
#pragma warning disable format
			if (parameterData is not (
				{
					Type: var parameterType,
					NullableAnnotation: var nullableAnnotation,
					RefKind: var parameterRefKind,
					Name: var parameterName
				},
				{ NamedArguments: var n },
				_
			))
#pragma warning restore format
			{
				continue;
			}

			var isReadOnly = n.TryGetValueOrDefault<bool>(IsReadOnlyByDefaultPropertyName, out var isReadOnlyLocal)
				? isReadOnlyLocal
				: typeKind == TypeKind.Struct && !isReadOnlyStruct || typeKind != TypeKind.Struct;
			var readOnlyModifier = typeKind == TypeKind.Struct ? "readonly " : string.Empty;
			var refKind = n.TryGetValueOrDefault<string?>(RefKindPropertyName, out var refKindLocal)
				? refKindLocal is not null
					? (refKindLocal.EndsWith(" ") ? refKindLocal : $"{refKindLocal} ").ToLower()
					: string.Empty
				: (isRefStruct, parameterRefKind) switch
				{
					(true, RefKind.Ref) => "ref ",
					(true, RefKind.RefReadOnlyParameter) => "ref readonly ",
					(true, RefKind.In) => "ref readonly ",
					_ => string.Empty
				};
			var parameterTypeString = parameterType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
			var nullableToken = nullableAnnotation == Annotated ? "?" : string.Empty;
			var namingRule = n.TryGetValueOrDefault<string>(NamingRulePropertyName, out var namingRuleLocal)
				? namingRuleLocal!
				: "_<@";
			var fieldName = namingRule
				.Replace(">@", parameterName.ToPascalCase())
				.Replace("<@", parameterName.ToCamelCase())
				.Replace("@", parameterName);
			var refAssignmentKindString = string.IsNullOrWhiteSpace(refKind) ? string.Empty : "ref ";
			var fieldAccessibility = n.TryGetValueOrDefault<string>(AccessibilityPropertyName, out var accessibilityLocal)
				? (accessibilityLocal!.EndsWith(" ") ? accessibilityLocal : $"{accessibilityLocal} ").ToLower()
				: "private ";
			fields.Add(
				$$"""
				/// <summary>
						/// The generated field declaration for parameter <c>{{parameterName}}</c>.
						/// </summary>
						[global::System.CodeDom.Compiler.GeneratedCodeAttribute("{{typeof(PrimaryConstructorMemberHandler).FullName}}", "{{Value}}")]
						[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
						{{fieldAccessibility}}{{readOnlyModifier}}{{refKind}}{{parameterTypeString}}{{nullableToken}} {{fieldName}} = {{refAssignmentKindString}}{{parameterName}};
				"""
			);
		}

		var properties = new List<string>();
		foreach (var parameterData in parametersMarked)
		{
#pragma warning disable format
			if (parameterData is not (
				{
					Type: var parameterType,
					NullableAnnotation: var nullableAnnotation,
					RefKind: var parameterRefKind,
					Name: var parameterName
				},
				_,
				{ NamedArguments: var n }
			))
#pragma warning restore format
			{
				continue;
			}

			var propertyAccessibility = n.TryGetValueOrDefault<string>(AccessibilityPropertyName, out var accessibilityLocal)
				? (accessibilityLocal!.EndsWith(" ") ? accessibilityLocal : $"{accessibilityLocal} ").ToLower()
				: "public ";
			var refKind = n.TryGetValueOrDefault<string?>(RefKindPropertyName, out var refKindLocal)
				? refKindLocal is not null
					? (refKindLocal.EndsWith(" ") ? refKindLocal : $"{refKindLocal} ").ToLower()
					: string.Empty
				: (isRefStruct, parameterRefKind) switch
				{
					(true, RefKind.Ref) => "ref ",
					(true, RefKind.RefReadOnlyParameter) => "ref readonly ",
					(true, RefKind.In) => "ref readonly ",
					_ => string.Empty
				};
			var parameterTypeString = parameterType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
			var nullableToken = nullableAnnotation == Annotated ? "?" : string.Empty;
			var namingRule = n.TryGetValueOrDefault<string>(NamingRulePropertyName, out var namingRuleLocal)
				? namingRuleLocal!
				: ">@";
			var propertyName = namingRule
				.Replace(">@", parameterName.ToPascalCase())
				.Replace("<@", parameterName.ToCamelCase())
				.Replace("@", parameterName);
			var emitPropertyStyle = n.TryGetValueOrDefault<int>(EmitPropertyStylePropertyName, out var emitPropertyStyleLocal)
				? emitPropertyStyleLocal
				: default;
			var setter = n.TryGetValueOrDefault<string>(SetterPropertyName, out var setterLocal)
				? setterLocal!
				: string.Empty;
			var assignment = emitPropertyStyle switch
			{
				(int)LocalEmitPropertyStyle.AssignToProperty
					=> $$"""{ get;{{(string.IsNullOrEmpty(setter) ? string.Empty : $" {setter};")}} } = {{parameterName}}""",
				(int)LocalEmitPropertyStyle.ReturnParameter
					=> $$"""=> {{parameterName}}""",
				_
					=> null
			};
			var readOnlyModifier = !isReadOnlyStruct // 1) It is inside a non-read-only struct (filters read-only structs).
				&& typeKind == TypeKind.Struct // 2) It must be a struct (filters classes).
				&& (string.IsNullOrEmpty(setter) || setter.Contains("init")) // 3) Setters must be none, or only init accessor.
				? "readonly "
				: string.Empty;
			properties.Add(
				$$"""
				/// <summary>
						/// The generated property declaration for parameter <c>{{parameterName}}</c>.
						/// </summary>
						[global::System.CodeDom.Compiler.GeneratedCodeAttribute("{{typeof(PrimaryConstructorMemberHandler).FullName}}", "{{Value}}")]
						[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
						{{propertyAccessibility}}{{readOnlyModifier}}{{refKind}}{{parameterTypeString}}{{nullableToken}} {{propertyName}} {{assignment}};
				"""
			);
		}

		var namespaceFullName = namespaceSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)["global::".Length..];
		var typeKindString = (typeKind, isRecord) switch
		{
			(TypeKind.Class, true) => "record ",
			(TypeKind.Class, _) => "class ",
			(TypeKind.Struct, true) => "record struct ",
			(TypeKind.Struct, _) => "struct "
		};
		var typeParametersString = typeParameters.Length == 0
			? string.Empty
			: $"<{string.Join(", ", from typeParameter in typeParameters select typeParameter.Name)}>";

		// Return result.
		return $$"""
			namespace {{namespaceFullName}}
			{
				partial {{typeKindString}}{{typeName}}{{typeParametersString}}
				{
					{{(fields.Count == 0 ? "// No fields generated." : string.Join("\r\n\r\n\t\t", fields))}}

					{{(properties.Count == 0 ? "// No properties generated." : string.Join("\r\n\r\n\t\t", properties))}}
				}
			}
			""";


		bool primaryConstructorPredicate(IMethodSymbol element)
			=> element is { MethodKind: MethodKind.Constructor, DeclaringSyntaxReferences: [var syntaxRef, ..] }
			&& syntaxRef.GetSyntax(cancellationToken) is TypeDeclarationSyntax;
	}
}

/// <summary>
/// Represents a local emit property style.
/// </summary>
file enum LocalEmitPropertyStyle
{
	/// <summary>
	/// Indicates the behavior is to generate an assignment to property:
	/// <code><![CDATA[public int Property { get; } = value;]]></code>
	/// </summary>
	AssignToProperty,

	/// <summary>
	/// Indicates the behavior is to generate a return statement that directly returns parameter:
	/// <code><![CDATA[public int Property => value;]]></code>
	/// </summary>
	ReturnParameter
}

/// <summary>
/// Represents local data of parameter symbol.
/// </summary>
/// <param name="Symbol">Indicates the symbol itself.</param>
/// <param name="FieldPart">Indicates the field part.</param>
/// <param name="PropertyPart">Indicates the property part.</param>
file sealed record ParameterLocalData(IParameterSymbol Symbol, AttributeData? FieldPart, AttributeData? PropertyPart);
