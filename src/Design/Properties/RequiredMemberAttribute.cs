// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if LEGACY // Required member shipped with C# 11 / .NET 7.0
#pragma warning disable IDE0130 // Must match existing location.

namespace System.Runtime.CompilerServices;

/// <summary>Specifies that a type has required members or that a member is required.</summary>
[AttributeUsage(
    AttributeTargets.Class
        | AttributeTargets.Struct
        | AttributeTargets.Field
        | AttributeTargets.Property,
    AllowMultiple = false,
    Inherited = false
)]
internal sealed class RequiredMemberAttribute : Attribute;

#pragma warning restore IDE0130
#endif
