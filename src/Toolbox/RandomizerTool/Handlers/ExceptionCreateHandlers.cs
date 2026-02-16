using System.Reflection;
using CreateAndFake.Design.Content;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Handlers;

#pragma warning disable SYSLIB0050 // Needed for backwards compatibility.

internal static class ExceptionCreateHandlers
{
    internal static IEnumerable<ICreateHandler> Handlers { get; } =
        TypeDescriber
            .FindLoadedSubclasses<Exception>()
            .Where(t => t.IsVisible)
            .Where(t => t.IsSerializable)
            .Where(t => t.Namespace?.StartsWith("System", StringComparison.Ordinal) == true)
            .Select(t => t.GetConstructor([typeof(string)]))
            .Where(c => c != null)
            .Select(c => new ExceptionCreateHandler(c!));

    /// <inheritdoc cref="ICreateHandler"/>
    private sealed class ExceptionCreateHandler(ConstructorInfo constructor) : ICreateHandler
    {
        /// <inheritdoc/>
        public Type? SupportedType => constructor.DeclaringType;

        /// <inheritdoc/>
        public object? CreateSupported(IRandomizerChainer randomizer)
        {
            return constructor.Invoke([randomizer.Create<string>()]);
        }
    }
}

#pragma warning restore SYSLIB0050
