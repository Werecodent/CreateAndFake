using System.Diagnostics.CodeAnalysis;

namespace CreateAndFake.Toolbox.FakerTool.Proxy;

/// <summary>Marker for OutRef.</summary>
[SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces",
    Justification = "For matching any OutRef in mock behavior.")]
public interface IOutRef { }
