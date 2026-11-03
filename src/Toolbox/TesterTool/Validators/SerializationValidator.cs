using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.ExtractorTool;

namespace Werecodent.CreateAndFake.TesterTool.Validators;

/// <summary>Automates common tests.</summary>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
internal sealed class SerializationValidator(TesterOptions options)
{
    /// <summary>Internal type needed for AggregateException serialization.</summary>
    private static readonly Type _ListDictionaryInternalType = typeof(IDictionary).Assembly.GetType(
        "System.Collections.ListDictionaryInternal"
    )!;

    /// <inheritdoc cref="Tester.Options"/>
    internal TesterOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc cref="ITester.VerifyJsonSerialization{T}(TesterMod)"/>
    public void VerifyJsonSerialization<T>()
    {
        VerifyJsonSerialization(Options.Randomizer.Create<T>());
    }

    /// <inheritdoc cref="ITester.VerifyJsonSerialization{T}(T,TesterMod)"/>
    public void VerifyJsonSerialization<T>(T instance)
    {
        VerifyJsonSerialization(typeof(T), instance);
    }

    /// <param name="type">The <paramref name="instance"/> <see cref="Type"/> for testing.</param>
    /// <inheritdoc cref="VerifyJsonSerialization"/>
    private void VerifyJsonSerialization(Type type, object? instance)
    {
        VerifySerialization(type, instance, new DataContractJsonSerializer(type));
    }

    /// <inheritdoc cref="ITester.VerifyXmlSerialization{T}(TesterMod)"/>
    public void VerifyXmlSerialization<T>()
    {
        VerifyXmlSerialization(Options.Randomizer.Create<T>());
    }

    /// <inheritdoc cref="ITester.VerifyXmlSerialization{T}(T,TesterMod)"/>
    public void VerifyXmlSerialization<T>(T instance)
    {
        VerifyXmlSerialization(typeof(T), instance);
    }

    /// <param name="type">The <paramref name="instance"/> <see cref="Type"/> for testing.</param>
    /// <inheritdoc cref="VerifyXmlSerialization"/>
    private void VerifyXmlSerialization(Type type, object? instance)
    {
        IContentMap contents = Options.Extractor.Extract(instance);
        DataContractSerializer serializer = new(
            type,
            contents.Select(d => d.GetType()).Concat(FindExtraKnownTypes(instance)).Distinct()
        );

        VerifySerialization(type, instance, serializer);
    }

    /// <summary>Finds known types needed for specific types.</summary>
    /// <param name="source">Object being serialized.</param>
    /// <returns>Known types to add.</returns>
    private static IEnumerable<Type> FindExtraKnownTypes(object? source)
    {
        if (source is Exception)
        {
            yield return typeof(Exception[]);
            yield return _ListDictionaryInternalType;
        }
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
            stream.Position = 0;
            result = serializer.ReadObject(stream);
        }
        catch (Exception e) when (e is SerializationException or InvalidDataContractException)
        {
            throw new TesterFailureException(
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
