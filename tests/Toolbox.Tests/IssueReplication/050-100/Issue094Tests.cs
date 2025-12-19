using System.Collections.Specialized;
using System.Globalization;
using System.Reflection;
using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Tooling;
using CreateAndFake.DuplicatorTool;
using CreateAndFake.ExtractorTool;
using CreateAndFake.FakerTool;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.MutatorTool;
using CreateAndFake.RandomizerTool;
using CreateAndFake.RunnerTool;
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
    internal static void Issue094_SizeAttributeOnlyTopCollection(
        [Size(20)] IEnumerable<string> data
    )
    {
        data.Assert().HasCount(20);
        data.First().Length.Assert().IsNot(20);
    }

    [Fact]
    internal static async Task Issue094_AsserterOptionsWorks()
    {
        await TestToolBehavior<AsserterOptions>();
        await TestToolBehavior<AsserterMod>();
    }

    [Fact]
    internal static async Task Issue094_DuplicatorOptionsWorks()
    {
        await TestToolBehavior<DuplicatorOptions>();
        await TestToolBehavior<DuplicatorMod>();
    }

    [Fact]
    internal static async Task Issue094_FakerOptionsWorks()
    {
        await TestToolBehavior<FakerOptions>();
        await TestToolBehavior<FakerMod>();
    }

    [Fact]
    internal static async Task Issue094_MutatorOptionsWorks()
    {
        await TestToolBehavior<MutatorOptions>();
        await TestToolBehavior<MutatorMod>();
    }

    [Fact]
    internal static async Task Issue094_ExtractorOptionsWorks()
    {
        await TestToolBehavior<ExtractorOptions>();
        await TestToolBehavior<ExtractorMod>();
    }

    [Fact]
    internal static async Task Issue094_RandomizerOptionsWorks()
    {
        await TestToolBehavior<RandomizerOptions>();
        await TestToolBehavior<RandomizerMod>();
    }

    [Fact]
    internal static async Task Issue094_TesterOptionsWorks()
    {
        await TestToolBehavior<TesterOptions>();
        await TestToolBehavior<TesterMod>();
    }

    [Fact]
    internal static async Task Issue094_ValuerOptionsWorks()
    {
        await TestToolBehavior<ValuerOptions>();
        await TestToolBehavior<ValuerMod>();
    }

    [Fact]
    internal static async Task Issue094_AsserterWorks()
    {
        await TestToolBehavior<IAsserter>();
        await TestToolBehavior<Asserter>();
    }

    [Fact]
    internal static async Task Issue094_DuplicatorWorks()
    {
        await TestToolBehavior<IDuplicator>();
        await TestToolBehavior<Duplicator>();
    }

    [Fact]
    internal static async Task Issue094_FakerWorks()
    {
        await TestToolBehavior<IFaker>();
        await TestToolBehavior<Faker>();
        await TestToolBehavior<FakeMetaProvider>();
    }

    [Fact]
    internal static async Task Issue094_MutatorWorks()
    {
        await TestToolBehavior<IMutator>();
        await TestToolBehavior<Mutator>();
    }

    [Fact]
    internal static async Task Issue094_ExtractorWorks()
    {
        await TestToolBehavior<IExtractor>();
        await TestToolBehavior<Extractor>();
    }

    [Fact]
    internal static async Task Issue094_RandomizerWorks()
    {
        await TestToolBehavior<IRandomizer>();
        await TestToolBehavior<Randomizer>();
    }

    [Fact]
    internal static async Task Issue094_TesterWorks()
    {
        await TestToolBehavior<ITester>();
        await TestToolBehavior<Tester>();
    }

    [Fact]
    internal static async Task Issue094_ValuerWorks()
    {
        await TestToolBehavior<IValuer>();
        await TestToolBehavior<Valuer>();
    }

    [Fact]
    internal static Task Issue094_IntPtrWorks()
    {
        return TestToolBehavior<IntPtr>();
    }

    [Fact]
    internal static Task Issue094_RuntimeArrayWorks()
    {
        return TestToolBehavior<Wrapped>();
    }

    [Fact]
    internal static async Task Issue094_MemberInfoWorks()
    {
        await TestToolBehavior<MemberInfo>();
        await TestToolBehavior<MethodBase>();
        await TestToolBehavior<ConstructorInfo>();
        await TestToolBehavior<MethodInfo>();
        await TestToolBehavior<PropertyInfo>();
        await TestToolBehavior<FieldInfo>();
        await TestToolBehavior<ParameterInfo>();
    }

    [Fact]
    internal static Task Issue094_SpanWorks()
    {
        return TestToolBehavior(typeof(Span<>));
    }

    [Fact]
    internal static Task Issue094_ValueTupleWorks()
    {
        return TestToolBehavior(typeof(ValueTuple<,>));
    }

    [Fact]
    internal static async Task Issue094_FormatProviderWorks()
    {
        await TestToolBehavior<IFormatProvider>();
        await TestToolBehavior<CultureInfo>();
        await TestToolBehavior<DateTimeFormatInfo>();
        await TestToolBehavior<NumberFormatInfo>();

        foreach (IFormatProvider provider in CultureInfo.GetCultures(CultureTypes.AllCultures))
        {
            provider.CreateDeepClone().Assert().Is(provider);
        }
    }

    [Fact]
    internal static async Task Issue094_FakedWorks()
    {
        await TestToolBehavior(typeof(IFaked));
        await TestToolBehavior(typeof(Fake<object>));
    }

    [Fact]
    internal static Task Issue094_MethodCallWrapperWorks()
    {
        return TestToolBehavior(typeof(MethodCallWrapper));
    }

    [Theory, RandomData]
    internal static void Issue094_MethodCallWrapperWithFakesWorks(
        MethodBase method,
        Fake<object> fake
    )
    {
        OrderedDictionary dict = new OrderedDictionary();
        dict.Add("test", fake);

        MethodCallWrapper wrapper = new MethodCallWrapper(method, dict);
        wrapper.Assert().Is(wrapper.CreateDeepClone());
    }

    private static Task TestToolBehavior<T>()
    {
        return TestToolBehavior(typeof(T));
    }

    private static async Task TestToolBehavior(Type type)
    {
        for (int i = 0; i < 10; i++)
        {
            object sample = Tools.Randomizer.Create(type);
            Tools.Asserter.IsNot(null, sample);
            Tools.Asserter.IsNot(sample, Tools.Mutator.Variant(sample));

            object dupe = Tools.Duplicator.Copy(sample);

            await Tools.AsyncAsserter.IsAsync(sample, dupe);
            Tools.Asserter.Is(sample, dupe);
        }
    }
}
