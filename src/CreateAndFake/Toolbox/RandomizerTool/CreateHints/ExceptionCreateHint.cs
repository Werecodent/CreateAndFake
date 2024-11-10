using System.Reflection;
using CreateAndFake.Design;

#pragma warning disable SYSLIB0050 // 'Type.IsSerializable' is obsolete: Needed for backwards compatibility.

namespace CreateAndFake.Toolbox.RandomizerTool.CreateHints;

/// <summary>Handles randomizing <see cref="Exception"/> instances for <see cref="IRandomizer"/>.</summary>
public sealed class ExceptionCreateHint : CreateHint
{
    /// <inheritdoc/>
    protected internal override (bool, object?) TryCreate(Type type, RandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));

        if (!type.Inherits<Exception>())
        {
            return (false, null);
        }

        ConstructorInfo[] options = type.FindLocalSubclasses()
            .Where(t => t.IsVisible)
            .Where(t => t.IsSerializable)
#if LEGACY // Security exceptions don't work with default serialization in .NET full.
            .Where(t => !t.Namespace.StartsWith("System.Security", StringComparison.Ordinal))
#endif
            .Select(t => t.GetConstructor([typeof(string)]))
            .Where(c => c != null)
            .Select(c => c!)
            .ToArray();

        return (options.Length != 0)
            ? (true, randomizer.Options.Gen.NextItem(options).Invoke([randomizer.Create<string>()]))
            : (false, null);
    }
}

#pragma warning restore SYSLIB0050 // 'Type.IsSerializable' is obsolete
