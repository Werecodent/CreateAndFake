using CreateAndFake.Design;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace CreateAndFake.NUnit;

/// <summary>Populates test methods with random values for testing.</summary>
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
///         public static void Test([Size(2)] string data)
///         {
///             data.Length.Assert().Is(2);
///         }</code>
///     </example>
/// </remarks>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class RandomDataAttribute : NUnitAttribute, ITestBuilder
{
    /// <summary>Number of times to test the associated method.</summary>
    /// <remarks>Default:<c>1</c></remarks>
    public int Trials { get; set; } = 1;

    /// <inheritdoc/>
    public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
    {
        ArgumentGuard.ThrowIfNull(method, nameof(method));
        ArgumentGuard.ThrowIfNull(suite, nameof(suite));

        for (int i = 0; i < Trials; i++)
        {
            yield return new NUnitTestCaseBuilder().BuildTestMethod(method, suite, new TestCaseParameters(
                new TestCaseAttribute([.. Tools.Runner.CreateFor(method.MethodInfo).Args])
                {
                    TestName = $"{method.Name}({string.Join(",", method.GetParameters().Select(p => p.ParameterType))})"
                }));
        }
    }
}
