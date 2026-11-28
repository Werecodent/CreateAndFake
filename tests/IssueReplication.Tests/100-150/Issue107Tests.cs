using Werecodent.CreateAndFake.Design.Exceptions;

namespace Werecodent.CreateAndFake.IssueReplication.Tests;

#pragma warning disable S2094 // For testing.

public static class Issue107Tests
{
    internal abstract class SelfReferenceContainer<T>
        where T : SelfReferenceContainer<T>;

    [Fact]
    internal static void Issue107_SelfReferenceGenericMustHaveSubclass()
    {
        typeof(SelfReferenceContainer<>)
            .Assert(x => x.Tools().CreateRandomInstance())
            .Throws<ToolException>();

        typeof(SelfReferenceContainer<>)
            .Assert(x => Tools.Randomizer.Inject(x, []))
            .Throws<ToolException>();
    }
}

#pragma warning restore
