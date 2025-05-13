using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using CreateAndFake.TesterTool;

namespace CreateAndFake.Design.Tests.Tooling;

/// <summary>Handles testing exceptions.</summary>
/// <typeparam name="T">Exception type to test.</typeparam>
public abstract class ExceptionTestBase<T>
    where T : Exception
{
    /// <inheritdoc cref="ITester.PreventsNullRefException"/>
    [Fact]
    public Task Exception_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<T>();
    }

    /// <inheritdoc cref="ITester.PreventsParameterMutation"/>
    [Fact]
    public Task Exception_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<T>();
    }

    /// <summary>Verifies the default constructor is present for serialization but private.</summary>
    [Fact]
    public void Exception_DefaultConstructorPrivate()
    {
        typeof(T).GetConstructor(Type.EmptyTypes).Assert().Is(null);
        Activator.CreateInstance(typeof(T), true).Assert().IsNot(null);
    }

    /// <summary>Verifies that the exception can make an xml roundtrip.</summary>
    [Theory, RandomData]
    public void Exception_XmlSerializes(T original)
    {
        DataContractSerializer formatter = new(
            typeof(T),
            new[] { original.InnerException }.Where(t => t != null).Select(t => t.GetType())
        );

        using MemoryStream stream = new();

        formatter.WriteObject(stream, original);
        _ = stream.Seek(0, SeekOrigin.Begin);

        formatter.ReadObject(stream).Assert().Is(original);
    }

    /// <summary>Verifies that the exception can make a json roundtrip.</summary>
    [Fact]
    public void Exception_JsonSerializes()
    {
        T original;
        do
        {
            original = Tools.Randomizer.Create<T>();
        } while (original.InnerException != null);

        DataContractJsonSerializer formatter = new(typeof(T));

        using MemoryStream stream = new();
        formatter.WriteObject(stream, original);
        _ = stream.Seek(0, SeekOrigin.Begin);

        formatter.ReadObject(stream).Assert().Is(original);
    }
}
