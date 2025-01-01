using System.Reflection;
using CreateAndFake.Toolbox.MutatorTool;
using CreateAndFake.Toolbox.RandomizerTool.CreateHints;
using CreateAndFakeTests.TestSamples;

namespace CreateAndFakeTests.Toolbox.MutatorTool;

public static class ContentMapTests
{
    [Fact]
    internal static void ContentMap_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<ContentMap>();
    }

    [Fact]
    internal static void ContentMap_NoParameterMutation()
    {
        // Fix me: Tools.Tester.PreventsParameterMutation<ContentMap>();
    }

    [Theory, RandomData]
    internal static void HasContent_UsesObjectByValue(DataHolderSample sample)
    {
        ContentMap map = ContentMap.Extract(sample);
        map.HasContent(Tools.Valuer, sample.NestedValue.CreateVariant()).Assert().Is(false);
        map.HasContent(Tools.Valuer, sample.NestedValue.CreateDeepClone()).Assert().Is(true);
    }

    [Theory, RandomData]
    internal static void HasContent_UsesValueByValue(DataHolderSample sample)
    {
        ContentMap map = ContentMap.Extract(sample);
        map.HasContent(Tools.Valuer, sample.NestedValue.NumberValue.CreateVariant()).Assert().Is(false);
        map.HasContent(Tools.Valuer, sample.NestedValue.NumberValue.CreateDeepClone()).Assert().Is(true);
    }

    [Theory, RandomData]
    internal static void HasContent_UsesStringByValue(DataHolderSample sample)
    {
        ContentMap map = ContentMap.Extract(sample);
        map.HasContent(Tools.Valuer, sample.NestedValue.StringValue.CreateVariant()).Assert().Is(false);
        map.HasContent(Tools.Valuer, sample.NestedValue.StringValue.CreateDeepClone()).Assert().Is(true);
    }

    [Theory, RandomData]
    internal static void FindAll_ContainsNestedObjects(DataHolderSample sample)
    {
        ContentMap map = ContentMap.Extract(sample);
        map.FindAll<DataSample>().Assert().Contains(sample.NestedValue);
        map.FindAll(typeof(DataSample)).Assert().Contains(sample.NestedValue);
    }

    [Theory, RandomData]
    internal static void FindAll_ContainsNestedValues(DataHolderSample sample)
    {
        ContentMap map = ContentMap.Extract(sample);
        map.FindAll<int>().Assert().Contains(sample.NestedValue.NumberValue);
        map.FindAll(typeof(int)).Assert().Contains(sample.NestedValue.NumberValue);
    }

    [Theory, RandomData]
    internal static void AllContent_ContainsEverything(DataHolderSample sample)
    {
        ContentMap map = ContentMap.Extract(sample);
        map.AllContent().Assert().Contains(sample.NestedValue);
        map.AllContent().Assert().Contains(sample.NestedValue.NumberValue);
    }

    [Theory, RandomData]
    internal static void FindSharedContent_ObjectValuesFound(DataHolderSample sample)
    {
        ContentMap map = ContentMap.Extract(sample);
        ContentMap test = ContentMap.Extract(sample.NestedValue);
        map.FindSharedContent(Tools.Valuer, test).Assert().Contains(sample.NestedValue);
        map.HasSharedContent(Tools.Valuer, test).Assert().Is(true);
    }

    [Theory, RandomData]
    internal static void FindSharedContent_ValueValuesFound(DataHolderSample sample)
    {
        ContentMap map = ContentMap.Extract(sample);
        ContentMap test = ContentMap.Extract(sample.NestedValue.NumberValue);
        map.FindSharedContent(Tools.Valuer, test).Assert().Contains(sample.NestedValue.NumberValue);
        map.HasSharedContent(Tools.Valuer, test).Assert().Is(true);
    }

    [Theory, RandomData]
    internal static void HasSharedContent_FalseWithNothingShared(string sample)
    {
        ContentMap map = ContentMap.Extract(sample);
        ContentMap test = ContentMap.Extract(sample.CreateVariant());
        map.HasSharedContent(Tools.Valuer, test).Assert().Is(false);
    }

    [Fact]
    internal static void ExtractData_CollectionsWork()
    {
        foreach (Type type in CollectionCreateHint.PotentialCollections)
        {
            ContentMap.Extract(Tools.Randomizer.Create(type)).AllContent().Assert().IsNotEmpty();
        }
    }

    [Fact]
    internal static void ExtractData_LegacyCollectionsWork()
    {
        foreach (Type type in LegacyCollectionCreateHint.PotentialCollections)
        {
            ContentMap.Extract(Tools.Randomizer.Create(type)).AllContent().Assert().IsNotEmpty();
        }
    }

    [Theory, RandomData]
    internal static void HasSharedContent_IgnoresSimpleTypes(char sample1, bool sample2, BindingFlags sample3)
    {
        ContentMap
            .Extract(sample1)
            .HasSharedContent(Tools.Valuer, ContentMap.Extract(sample1.CreateDeepClone()))
            .Assert()
            .Is(false);

        ContentMap
            .Extract(sample2)
            .HasSharedContent(Tools.Valuer, ContentMap.Extract(sample2.CreateDeepClone()))
            .Assert()
            .Is(false);

        ContentMap
            .Extract(sample3)
            .HasSharedContent(Tools.Valuer, ContentMap.Extract(sample3.CreateDeepClone()))
            .Assert()
            .Is(false);
    }

    [Theory, RandomData]
    internal static void FindSharedContent_EmptyCollectionsIgnored([Size(0)] object[] sample)
    {
        ContentMap
            .Extract(sample)
            .FindSharedContent(Tools.Valuer, ContentMap.Extract(Array.Empty<object>()))
            .Assert()
            .IsEmpty();
    }
}
