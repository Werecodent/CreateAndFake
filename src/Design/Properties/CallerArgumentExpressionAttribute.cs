// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if LEGACY // Required feature shipped with C# 14 / .NET 10.0
#pragma warning disable IDE0130 // Must match existing location.

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
internal sealed class CallerArgumentExpressionAttribute(string parameterName) : Attribute
{
    public string ParameterName { get; } = parameterName;
}

#pragma warning restore
#endif
