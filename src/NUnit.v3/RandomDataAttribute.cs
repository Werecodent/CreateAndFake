using CreateAndFake.RunnerTool;
using CreateAndFake.RunnerTool.Attributes;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace CreateAndFake.NUnit.v3;

/// <summary>Flags test methods to be populated with random values for testing.</summary>
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
///         [RandomData]
///         public static void Test([Size(2)] string data)
///         {
///             data.Length.Assert().Is(2);
///         }</code>
///     </example>
/// </remarks>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class RandomDataAttribute : NUnitAttribute, ITestBuilder, IRandomDataMarker
{
    /// <inheritdoc/>
    public int Trials { get; set; } = 1;

    /// <inheritdoc/>
    public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
    {
        if (method == null)
        {
            return [];
        }

        List<TestMethod> results = [];
        for (int i = 0; i < Trials; i++)
        {
            MethodCallWrapper data;
            try
            {
                data = Tools.Runner.CreateFor(method.MethodInfo, default);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Test generation failure on {method}= {e.Message}");
                continue;
            }

            string args = string.Join(",", method.GetParameters().Select(p => p.ParameterType));

            results.Add(
                new NUnitTestCaseBuilder().BuildTestMethod(
                    method,
                    suite,
                    new TestCaseParameters(
                        new TestCaseAttribute([.. data.Args])
                        {
                            TestName = $"{method.Name}({args})",
                        }
                    )
                )
            );
        }
        return results;
    }
}
