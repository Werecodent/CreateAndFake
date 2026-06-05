using System.Reflection;
using CreateAndFake.FakerTool;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.Samples.Scenarios;
using CreateAndFake.Tests.FakerTool.TestSamples;

namespace CreateAndFake.Tests.FakerTool.Proxy;

public static class SubclasserTests
{
    [Fact]
    internal static void Create_InterfacesWork()
    {
        Subclasser.Create<IFakeSample>(Tools.Faker.Options).Assert().IsNot(null);
        Subclasser
            .Create<IFakeSample>(Tools.Faker.Options, typeof(IClashingFakeSample))
            .Assert()
            .IsNot(null);
    }

    [Fact]
    internal static void Create_ClassesWork()
    {
        Subclasser.Create<AbstractFakeSample>(Tools.Faker.Options).Assert().IsNot(null);
        Subclasser.Create<VirtualFakeSample>(Tools.Faker.Options).Assert().IsNot(null);
    }

    [Fact]
    internal static void Create_BothWork()
    {
        Subclasser
            .Create<AbstractFakeSample>(Tools.Faker.Options, typeof(IFakeSample))
            .Assert()
            .IsNot(null);
        Subclasser
            .Create<VirtualFakeSample>(
                Tools.Faker.Options,
                typeof(IFakeSample),
                typeof(IClashingFakeSample)
            )
            .Assert()
            .IsNot(null);
    }

    [Fact]
    internal static void Create_IFakedDefault()
    {
        Subclasser.Create<object>(Tools.Faker.Options).GetType().Assert().Inherits<IFaked>();
        Subclasser.Create(null, Tools.Faker.Options, null).Assert().IsNot(null);
    }

    [Fact]
    internal static void Create_IFakedFunctional()
    {
        Subclasser.Create<IFaked>(Tools.Faker.Options).FakeMeta.Assert().IsNot(null);
    }

    [Fact]
    internal static void Create_OnlyMultipleInterfaces()
    {
        typeof(object)
            .Assert(t => Subclasser.Create<DataSample>(Tools.Faker.Options, t))
            .Throws<ArgumentException>();
    }

    [Fact]
    internal static void Create_SealedTypesThrow()
    {
        typeof(string)
            .Assert(t => Subclasser.Create(t, Tools.Faker.Options))
            .Throws<ArgumentException>();
    }

    [Fact]
    internal static void CreateInfo_NoDuplicatesCreated()
    {
        Subclasser
            .CreateInfo(typeof(IFakeSample), typeof(IClashingFakeSample))
            .Assert()
            .ReferenceEqual(
                Subclasser.CreateInfo(typeof(IClashingFakeSample), typeof(IFakeSample))
            );
    }

    [Fact]
    internal static void CreateInfo_IgnoreDupeInterfaces()
    {
        Subclasser
            .CreateInfo(typeof(IFakeSample))
            .Assert()
            .ReferenceEqual(Subclasser.CreateInfo(typeof(IFakeSample), typeof(IFakeSample)));
    }

    [Fact]
    internal static void Create_DefinedGenericsWork()
    {
        Subclasser
            .Create<ConstraintSample<int, DataSample>>(Tools.Faker.Options)
            .Assert()
            .IsNot(null);
        Subclasser
            .Create<ConstraintSample<bool, DataSample>>(Tools.Faker.Options)
            .Assert()
            .IsNot(null);
    }

    [Fact]
    internal static void Create_UndefinedGenericsThrow()
    {
        typeof(ConstraintSample<,>)
            .Assert(t => Subclasser.Create(t, Tools.Faker.Options))
            .Throws<ArgumentException>();
    }

    [Fact]
    internal static void Create_PointersThrow()
    {
        typeof(void*)
            .Assert(t => Subclasser.Create(t, Tools.Faker.Options))
            .Throws<ArgumentException>();
    }

    [Fact]
    internal static void Create_InternalTypesThrow()
    {
        typeof(InternalSample)
            .Assert(t => Subclasser.Create(t, Tools.Faker.Options))
            .Throws<ArgumentException>();
    }

    [Fact]
    internal static void Supports_FalseWithWithNonVisibleTypes()
    {
        const TypeAttributes invisibleAttributes = TypeAttributes.NotPublic | TypeAttributes.Class;

        Type type = Tools.Faker.Stub<Type>().Dummy;

        type.ToFake().Setup("GetAttributeFlagsImpl", [], Behavior.Returns(invisibleAttributes));
        type.ToFake().Setup("HasElementTypeImpl", [], Behavior.Returns(false));
        type.ToFake().Setup("IsPointerImpl", [], Behavior.Returns(false));
        type.Assembly.SetupReturn(typeof(object).Assembly);
        type.Name.SetupReturn("TestInvisibleType");

        type.Assert(t => Subclasser.Create(t, Tools.Faker.Options))
            .Throws<ArgumentException>()
            .Exception.Message.Assert()
            .Contains("InternalsVisibleTo");
    }
}
