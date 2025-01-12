using CreateAndFake.Toolbox.AsserterTool;
using CreateAndFake.Toolbox.DuplicatorTool;
using CreateAndFake.Toolbox.ExtractorTool;
using CreateAndFake.Toolbox.FakerTool;
using CreateAndFake.Toolbox.MutatorTool;
using CreateAndFake.Toolbox.RandomizerTool;
using CreateAndFake.Toolbox.TesterTool;
using CreateAndFake.Toolbox.ValuerTool;

namespace CreateAndFakeTests.IssueReplication;

public static class Issue094Tests
{
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

    private static void TestToolBehavior<T>()
    {
        for (int i = 0; i < 10; i++)
        {
            T sample = Tools.Randomizer.Create<T>();
            Tools.Asserter.IsNot(null, sample);
            Tools.Asserter.IsNot(sample, Tools.Mutator.Variant(sample));

            T dupe = Tools.Duplicator.Copy(sample);

            Tools.Asserter.Is(sample, dupe);
            Tools.Asserter.Is(Tools.Valuer.GetHashCode(sample), Tools.Valuer.GetHashCode(dupe));
        }
    }
}