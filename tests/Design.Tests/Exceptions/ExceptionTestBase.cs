namespace Werecodent.CreateAndFake.Design.Tests.Exceptions;

public abstract class ExceptionTestBase<T>
    where T : Exception
{
    [Theory, RandomData]
    public void Debug_Exception_ToString(T error)
    {
        error.Assert().Debug();
    }

    [Fact]
    public Task Exception_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<T>(TestContext.Current.CancellationToken);
    }

    [Fact]
    public Task Exception_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<T>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public void Exception_DefaultConstructorPrivate()
    {
        typeof(T).GetConstructor(Type.EmptyTypes).Assert().IsNull();
        Activator.CreateInstance(typeof(T), true).Assert().IsNotNull();
    }

    [Theory, RandomData]
    public void Exception_XmlSerializes(T original)
    {
        Tools.Tester.VerifyXmlSerialization(original);
    }

    [Fact]
    public void Exception_JsonSerializes()
    {
        T original;
        do
        {
            original = Tools.Randomizer.Create<T>();
        } while (original.InnerException != null);

        Tools.Tester.VerifyJsonSerialization(original);
    }
}
