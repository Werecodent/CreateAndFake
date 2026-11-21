namespace Werecodent.CreateAndFake.RunnerTool.Attributes;

/// <summary>Flags test methods to be populated with random values for testing.</summary>
public interface IRandomDataMarker
{
    /// <summary>Number of times to test the associated method.</summary>
    /// <remarks>Default:<c>1</c></remarks>
    int Trials { get; set; }
}
