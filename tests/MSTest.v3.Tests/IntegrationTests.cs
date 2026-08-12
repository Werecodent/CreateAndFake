using System.Reflection;
using Werecodent.CreateAndFake.Design.Randomization;
using Werecodent.CreateAndFake.FakerTool;

namespace Werecodent.CreateAndFake.MSTest.v3.Tests;

[TestClass]
public class IntegrationTests
{
    internal static MethodInfo AttributeMethod { get; } =
        typeof(IntegrationTests).GetMethod(
            nameof(Integration_AllAttributesWork),
            BindingFlags.Instance | BindingFlags.Public
        );

    public sealed class Wrapper(IRandom gen)
    {
        public string NextName => gen.NextItem<string>([]);
    }

    [TestMethod, RandomData]
    public void Integration_UsesParameterAttributes(
        [Stub] IRandom gen,
        [Inject] Wrapper context,
        [Size(2)] string name
    )
    {
        name.Length.Assert().Is(2);
        gen.NextItem(Arg.Any<string[]>()).SetupReturn(name);
        context.NextName.Assert().Is(name);
    }

    [TestMethod, RandomData]
    public void Integration_AllAttributesWork(
        [Cap(5)] int intCapMax,
        [Copy] int intCopyMax,
        [Cap(8, 11)] int intCapRange,
        [Cap(5d)] double doubleCapMax,
        [Cap(20f, 24f)] float floatCapRange,
        [Copy] int intCopyRange,
        [Stub] IRandom stubGen,
        [Inject] Wrapper stubContext,
        [Fake] IRandom fakeGen,
        [Inject] Wrapper fakeContext,
        [Size(2)] string[] stringSizeSet,
        [Size(3, 5)] string[] stringSizeRange,
        string stringNone,
        [Unique] string stringUnique
    )
    {
        intCapMax.Assert().GreaterThanOrIs(0).And.LessThan(5);
        intCopyMax.Assert().Is(intCapMax);
        intCapRange.Assert().GreaterThanOrIs(8).And.LessThanOrIs(11);
        doubleCapMax.Assert().GreaterThanOrIs(0d).And.LessThan(5d);
        floatCapRange.Assert().GreaterThanOrIs(20f).And.LessThanOrIs(24f);
        intCopyRange.Assert().Is(intCapRange);
        stubGen.NextBytes(0).Assert().IsNull();
        stubContext.NextName.Assert().IsNull();
        fakeGen.NextBytes(0).Assert().IsNotNull();
        fakeContext.NextName.Assert().IsNotNull();
        stringSizeSet.Assert().HasCount(2);
        stringSizeRange.Assert().HasCountMoreOrExactly(3).And.HasCountLessOrExactly(5);
        stringNone.Assert().IsNot(stringSizeSet).And.IsNot(stringSizeRange);
        stringUnique.Assert().IsNot(stringNone).And.IsNot(stringSizeSet).And.IsNot(stringSizeRange);
    }

    [TestMethod, RandomData]
    public void Issue118_FixesStubTypeRandomData([Stub] Type type)
    {
        type.Assert().IsNotNull();
    }
}
