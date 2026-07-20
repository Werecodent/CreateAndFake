using System.Reflection;
using CreateAndFake.RunnerTool;
using CreateAndFake.RunnerTool.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreateAndFake.MSTest.v3;

/// <summary>
///     Flags <see cref="TestMethodAttribute"/> methods
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
///         [TestMethod, RandomData]
///         public void Test([Size(2)] string data)
///         {
///             data.Length.Assert().Is(2);
///         }</code>
///     </example>
/// </remarks>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class RandomDataAttribute : Attribute, ITestDataSource, IRandomDataMarker
{
    /// <inheritdoc/>
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
                MethodCallWrapper data = Tools.Runner.CreateFor(methodInfo, default);
                results.Add([.. data.Args]);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Test generation failure on {methodInfo}= {e.Message}");
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
            return null;
        }
    }
}
