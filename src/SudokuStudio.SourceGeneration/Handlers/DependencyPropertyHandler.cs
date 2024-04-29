namespace Sudoku.SourceGeneration.Handlers;

/// <summary>
/// The generator handler for dependency properties.
/// </summary>
internal static class DependencyPropertyHandler
{
	/// <inheritdoc/>
	public static void Output(SourceProductionContext spc, ImmutableArray<CollectedResult> values)
	{
		var types = new List<string>();
		foreach (var group in values.GroupBy(static data => data.Type, (IEqualityComparer<INamedTypeSymbol>)SymbolEqualityComparer.Default))
		{
			var containingType = group.Key;
			var containingTypeStr = containingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
			var namespaceStr = containingType.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
			var dependencyProperties = new List<string>();
			var properties = new List<string>();
			foreach (var (_, propertiesData) in group)
			{
				foreach (
					var (
						propertyName, propertyType, docData, generatorMemberName,
						generatorMemberKind, defaultValue, callbackMethodName, isNullable,
						accessibility, membersNotNullWhenReturnsTrue
					) in propertiesData
				)
				{
					var propertyTypeStr = propertyType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
					var doc = XamlBinding.GetDocumentationComment(propertyName, docData, true);
					var defaultValueCreatorStr = XamlBinding.GetPropertyMetadataString(defaultValue, propertyType, generatorMemberName, generatorMemberKind, callbackMethodName, propertyTypeStr);
					if (defaultValueCreatorStr is null)
					{
						// Error case has been encountered.
						continue;
					}

					var nullableToken = isNullable ? "?" : string.Empty;
					var accessibilityModifier = accessibility.GetName();
					var memberNotNullComment = membersNotNullWhenReturnsTrue is not null ? string.Empty : "//";
					var notNullMembersStr = membersNotNullWhenReturnsTrue switch
					{
						null or [] => "new string[0]",
						_ => string.Join(", ", from element in membersNotNullWhenReturnsTrue select $"nameof({element})")
					};

					dependencyProperties.Add(
						$"""
						/// <summary>
								/// Defines a dependency property that binds with property <see cref="{propertyName}"/>.
								/// </summary>
								/// <seealso cref="{propertyName}"/>
								[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
								[global::System.CodeDom.Compiler.GeneratedCodeAttribute("{typeof(DependencyPropertyHandler).FullName}", "{Value}")]
								{accessibilityModifier} static readonly global::Microsoft.UI.Xaml.DependencyProperty {propertyName}Property =
									global::Microsoft.UI.Xaml.DependencyProperty.Register(nameof({propertyName}), typeof({propertyTypeStr}), typeof({containingTypeStr}), {defaultValueCreatorStr});
						"""
					);

					properties.Add(
						$$"""
						{{doc}}
								{{memberNotNullComment}}[global::System.Diagnostics.CodeAnalysis.MemberNotNullWhenAttribute(true, {{notNullMembersStr}})]
								[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
								[global::System.CodeDom.Compiler.GeneratedCodeAttribute("{{typeof(DependencyPropertyHandler).FullName}}", "{{Value}}")]
								{{accessibilityModifier}} {{propertyTypeStr}}{{nullableToken}} {{propertyName}}
								{
									[global::System.Diagnostics.DebuggerStepThroughAttribute]
									[global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
									get => ({{propertyTypeStr}}{{nullableToken}})GetValue({{propertyName}}Property);

									[global::System.Diagnostics.DebuggerStepThroughAttribute]
									[global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
									set => SetValue({{propertyName}}Property, value);
								}
						"""
					);
				}
			}

			types.Add(
				$$"""
				namespace {{namespaceStr["global::".Length..]}}
				{
					partial class {{containingType.Name}}
					{
						//
						// Declaration of dependency properties
						//
						#region Dependency properties
						{{string.Join("\r\n\r\n\t\t", dependencyProperties)}}
						#endregion
				
				
						//
						// Declaration of interactive properties
						//
						#region Interactive properties
						{{string.Join("\r\n\r\n\t\t", properties)}}
						#endregion
					}
				}
				"""
			);
		}

		spc.AddSource(
			"DependencyProperties.g.cs",
			$$"""
			{{Banner.AutoGenerated}}

			#nullable enable

			{{string.Join("\r\n\r\n", types)}}
			"""
		);
	}

