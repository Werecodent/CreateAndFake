using System.Reflection;
using CreateAndFake.Design.Context;
using CreateAndFake.Design.Randomization;

namespace CreateAndFake.Design.Tests.Context;

public abstract class DataContextTestBase<T>
    where T : BaseDataContext
{
    [Fact]
    public Task DataContext_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<T>();
    }

    [Fact]
    public Task DataContext_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<T>(opt =>
            opt with
            {
                InjectionValues = [new FastRandom()],
            }
        );
    }

    [Theory, RandomData]
    public void DataContext_MaintainsValues(T testInstance)
    {
        foreach (PropertyInfo prop in typeof(T).GetProperties())
        {
            prop.GetValue(testInstance).Assert().Is(prop.GetValue(testInstance));
        }
    }

    [Theory, RandomData]
    public void DataContext_DataVaries(T testInstance)
    {
        testInstance.CreateVariant().Assert().IsNot(testInstance);
    }
}
