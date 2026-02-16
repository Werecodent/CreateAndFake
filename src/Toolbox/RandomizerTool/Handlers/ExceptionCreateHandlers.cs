using System.Collections.Frozen;
using System.Reflection;
using CreateAndFake.Design.Content;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Handlers;

#pragma warning disable SYSLIB0050 // Needed for backwards compatibility.

internal static class ExceptionCreateHandlers
{
    /// <summary>Exceptions that have issues in legacy .NET.</summary>
    private static readonly FrozenSet<string> UnsupportedExceptions =
    [
        "System.Diagnostics.Eventing.Reader.EventLogProviderDisabledException",
        "System.Diagnostics.Eventing.Reader.EventLogInvalidDataException",
        "System.Diagnostics.Eventing.Reader.EventLogNotFoundException",
        "System.Diagnostics.Eventing.Reader.EventLogReadingException",
        "System.Configuration.ConfigurationErrorsException",
        "System.Net.NetworkInformation.PingException",
        "System.Security.SecurityException",
    ];

    internal static IEnumerable<ICreateHandler> Handlers { get; } =
        TypeDescriber
            .FindLoadedSubclasses<Exception>()
            .Where(t => t.IsVisible)
            .Where(t => t.IsSerializable)
            .Where(t => t.Namespace!.StartsWith("System", StringComparison.Ordinal))
            .Where(t => !UnsupportedExceptions.Contains(t.FullName!))
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
