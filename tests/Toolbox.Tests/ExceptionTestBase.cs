using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace CreateAndFake.Tests;

public abstract class ExceptionTestBase<T>
    where T : Exception
{
    [Fact]
    public Task Exception_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<T>(TestContext.Current.CancellationToken);
    }

    [Fact]
    public Task Exception_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<T>(TestContext.Current.CancellationToken);
    }

    [Fact]
    public void Exception_DefaultConstructorPrivate()
    {
        typeof(T).GetConstructor(Type.EmptyTypes).Assert().Is(null);
        Activator.CreateInstance(typeof(T), true).Assert().IsNot(null);
    }

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
