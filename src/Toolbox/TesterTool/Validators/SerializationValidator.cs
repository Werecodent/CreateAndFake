using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using CreateAndFake.Design;
using CreateAndFake.Design.Types;
using CreateAndFake.ExtractorTool;

namespace CreateAndFake.TesterTool.Validators;

/// <summary>Automates common tests.</summary>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
internal sealed class SerializationValidator(TesterOptions options)
{
    /// <inheritdoc/>
    internal TesterOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public void VerifyJsonSerialization<T>()
    {
        VerifyJsonSerialization(Options.Randomizer.Create<T>());
    }

    /// <inheritdoc/>
    public void VerifyJsonSerialization<T>(T instance)
    {
        VerifyJsonSerialization(typeof(T), instance);
    }

    /// <inheritdoc/>
    public void VerifyJsonSerialization(object instance)
    {
        ArgumentGuard.ThrowIfNull(instance);
        VerifyJsonSerialization(instance.GetType(), instance);
    }

    /// <param name="type">The <paramref name="instance"/> <see cref="Type"/> for testing.</param>
    /// <inheritdoc cref="VerifyJsonSerialization(object)"/>
    private void VerifyJsonSerialization(Type type, object? instance)
    {
        VerifySerialization(type, instance, new DataContractJsonSerializer(type));
    }

    /// <inheritdoc/>
    public void VerifyXmlSerialization<T>()
    {
        VerifyXmlSerialization(Options.Randomizer.Create<T>());
    }

    /// <inheritdoc/>
    public void VerifyXmlSerialization<T>(T instance)
    {
        VerifyXmlSerialization(typeof(T), instance);
    }

    /// <inheritdoc/>
    public void VerifyXmlSerialization(object instance)
    {
        ArgumentGuard.ThrowIfNull(instance);
        VerifyXmlSerialization(instance.GetType(), instance);
    }

    /// <param name="type">The <paramref name="instance"/> <see cref="Type"/> for testing.</param>
    /// <inheritdoc cref="VerifyXmlSerialization(object)"/>
    private void VerifyXmlSerialization(Type type, object? instance)
    {
        IContentMap contents = Options.Extractor.Extract(instance);
        DataContractSerializer serializer = new(
            type,
            contents.AllContent().Select(d => d.GetType()).Distinct()
        );

        VerifySerialization(type, instance, serializer);
    }

    /// <param name="type">The <paramref name="instance"/> <see cref="Type"/> for testing.</param>
    /// <inheritdoc cref="VerifyXmlSerialization(Type,object)"/>
    private void VerifySerialization(Type type, object? instance, XmlObjectSerializer serializer)
    {
        using MemoryStream stream = new();
        object? result;
        try
        {
            serializer.WriteObject(stream, instance);
            _ = stream.Seek(0, SeekOrigin.Begin);
            result = serializer.ReadObject(stream);
        }
        catch (Exception e) when (e is SerializationException or InvalidDataContractException)
        {
            throw new SerializationException(
                $"Ran into problem trying to serialize type '{GenericConverter.ExpandName(type)}'.",
                e
            );
        }

        Options.Asserter.Is(
            result,
            instance,
            $"Instance of type '{GenericConverter.ExpandName(type)}' did not deserialize with the same values."
        );
    }
}
