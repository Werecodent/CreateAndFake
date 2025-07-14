using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Hints;

#pragma warning disable SYSLIB0050 // Needed for backwards compatibility.

/// <summary>Handles randomizing <see cref="Exception"/> instances for <see cref="IRandomizer"/>.</summary>
public sealed class ExceptionCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));

        if (!type.Inherits<Exception>())
        {
            return CreateHintResult.None;
        }

        ConstructorInfo[] options =
        [
            .. TypeDescriber
                .FindLocalSubclasses(type)
                .Where(t => t.IsVisible)
                .Where(t => t.IsSerializable)
#if LEGACY // Security exceptions don't work with default serialization in .NET full.
                .Where(t => !t.Namespace.StartsWith("System.Security", StringComparison.Ordinal))
#endif
                .Select(t => t.GetConstructor([typeof(string)]))
                .Where(c => c != null)
                .Select(c => c!),
        ];

        return (options.Length != 0)
            ? new(randomizer.Options.Gen.NextItem(options).Invoke([randomizer.Create<string>()]))
            : CreateHintResult.None;
    }
}

#pragma warning restore SYSLIB0050
