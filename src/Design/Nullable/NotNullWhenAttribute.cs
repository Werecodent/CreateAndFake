// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if LEGACY // Attribute added in .NET Standard 2.1 & Core 3.0

namespace System.Diagnostics.CodeAnalysis;

/// <summary>
///     Specifies the attached data will not be <c>null</c>
///     when the call returns with <paramref name="returnValue"/>.
/// </summary>
/// <param name="returnValue"><inheritdoc cref="ReturnValue" path="/summary"/></param>
[ExcludeFromCodeCoverage, AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
internal sealed class NotNullWhenAttribute(bool returnValue) : Attribute
{
    /// <summary>Result when the attached data will not be <c>null</c>.</summary>
    public bool ReturnValue { get; } = returnValue;
}

#endif
