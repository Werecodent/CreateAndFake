using System.Reflection;
using CreateAndFake.Design.Types;

namespace CreateAndFake.Design.Tests.Types;

#pragma warning disable IDE0032, IDE0051, RCS1170, RCS1213, S2376 // For testing.

public static class PropertyScannerTests
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
        public static int BaseStaticProp { get; set; } = 0;
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
        public static int StaticProp { get; set; } = 0;
        public double InternalGetterWithSet { internal get; set; }
        public double InternalSetterWithGet { get; internal set; }
        internal int PrivateGetterWithInternalSet { private get; set; }
        internal int PrivateSetterWithInternalGet { get; private set; }
    }

    [Fact]
    internal static Task PropertyScanner_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<PropertyScanner>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task PropertyScanner_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<PropertyScanner>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void All_PropertiesFound()
    {
        HashSet<PropertyInfo> expectedProperties =
        [
            typeof(BaseMembers).GetProperty("BasePrivateProp", _AllScope),
            typeof(Members).GetProperty(nameof(BaseMembers.InterfaceInternalGetter), _AllScope),
            typeof(Members).GetProperty(nameof(BaseMembers.InterfaceInternalSetter), _AllScope),
            typeof(Members).GetProperty(nameof(BaseMembers.InterfacePublicProp), _AllScope),
            typeof(Members).GetProperty(nameof(BaseMembers.BaseInternalGetter), _AllScope),
            typeof(Members).GetProperty(nameof(BaseMembers.BaseInternalSetter), _AllScope),
            typeof(Members).GetProperty(nameof(Members.InternalGetterWithSet), _AllScope),
            typeof(Members).GetProperty(nameof(Members.InternalSetterWithGet), _AllScope),
            typeof(Members).GetProperty(nameof(Members.PrivateGetterWithInternalSet), _AllScope),
            typeof(Members).GetProperty(nameof(Members.PrivateSetterWithInternalGet), _AllScope),
        ];

        new PropertyScanner(typeof(Members)).All.ToHashSet().Assert().Is(expectedProperties);
    }

    [Fact]
    internal static void PublicOrInternal_PropertiesFound()
    {
        HashSet<PropertyInfo> expectedProperties =
        [
            typeof(Members).GetProperty(nameof(BaseMembers.InterfaceInternalGetter), _AllScope),
            typeof(Members).GetProperty(nameof(BaseMembers.InterfaceInternalSetter), _AllScope),
            typeof(Members).GetProperty(nameof(BaseMembers.InterfacePublicProp), _AllScope),
            typeof(Members).GetProperty(nameof(BaseMembers.BaseInternalGetter), _AllScope),
            typeof(Members).GetProperty(nameof(BaseMembers.BaseInternalSetter), _AllScope),
            typeof(Members).GetProperty(nameof(Members.InternalGetterWithSet), _AllScope),
            typeof(Members).GetProperty(nameof(Members.InternalSetterWithGet), _AllScope),
            typeof(Members).GetProperty(nameof(Members.PrivateGetterWithInternalSet), _AllScope),
            typeof(Members).GetProperty(nameof(Members.PrivateSetterWithInternalGet), _AllScope),
        ];

        PropertyScanner scanner = new(typeof(Members));
        scanner.PublicOrInternal.ToHashSet().Assert().Is(expectedProperties);

        scanner.Visible.Assert().Is(scanner.PublicOrInternal);
    }

    [Fact]
    internal static void OnlyPublic_PropertiesFound()
    {
        HashSet<PropertyInfo> expectedProperties =
        [
            typeof(Members).GetProperty(nameof(BaseMembers.InterfaceInternalGetter), _AllScope),
            typeof(Members).GetProperty(nameof(BaseMembers.InterfaceInternalSetter), _AllScope),
            typeof(Members).GetProperty(nameof(BaseMembers.InterfacePublicProp), _AllScope),
            typeof(Members).GetProperty(nameof(Members.InternalGetterWithSet), _AllScope),
            typeof(Members).GetProperty(nameof(Members.InternalSetterWithGet), _AllScope),
        ];

        new PropertyScanner(typeof(Members)).OnlyPublic.ToHashSet().Assert().Is(expectedProperties);
    }

    [Fact]
    internal static void SetAndGetable_PropertiesFound()
    {
        HashSet<PropertyInfo> expectedProperties =
        [
            typeof(Members).GetProperty(nameof(BaseMembers.InterfaceInternalGetter), _AllScope),
            typeof(Members).GetProperty(nameof(BaseMembers.InterfaceInternalSetter), _AllScope),
            typeof(Members).GetProperty(nameof(BaseMembers.InterfacePublicProp), _AllScope),
            typeof(Members).GetProperty(nameof(Members.InternalGetterWithSet), _AllScope),
            typeof(Members).GetProperty(nameof(Members.InternalSetterWithGet), _AllScope),
        ];

        new PropertyScanner(typeof(Members))
            .SetAndGetable.ToHashSet()
            .Assert()
            .Is(expectedProperties);
    }

    [Fact]
    internal static void Settable_PropertiesFound()
    {
        HashSet<PropertyInfo> expectedProperties =
        [
            typeof(Members).GetProperty(nameof(BaseMembers.InterfaceInternalGetter), _AllScope),
            typeof(Members).GetProperty(nameof(BaseMembers.InterfaceInternalSetter), _AllScope),
            typeof(Members).GetProperty(nameof(BaseMembers.InterfacePublicProp), _AllScope),
            typeof(Members).GetProperty(nameof(BaseMembers.BaseInternalSetter), _AllScope),
            typeof(Members).GetProperty(nameof(Members.InternalGetterWithSet), _AllScope),
            typeof(Members).GetProperty(nameof(Members.InternalSetterWithGet), _AllScope),
            typeof(Members).GetProperty(nameof(Members.PrivateGetterWithInternalSet), _AllScope),
        ];

        new PropertyScanner(typeof(Members)).Settable.ToHashSet().Assert().Is(expectedProperties);
    }
}

#pragma warning restore