	/// <inheritdoc/>
	public static CollectedResult? Transform(GeneratorAttributeSyntaxContext gasc, CancellationToken cancellationToken)
	{
		if (gasc is not
			{
				TargetSymbol: INamedTypeSymbol typeSymbol,
				Attributes: var attributes,
				SemanticModel.Compilation: var compilation
			})
		{
			return null;
		}

		if (compilation.GetTypeByMetadataName("Microsoft.UI.Xaml.DependencyObject") is not { } dependencyObjectType)
		{
			return null;
		}

		if (!typeSymbol.IsDerivedFrom(dependencyObjectType))
		{
			return null;
		}

		var propertiesData = new List<Data>();
		foreach (var attributeData in attributes)
		{
			if (attributeData is not
				{
					ConstructorArguments: [{ Value: string propertyName }],
					NamedArguments: var namedArgs,
					AttributeClass.TypeArguments: [ITypeSymbol { IsReferenceType: var isReferenceType } propertyType]
				})
			{
				continue;
			}

			var docCref = default(string);
			var docPath = default(string);
			var defaultValueGenerator = default(string);
			var defaultValue = default(object);
			var callbackMethodName = default(string);
			var docSummary = default(string);
			var docRemarks = default(string);
			var membersNotNullWhenReturnsTrue = default(string[]);
			var accessibility = Accessibility.Public;
			foreach (var pair in namedArgs)
			{
				switch (pair)
				{
					case ("DocReferencedMemberName", { Value: string v }):
					{
						docCref = v;
						break;
					}
					case ("DocReferencedPath", { Value: string v }):
					{
						docPath = v;
						break;
					}
					case ("DefaultValueGeneratingMemberName", { Value: string v }):
					{
						defaultValueGenerator = v;
						break;
					}
					case ("DefaultValue", { Value: { } v }):
					{
						defaultValue = v;
						break;
					}
					case ("CallbackMethodName", { Value: string v }):
					{
						callbackMethodName = v;
						break;
					}
					case ("DocSummary", { Value: string v }):
					{
						docSummary = v;
						break;
					}
					case ("DocRemarks", { Value: string v }):
					{
						docRemarks = v;
						break;
					}
					case ("Accessibility", { Value: int v }):
					{
						accessibility = (Accessibility)v;
						break;
					}
					case ("MembersNotNullWhenReturnsTrue", { Values: var rawValues }):
					{
						membersNotNullWhenReturnsTrue = [.. from rawValue in rawValues select (string)rawValue.Value!];
						break;
					}
				}
			}

			const string callbackMethodSuffix = "PropertyCallback";
			var callbackAttribute = compilation.GetTypeByMetadataName("SudokuStudio.ComponentModel.CallbackAttribute")!;
			callbackMethodName ??= (
				from methodSymbol in typeSymbol.GetMembers().OfType<IMethodSymbol>()
				where methodSymbol is { IsStatic: true, ReturnsVoid: true }
				let methodName = methodSymbol.Name
				where methodName.EndsWith(callbackMethodSuffix)
				let relatedPropertyName = methodName[..methodName.IndexOf(callbackMethodSuffix)]
				where relatedPropertyName == propertyName || $"{relatedPropertyName}?" == propertyName
				let attributesData = methodSymbol.GetAttributes()
				where attributesData.Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, callbackAttribute))
				select methodName
			).FirstOrDefault();

			const string defaultValueFieldSuffix = "DefaultValue";
			var defaultValueAttribute = compilation.GetTypeByMetadataName("SudokuStudio.ComponentModel.DefaultAttribute")!;
			defaultValueGenerator ??= (
				from fieldSymbol in typeSymbol.GetMembers().OfType<IFieldSymbol>()
				where fieldSymbol.IsStatic
				let fieldName = fieldSymbol.Name
				where fieldName.EndsWith(defaultValueFieldSuffix)
				let relatedPropertyName = fieldName[..fieldName.IndexOf(defaultValueFieldSuffix)]
				where relatedPropertyName == propertyName || $"{relatedPropertyName}?" == propertyName
				let attributesData = fieldSymbol.GetAttributes()
				where attributesData.Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, defaultValueAttribute))
				select fieldName
			).FirstOrDefault();

			var defaultValueGeneratorKind = default(DefaultValueGeneratingMemberKind?);
			if (defaultValueGenerator is not null)
			{
				defaultValueGeneratorKind = typeSymbol.GetAllMembers().FirstOrDefault(m => m.Name == defaultValueGenerator) switch
				{
					IFieldSymbol { Type: var t, IsStatic: true } when e(t)
						=> DefaultValueGeneratingMemberKind.Field,
					IPropertySymbol { Type: var t, IsStatic: true } when e(t)
						=> DefaultValueGeneratingMemberKind.Property,
					IMethodSymbol { Parameters: [], ReturnType: var t, IsStatic: true } when e(t)
						=> DefaultValueGeneratingMemberKind.ParameterlessMethod,
					null
						=> DefaultValueGeneratingMemberKind.CannotReference,
					_
						=> DefaultValueGeneratingMemberKind.Otherwise
				};
			}

			if (defaultValueGeneratorKind is DefaultValueGeneratingMemberKind.CannotReference or DefaultValueGeneratingMemberKind.Otherwise)
			{
				// Invalid generator name.
				continue;
			}

			var isNullable = propertyName[^1] == '?';
			propertiesData.Add(
				new(
					isNullable ? propertyName[..^1] : propertyName,
					propertyType,
					new(docSummary, docRemarks, docCref, docPath),
					defaultValueGenerator,
					defaultValueGeneratorKind,
					defaultValue,
					callbackMethodName,
					isNullable,
					accessibility,
					membersNotNullWhenReturnsTrue
				)
			);


			bool e(ITypeSymbol t) => SymbolEqualityComparer.Default.Equals(t, propertyType);
		}

		return new(typeSymbol, propertiesData);
	}


	/// <summary>
	/// The nesting data structure for <see cref="CollectedResult"/>.
	/// </summary>
	/// <seealso cref="CollectedResult"/>
	internal sealed record Data(
		string PropertyName,
		ITypeSymbol PropertyType,
		DocumentationCommentData DocumentationCommentData,
		string? DefaultValueGeneratingMemberName,
		DefaultValueGeneratingMemberKind? DefaultValueGeneratingMemberKind,
		object? DefaultValue,
		string? CallbackMethodName,
		bool IsNullable,
		Accessibility Accessibility,
		string[]? MembersNotNullWhenReturnsTrue
	);

	/// <summary>
	/// Indicates the data collected via <see cref="DependencyPropertyHandler"/>.
	/// </summary>
	/// <seealso cref="DependencyPropertyHandler"/>
	internal sealed record CollectedResult(INamedTypeSymbol Type, List<Data> PropertiesData);
}