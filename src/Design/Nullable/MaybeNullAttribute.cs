// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if LEGACY // Attribute added in .NET Standard 2.1 & Core 3.0
#pragma warning disable IDE0130 // Must match existing location.

namespace System.Diagnostics.CodeAnalysis;

/// <summary>Specifies the attached data may actually be <see langword="null"/>.</summary>
[ExcludeFromCodeCoverage]
[AttributeUsage(
    AttributeTargets.Parameter
        | AttributeTargets.Property
        | AttributeTargets.Field
        | AttributeTargets.ReturnValue,
    Inherited = false
)]
internal sealed class MaybeNullAttribute : Attribute;

#pragma warning restore IDE0130
#endif
