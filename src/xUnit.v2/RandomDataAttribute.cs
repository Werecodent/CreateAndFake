using System.Reflection;
using CreateAndFake.FakerTool;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.Fluent;
using CreateAndFake.RunnerTool;
using CreateAndFake.RunnerTool.Attributes;
using Xunit.Sdk;

namespace CreateAndFake.xUnit.v2;

/// <summary>
///     Flags <see cref="Xunit.TheoryAttribute"/> methods
///     to be populated with random values for testing.
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
    /// <summary>Number of times to run the attached test.</summary>
    public int Trials { get; set; } = 1;

    /// <inheritdoc/>
    public override IEnumerable<object?[]> GetData(MethodInfo testMethod)
    {
        if (testMethod == null)
        {
            return [];
        }

        List<object?[]> results = [];
        for (int i = 0; i < Trials; i++)
        {
            try
            {
                MethodCallWrapper data = Tools.Runner.CreateFor(testMethod, default);
                results.Add([.. data.Args.Select(FixArg)]);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Test generation failure on {testMethod}:{e.Message}");
            }
        }
        return results;
    }

    /// <summary>Fixes <paramref name="arg"/> to be compatible with xUnit.</summary>
    /// <param name="arg">Generated Parameter argument to fix.</param>
    /// <returns><paramref name="arg"/>, modified if necessary.</returns>
    /// <remarks>Prevents crashes due to displaying <paramref name="arg"/> in results/windows.</remarks>
    private static object? FixArg(object? arg)
    {
        if (arg is IFaked and Type type)
        {
            type.UnderlyingSystemType.SetupReturn(typeof(Type).UnderlyingSystemType, Times.Any);
            type.FullName.SetupReturn(typeof(Type).FullName, Times.Any);
            type.IsArray.SetupReturn(typeof(Type).IsArray, Times.Any);
        }
        return arg;
    }
}
