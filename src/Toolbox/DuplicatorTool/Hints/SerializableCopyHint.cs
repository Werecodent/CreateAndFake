using System.Runtime.Serialization;
using CreateAndFake.Design;
using CreateAndFake.DuplicatorTool.Engine;
using CreateAndFake.ExtractorTool;

namespace CreateAndFake.DuplicatorTool.Hints;

#pragma warning disable CS0252 // Intended.

/// <summary>Handles cloning <see cref="ISerializable"/> instances for <see cref="IDuplicator"/> .</summary>
public sealed class SerializableCopyHint : CopyHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CopyPriority.SerializableHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [typeof(ISerializable)];

    /// <inheritdoc/>
    public override CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(duplicator);

        if (source is ISerializable) // && source.GetType().IsSerializable) // && HasSerializationConstructor(source))
        {
            IContentMap contents = duplicator.Options.Extractor.Extract(source);

            DataContractSerializer serializer = new(
                source.GetType(),
                contents
                    .AllContent()
                    .Select(d => d.GetType())
                    .Concat(FindExtraKnownTypes(source))
                    .Distinct()
            );

            using MemoryStream stream = new();
            try
            {
                serializer.WriteObject(stream, source);
                _ = stream.Seek(0, SeekOrigin.Begin);
                return new(serializer.ReadObject(stream));
            }
            catch (Exception e) when (e is SerializationException or InvalidDataContractException)
            {
                throw new SerializationException(
                    $"Ran into problem trying to serialize type '{source.GetType()}'.",
                    e
                );
            }
        }
        else
        {
            return CopyHintResult.None;
        }
    }

    /*private static bool HasSerializationConstructor(object source)
    {
        return source
                .GetType()
                .GetConstructor(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    [typeof(SerializationInfo), typeof(StreamingContext)],
                    null
                ) != null;
    }*/

    /// <summary>Finds known types needed for specific types.</summary>
    /// <param name="source">Object being serialized.</param>
    /// <returns>Known types to add.</returns>
    private static IEnumerable<Type> FindExtraKnownTypes(object source)
    {
        if (source is AggregateException)
        {
            yield return typeof(Exception[]);
        }
        yield return typeof(string[]);
    }
}

#pragma warning restore CS0252
