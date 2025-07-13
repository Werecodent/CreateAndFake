using System.Reflection;
using CreateAndFake.RunnerTool;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreateAndFake.MSTest;

#pragma warning disable CA1031 // Avoid breaking test runner.

/// <summary>Populates <see cref="TestMethodAttribute"/> methods with random values for testing.</summary>
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
///         [TestMethod, RandomData]
///         public void Test([Size(2)] string data)
///         {
///             data.Length.Assert().Is(2);
///         }</code>
///     </example>
/// </remarks>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class RandomDataAttribute : Attribute, ITestDataSource
{
    /// <summary>Number of times to test the associated method.</summary>
    /// <remarks>Default:<c>1</c></remarks>
    public int Trials { get; set; } = 1;

    /// <inheritdoc/>
    public IEnumerable<object?[]> GetData(MethodInfo methodInfo)
    {
        if (methodInfo == null)
        {
            return [];
        }

        List<object?[]> results = [];
        for (int i = 0; i < Trials; i++)
        {
            try
            {
                MethodCallWrapper data = Tools.Runner.CreateFor(methodInfo);
                results.Add([.. data.Args]);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Test generation failure on {methodInfo}:{e.Message}");
            }
        }
        return results;
    }

    /// <inheritdoc/>
    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        if (methodInfo != null)
        {
            string args = string.Join(",", methodInfo.GetParameters().Select(p => p.ParameterType));
            return $"{methodInfo.Name}({args})";
        }
        else
        {
            return "Null";
        }
    }
}

#pragma warning restore CA1031
