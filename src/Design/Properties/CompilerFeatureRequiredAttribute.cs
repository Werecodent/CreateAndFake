// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if LEGACY // Required feature shipped with C# 13 / .NET 9.0
#pragma warning disable IDE0130, MA0182 // Must match existing location.

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

/// <summary>
///     Indicates that compiler support for a particular feature
///     is required for the location where this attribute is applied.
/// </summary>
/// <param name="featureName"><inheritdoc cref="FeatureName" path="/summary"/></param>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
internal sealed class CompilerFeatureRequiredAttribute(string featureName) : Attribute
{
    /// <summary>The name of the compiler feature.</summary>
    public string FeatureName { get; } = featureName;

    /// <summary>
    ///     If true, the compiler can choose to allow access to the location where
    ///     this attribute is applied if it does not understand <see cref="FeatureName"/>.
    /// </summary>
    public bool IsOptional { get; init; }

    /// <summary>The <see cref="FeatureName"/> used for the ref structs C# feature.</summary>
    public const string RefStructs = nameof(RefStructs);

    /// <summary>The <see cref="FeatureName"/> used for the required members C# feature.</summary>
    public const string RequiredMembers = nameof(RequiredMembers);
}

#pragma warning restore
#endif
