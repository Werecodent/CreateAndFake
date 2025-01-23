using CreateAndFake.Fluent;
using CreateAndFake.Toolbox.FakerTool.Proxy;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

#pragma warning disable CA1510 // Use 'ArgumentNullException.ThrowIfNull' instead: Not available in all version.

namespace CreateAndFake.NUnit;

/// <summary>Populates data with random values.</summary>
/// <seealso cref="Toolbox.RandomizerTool.IRandomizer.CreateFor"/>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class RandomDataAttribute : NUnitAttribute, ITestBuilder
{
    /// <summary>Number of times to test the method.</summary>
    public int Trials { get; set; } = 1;

    /// <inheritdoc/>
    public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
    {
        if (method == null)
        {
            throw new ArgumentNullException(nameof(method));
        }
        if (suite == null)
        {
            throw new ArgumentNullException(nameof(suite));
        }
        for (int i = 0; i < Trials; i++)
        {
            yield return new NUnitTestCaseBuilder().BuildTestMethod(method, suite, new TestCaseParameters(
                new TestCaseAttribute([.. Tools.Randomizer.CreateFor(method.MethodInfo).Args.Select(FixArg)])));
        }
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

#pragma warning restore CA1040 // Use 'ArgumentNullException.ThrowIfNull' instead
