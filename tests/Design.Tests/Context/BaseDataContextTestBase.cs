using System.Reflection;
using Werecodent.CreateAndFake.Design.Context;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Design.Tests.Context;

public abstract class BaseDataContextTestBase<T> : AutoTester<T>
    where T : BaseDataContext
{
    [Theory, RandomData]
    public void BaseDataContext_MaintainsValues(T testInstance)
    {
        foreach (PropertyInfo prop in TypeDescriber.For<T>().Properties.All)
        {
            prop.GetValue(testInstance).Assert().Is(prop.GetValue(testInstance));
        }
    }

    [Theory, RandomData]
    public void BaseDataContext_DataVaries(T testInstance, T variant)
    {
        variant.Assert().IsNot(testInstance);
    }
}
