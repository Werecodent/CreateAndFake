using System.Runtime.Serialization;

namespace Werecodent.CreateAndFake.Design.Exceptions;

/// <summary><see cref="Exception"/> <see cref="Type"/> for issues encountered during automated testing.</summary>
[Serializable]
public sealed class AutoTesterFailureException : CreateAndFakeException
{
    /// <inheritdoc cref="AutoTesterFailureException"/>
    /// <remarks>Serialization constructor.</remarks>
    private AutoTesterFailureException()
        : base() { }

    /// <param name="testClass">Class containing the test that failed.</param>
    /// <inheritdoc cref="AutoTesterFailureException"/>
    public AutoTesterFailureException(Type testClass)
        : base($"The '{testClass?.Name}' AutoTester encountered an issue.") { }

    /// <param name="testClass">Class containing the test that failed.</param>
    /// <inheritdoc cref="AutoTesterFailureException"/>
    public AutoTesterFailureException(Type testClass, Exception? innerException)
        : base($"The '{testClass?.Name}' AutoTester encountered an issue.", innerException) { }

    /// <inheritdoc/>
    /// <remarks>Serialization constructor.</remarks>
#if NET5_0_OR_GREATER
    [Obsolete("ISerializable has been disabled.", DiagnosticId = "SYSLIB0051")]
#endif
    private AutoTesterFailureException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
