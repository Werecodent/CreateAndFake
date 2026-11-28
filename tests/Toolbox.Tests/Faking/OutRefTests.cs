namespace Werecodent.CreateAndFake.Tests.Faking;

public sealed class OutRefTests() : AutoTester(typeof(OutRef<>))
{
    protected override TesterMod OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(MemberAccessException)] };
}
