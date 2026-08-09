using System.Reflection;
using Werecodent.CreateAndFake.Design.Context;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Design.Tests.Context;

public abstract class BaseDataContextTestBase<T>
    where T : BaseDataContext
{
    [Fact]
    public Task BaseDataContext_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<T>(TestContext.Current.CancellationToken);
    }

    [Fact]
    public Task BaseDataContext_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<T>(
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    public void BaseDataContext_MaintainsValues(T testInstance)
    {
        foreach (PropertyInfo prop in TypeDescriber.For<T>().Properties.All)
        {
            prop.GetValue(testInstance).Assert().Is(prop.GetValue(testInstance));
        }
    }

    [Theory, RandomData]
    public void BaseDataContext_DataVaries(T testInstance)
    {
        testInstance.Tools().Variant().Assert().IsNot(testInstance);
    }
}
