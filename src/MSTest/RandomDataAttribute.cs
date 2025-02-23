using System.Reflection;
using CreateAndFake.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreateAndFake.MSTest;

/// <summary>Populates <seealso cref="TestMethodAttribute"/> methods with random values for testing.</summary>
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
        return Enumerable
            .Range(0, Math.Max(0, Trials))
            .Select(_ => Tools.Runner.CreateFor(methodInfo).Args.ToArray());
    }

    /// <inheritdoc/>
    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        ArgumentGuard.ThrowIfNull(methodInfo, nameof(methodInfo));

        return $"{methodInfo.Name}({string.Join(",", methodInfo.GetParameters().Select(p => p.ParameterType))})";
    }
}
