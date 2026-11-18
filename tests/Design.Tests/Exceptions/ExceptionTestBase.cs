using Werecodent.CreateAndFake.Design.Reiteration;

namespace Werecodent.CreateAndFake.Design.Tests.Exceptions;

public abstract class ExceptionTestBase<T> : AutoTester<T>
    where T : Exception
{
    [Theory, RandomData]
    public void Debug_Exception_ToString(T error)
    {
        error.Assert().Debug();
    }

    [Fact]
    public void Exception_DefaultConstructorPrivate()
    {
        typeof(T).GetConstructor(Type.EmptyTypes).Assert().IsNull();
        Activator.CreateInstance(typeof(T), true).Assert().IsNotNull();
    }

    [Fact]
    public void Exception_XmlSerializes()
    {
        Limiter.Score.Retry(
            "Not all System exceptions support xml serialization.",
            () => Tools.Tester.VerifyXmlSerialization<T>(),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public void Exception_JsonSerializes()
    {
        Limiter.Score.Retry(
            "Not all System exceptions support json serialization.",
            () => Tools.Tester.VerifyJsonSerialization<T>(),
            TestContext.Current.CancellationToken
        );
    }
}
