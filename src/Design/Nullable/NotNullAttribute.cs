// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if LEGACY // Attribute added in .NET Standard 2.1 & Core 3.0

namespace System.Diagnostics.CodeAnalysis;

/// <summary>
///     Specifies the attached data will not be <see langword="null"/> when the call returns.
/// </summary>
[
    ExcludeFromCodeCoverage,
    AttributeUsage(
        AttributeTargets.Parameter
            | AttributeTargets.Property
            | AttributeTargets.Field
            | AttributeTargets.ReturnValue,
        Inherited = false
    )
]
internal sealed class NotNullAttribute : Attribute;

#endif
