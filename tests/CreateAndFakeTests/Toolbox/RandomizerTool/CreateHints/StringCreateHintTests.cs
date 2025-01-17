using System.Collections.Frozen;
using CreateAndFake.Toolbox.RandomizerTool;
using CreateAndFake.Toolbox.RandomizerTool.CreateHints;
using CreateAndFakeTests.TestBases;

namespace CreateAndFakeTests.Toolbox.RandomizerTool.CreateHints;

public sealed class StringCreateHintTests : CreateHintTestBase<StringCreateHint>
{
    private static readonly StringCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(string)];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public StringCreateHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Fact]
    internal static void TryCreate_SizeConstraintsWork()
    {
        RandomizerOptions options = Tools.Randomizer.Options with
        {
            StringMinSize = 2,
            StringMaxSize = 5
        };
        StringCreateHint hint = new();

        for (int i = 0; i < 1000; i++)
        {
            string result = (string)hint.TryCreate(typeof(string), CreateChainer(options)).Data;

            result.Length.Assert().GreaterThanOrEqualTo(options.StringMinSize, "Result was too small.");
            result.Length.Assert().LessThanOrEqualTo(options.StringMaxSize, "Result was too big.");
        }
    }

    [Fact]
    internal static void TryCreate_UsesCharSet()
    {
        RandomizerOptions options = Tools.Randomizer.Options with
        {
            StringMinSize = 3,
            StringMaxSize = 3,
            StringCharacterSet = FrozenSet.ToFrozenSet("a")
        };
        StringCreateHint hint = new();
        object value = "aaa";
        for (int i = 0; i < 100; i++)
        {
            hint.TryCreate(typeof(string), CreateChainer(options)).Assert().Is(new CreateHintResult(value));
        }

        RandomizerOptions options2 = options with { StringCharacterSet = FrozenSet.ToFrozenSet("ab") };
        for (int i = 0; i < 100; i++)
        {
            CreateHintResult result = hint.TryCreate(typeof(string), CreateChainer(options2));

            result.HasData.Assert().Is(true);
            ((string)result.Data).Trim('a', 'b').Length.Assert().Is(0);
        }
    }
}
