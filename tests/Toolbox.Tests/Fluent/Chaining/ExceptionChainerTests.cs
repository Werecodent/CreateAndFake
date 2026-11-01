using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Tests.Fluent.Chaining;

public sealed class ExceptionChainerTests() : AutoTester(typeof(ExceptionChainer<>))
{
    [Theory, RandomData]
    internal static void That_SupportsArgumentException(ExceptionChainer<ArgumentException> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertError));
    }
}
