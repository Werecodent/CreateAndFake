using System.Reflection;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.Fluent;
using CreateAndFake.RunnerTool;
using Xunit;
using Xunit.Sdk;
using Xunit.v3;

namespace CreateAndFake.xUnit.v3;

/// <summary>Populates <see cref="TheoryAttribute"/> methods with random values for testing.</summary>
/// <remarks>
///     Earlier Parameters will be used to construct later Parameters if possible.<br/>
///     Use with Parameter attributes to control randomization behavior:
///     <list type="bullet"><item>
///         <see cref="SizeAttribute"/>,
///         <see cref="FakeAttribute"/> &amp;
///         <see cref="StubAttribute"/>
///     </item></list>
///     <example>
///         Example test:<code>
///         [Theory, RandomData]
///         internal static void Test([Size(2)] string data)
///         {
///             data.Length.Assert().Is(2);
///         }</code>
///     </example>
/// </remarks>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class RandomDataAttribute : DataAttribute
{
    /// <summary>Number of times to test the associated method.</summary>
    /// <remarks>Default:<c>1</c></remarks>
    public int Trials { get; set; } = 1;

    /// <inheritdoc/>
    public override ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(
        MethodInfo testMethod,
        DisposalTracker disposalTracker
    )
    {
        List<ITheoryDataRow> data = [];
        for (int i = 0; i < Trials; i++)
        {
            MethodCallWrapper test = Tools.Runner.CreateFor(
                testMethod,
                opt => opt with { InheritIReflectableTypeOnFakedType = true }
            );

            data.Add(new TheoryDataRow([.. test.Args.Select(FixArg)]));
        }

        disposalTracker?.AddRange(data.SelectMany(row => row.GetData()).OfType<IDisposable>());
        disposalTracker?.AddRange(data.SelectMany(row => row.GetData()).OfType<IAsyncDisposable>());

        return new ValueTask<IReadOnlyCollection<ITheoryDataRow>>(data);
    }

    /// <inheritdoc/>
    public override bool SupportsDiscoveryEnumeration()
    {
        return false;
    }

    /// <summary>Fixes <paramref name="arg"/> to be compatible with xUnit.</summary>
    /// <param name="arg">Generated Parameter argument to fix.</param>
    /// <returns><paramref name="arg"/>, modified if necessary.</returns>
    /// <remarks>Prevents crashes due to displaying <paramref name="arg"/> in results/windows.</remarks>
    private static object? FixArg(object? arg)
    {
        if (arg is IFaked and IReflectableType reflectable)
        {
            reflectable.GetTypeInfo().SetupReturn(typeof(Type));
        }
        return arg;
    }
}
