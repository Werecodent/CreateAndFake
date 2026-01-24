using System.Reflection;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Context;

namespace CreateAndFake.Design.Tests.Context;

public abstract class DataContextTestBase<T>
    where T : BaseDataContext
{
    [Fact]
    public Task DataContext_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<T>(TestContext.Current.CancellationToken);
    }

    [Fact]
    public Task DataContext_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<T>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    public void DataContext_MaintainsValues(T testInstance)
    {
        foreach (PropertyInfo prop in TypeDescriber.GetAllProperties<T>())
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
