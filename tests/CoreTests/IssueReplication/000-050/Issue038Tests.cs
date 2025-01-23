using CreateAndFake.Toolbox.FakerTool;
using CreateAndFake.Toolbox.FakerTool.Proxy;

namespace CreateAndFakeTests.IssueReplication;

public static class Issue038Tests
{
    public interface ISample
    {
        string Value { get; set; }

        int Check(InnerSample value);

        int Check(string item1, string item2);
    }

    public class InnerSample
    {
        public string Value { get; set; }
    }

    [Theory, RandomData]
    internal static void Issue038_FluentStubSetup([Stub] ISample sample, string value)
    {
        sample.Value.Assert().Is(null);
        sample.Value.SetupReturn(value);
        sample.Value.Assert().Is(value);
        sample.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Issue038_FluentFakeSetup([Fake] ISample sample, string value)
    {
        sample.Value.Assert().IsNot(null);
        sample.Value.SetupReturn(value);
        sample.Value.Assert().Is(value);
        sample.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Issue038_FluentArgsByValue([Stub] ISample sample, InnerSample item, int result)
    {
        sample.Check(item).SetupReturn(result);
        sample.Check(Tools.Duplicator.Copy(item)).Assert().Is(result);
        sample.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Issue038_FluentArgWorks([Stub] ISample sample, string item1, string item2, int result)
    {
        sample.Check(Arg.Any<string>(), Arg.Where<string>(s => s == item2)).SetupReturn(result);
        sample.Check(item2, item1).Assert().Is(0);
        sample.Check(item1, item2).Assert().Is(result);
        sample.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Issue038_FluentTimesCorrect([Stub] ISample sample, string value)
    {
        sample.Value.SetupReturn(value, Times.Exactly(2));
        sample.Value.Assert().Is(value);
        sample.Assert(s => s.Assert().Called()).Throws<FakeVerifyException>();
        sample.Value.Assert().Is(value);
        sample.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Issue038_FluentFakeCastWorks([Stub] ISample sample, string value, string identity)
    {
        sample.ToFake().Setup(d => d.Value, Behavior.Returns(value));
        sample.ToFake<object>().Setup(d => d.ToString(), Behavior.Returns(identity));

        sample.Value.Assert().Is(value);
        sample.ToString().Assert().Is(identity);

        sample.ToFake().ToFake<object>().VerifyAll();
    }
}