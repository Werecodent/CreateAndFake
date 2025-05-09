using System.Runtime.Serialization;
using CreateAndFake.ExtractorTool;

namespace CreateAndFake.DuplicatorTool.CopyHints;

#pragma warning disable CS0252 // Possible unintended reference comparison: Intended.

/// <summary>Handles cloning <see cref="ISerializable"/> instances for <see cref="IDuplicator"/> .</summary>
public sealed class SerializableCopyHint : CopyHint
{
    /// <summary>Reference to System.RuntimeType.</summary>
    private static readonly Type _RuntimeType = typeof(Type).GetType();

    /// <inheritdoc/>
    public override CopyHintResult TryCopy(object source, DuplicatorChainer duplicator)
    {
        if (source is ISerializable && source != _RuntimeType)
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

    /// <summary>Finds known types needed for specific types.</summary>
    /// <param name="source">Object being serialized.</param>
    /// <returns>Known types to add.</returns>
    private static IEnumerable<Type> FindExtraKnownTypes(object source)
    {
        if (source is AggregateException)
        {
            yield return typeof(Exception[]);
        }
    }
}

#pragma warning restore CS0252 // Possible unintended reference comparison; left hand side needs cast
