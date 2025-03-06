using CreateAndFake.AsserterTool;

namespace CreateAndFake.Tests.AsserterTool;

public static class AsserterTypeTests
{
    private interface IParentType : IChildType { }

    private interface IChildType { }

    [Fact]
    internal static void Inherits_ParentToChild()
    {
        typeof(IParentType).Assert().Inherits<IChildType>().And.Inherits(typeof(IChildType));
    }

    [Theory, RandomData]
    internal static void Inherits_ParentToChildWithOptions(AsserterMod mod)
    {
        typeof(IParentType)
            .Assert()
            .Inherits<IChildType>(mod)
            .And.Inherits(typeof(IChildType), mod);
    }

    [Fact]
    internal static void Inherits_ChildToParent()
    {
        typeof(IChildType)
            .Assert(t => t.Assert().Inherits<IParentType>())
            .Throws<AssertException>();
        typeof(IChildType)
            .Assert(t => t.Assert().Inherits(typeof(IParentType)))
            .Throws<AssertException>();
    }

    [Fact]
    internal static void InheritedBy_ChildToParent()
    {
        typeof(IChildType).Assert().InheritedBy<IParentType>().And.InheritedBy(typeof(IParentType));
    }

    [Fact]
    internal static void InheritedBy_ParentToChild()
    {
        typeof(IParentType)
            .Assert(t => t.Assert().InheritedBy<IChildType>())
            .Throws<AssertException>();
        typeof(IParentType)
            .Assert(t => t.Assert().InheritedBy(typeof(IChildType)))
            .Throws<AssertException>();
    }
}
