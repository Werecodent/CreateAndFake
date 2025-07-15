using System.Reflection;
using CreateAndFake.ExtractorTool;
using CreateAndFake.Samples.OldSamples;

namespace CreateAndFake.Tests.ExtractorTool;

public static class ContentMapTests
{
    [Fact]
    internal static Task ContentMap_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<ContentMap>();
    }

    [Fact]
    internal static Task ContentMap_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<ContentMap>();
    }

    [Theory, RandomData]
    internal static void HasContent_UsesObjectByValue(DataHolderSample sample)
    {
        IContentMap map = Tools.Extractor.Extract(sample);
        map.HasContent(sample.NestedValue.CreateVariant()).Assert().Is(false);
        map.HasContent(sample.NestedValue.CreateDeepClone()).Assert().Is(true);
    }

    [Theory, RandomData]
    internal static void HasContent_UsesValueByValue(DataHolderSample sample)
    {
        IContentMap map = Tools.Extractor.Extract(sample);
        map.HasContent(sample.NestedValue.NumberValue.CreateVariant()).Assert().Is(false);
        map.HasContent(sample.NestedValue.NumberValue.CreateDeepClone()).Assert().Is(true);
    }

    [Theory, RandomData]
    internal static void HasContent_UsesStringByValue(DataHolderSample sample)
    {
        IContentMap map = Tools.Extractor.Extract(sample);
        map.HasContent(sample.NestedValue.StringValue.CreateVariant()).Assert().Is(false);
        map.HasContent(sample.NestedValue.StringValue.CreateDeepClone()).Assert().Is(true);
    }

    [Theory, RandomData]
    internal static void FindAll_ContainsNestedObjects(DataHolderSample sample)
    {
        IContentMap map = Tools.Extractor.Extract(sample);
        map.FindAll<DataSample>().Assert().Contains(sample.NestedValue);
        map.FindAll(typeof(DataSample)).Assert().Contains(sample.NestedValue);
    }

    [Theory, RandomData]
    internal static void FindAll_ContainsNestedValues(DataHolderSample sample)
    {
        IContentMap map = Tools.Extractor.Extract(sample);
        map.FindAll<int>().Assert().Contains(sample.NestedValue.NumberValue);
        map.FindAll(typeof(int)).Assert().Contains(sample.NestedValue.NumberValue);
    }

    [Theory, RandomData]
    internal static void AllContent_ContainsEverything(DataHolderSample sample)
    {
        IContentMap map = Tools.Extractor.Extract(sample);
        map.AllContent().Assert().Contains(sample.NestedValue);
        map.AllContent().Assert().Contains(sample.NestedValue.NumberValue);
    }

    [Theory, RandomData]
    internal static void FindSharedContent_ObjectValuesFound(DataHolderSample sample)
    {
        IContentMap map = Tools.Extractor.Extract(sample);
        IContentMap test = Tools.Extractor.Extract(sample.NestedValue);
        map.FindSharedContent(test).Assert().Contains(sample.NestedValue);
        map.HasSharedContent(test).Assert().Is(true);
    }

    [Theory, RandomData]
    internal static void FindSharedContent_ValueValuesFound(DataHolderSample sample)
    {
        IContentMap map = Tools.Extractor.Extract(sample);
        IContentMap test = Tools.Extractor.Extract(sample.NestedValue.NumberValue);
        map.FindSharedContent(test).Assert().Contains(sample.NestedValue.NumberValue);
        map.HasSharedContent(test).Assert().Is(true);
    }

    [Theory, RandomData]
    internal static void HasSharedContent_FalseWithNothingShared(string sample)
    {
        IContentMap map = Tools.Extractor.Extract(sample);
        IContentMap test = Tools.Extractor.Extract(sample.CreateVariant());
        map.HasSharedContent(test).Assert().Is(false);
    }

    [Theory, RandomData]
    internal static void HasSharedContent_IgnoresSimpleTypes(
        char sample1,
        bool sample2,
        BindingFlags sample3
    )
    {
        Tools
            .Extractor.Extract(sample1)
            .HasSharedContent(Tools.Extractor.Extract(sample1.CreateDeepClone()))
            .Assert()
            .Is(false);

        Tools
            .Extractor.Extract(sample2)
            .HasSharedContent(Tools.Extractor.Extract(sample2.CreateDeepClone()))
            .Assert()
            .Is(false);

        Tools
            .Extractor.Extract(sample3)
            .HasSharedContent(Tools.Extractor.Extract(sample3.CreateDeepClone()))
            .Assert()
            .Is(false);
    }

    [Theory, RandomData]
    internal static void FindSharedContent_EmptyCollectionsIgnored([Size(0)] object[] sample)
    {
        Tools
            .Extractor.Extract(sample)
            .FindSharedContent(Tools.Extractor.Extract(Array.Empty<object>()))
            .Assert()
            .IsEmpty();
    }
}
