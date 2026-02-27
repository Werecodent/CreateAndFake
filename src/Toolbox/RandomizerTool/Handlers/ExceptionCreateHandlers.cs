using System.Collections.Frozen;
using System.Reflection;
using System.Runtime.InteropServices;
using CreateAndFake.Design.Types;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Handlers;

#pragma warning disable CS0618, SYSLIB0050 // Needed for backwards compatibility.

internal static class ExceptionCreateHandlers
{
    /// <summary>Exceptions that have issues in legacy .NET.</summary>
    private static readonly FrozenSet<string> _UnsupportedExceptions =
    [
        "System.Diagnostics.Eventing.Reader.EventLogProviderDisabledException",
        "System.Diagnostics.Eventing.Reader.EventLogInvalidDataException",
        "System.Diagnostics.Eventing.Reader.EventLogNotFoundException",
        "System.Diagnostics.Eventing.Reader.EventLogReadingException",
        "System.ComponentModel.DataAnnotations.ValidationException",
        "System.Diagnostics.Eventing.Reader.EventLogException",
        "System.Runtime.Serialization.SerializationException",
        "System.Configuration.ConfigurationErrorsException",
        "System.Net.NetworkInformation.PingException",
        "System.Net.Http.HttpRequestException",
        "System.Security.SecurityException",
        "System.Web.HttpParseException",
    ];

    /// <summary>Exceptions that shouldn't be registered as supported.</summary>
    private static readonly FrozenSet<Type> _FatalExceptions =
    [
        typeof(AppDomainUnloadedException),
        typeof(AccessViolationException),
        typeof(ExecutionEngineException),
        typeof(BadImageFormatException),
        typeof(StackOverflowException),
        typeof(OutOfMemoryException),
        typeof(ThreadAbortException),
        typeof(ExternalException),
        typeof(COMException),
        typeof(SEHException),
    ];

    internal static IEnumerable<ICreateHandler> Handlers { get; } =
        InheritanceTracker
            .For<Exception>()
            .FindLoadedSubclasses()
            .Where(t => t.IsVisible)
            .Where(t => t.IsSerializable)
            .Where(t => t.Namespace!.StartsWith("System", StringComparison.Ordinal))
            .Where(t => !_UnsupportedExceptions.Contains(t.FullName!))
            .Where(t => !_FatalExceptions.Contains(t))
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

#pragma warning restore CS0618, SYSLIB0050
