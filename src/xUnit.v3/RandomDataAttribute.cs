using System.Reflection;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.FakerTool.Proxy;
using Werecodent.CreateAndFake.Fluent;
using Werecodent.CreateAndFake.RunnerTool;
using Werecodent.CreateAndFake.RunnerTool.Attributes;
using Xunit;
using Xunit.Sdk;
using Xunit.v3;

namespace Werecodent.CreateAndFake.xUnit.v3;

/// <summary>
///     Flags <see cref="TheoryAttribute"/> methods to be populated with random values for testing.
/// </summary>
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
public sealed class RandomDataAttribute : DataAttribute, IRandomDataMarker
{
    /// <inheritdoc/>
    public int Trials { get; set; } = 1;

    /// <inheritdoc/>
    public override async ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(
        MethodInfo testMethod,
        DisposalTracker disposalTracker
    )
    {
        if (testMethod == null)
        {
            return [];
        }

        List<ITheoryDataRow> data = [];
        for (int i = 0; i < Trials; i++)
        {
            try
            {
                MethodCallWrapper test = await Tools
                    .Runner.CreateForAsync(
                        testMethod,
                        default,
                        opt => opt with { InheritIReflectableTypeOnFakedType = true }
                    )
                    .ConfigureAwait(false);

                data.Add(
                    new TheoryDataRow([.. test.Args.Select(FixArg)])
                    {
                        Label = GenericConverter.BuildTestParametersName(test.Method),
                    }
                );
            }
            catch (Exception e)
            {
                await Console
                    .Error.WriteLineAsync(
                        $"Test generation failure on {testMethod.Name}= {e.Message}"
                    )
                    .ConfigureAwait(false);
            }
        }

        disposalTracker?.AddRange(
            data.SelectMany(row => row.GetData())
                .Where(item => item is IDisposable or IAsyncDisposable)
        );

        return data;
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
        if (arg is IFaked faked)
        {
            if (arg is IReflectableType reflectable)
            {
                reflectable.GetTypeInfo().SetupReturn(typeof(Type), Times.Any);
            }
            faked.ToString().SetupReturn(GenericConverter.ExpandName(faked), Times.Any);
        }
        return arg;
    }
}
