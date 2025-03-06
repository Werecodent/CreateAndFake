using System.Reflection;
using CreateAndFake.Design.Context;
using CreateAndFake.Design.Randomization;
using CreateAndFake.TesterTool;

namespace CreateAndFake.Design.Tests.Context;

/// <summary>Handles testing data context classes.</summary>
/// <typeparam name="T">Type to test.</typeparam>
public abstract class DataContextTestBase<T>
    where T : BaseDataContext
{
    /// <inheritdoc cref="ITester.PreventsNullRefException"/>
    [Fact]
    public void DataContext_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<T>();
    }

    /// <inheritdoc cref="ITester.PreventsParameterMutation"/>
    [Fact]
    public void DataContext_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<T>(opt =>
            opt with
            {
                InjectionValues = [new FastRandom()],
            }
        );
    }

    /// <summary>Verifies data remains consistent on instance.</summary>
    [Theory, RandomData]
    public void DataContext_MaintainsValues(T testInstance)
    {
        foreach (PropertyInfo prop in typeof(T).GetProperties())
        {
            prop.GetValue(testInstance).Assert().Is(prop.GetValue(testInstance));
        }
    }

    /// <summary>Verifies instances are not equal.</summary>
    [Theory, RandomData]
    public void DataContext_DataVaries(T testInstance)
    {
        testInstance.CreateVariant().Assert().IsNot(testInstance);
    }
}
