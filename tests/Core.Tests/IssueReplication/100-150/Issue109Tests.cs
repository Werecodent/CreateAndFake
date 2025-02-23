using CreateAndFake.DuplicatorTool;
using CreateAndFake.DuplicatorTool.CopyHints;

#pragma warning disable CA2227 // Change collection to read-only property setter: For testing.

namespace CreateAndFake.Tests.IssueReplication;

public static class Issue109Tests
{
    public interface IDetails
    {
        string Name { get; set; }
    }

    [Serializable]
    public sealed class Details : IDetails
    {
        public string Name { get; set; }
        public int Year { get; set; }

        public Details() { }
    }

    public interface IContainer
    {
        IDetails Item { get; set; }
    }

    [Serializable]
    public sealed class Container : IContainer
    {
        public IDetails Item { get; set; }

        private List<IDetails> Values { get; set; }

        public Dictionary<IDetails, List<IDetails>> Map { get; set; }

        public Container() { }

        public override string ToString()
        {
            return Values?.ToString() + Map?.ToString();
        }
    }

    [Theory, RandomData]
    internal static void Issue109_BinarySerializationRemovedDateTime(DateTime date)
    {
        date.CreateDeepClone().Assert().Is(date);
    }

    [Theory, RandomData]
    internal static void Issue109_BinarySerializationRemovedSample(IContainer sample)
    {
        sample.CreateDeepClone().Assert().Is(sample);
        new Duplicator(Tools.Duplicator.Options with { Hints = [new SerializableCopyHint()] })
            .Copy(sample)
            .Assert()
            .Is(sample);
    }
}

#pragma warning restore CA2227
