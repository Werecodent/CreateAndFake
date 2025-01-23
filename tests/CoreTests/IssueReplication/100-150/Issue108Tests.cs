namespace CreateAndFakeTests.IssueReplication;

public static class Issue108Tests
{
    public abstract class InnerStuff<T> where T : InnerStuff<T>
    {
        public string Message { get; set; }
    }

    public sealed class Wrapper : InnerStuff<Wrapper>
    {
        public int Value { get; set; }
    }

    public class Container<T> where T : InnerStuff<T>
    {
        public T Content { get; set; }
    }

    public sealed class WrapperContainer : Container<Wrapper> { }

    [Fact]
    internal static void Issue108_SupportsSelfReference()
    {
        typeof(InnerStuff<>).CreateRandomInstance().Assert().IsNot(null);
    }

    [Theory, RandomData]
    internal static void Issue108_SupportsWrappingSelfReference(Wrapper instance)
    {
        instance.Assert().IsNot(null);
    }

    [Fact]
    internal static void Issue108_SupportsContainerWithSelfReferenceContent()
    {
        typeof(Container<>).CreateRandomInstance().Assert().IsNot(null);
    }

    [Theory, RandomData]
    internal static void Issue108_SupportsContainerWithWrappedSelfReference(Container<Wrapper> instance)
    {
        instance.Assert().IsNot(null);
    }

    [Theory, RandomData]
    internal static void Issue108_SupportsWrappedContainer(WrapperContainer instance)
    {
        instance.Assert().IsNot(null);
    }
}