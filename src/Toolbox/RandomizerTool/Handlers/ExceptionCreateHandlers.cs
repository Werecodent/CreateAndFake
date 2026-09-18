using System.Collections.Frozen;
using System.Reflection;
using System.Runtime.InteropServices;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.RandomizerTool.Engine;

namespace Werecodent.CreateAndFake.RandomizerTool.Handlers;

#pragma warning disable CS0618, SYSLIB0050 // Needed for backwards compatibility.

internal static class ExceptionCreateHandlers
{
    /// <summary>Exceptions that have issues in legacy .NET.</summary>
    private static readonly FrozenSet<string> _UnsupportedExceptions =
    [
        "System.DirectoryServices.ActiveDirectory.SyncFromAllServersOperationException",
        "System.DirectoryServices.ActiveDirectory.ForestTrustCollisionException",
        "System.Diagnostics.Eventing.Reader.EventLogProviderDisabledException",
        "System.Deployment.Application.CompatibleFrameworkMissingException",
        "System.Diagnostics.Eventing.Reader.EventLogInvalidDataException",
        "System.Deployment.Application.DependentPlatformMissingException",
        "System.Deployment.Application.SupportedRuntimeMissingException",
        "System.Diagnostics.Eventing.Reader.EventLogNotFoundException",
        "System.Diagnostics.Eventing.Reader.EventLogReadingException",
        "System.ComponentModel.Composition.ChangeRejectedException",
        "System.ComponentModel.DataAnnotations.ValidationException",
        "System.Deployment.Application.DeploymentDownloadException",
        "System.Deployment.Application.InvalidDeploymentException",
        "System.Deployment.Application.TrustNotGrantedException",
        "System.ComponentModel.Composition.CompositionException",
        "System.Security.Principal.IdentityNotMappedException",
        "System.Diagnostics.Eventing.Reader.EventLogException",
        "System.Runtime.Serialization.SerializationException",
        "System.Configuration.ConfigurationErrorsException",
        "System.Deployment.Application.DeploymentException",
        "System.DirectoryServices.Protocols.LdapException",
        "System.Net.Mail.SmtpFailedRecipientsException",
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

    /// <summary>Exception that the handlers can create.</summary>
    internal static IEnumerable<Type> PotentialExceptions { get; } =
        TypeDescriber
            .For<Exception>()
            .FindLoadedSubclasses()
            .Where(t => t.IsVisible)
            .Where(t => t.IsSerializable)
            .Where(t => t.Namespace!.StartsWith("System", StringComparison.Ordinal))
            .Where(t => !_UnsupportedExceptions.Contains(t.FullName!))
            .Where(t => !_FatalExceptions.Contains(t))
            .Where(t => t.GetConstructor([typeof(string)]) != null)
            .ToFrozenSet()!;

    internal static IEnumerable<ICreateHandler> Handlers { get; } =
        PotentialExceptions
            .Select(t => t.GetConstructor([typeof(string)]))
            .Select(c => new ExceptionCreateHandler(c!));

    /// <inheritdoc cref="ICreateHandler"/>
    private sealed class ExceptionCreateHandler(ConstructorInfo constructor) : ICreateHandler
    {
        /// <inheritdoc/>
        public Type? SupportedType => constructor.ReflectedType;

        /// <inheritdoc/>
        public object? CreateSupported(IRandomizerChainer randomizer)
        {
            return constructor.Invoke([randomizer.Create<string>()]);
        }
    }
}

#pragma warning restore
