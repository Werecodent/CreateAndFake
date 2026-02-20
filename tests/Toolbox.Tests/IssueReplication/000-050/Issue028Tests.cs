namespace CreateAndFake.Tests.IssueReplication;

public static class Issue028Tests
{
    internal sealed class Sample
    {
        public int Value { get; set; }
    }

    [Fact]
    internal static void Issue028_ConditionalCreation()
    {
        Tools
            .Randomizer.Create<Sample>(opt =>
                opt with
                {
                    FinalCondition = r => r is Sample s && s.Value > 0,
                }
            )
            .Value.Assert()
            .GreaterThan(0);
    }
}
