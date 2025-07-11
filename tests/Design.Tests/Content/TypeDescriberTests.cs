using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Tests.Content;

public static class TypeDescriberTests
{
    [Fact]
    internal static Task TypeDescriber_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<TypeDescriber>();
    }
}
