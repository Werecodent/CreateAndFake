namespace Werecodent.CreateAndFake.Design.Tooling;

/// <summary>Handles the recursive portion of tool behavior.</summary>
/// <typeparam name="TOptions">Type for the options.</typeparam>
/// <typeparam name="THint">Type for the hint.</typeparam>
public interface IToolChainer<TOptions, THint> : IHintTool<TOptions, THint>
    where TOptions : IToolHintOptions<TOptions, THint>
    where THint : IToolHint;
