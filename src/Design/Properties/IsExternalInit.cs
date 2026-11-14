// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if LEGACY // Init type shipped with C# 7 / .NET 5.0
#pragma warning disable IDE0130, MA0036, MA0182 // Must match existing version & location.

using System.ComponentModel;

namespace System.Runtime.CompilerServices;

/// <summary>
///     Reserved to be used by the compiler for tracking metadata.
///     This class should not be used by developers in source code.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
internal sealed class IsExternalInit;

#pragma warning restore
#endif
