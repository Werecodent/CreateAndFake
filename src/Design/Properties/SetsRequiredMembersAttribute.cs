// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if LEGACY // Required feature shipped with C# 13 / .NET 11.0
#pragma warning disable IDE0130 // Must match existing location.

namespace System.Diagnostics.CodeAnalysis;

/// <summary>
///     Specifies that this constructor sets all required members for the current type,
///     and callers do not need to set any required members themselves.
/// </summary>
[AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
internal sealed class SetsRequiredMembersAttribute : Attribute;

#pragma warning restore IDE0130
#endif
