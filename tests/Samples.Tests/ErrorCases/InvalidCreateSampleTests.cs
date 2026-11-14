using Werecodent.CreateAndFake.Samples.ErrorCases;

namespace Werecodent.CreateAndFake.Samples.Tests.ErrorCases;

public static class InvalidCreateSampleTests
{
    [Fact]
    public static void InvalidCreateSample_CannotBeCreated()
    {
        true.Assert(_ => new InvalidCreateSample()).Throws<InvalidOperationException>();
    }
}
