using System.Reflection;
using CreateAndFake.Fluent;
using CreateAndFake.Toolbox.FakerTool.Proxy;
using Xunit;
using Xunit.Sdk;
using Xunit.v3;

namespace CreateAndFake.xUnit.v3;

/// <summary>Populates <see cref="TheoryAttribute"/> data with random values.</summary>
/// <seealso cref="Toolbox.RandomizerTool.IRandomizer.CreateFor"/>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class RandomDataAttribute : DataAttribute
{
    /// <summary>Number of times to test the method.</summary>
    public int Trials { get; set; } = 1;

    /// <inheritdoc/>
    public override ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(
        MethodInfo testMethod, DisposalTracker disposalTracker)
    {
        IReadOnlyCollection<ITheoryDataRow> data = [.. Enumerable
            .Range(0, Math.Max(0, Trials))
            .Select(_ => Tools.Randomizer.CreateFor(testMethod).Args.Select(FixArg).ToArray())
            .Select(data => new TheoryDataRow(data))];

        foreach (IDisposable disposable in data.SelectMany(row => row.GetData()).OfType<IDisposable>())
        {
            disposalTracker?.Add(disposable);
        }
        foreach (IAsyncDisposable disposable in data.SelectMany(row => row.GetData()).OfType<IAsyncDisposable>())
        {
            disposalTracker?.Add(disposable);
        }

        return new ValueTask<IReadOnlyCollection<ITheoryDataRow>>(data);
    }

    /// <inheritdoc/>
    public override bool SupportsDiscoveryEnumeration()
    {
        return false;
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