using System.Reflection;
using CreateAndFake.AsserterTool;
using CreateAndFake.DuplicatorTool;
using CreateAndFake.ExtractorTool;
using CreateAndFake.FakerTool;
using CreateAndFake.MutatorTool;
using CreateAndFake.RandomizerTool;
using CreateAndFake.TesterTool;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.Tests.IssueReplication;

public static class Issue094Tests
{
    public sealed class Wrapped(IEnumerable<object> data)
    {
        public IEnumerable<object> Data { get; } = [.. data];
    }

    [Theory, RandomData]
    internal static void Issue094_StringSizeModifiedBySizeAttribute([Size(20)] string data)
    {
        data.Assert().HasCount(20);
    }

    [Theory, RandomData]
    internal static void Issue094_SizeAttributeOnlyTopCollection([Size(20)] IEnumerable<string> data)
    {
        data.Assert().HasCount(20);
        data.First().Length.Assert().IsNot(20);
    }

    [Fact]
    internal static void Issue094_AsserterOptionsWorks()
    {
        TestToolBehavior<AsserterOptions>();
        TestToolBehavior<AsserterMod>();
    }

    [Fact]
    internal static void Issue094_DuplicatorOptionsWorks()
    {
        TestToolBehavior<DuplicatorOptions>();
        TestToolBehavior<DuplicatorMod>();
    }

    [Fact]
    internal static void Issue094_FakerOptionsWorks()
    {
        TestToolBehavior<FakerOptions>();
        TestToolBehavior<FakerMod>();
    }

    [Fact]
    internal static void Issue094_MutatorOptionsWorks()
    {
        TestToolBehavior<MutatorOptions>();
        TestToolBehavior<MutatorMod>();
    }

    [Fact]
    internal static void Issue094_ExtractorOptionsWorks()
    {
        TestToolBehavior<ExtractorOptions>();
        TestToolBehavior<ExtractorMod>();
    }

    [Fact]
    internal static void Issue094_RandomizerOptionsWorks()
    {
        TestToolBehavior<RandomizerOptions>();
        TestToolBehavior<RandomizerMod>();
    }

    [Fact]
    internal static void Issue094_TesterOptionsWorks()
    {
        TestToolBehavior<TesterOptions>();
        TestToolBehavior<TesterMod>();
    }

    [Fact]
    internal static void Issue094_ValuerOptionsWorks()
    {
        TestToolBehavior<ValuerOptions>();
        TestToolBehavior<ValuerMod>();
    }

    [Fact]
    internal static void Issue094_AsserterWorks()
    {
        TestToolBehavior<IAsserter>();
        TestToolBehavior<Asserter>();
    }

    [Fact]
    internal static void Issue094_DuplicatorWorks()
    {
        TestToolBehavior<IDuplicator>();
        TestToolBehavior<Duplicator>();
    }

    [Fact]
    internal static void Issue094_FakerWorks()
    {
        TestToolBehavior<IFaker>();
        TestToolBehavior<Faker>();
    }

    [Fact]
    internal static void Issue094_MutatorWorks()
    {
        TestToolBehavior<IMutator>();
        TestToolBehavior<Mutator>();
    }

    [Fact]
    internal static void Issue094_ExtractorWorks()
    {
        TestToolBehavior<IExtractor>();
        TestToolBehavior<Extractor>();
    }

    [Fact]
    internal static void Issue094_RandomizerWorks()
    {
        TestToolBehavior<IRandomizer>();
        TestToolBehavior<Randomizer>();
    }

    [Fact]
    internal static void Issue094_TesterWorks()
    {
        TestToolBehavior<ITester>();
        TestToolBehavior<Tester>();
    }

    [Fact]
    internal static void Issue094_ValuerWorks()
    {
        TestToolBehavior<IValuer>();
        TestToolBehavior<Valuer>();
    }

    [Fact]
    internal static void Issue094_IntPtrWorks()
    {
        TestToolBehavior<IntPtr>();
    }

    [Fact]
    internal static void Issue094_RuntimeArrayWorks()
    {
        TestToolBehavior<Wrapped>();
    }

    [Fact]
    internal static void Issue094_MemberInfoWorks()
    {
        TestToolBehavior<MemberInfo>();
    }

    [Fact]
    internal static void Issue094_SpanWorks()
    {
        TestToolBehavior(typeof(Span<>));
    }

    [Fact]
    internal static void Issue094_ValueTupleWorks()
    {
        TestToolBehavior(typeof(ValueTuple<,>));
    }

    private static void TestToolBehavior<T>()
    {
        TestToolBehavior(typeof(T));
    }

    private static void TestToolBehavior(Type type)
    {
        for (int i = 0; i < 10; i++)
        {
            object sample = Tools.Randomizer.Create(type);
            Tools.Asserter.IsNot(null, sample);
            Tools.Asserter.IsNot(sample, Tools.Mutator.Variant(sample));

            object dupe = Tools.Duplicator.Copy(sample);

            Tools.Asserter.Is(sample, dupe);
            Tools.Asserter.Is(Tools.Valuer.GetHashCode(sample), Tools.Valuer.GetHashCode(dupe), $"{sample}");
        }
    }
}