using System.Reflection;
using CreateAndFake.Toolbox.FakerTool;
using CreateAndFake.Toolbox.FakerTool.Proxy;
using CreateAndFakeTests.TestSamples;
using CreateAndFakeTests.Toolbox.FakerTool.TestSamples;

namespace CreateAndFakeTests.Toolbox.FakerTool.Proxy;

public static class SubclasserTests
{
    [Fact]
    internal static void Subclasser_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException(typeof(Subclasser));
    }

    [Fact]
    internal static void Subclasser_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation(typeof(Subclasser));
    }

    [Fact]
    internal static void Create_InterfacesWork()
    {
        Subclasser.Create<IFakeSample>().Assert().IsNot(null);
        Subclasser.Create<IFakeSample>(typeof(IClashingFakeSample)).Assert().IsNot(null);
    }

    [Fact]
    internal static void Create_ClassesWork()
    {
        Subclasser.Create<AbstractFakeSample>().Assert().IsNot(null);
        Subclasser.Create<VirtualFakeSample>().Assert().IsNot(null);
    }

    [Fact]
    internal static void Create_BothWork()
    {
        Subclasser.Create<AbstractFakeSample>(typeof(IFakeSample)).Assert().IsNot(null);
        Subclasser.Create<VirtualFakeSample>(typeof(IFakeSample), typeof(IClashingFakeSample)).Assert().IsNot(null);
    }

    [Fact]
    internal static void Create_IFakedDefault()
    {
        Subclasser.Create<object>().GetType().Assert().Inherits<IFaked>();
        Subclasser.Create(null, null).Assert().IsNot(null);
    }

    [Fact]
    internal static void Create_IFakedFunctional()
    {
        Subclasser.Create<IFaked>().FakeMeta.Assert().IsNot(null);
    }

    [Fact]
    internal static void Create_OnlyMultipleInterfaces()
    {
        typeof(object).Assert(t => Subclasser.Create<DataSample>(t)).Throws<ArgumentException>();
    }

    [Fact]
    internal static void Create_SealedTypesThrow()
    {
        typeof(string).Assert(t => Subclasser.Create(t)).Throws<ArgumentException>();
    }

    [Fact]
    internal static void CreateInfo_NoDuplicatesCreated()
    {
        Subclasser.CreateInfo(typeof(IFakeSample), typeof(IClashingFakeSample)).Assert().ReferenceEqual(
            Subclasser.CreateInfo(typeof(IClashingFakeSample), typeof(IFakeSample)));
    }

    [Fact]
    internal static void CreateInfo_IgnoreDupeInterfaces()
    {
        Subclasser.CreateInfo(typeof(IFakeSample)).Assert().ReferenceEqual(
            Subclasser.CreateInfo(typeof(IFakeSample), typeof(IFakeSample)));
    }

    [Fact]
    internal static void Create_DefinedGenericsWork()
    {
        Subclasser.Create<ConstraintSample<int, DataSample>>().Assert().IsNot(null);
        Subclasser.Create<ConstraintSample<bool, DataSample>>().Assert().IsNot(null);
    }

    [Fact]
    internal static void Create_UndefinedGenericsThrow()
    {
        typeof(ConstraintSample<,>).Assert(t => Subclasser.Create(t)).Throws<ArgumentException>();
    }

    [Fact]
    internal static void Create_PointersThrow()
    {
        typeof(void*).Assert(t => Subclasser.Create(t)).Throws<ArgumentException>();
    }

    [Fact]
    internal static void Create_InternalTypesThrow()
    {
        typeof(InternalSample).Assert(t => Subclasser.Create(t)).Throws<ArgumentException>();
    }

    [Fact]
    internal static void Supports_FalseWithWithNonVisibleTypes()
    {
        TypeAttributes invisibleAttributes
            = TypeAttributes.NotPublic
            | TypeAttributes.Class;

        Type type = Tools.Faker.Stub<Type>().Dummy;

        type.ToFake().Setup("GetAttributeFlagsImpl", [], Behavior.Returns(invisibleAttributes));
        type.ToFake().Setup("HasElementTypeImpl", [], Behavior.Returns(false));
        type.ToFake().Setup("IsPointerImpl", [], Behavior.Returns(false));
        type.Assembly.SetupReturn(typeof(object).Assembly);
        type.Name.SetupReturn("TestInvisibleType");

        type.Assert(t => Subclasser.Create(t))
            .Throws<ArgumentException>().Message
            .Assert().Contains("InternalsVisibleTo");
    }
}
