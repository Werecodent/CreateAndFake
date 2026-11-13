namespace Werecodent.CreateAndFake.ExtractorTool.Handlers;

/// <typeparam name="T"><inheritdoc cref="ConvertExtractHandler.SupportedType" path="/summary"/></typeparam>
/// <inheritdoc cref="ConvertExtractHandler"/>
internal sealed class ConvertExtractHandler<T>(Func<T, ICollection<object?>> factory)
    : ConvertExtractHandler(typeof(T), value => factory?.Invoke((T)value)!);
