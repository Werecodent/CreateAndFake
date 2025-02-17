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
            ContentMap contents = duplicator.Options.Extractor.Extract(source, opt => opt with { ExtractPrivateMembers = true });

            DataContractSerializer serializer = new(source.GetType(), contents
                .AllContent()
                .Select(d => d.GetType())
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
        }
        else
        {
            return CopyHintResult.None;
        }
    }
}
