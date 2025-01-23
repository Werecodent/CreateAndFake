using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Toolbox.FakerTool.Proxy;

namespace CreateAndFake.Toolbox.FakerTool;

/// <summary>For matching <c>out</c> and <c>ref</c> arguments.</summary>
/// <typeparam name="T">Argument <c>Type</c> to match.</typeparam>
public sealed class OutRef<T> : IOutRef
{
    /// <summary>Used as the out/ref argument.</summary>
    [SuppressMessage("Microsoft.Design",
        "CA1051:DoNotDeclareVisibleInstanceFields",
        Justification = "Required to match out/ref.")]
    public T? Var = default;
}
