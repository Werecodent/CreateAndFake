using Werecodent.CreateAndFake.Design.Data;
using Werecodent.CreateAndFake.Design.Reiteration;

namespace Werecodent.CreateAndFake.Tests.IssueReplication;

public static class Issue021Tests
{
    internal sealed class Sample
    {
        public string FirstName { get; set; }
    }

    [Theory, RandomData]
    internal static void Issue021_MutatorMutates(Sample sample)
    {
        string original = sample.FirstName;
        Tools.Mutator.Modify(sample);
        sample.FirstName.Assert().IsNot(original);
    }

    [Theory, RandomData]
    internal static void Issue021_MutatorUsesSmartData(Sample sample)
    {
        Limiter.Few.Retry(
            "Retry if completely random name is used due to random smart name match.",
            () =>
            {
                Tools.Mutator.Modify(sample).Assert().Is(true);
                NameData.Values.Assert().Contains(sample.FirstName);
            }
        );
    }
}
