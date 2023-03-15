﻿namespace System.Diagnostics.CodeGen;

/// <summary>
/// Defines an attribute that specifies a parameter used by a deconstruction method.
/// </summary>
/// <param name="referencedMemberName">The referenced member name.</param>
[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
public sealed class DeconstructionMethodArgumentAttribute(
#pragma warning disable IDE0060, CS9113
	string referencedMemberName
#pragma warning restore IDE0060, CS9113
) : Attribute;
