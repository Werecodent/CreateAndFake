using System.Reflection;
using CreateAndFake.Fluent;
using CreateAndFake.Toolbox.FakerTool.Proxy;
using Xunit.Sdk;

namespace CreateAndFake.xUnit.v2;

/// <summary>Populates <see cref="Xunit.TheoryAttribute"/> data with random values.</summary>
/// <seealso cref="Toolbox.RandomizerTool.IRandomizer.CreateFor"/>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class RandomDataAttribute : DataAttribute
{
    /// <summary>Number of times to test the method.</summary>
    public int Trials { get; set; } = 1;

    /// <summary>Generates data for a <see cref="Xunit.TheoryAttribute"/> test.</summary>
    /// <param name="testMethod">Method details <c>this</c> is attached to.</param>
    /// <returns>The generated data to run the test with.</returns>
    public override IEnumerable<object?[]> GetData(MethodInfo testMethod)
    {
        return Enumerable
            .Range(0, Math.Max(0, Trials))
            .Select(_ => Tools.Randomizer.CreateFor(testMethod).Args.Select(FixArg).ToArray());
    }

    /// <summary>Fixes <paramref name="arg"/> to be suitable for Xunit.</summary>
    /// <param name="arg">Instance to fix.</param>
    /// <returns><paramref name="arg"/> modified (if necessary) for Xunit.</returns>
    private object? FixArg(object? arg)
    {
        if (arg is IFaked and Type type)
        {
            type.UnderlyingSystemType.SetupReturn(typeof(Type).UnderlyingSystemType);
            type.FullName.SetupReturn(typeof(Type).FullName);
            type.IsArray.SetupReturn(typeof(Type).IsArray);
        }
        return arg;
    }
}
