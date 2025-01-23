using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreateAndFake.MSTest;

/// <summary>Populates method data with random values.</summary>
/// <seealso cref="Toolbox.RandomizerTool.IRandomizer.CreateFor"/>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class RandomDataAttribute : Attribute, ITestDataSource
{
    /// <summary>Number of times to test the method.</summary>
    public int Trials { get; set; } = 1;

    /// <summary>Generates data for a test.</summary>
    /// <param name="methodInfo">Method details <c>this</c> is attached to.</param>
    /// <returns>The generated data to run the test with.</returns>
    public IEnumerable<object?[]> GetData(MethodInfo methodInfo)
    {
        return Enumerable
            .Range(0, Math.Max(0, Trials))
            .Select(_ => Tools.Randomizer.CreateFor(methodInfo).Args.ToArray());
    }

    /// <inheritdoc/>
    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        return methodInfo?.Name;
    }
}
