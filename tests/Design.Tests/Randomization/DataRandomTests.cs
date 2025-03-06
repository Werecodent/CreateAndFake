using System.Reflection;
using CreateAndFake.Design.Randomization;

namespace CreateAndFake.Design.Tests.Randomization;

public static class DataRandomTests
{
    [Fact]
    internal static void DataRandom_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<DataRandom>();
    }

    [Fact]
    internal static void DataRandom_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<DataRandom>();
    }

    [Theory, RandomData]
    internal static void DataRandom_MaintainsValues(DataRandom testInstance)
    {
        foreach (PropertyInfo prop in typeof(DataRandom).GetProperties())
        {
            prop.GetValue(testInstance).Assert().Is(prop.GetValue(testInstance));
        }
    }

    [Theory, RandomData]
    internal static void DataRandom_DataVaries(DataRandom testInstance)
    {
        testInstance.CreateVariant().Assert().IsNot(testInstance);
    }

    [Theory, RandomData]
    internal static void Find_AllSearchable(DataRandom testInstance)
    {
        foreach (string item in DataRandom.SupportedProperties)
        {
            testInstance.Find(item).Assert().IsNot(null);
        }
    }

    [Theory, RandomData]
    internal static void Find_IgnoresSpecialChars(DataRandom testInstance)
    {
        testInstance
            .Find("_" + Tools.Gen.NextItem(DataRandom.SupportedProperties))
            .Assert()
            .IsNot(null);
    }

    [Theory, RandomData]
    internal static void Find_MissingName(DataRandom testInstance, string name)
    {
        testInstance.Find(name).Assert().Is(null);
    }
}
