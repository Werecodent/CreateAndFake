using System.Reflection;
using CreateAndFake.FakerTool;

namespace CreateAndFake.Tests.FakerTool;

public static class ArgTests
{
    [Fact]
    internal static void Arg_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<Arg>();
    }

    [Fact]
    internal static void Arg_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<Arg>();
    }

    [Fact]
    internal static void Arg_PairsWithLambda()
    {
        string[] methods =
        [
            .. typeof(Arg)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Select(m => m.Name),
        ];

        methods
            .Where(m => !m.StartsWith("Lambda", StringComparison.InvariantCulture))
            .Where(m => !methods.Contains("Lambda" + m))
            .Assert()
            .IsEmpty("Methods should have a lambda version to convert to.");
    }

    [Fact]
    internal static void Arg_Random()
    {
        Arg.Any<int>().Assert().IsNot(default(int));
        Arg.Where<int>(null).Assert().IsNot(default(int));

        Arg.Any<string>().Assert().IsNot(default(string));
        Arg.NotNull<string>().Assert().IsNot(default(string));
        Arg.Where<string>(null).Assert().IsNot(default(string));
    }

    [Fact]
    internal static void Arg_OutRefDefault()
    {
        Arg.AnyRef<int>().Var.Assert().Is(default(int));
        Arg.WhereRef<int>(null).Var.Assert().Is(default(int));

        Arg.AnyRef<string>().Var.Assert().Is(default(string));
        Arg.WhereRef<string>(null).Var.Assert().Is(default(string));
    }

    [Theory, RandomData]
    internal static void LambdaAny_MatchesValues(int value, string name)
    {
        Arg.LambdaAny<int>().Matches(value).Assert().Is(true);
        Arg.LambdaAny<string>().Matches(name).Assert().Is(true);
    }

    [Fact]
    internal static void LambdaAny_NullsMatch()
    {
        Arg.LambdaAny<int>().Matches(null).Assert().Is(true);
        Arg.LambdaAny<string>().Matches(null).Assert().Is(true);
    }

    [Theory, RandomData]
    internal static void LambdaAny_Mismatches(string name, int value)
    {
        Arg.LambdaAny<int>().Matches(name).Assert().Is(false);
        Arg.LambdaAny<string>().Matches(value).Assert().Is(false);
    }

    [Theory, RandomData]
    internal static void LambdaAnyRef_MatchesValues(OutRef<string> nameRef, OutRef<int> valueRef)
    {
        Arg.LambdaAnyRef<int>().Matches(valueRef).Assert().Is(true);
        Arg.LambdaAnyRef<string>().Matches(nameRef).Assert().Is(true);
    }

    [Theory, RandomData]
    internal static void LambdaNotNull_MatchesValues(string name)
    {
        Arg.LambdaNotNull<string>().Matches(name).Assert().Is(true);
    }

    [Fact]
    internal static void LambdaNotNull_NullsInvalid()
    {
        Arg.LambdaNotNull<string>().Matches(null).Assert().Is(false);
    }

    [Theory, RandomData]
    internal static void LambdaWhere_MatchesCondition(string name)
    {
        Arg.LambdaWhere<string>(null).Matches(name).Assert().Is(true);
        Arg.LambdaWhere<string>(s => true).Matches(name).Assert().Is(true);
        Arg.LambdaWhere<string>(s => false).Matches(name).Assert().Is(false);
    }

    [Theory, RandomData]
    internal static void LambdaWhereRef_MatchesCondition(OutRef<string> nameRef)
    {
        Arg.LambdaWhereRef<string>(null).Matches(nameRef).Assert().Is(true);
        Arg.LambdaWhereRef<string>(s => true).Matches(nameRef).Assert().Is(true);
        Arg.LambdaWhereRef<string>(s => false).Matches(nameRef).Assert().Is(false);
    }

    [Theory, RandomData]
    internal static void Arg_Cloneable(string value)
    {
        Arg origin = Arg.LambdaWhere<string>(d => d == value);
        Arg copy = origin.CreateDeepClone();

        copy.Assert().Is(origin);
        copy.Matches(value).Assert().Is(true);
        copy.Matches(value.CreateVariant()).Assert().Is(false);
    }
}
