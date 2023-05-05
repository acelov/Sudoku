using Sudoku.Diagnostics.CodeGen;

namespace Sudoku.SourceGeneration.Handlers;

/// <summary>
/// The generator handler for attached properties.
/// </summary>
internal sealed class AttachedPropertyHandler : IIncrementalGeneratorAttributeHandler<AttachedPropertyCollectedResult>
{
	/// <inheritdoc/>
	public void Output(SourceProductionContext spc, ImmutableArray<AttachedPropertyCollectedResult> values)
	{
		foreach (var group in values.GroupBy(static data => data.Type, (IEqualityComparer<INamedTypeSymbol>)SymbolEqualityComparer.Default))
		{
			var containingType = group.Key;
			var containingTypeStr = containingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
			var namespaceStr = containingType.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
			var attachedProperties = new List<string>();
			var setterMethods = new List<string>();
			foreach (var (_, propertiesData) in group)
			{
				foreach (
					var (
						propertyName, propertyType, docData, generatorMemberName,
						generatorMemberKind, defaultValue, callbackMethodName, isNullable
					) in propertiesData
				)
				{
					var propertyTypeStr = propertyType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
					var doc = XamlBinding.GetDocumentationComment(propertyName, docData, false);

					var defaultValueCreatorStr = XamlBinding.GetPropertyMetadataString(defaultValue, propertyType, generatorMemberName, generatorMemberKind, callbackMethodName, propertyTypeStr);
					if (defaultValueCreatorStr is null)
						// Error case has been encountered.
						continue;

					var nullableToken = isNullable ? "?" : string.Empty;
					attachedProperties.Add(
						$"""
						/// <summary>
							/// Defines a attached property that binds with setter and getter methods <c>{propertyName}</c>.
							/// </summary>
							[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
							[global::System.CodeDom.Compiler.GeneratedCodeAttribute("{GetType().FullName}", "{VersionValue}")]
							public static readonly global::Microsoft.UI.Xaml.DependencyProperty {propertyName}Property =
								global::Microsoft.UI.Xaml.DependencyProperty.RegisterAttached("{propertyName}", typeof({propertyTypeStr}), typeof({containingTypeStr}), {defaultValueCreatorStr});
						"""
					);

					setterMethods.Add(
						$$"""
						{{doc}}
							[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
							[global::System.CodeDom.Compiler.GeneratedCodeAttribute("{{GetType().FullName}}", "{{VersionValue}}")]
							[global::System.Diagnostics.DebuggerStepThroughAttribute]
							[global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
							public static void Set{{propertyName}}(global::Microsoft.UI.Xaml.DependencyObject obj, {{propertyTypeStr}}{{nullableToken}} value)
								=> obj.SetValue({{propertyName}}Property, value);

							{{doc}}
							[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
							[global::System.CodeDom.Compiler.GeneratedCodeAttribute("{{GetType().FullName}}", "{{VersionValue}}")]
							[global::System.Diagnostics.DebuggerStepThroughAttribute]
							[global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
							public static {{propertyTypeStr}}{{nullableToken}} Get{{propertyName}}(global::Microsoft.UI.Xaml.DependencyObject obj)
								=> ({{propertyTypeStr}})obj.GetValue({{propertyName}}Property);
						"""
					);
				}
			}

			spc.AddSource(
				$"{containingType.ToFileName()}.g.{Shortcuts.AttachedProperty}.cs",
				$$"""
				// <auto-generated />

				#nullable enable

				namespace {{namespaceStr["global::".Length..]}};

				partial class {{containingType.Name}}
				{
					//
					// Declaration of attached properties
					//
					#region Attached properties
					{{string.Join("\r\n\r\n\t", attachedProperties)}}
					#endregion


					//
					// Declaration of interactive methods
					//
					#region Interactive properties
					{{string.Join("\r\n\r\n\t", setterMethods)}}
					#endregion
				}
				"""
			);
		}
	}

	/// <inheritdoc/>
	public AttachedPropertyCollectedResult? Transform(GeneratorAttributeSyntaxContext gasc, CancellationToken cancellationToken)
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

		var propertiesData = new List<AttachedPropertyData>();
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
			var isNullable = false;
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
					case ("IsNullable", { Value: bool v }) when isReferenceType:
					{
						isNullable = v;
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
				where relatedPropertyName == propertyName
				let attributesData = methodSymbol.GetAttributes()
				where attributesData.Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, callbackAttribute))
				select methodName
			).FirstOrDefault();

			const string defaultValueFieldSuffix = "DefaultValue";
			var defaultValueAttribute = compilation.GetTypeByMetadataName("SudokuStudio.ComponentModel.DefaultValueAttribute")!;
			defaultValueGenerator ??= (
				from fieldSymbol in typeSymbol.GetMembers().OfType<IFieldSymbol>()
				where fieldSymbol.IsStatic
				let fieldName = fieldSymbol.Name
				where fieldName.EndsWith(defaultValueFieldSuffix)
				let relatedPropertyName = fieldName[..fieldName.IndexOf(defaultValueFieldSuffix)]
				where relatedPropertyName == propertyName
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
				// Invalid generator name.
				continue;

			propertiesData.Add(
				new(
					propertyName, propertyType, new(docSummary, docRemarks, docCref, docPath),
					defaultValueGenerator, defaultValueGeneratorKind, defaultValue, callbackMethodName, isNullable
				)
			);


			bool e(ITypeSymbol t) => SymbolEqualityComparer.Default.Equals(t, propertyType);
		}

		return new(typeSymbol, propertiesData);
	}
}
