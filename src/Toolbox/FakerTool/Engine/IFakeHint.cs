using CreateAndFake.Design.Tooling;
using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.FakerTool.Engine;

/// <summary>Handles faking the <see cref="IToolHint.SupportedTypes"/>.</summary>
public interface IFakeHint : IToolHint
{
    /// <summary>If the hint has behavior for <see cref="TryToFake"/>.</summary>
    bool SupportsToFake { get; }

    /// <summary>If the hint has behavior for <see cref="TryToSetup"/>.</summary>
    bool SupportsToSetup { get; }

    /// <summary>Tries to fake an instance of the given <paramref name="parent"/>.</summary>
    /// <param name="parent"><see cref="Type"/> to generate.</param>
    /// <param name="interfaces">Extra interfaces to implement.</param>
    /// <param name="faker">Handles faking child values.</param>
    /// <returns>Possible result.</returns>
    FakeHintResult TryToFake(Type parent, IEnumerable<Type> interfaces, IFakerChainer faker);

    /// <summary>Tries to configure the fake <paramref name="instance"/>.</summary>
    /// <param name="instance">Fake to configure.</param>
    /// <param name="faker">Handles faking child values.</param>
    /// <returns>Possible result.</returns>
    SetupHintResult TryToSetup(IFaked instance, IFakerChainer faker);
}
