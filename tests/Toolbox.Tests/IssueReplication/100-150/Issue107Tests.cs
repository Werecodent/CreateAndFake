using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Tooling;

namespace Werecodent.CreateAndFake.Tests.IssueReplication;

public static class Issue107Tests
{
    internal abstract class SelfReferenceContainer<T>
        where T : SelfReferenceContainer<T> { }

    [Fact]
    internal static void Issue107_SelfReferenceGenericMustHaveSubclass()
    {
        typeof(SelfReferenceContainer<>)
            .Assert(x => x.Tools().CreateRandomInstance())
            .Throws<ToolException>();
    }
}
