using System.Reflection;
using CreateAndFake.Design.Comparisons;
using CreateAndFake.Design.Types;
using CreateAndFake.Samples.Scenarios;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.Design.Tests.Types;

#pragma warning disable IDE0032, IDE0051, RCS1170, RCS1213, S1144, S2376 // For testing.

public static class InheritanceTrackerTests
{
    private const BindingFlags _AllScope =
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    internal interface IScopeMembers
    {
        string InterfaceInternalGetter { internal get; set; }
        string InterfaceInternalSetter { get; internal set; }
        int InterfacePublicProp { get; set; }
    }

    internal abstract class BaseMembers : IScopeMembers
    {
        private int _privateSetterBacking = 0;
        internal string _baseInternalField = "";
        internal double _basePublicField = 0;
        public string InterfaceInternalGetter { get; set; }
        public string InterfaceInternalSetter { get; set; }
        public int InterfacePublicProp { get; set; }
        internal int BaseInternalGetter => _privateSetterBacking;
        internal int BaseInternalSetter
        {
            set => _privateSetterBacking = value;
        }
        private string BasePrivateProp { get; set; } = "";
    }

    internal sealed class Members : BaseMembers
    {
        public static int _StaticField = 0;
        public static int StaticProp { get; set; } = 0;
        public string _internalField = "";
        public double InternalGetterWithSet { internal get; set; }
        public double InternalSetterWithGet { get; internal set; }
        internal int PrivateGetterWithInternalSet { private get; set; }
        internal int PrivateSetterWithInternalGet { get; private set; }
    }

    [Fact]
    internal static Task InheritanceTracker_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<InheritanceTracker>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task InheritanceTracker_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<InheritanceTracker>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void Properties_FindsAllInstanceProperties()
    {
        HashSet<PropertyInfo> expectedProperties =
        [
            typeof(Members).GetProperty(nameof(BaseMembers.InterfaceInternalGetter), _AllScope),
            typeof(Members).GetProperty(nameof(BaseMembers.InterfaceInternalSetter), _AllScope),
            typeof(Members).GetProperty(nameof(BaseMembers.InterfacePublicProp), _AllScope),
            typeof(Members).GetProperty(nameof(BaseMembers.BaseInternalGetter), _AllScope),
            typeof(Members).GetProperty(nameof(BaseMembers.BaseInternalSetter), _AllScope),
            typeof(BaseMembers).GetProperty("BasePrivateProp", _AllScope),
            typeof(Members).GetProperty(nameof(Members.InternalGetterWithSet), _AllScope),
            typeof(Members).GetProperty(nameof(Members.InternalSetterWithGet), _AllScope),
            typeof(Members).GetProperty(nameof(Members.PrivateGetterWithInternalSet), _AllScope),
            typeof(Members).GetProperty(nameof(Members.PrivateSetterWithInternalGet), _AllScope),
        ];

        InheritanceTracker.For<Members>().Properties.All.Assert().Is(expectedProperties);
    }

    [Fact]
    internal static void Fields_FindsAllInstanceFields()
    {
        HashSet<FieldInfo> expectedNonAutoBackingFields =
        [
            typeof(BaseMembers).GetField("_privateSetterBacking", _AllScope),
            typeof(Members).GetField(nameof(BaseMembers._baseInternalField), _AllScope),
            typeof(Members).GetField(nameof(BaseMembers._basePublicField), _AllScope),
            typeof(Members).GetField(nameof(Members._internalField), _AllScope),
        ];

        IEnumerable<FieldInfo> fields = InheritanceTracker.For<Members>().Fields.All;

        expectedNonAutoBackingFields.Except(fields).Assert().IsEmpty();
        fields.Assert().HasCount(expectedNonAutoBackingFields.Count + 8);
    }

    [Fact]
    internal static void GetPublicFields_FindsPublicFields()
    {
        InheritanceTracker.For<FieldSample>().Fields.OnlyPublic.Assert().IsNotEmpty();
    }

    [Fact]
    internal static void GetAllFields_FindsInheritedPrivates()
    {
        InheritanceTracker.For<InheritedPrivatesSample>().Fields.OnlyPublic.Assert().IsEmpty();
        InheritanceTracker.For<InheritedPrivatesSample>().Fields.All.Assert().IsNotEmpty();
    }

    [Fact]
    internal static void GetPublicProperties_FindsPublicProperties()
    {
        InheritanceTracker.For<DataSample>().Properties.OnlyPublic.Assert().IsNotEmpty();
    }

    [Fact]
    internal static void GetAllProperties_FindsInheritedPrivates()
    {
        InheritanceTracker.For<InheritedPrivatesSample>().Properties.OnlyPublic.Assert().IsEmpty();
        InheritanceTracker.For<InheritedPrivatesSample>().Properties.All.Assert().IsNotEmpty();
    }

    [Fact]
    internal static void For_Cached()
    {
        InheritanceTracker.For<string>().Assert().ReferenceEqual(InheritanceTracker.For<string>());
    }

    [Fact]
    internal static void For_NullCachedAsEmpty()
    {
        InheritanceTracker nullTracker = InheritanceTracker.For(null);
        InheritanceTracker.For(null).Assert().ReferenceEqual(nullTracker);
        nullTracker.InheritedTypes.Assert().IsEmpty();
    }

    [Fact]
    internal static void Inherits_IncludesGenerics()
    {
        InheritanceTracker collection = InheritanceTracker.For<List<int>>();
        collection.Inherits<List<string>>().Assert().Is(false);
        collection.Inherits<IEnumerable<string>>().Assert().Is(false);
        collection.Inherits<IEnumerable<int>>().Assert().Is(true);
    }

    [Fact]
    internal static void Inherits_IncludesGenericBases()
    {
        InheritanceTracker collection = InheritanceTracker.For(typeof(IList<>));
        collection.Inherits<IEnumerable<string>>().Assert().Is(false);
        collection.Inherits<List<int>>().Assert().Is(false);
        collection.Inherits(typeof(List<>)).Assert().Is(false);
        collection.Inherits(typeof(IEnumerable<>)).Assert().Is(true);
    }

    [Fact]
    internal static void Inherits_SelfIncluded()
    {
        InheritanceTracker.For<string>().Inherits<string>().Assert().Is(true);
    }

    [Fact]
    internal static void FindLocalSubclasses_ExcludesOtherAssemblies()
    {
        InheritanceTracker
            .For<IValuerEquatable>()
            .FindLocalSubclasses()
            .Assert()
            .ContainsNot(typeof(PrivateValuerEquatableSample));
    }

    [Fact]
    internal static void FindLocalSubclasses_SelfIncluded()
    {
        InheritanceTracker
            .For<ValueComparer>()
            .FindLocalSubclasses()
            .Assert()
            .Contains(typeof(ValueComparer));
    }

    [Fact]
    internal static void FindLoadedSubclasses_IncludesFromDifferentAssemblies()
    {
        InheritanceTracker
            .For<IValuerEquatable>()
            .FindLoadedSubclasses()
            .Assert()
            .Contains(typeof(PrivateValuerEquatableSample));
    }

    [Fact]
    internal static void FindLoadedSubclasses_SelfIncluded()
    {
        InheritanceTracker
            .For<ValueComparer>()
            .FindLoadedSubclasses()
            .Assert()
            .Contains(typeof(ValueComparer));
    }
}

#pragma warning restore IDE0032, IDE0051, RCS1170, RCS1213, S1144, S2376
