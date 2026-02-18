namespace CreateAndFake.Tests.IssueReplication;

public static class Issue043Tests
{
    [Fact]
    internal static Task Issue043_SupportsTask()
    {
        return TestSample<Task>();
    }

    [Fact]
    internal static Task Issue043_SupportsGenericTask()
    {
        return TestSample<Task<string>>();
    }

    private static async Task TestSample<T>()
    {
        CancellationToken ct = TestContext.Current.CancellationToken;
        for (int i = 0; i < 50; i++)
        {
            T sample = Tools.Randomizer.Create<T>();
            await Tools.Asserter.IsNotAsync(null, sample, ct);
            await Tools.Asserter.IsNotAsync(sample, Tools.Mutator.Variant(sample), ct);

            T dupe = Tools.Duplicator.Copy(sample);

            await Tools.Asserter.IsAsync(sample, dupe, ct);
            await Tools.Asserter.IsAsync(
                Tools.Valuer.GetHashCodeAsync(sample, ct),
                Tools.Valuer.GetHashCodeAsync(dupe, ct),
                ct
            );
        }
    }
}
