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
        Subclasser.Create<IFakeSample>(Tools.Faker.Options).Assert().IsNotNull();
        Subclasser
            .Create<IFakeSample>(Tools.Faker.Options, typeof(IClashingFakeSample))
            .Assert()
            .IsNotNull();
    }

    [Fact]
    internal static void Create_ClassesWork()
    {
        Subclasser.Create<AbstractFakeSample>(Tools.Faker.Options).Assert().IsNotNull();
        Subclasser.Create<VirtualFakeSample>(Tools.Faker.Options).Assert().IsNotNull();
    }

    [Fact]
    internal static void Create_BothWork()
    {
        Subclasser
            .Create<AbstractFakeSample>(Tools.Faker.Options, typeof(IFakeSample))
            .Assert()
            .IsNotNull();
        Subclasser
            .Create<VirtualFakeSample>(
                Tools.Faker.Options,
                typeof(IFakeSample),
                typeof(IClashingFakeSample)
            )
            .Assert()
            .IsNotNull();
    }

    [Fact]
    internal static void Create_IFakedDefault()
    {
        Subclasser.Create<object>(Tools.Faker.Options).GetType().Assert().Inherits<IFaked>();
        Subclasser.Create(null, Tools.Faker.Options, null).Assert().IsNotNull();
    }

    [Fact]
    internal static void Create_IFakedFunctional()
    {
        Subclasser.Create<IFaked>(Tools.Faker.Options).FakeMeta.Assert().IsNotNull();
    }

    [Fact]
    internal static void Create_OnlyMultipleInterfaces()
    {
        typeof(object)
            .Assert(x => Subclasser.Create<DataSample>(Tools.Faker.Options, x))
            .Throws<ArgumentException>();
    }

    [Fact]
    internal static void Create_SealedTypesThrow()
    {
        typeof(string)
            .Assert(x => Subclasser.Create(x, Tools.Faker.Options))
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
            .IsNotNull();
        Subclasser
            .Create<ConstraintSample<bool, DataSample>>(Tools.Faker.Options)
            .Assert()
            .IsNotNull();
    }

    [Fact]
    internal static void Create_UndefinedGenericsThrow()
    {
        typeof(ConstraintSample<,>)
            .Assert(x => Subclasser.Create(x, Tools.Faker.Options))
            .Throws<ArgumentException>();
    }

    [Fact]
    internal static void Create_PointersThrow()
    {
        typeof(void*)
            .Assert(x => Subclasser.Create(x, Tools.Faker.Options))
            .Throws<ArgumentException>();
    }

    [Fact]
    internal static void Create_InternalTypesThrow()
    {
        typeof(InternalSample)
            .Assert(x => Subclasser.Create(x, Tools.Faker.Options))
            .Throws<ArgumentException>();
    }

    [Fact]
    internal static void Supports_FalseWithWithNonVisibleTypes()
    {
        const TypeAttributes invisibleAttributes = TypeAttributes.NotPublic | TypeAttributes.Class;

        Type type = Tools.Faker.Stub<Type>().Dummy;

        type.Tools()
            .ToFake()
            .Setup("GetAttributeFlagsImpl", [], Behavior.Returns(invisibleAttributes));
        type.Tools().ToFake().Setup("HasElementTypeImpl", [], Behavior.Returns(false));
        type.Tools().ToFake().Setup("IsPointerImpl", [], Behavior.Returns(false));
        type.Assembly.SetupReturn(typeof(object).Assembly);
        type.Name.SetupReturn("TestInvisibleType");

        type.Assert(x => Subclasser.Create(x, Tools.Faker.Options))
            .Throws<ArgumentException>()
            .With.Message.Assert()
            .Contains("InternalsVisibleTo");
    }
}
