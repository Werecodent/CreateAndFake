using System.Collections.Frozen;
using Werecodent.CreateAndFake.RandomizerTool;
using Werecodent.CreateAndFake.RandomizerTool.Engine;
using Werecodent.CreateAndFake.RandomizerTool.Handlers;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Handlers;

public static class StringCreateHandlerTests
{
    [Fact]
    internal static void StringCreateHandler_InternalOnly()
    {
        typeof(StringCreateHandler).IsPublic.Assert().Is(false);
    }

    [Fact]
    internal static void CreateSupported_SizeConstraintsWork()
    {
        RandomizerOptions options = Tools.Randomizer.Options with
        {
            StringMinSize = 2,
            StringMaxSize = 5,
        };
        StringCreateHandler hint = new();
        RandomizerChainer chainer = new(options, new RandomizerEngine());

        for (int i = 0; i < 1000; i++)
        {
            string result = (string)hint.CreateSupported(chainer);

            result
                .Length.Assert()
                .GreaterThanOrEqualTo(options.StringMinSize, "Result was too small.");
            result.Length.Assert().LessThanOrEqualTo(options.StringMaxSize, "Result was too big.");
        }
    }

    [Fact]
    internal static void CreateSupported_UsesCharSet()
    {
        RandomizerOptions options = Tools.Randomizer.Options with
        {
            StringMinSize = 3,
            StringMaxSize = 3,
            StringCharacterSet = FrozenSet.ToFrozenSet("a"),
        };
        StringCreateHandler hint = new();
        object value = "aaa";
        for (int i = 0; i < 100; i++)
        {
            hint.CreateSupported(new RandomizerChainer(options, new RandomizerEngine()))
                .Assert()
                .Is(value);
        }

        RandomizerOptions options2 = options with
        {
            StringCharacterSet = FrozenSet.ToFrozenSet("ab"),
        };
        RandomizerChainer chainer = new(options2, new RandomizerEngine());
        for (int i = 0; i < 100; i++)
        {
            ((string)hint.CreateSupported(chainer)).Trim('a', 'b').Length.Assert().Is(0);
        }
    }
}
