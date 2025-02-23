using CreateAndFake.AsserterTool;

namespace CreateAndFake.Tests.AsserterTool;

public sealed class AssertExceptionTests : ExceptionTestBase<AssertException>
{
    [Fact]
    internal static void AssertException_UnknownMessageDefault()
    {
        new AssertException(null, null, null).Message.Assert().Is("Unknown assert failure.");
    }

    [Theory, RandomData]
    internal static void AssertException_MessageFormat(
        string message, string details, string content, int seed, Exception ex)
    {
        new AssertException(message, null, null).Message.Assert().Contains(message);
        new AssertException(message, null, seed).Message.Assert().Contains($"{seed}");

        new AssertException(message, details, seed).Message.Assert().Contains(message);
        new AssertException(message, details, seed).Message.Assert().Contains(details);
        new AssertException(message, details, seed).Message.Assert().Contains($"{seed}");
        new AssertException(message, details, seed, ex).Message.Assert().ContainsNot(ex.ToString());

        new AssertException(message, null, seed, content).Message.Assert().Contains(message);
        new AssertException(message, null, seed, content).Message.Assert().Contains(content);
        new AssertException(message, null, seed, content).Message.Assert().Contains($"{seed}");

        new AssertException(message, details, seed, content).Message.Assert().Contains(message);
        new AssertException(message, details, seed, content).Message.Assert().Contains(details);
        new AssertException(message, details, seed, content).Message.Assert().Contains(content);
        new AssertException(message, details, seed, content).Message.Assert().Contains($"{seed}");

        new AssertException(message, details, seed, content, ex).Message.Assert().Contains(message);
        new AssertException(message, details, seed, content, ex).Message.Assert().Contains(details);
        new AssertException(message, details, seed, content, ex).Message.Assert().Contains(content);
        new AssertException(message, details, seed, content, ex).Message.Assert().Contains($"{seed}");
        new AssertException(message, details, seed, content, ex).Message.Assert().ContainsNot(ex.ToString());
    }
}
