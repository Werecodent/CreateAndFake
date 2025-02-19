using System.Runtime.Serialization;
using CreateAndFake.Toolbox.ExtractorTool;

namespace CreateAndFake.Toolbox.DuplicatorTool.CopyHints;

/// <summary>Handles cloning <see cref="ISerializable"/> instances for <see cref="IDuplicator"/> .</summary>
public sealed class SerializableCopyHint : CopyHint
{
    /// <inheritdoc/>
    public override CopyHintResult TryCopy(object source, DuplicatorChainer duplicator)
    {
        if (source is ISerializable)
        {
            ContentMap contents = duplicator.Options.Extractor.Extract(source);

            DataContractSerializer serializer = new(source.GetType(), contents
                .AllContent()
                .Select(d => d.GetType())
                .Concat(FindExtraKnownTypes(source))
                .Distinct());

            using MemoryStream stream = new();
            try
            {
                serializer.WriteObject(stream, source);
                _ = stream.Seek(0, SeekOrigin.Begin);
                return new(serializer.ReadObject(stream));
            }
            catch (InvalidDataContractException e)
            {
                throw new InvalidDataContractException(
                    $"Ran into problem trying to serialize type '{source.GetType()}'.", e);
            }
            catch (SerializationException e)
            {
                throw new SerializationException(
                    $"Ran into problem trying to serialize type '{source.GetType()}'.", e);
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
