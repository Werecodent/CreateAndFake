using CreateAndFake.Design.Tooling;
using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.FakerTool.Engine;

/// <summary>Handles faking the <see cref="IToolHint.SupportedTypes"/>.</summary>
public interface IFakeHint : IToolHint
{
    /// <summary>Tries to fake an instance of the given <paramref name="parent"/>.</summary>
    /// <param name="parent"><see cref="Type"/> to generate.</param>
    /// <param name="interfaces">Extra interfaces to implement.</param>
    /// <param name="faker">Handles faking child values.</param>
    /// <returns>Possible result.</returns>
    FakeHintResult TryToFake(Type parent, IEnumerable<Type> interfaces, IFakerChainer faker);

    /// <summary>Tries to configure a fake of the given <paramref name="inherited"/> <see cref="Type"/>.</summary>
    /// <param name="inherited"><see cref="Type"/> of the <paramref name="instance"/> to configure.</param>
    /// <param name="instance">Fake to configure.</param>
    /// <returns>Possible result.</returns>
    SetupHintResult TryToSetup(Type inherited, IFaked instance);
}
