using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Reiteration;
using Werecodent.CreateAndFake.ExtractorTool;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.MutatorTool;
using Werecodent.CreateAndFake.MutatorTool.Engine;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.Tests.MutatorTool.Engine;

public static class MutatorEngineTests
{
    private static readonly MutatorEngine _TestInstance = new();

    [Fact]
    internal static Task MutatorEngine_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<MutatorEngine>(
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static void Variant_AcceptsNull(string value)
    {
        new Mutator(Tools.Mutator.Options).Variant<string>(null).Assert().IsNotNull();

        new Mutator(Tools.Mutator.Options)
            .Variant(value, null)
            .Assert()
            .IsNot(value)
            .And()
            .IsNotNull();
    }

    [Fact]
    internal static void VariantOf_ThrowsIfImpossible()
    {
        Tools.Mutator.Assert(x => x.VariantOf([true, false])).Throws<ToolException>();
    }

    [Theory, RandomData]
    internal static void VariantOf_ManyValuesWorks([Size(3000)] int[] data)
    {
        IValuer valuer = Tools.Valuer.WithOptions(opt =>
            opt with
            {
                IterationLimit = data.Length + 1,
            }
        );

        int result = Tools.Mutator.VariantOf(data, opt => opt with { Valuer = valuer });
        data.Assert().ContainsNot(result, opt => opt with { Valuer = valuer });
    }

    [Theory, RandomData]
    internal static void Variant_TimesOut([Fake] IValuer fakeValuer, DataSample sample)
    {
        fakeValuer.Options.SetupReturn(Tools.Valuer.Options);
        fakeValuer
            .Equals(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<ValuerMod>())
            .SetupReturn(true);

        new Mutator(
            Tools.Mutator.Options with
            {
                Valuer = fakeValuer,
                CreateVariantAttemptLimit = new Limiter(3),
            }
        )
            .Assert(x => x.Variant(sample))
            .Throws<ToolException>();

        fakeValuer.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Variant_RepeatsUntilUnequal([Fake] IValuer fakeValuer, DataSample sample)
    {
        fakeValuer.Options.SetupReturn(Tools.Valuer.Options);
        fakeValuer
            .Equals(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<ValuerMod>())
            .SetupReturn(Behavior.Series(true, true, true, false));

        new Mutator(
            Tools.Mutator.Options with
            {
                Valuer = fakeValuer,
                CreateVariantAttemptLimit = new Limiter(5),
            }
        )
            .Variant(sample)
            .Assert()
            .IsNotNull();

        fakeValuer.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Variant_RepeatsUntilBothUnequal(
        [Fake] IValuer fakeValuer,
        DataSample sample1,
        DataSample sample2
    )
    {
        fakeValuer.Options.SetupReturn(Tools.Valuer.Options);
        fakeValuer
            .Equals(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<ValuerMod>())
            .SetupReturn(Behavior.Series(false, true, true, false, true, true, false, false));

        new Mutator(
            Tools.Mutator.Options with
            {
                Valuer = fakeValuer,
                CreateVariantAttemptLimit = new Limiter(5),
            }
        )
            .VariantOf([sample1, sample2])
            .Assert()
            .IsNotNull();

        fakeValuer.Assert().Called();
    }

    [Theory, RandomData]
    internal static async Task VariantAsync_AcceptsNull(string value)
    {
        await new Mutator(Tools.Mutator.Options)
            .VariantAsync<string>(null, TestContext.Current.CancellationToken)
            .Assert()
            .HasResultAsync(TestContext.Current.CancellationToken)
            .That()
            .IsNotNull();

        await new Mutator(Tools.Mutator.Options)
            .VariantAsync(value, TestContext.Current.CancellationToken, null)
            .Assert()
            .HasResultAsync(TestContext.Current.CancellationToken)
            .That()
            .IsNot(value);
    }

    [Fact]
    internal static Task VariantOfAsync_ThrowsIfImpossible()
    {
        return Tools
            .Mutator.VariantOfAsync([true, false], TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<ToolException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task VariantOfAsync_ManyValuesWorks([Size(3000)] int[] data)
    {
        IValuer valuer = Tools.Valuer.WithOptions(opt =>
            opt with
            {
                IterationLimit = data.Length + 1,
            }
        );

        int result = await Tools.Mutator.VariantOfAsync(
            data,
            TestContext.Current.CancellationToken,
            opt => opt with { Valuer = valuer }
        );
        await data.Assert()
            .ContainsNotAsync(
                result,
                TestContext.Current.CancellationToken,
                opt => opt with { Valuer = valuer }
            );
    }

    [Theory, RandomData]
    internal static async Task VariantAsync_TimesOut([Fake] IValuer fakeValuer, DataSample sample)
    {
        fakeValuer
            .EqualsAsync(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<CancellationToken>())
            .SetupReturn(Task.FromResult(true));

        await new Mutator(
            Tools.Mutator.Options with
            {
                Valuer = fakeValuer,
                CreateVariantAttemptLimit = new Limiter(3),
            }
        )
            .VariantAsync(sample, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<ToolException>(TestContext.Current.CancellationToken);

        fakeValuer.Assert().Called();
    }

    [Theory, RandomData]
    internal static async Task VariantAsync_RepeatsUntilUnequal(
        [Fake] IValuer fakeValuer,
        DataSample sample
    )
    {
        fakeValuer
            .EqualsAsync(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<CancellationToken>())
            .SetupReturn(
                Behavior.Series(
                    Task.FromResult(true),
                    Task.FromResult(true),
                    Task.FromResult(true),
                    Task.FromResult(false)
                )
            );

        await new Mutator(
            Tools.Mutator.Options with
            {
                Valuer = fakeValuer,
                CreateVariantAttemptLimit = new Limiter(5),
            }
        )
            .VariantAsync(sample, TestContext.Current.CancellationToken)
            .Assert()
            .HasResultAsync(TestContext.Current.CancellationToken)
            .That()
            .IsNotNull();

        fakeValuer.Assert().Called();
    }

    [Theory, RandomData]
    internal static async Task VariantAsync_RepeatsUntilBothUnequal(
        [Fake] IValuer fakeValuer,
        DataSample sample1,
        DataSample sample2
    )
    {
        fakeValuer
            .EqualsAsync(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<CancellationToken>())
            .SetupReturn(
                Behavior.Series(
                    Task.FromResult(false),
                    Task.FromResult(true),
                    Task.FromResult(true),
                    Task.FromResult(false),
                    Task.FromResult(true),
                    Task.FromResult(true),
                    Task.FromResult(false),
                    Task.FromResult(false)
                )
            );

        await new Mutator(
            Tools.Mutator.Options with
            {
                Valuer = fakeValuer,
                CreateVariantAttemptLimit = new Limiter(5),
            }
        )
            .VariantOfAsync([sample1, sample2], TestContext.Current.CancellationToken)
            .Assert()
            .HasResultAsync(TestContext.Current.CancellationToken)
            .That()
            .IsNotNull();

        fakeValuer.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Unique_AcceptsNull(string value)
    {
        Tools.Mutator.Unique<string>(null).Assert().IsNotNull();
        Tools.Mutator.UniqueOf(value, null).Assert().IsNot(value).And().IsNotNull();
    }

    [Theory, RandomData]
    internal static void UniqueOf_ManyValuesWorks([Size(100)] int[] data)
    {
        int result = Tools.Mutator.UniqueOf(data);
        data.Assert().ContainsNot(result);
    }

    [Theory, RandomData]
    internal static void Unique_TimesOut([Fake] IValuer fakeValuer, DataSample sample)
    {
        fakeValuer.Equals(Arg.Any<object>(), Arg.Any<object>()).SetupReturn(true);
        fakeValuer.GetHashCode(Arg.Any<object>()).SetupReturn(0);

        new Mutator(
            Tools.Mutator.Options with
            {
                Valuer = fakeValuer,
                Extractor = new Extractor(Tools.Extractor.Options with { Valuer = fakeValuer }),
            }
        )
            .Assert(x => x.Unique(sample))
            .Throws<ToolException>();
    }

    [Theory, RandomData]
    internal static void Unique_RepeatsUntilUnequal([Fake] IValuer fakeValuer, string sample)
    {
        fakeValuer
            .Equals(Arg.Any<object>(), Arg.Any<object>())
            .SetupReturn(Behavior.Series(true, true, true, false));
        fakeValuer.GetHashCode(Arg.Any<object>()).SetupReturn(0);

        new Mutator(
            Tools.Mutator.Options with
            {
                Valuer = fakeValuer,
                CreateUniqueAttemptLimit = new Limiter(5),
            }
        )
            .Unique(sample)
            .Assert()
            .IsNotNull();
    }

    [Theory, RandomData]
    internal static void Unique_RepeatsUntilBothUnequal(
        [Fake] IValuer fakeValuer,
        string sample1,
        string sample2
    )
    {
        fakeValuer
            .Equals(Arg.Any<object>(), Arg.Any<object>())
            .SetupReturn(Behavior.Series(false, true, true, false, true, true, false, false));
        fakeValuer.GetHashCode(Arg.Any<object>()).SetupReturn(0);

        new Mutator(
            Tools.Mutator.Options with
            {
                Valuer = fakeValuer,
                CreateUniqueAttemptLimit = new Limiter(5),
            }
        )
            .UniqueOf([sample1, sample2])
            .Assert()
            .IsNotNull();
    }

    [Theory, RandomData]
    internal static async Task UniqueAsync_AcceptsNull(string value)
    {
        await Tools
            .Mutator.UniqueAsync<string>(null, TestContext.Current.CancellationToken)
            .Assert()
            .HasResultAsync(TestContext.Current.CancellationToken)
            .That()
            .IsNotNull();
        await Tools
            .Mutator.UniqueOfAsync([value, null], TestContext.Current.CancellationToken)
            .Assert()
            .HasResultAsync(TestContext.Current.CancellationToken)
            .That()
            .IsNot(value)
            .And()
            .IsNotNull();
    }

    [Theory, RandomData]
    internal static async Task UniqueOfAsync_ManyValuesWorks([Size(100)] int[] data)
    {
        int result = await Tools.Mutator.UniqueOfAsync(data, TestContext.Current.CancellationToken);
        await data.Assert().ContainsNotAsync(result, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task UniqueAsync_TimesOut([Fake] IValuer fakeValuer, DataSample sample)
    {
        fakeValuer
            .EqualsAsync(
                Arg.Any<object>(),
                Arg.Any<object>(),
                TestContext.Current.CancellationToken
            )
            .SetupReturn(Task.FromResult(true));
        fakeValuer
            .GetHashCodeAsync(Arg.Any<object>(), TestContext.Current.CancellationToken)
            .SetupReturn(Task.FromResult(0));

        return new Mutator(
            Tools.Mutator.Options with
            {
                Valuer = fakeValuer,
                Extractor = new Extractor(Tools.Extractor.Options with { Valuer = fakeValuer }),
            }
        )
            .UniqueAsync(sample, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<ToolException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task UniqueAsync_RepeatsUntilUnequal([Fake] IValuer fakeValuer, string sample)
    {
        fakeValuer
            .EqualsAsync(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<CancellationToken>())
            .SetupReturn(
                Behavior.Series(
                    Task.FromResult(true),
                    Task.FromResult(true),
                    Task.FromResult(true),
                    Task.FromResult(false)
                )
            );
        fakeValuer
            .GetHashCodeAsync(Arg.Any<object>(), Arg.Any<CancellationToken>())
            .SetupReturn(Task.FromResult(0));

        return new Mutator(
            Tools.Mutator.Options with
            {
                Valuer = fakeValuer,
                CreateUniqueAttemptLimit = new Limiter(5),
            }
        )
            .UniqueAsync(sample, TestContext.Current.CancellationToken)
            .Assert()
            .HasResultAsync(TestContext.Current.CancellationToken)
            .That()
            .IsNotNull();
    }

    [Theory, RandomData]
    internal static Task UniqueAsync_RepeatsUntilBothUnequal(
        [Fake] IValuer fakeValuer,
        string sample1,
        string sample2
    )
    {
        fakeValuer
            .EqualsAsync(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<CancellationToken>())
            .SetupReturn(
                Behavior.Series(
                    Task.FromResult(false),
                    Task.FromResult(true),
                    Task.FromResult(true),
                    Task.FromResult(false),
                    Task.FromResult(true),
                    Task.FromResult(true),
                    Task.FromResult(false),
                    Task.FromResult(false)
                )
            );
        fakeValuer
            .GetHashCodeAsync(Arg.Any<object>(), Arg.Any<CancellationToken>())
            .SetupReturn(Task.FromResult(0));

        return new Mutator(
            Tools.Mutator.Options with
            {
                Valuer = fakeValuer,
                CreateUniqueAttemptLimit = new Limiter(5),
            }
        )
            .UniqueOfAsync([sample1, sample2], TestContext.Current.CancellationToken)
            .Assert()
            .HasResultAsync(TestContext.Current.CancellationToken)
            .That()
            .IsNotNull();
    }

    [Theory, RandomData]
    internal static void Modify_NoHintsUnsupported(object data)
    {
        _TestInstance
            .Assert(x => x.Modify(data, CreateHintChainer(null)))
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal static void Modify_NullResultSafe([Stub] IMutateHint hint, object data)
    {
        _TestInstance
            .Assert(x => x.Modify(data, CreateHintChainer(hint)))
            .Throws<UnsupportedException>();
    }

    [Theory, RandomData]
    internal static void Modify_WrapsError([Stub] IMutateHint hint, object data)
    {
        hint.TryToModify(data, Arg.Any<IMutatorChainer>())
            .SetupReturn(Behavior<MutateHintResult>.Throw<InvalidOperationException>());

        _TestInstance.Assert(x => x.Modify(data, CreateHintChainer(hint))).Throws<ToolException>();
    }

    private static MutatorChainer CreateHintChainer(IMutateHint hint)
    {
        return new MutatorChainer(
            Tools.Mutator.Options with
            {
                IncludeFoundHints = false,
                IncludeFrameworkHints = false,
                Hints = hint != null ? [hint] : [],
            },
            new MutatorEngine()
        );
    }
}
