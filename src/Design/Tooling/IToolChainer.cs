namespace CreateAndFake.Design.Tooling;

/// <summary>Handles recursive tool behavior.</summary>
/// <typeparam name="TOptions">Type for the options.</typeparam>
public interface IToolChainer<out TOptions> : ITool<TOptions>
    where TOptions : IToolOptions;
