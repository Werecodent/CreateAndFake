using CreateAndFake.Design.Randomization;
using CreateAndFake.ValuerTool;
using CreateAndFake.ValuerTool.CompareHints;

namespace CreateAndFake.Tests.ValuerTool.CompareHints;

public sealed class SeededRandomCompareHintTests : CompareHintTestBase<SeededRandomCompareHint>
{
    private static readonly SeededRandomCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(SeededRandom)];

    private static readonly Type[] _InvalidTypes = [typeof(FastRandom), typeof(object)];

    public SeededRandomCompareHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal static void Compare_CanIgnoreSeed(SeededRandom gen)
    {
        SeededRandom copy = gen.CreateDeepClone();
        _ = copy.Next<int>();
        _TestInstance
            .TryCompare(gen, copy, CreateChainer(Tools.Valuer.Options with
            {
                IgnoreCurrentRandomSeed = true
            }))
            .Assert()
            .Is(new DifferenceHintResult([]));
    }

    [Theory, RandomData]
    internal static void Compare_CanIncludeSeed(SeededRandom gen)
    {
        SeededRandom copy = gen.CreateDeepClone();
        _ = copy.Next<int>();
        _TestInstance
            .TryCompare(gen, copy, CreateChainer(Tools.Valuer.Options with
            {
                IgnoreCurrentRandomSeed = false
            }))
            .Assert()
            .IsNot(new DifferenceHintResult([]));
    }

    [Theory, RandomData]
    internal static void GetHashCode_CanIgnoreSeed(SeededRandom gen)
    {
        ValuerChainer chainer = CreateChainer(Tools.Valuer.Options with
        {
            IgnoreCurrentRandomSeed = true
        });
        _TestInstance
            .TryGetHashCode(gen, chainer)
            .Assert()
            .Is(new HashCodeHintResult(Tools.Valuer.GetHashCode(gen.InitialSeed)));
    }

    [Theory, RandomData]
    internal static void GetHashCode_CanIncludeSeed(SeededRandom gen)
    {
        ValuerChainer chainer = CreateChainer(Tools.Valuer.Options with
        {
            IgnoreCurrentRandomSeed = false
        });
        _TestInstance
            .TryGetHashCode(gen, chainer)
            .Assert()
            .IsNot(new HashCodeHintResult(Tools.Valuer.GetHashCode(gen.InitialSeed)));
    }
}
