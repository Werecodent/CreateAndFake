// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if LEGACY // Attribute added in .NET Standard 2.1 & Core 3.0
#pragma warning disable IDE0130 // Must match existing location.

namespace System.Diagnostics.CodeAnalysis;

/// <summary>
///     Specifies the attached data will not be <see langword="null"/>
///     when <paramref name="parameterName"/> is not <see langword="null"/>.
/// </summary>
/// <param name="parameterName"><inheritdoc cref="ParameterName" path="/summary"/></param>
[ExcludeFromCodeCoverage]
[AttributeUsage(
    AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue,
    AllowMultiple = true,
    Inherited = false
)]
internal sealed class NotNullIfNotNullAttribute(string parameterName) : Attribute
{
    /// <summary>Name of the associated parameter that matches nullability.</summary>
    public string ParameterName { get; } = parameterName;
}

#pragma warning restore IDE0130
#endif
