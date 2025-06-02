using System.Reflection;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.Fluent;
using Xunit.Sdk;

namespace CreateAndFake.xUnit.v2;

/// <summary>Populates <seealso cref="Xunit.TheoryAttribute"/> methods with random values for testing.</summary>
/// <remarks>
///     Earlier Parameters will be used to construct later Parameters if possible.<br/>
///     Use with Parameter attributes to control randomization behavior:
///     <list type="bullet"><item>
///         <seealso cref="SizeAttribute"/>,
///         <seealso cref="FakeAttribute"/> &amp;
///         <seealso cref="StubAttribute"/>
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
    public override IEnumerable<object?[]> GetData(MethodInfo testMethod)
    {
        return Enumerable
            .Range(0, Math.Max(0, Trials))
            .Select(_ => Tools.Runner.CreateFor(testMethod).Args.Select(FixArg).ToArray());
    }

    /// <summary>Fixes <paramref name="arg"/> to be compatible with xUnit.</summary>
    /// <param name="arg">Generated Parameter argument to fix.</param>
    /// <returns><paramref name="arg"/>, modified if necessary.</returns>
    /// <remarks>Prevents crashes due to displaying <paramref name="arg"/> in results/windows.</remarks>
    private static object? FixArg(object? arg)
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
