using System.Reflection;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Randomization;

namespace CreateAndFake.Design.Tests.Randomization;

public static class DataRandomTests
{
    [Fact]
    internal static Task DataRandom_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<DataRandom>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task DataRandom_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<DataRandom>(
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static void DataRandom_MaintainsValues(DataRandom testInstance)
    {
        foreach (PropertyInfo prop in TypeDescriber.GetAllProperties(typeof(DataRandom)))
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
    internal static void Find_NullWithMissingName(DataRandom testInstance, string name)
    {
        testInstance.Find(name).Assert().Is(null);
    }

    [Theory, RandomData]
    internal static void Find_FieldNamesWork(DataRandom testInstance)
    {
        Type type = typeof(SampleNameData);
        testInstance.Find(type.GetField(nameof(SampleNameData._firstName))).Assert().IsNot(null);
        testInstance.Find(type.GetField(nameof(SampleNameData._lastName))).Assert().IsNot(null);
        testInstance.Find(type.GetField(nameof(SampleNameData._bad))).Assert().Is(null);
    }

    [Theory, RandomData]
    internal static void Find_PropertyNamesWork(DataRandom testInstance)
    {
        Type type = typeof(SampleNameData);
        testInstance.Find(type.GetProperty(nameof(SampleNameData.FirstName))).Assert().IsNot(null);
        testInstance.Find(type.GetProperty(nameof(SampleNameData.LastName))).Assert().IsNot(null);
        testInstance.Find(type.GetProperty(nameof(SampleNameData.Bad))).Assert().Is(null);
    }

    private sealed class SampleNameData
    {
        public object _firstName = default;

        public string _lastName = default;

        public string _bad = default;

        public object FirstName { get; } = default;

        public string LastName { get; } = default;

        public string Bad { get; } = default;
    }
}
