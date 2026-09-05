using System.Reflection;
using Werecodent.CreateAndFake.Design.Randomization;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Design.Tests.Randomization;

public static class DataRandomTests
{
    [Fact]
    internal static Task DataRandom_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<DataRandom>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task DataRandom_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<DataRandom>(
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static void DataRandom_MaintainsValues(DataRandom testInstance)
    {
        foreach (PropertyInfo prop in TypeDescriber.For(typeof(DataRandom)).Properties.All)
        {
            prop.GetValue(testInstance).Assert().Is(prop.GetValue(testInstance));
        }
    }

    [Theory, RandomData]
    internal static void DataRandom_DataVaries(DataRandom testInstance)
    {
        testInstance.Tools().Variant().Assert().IsNot(testInstance);
    }

    [Theory, RandomData]
    internal static void Find_AllSearchable(DataRandom testInstance)
    {
        foreach (string item in DataRandom.SupportedProperties)
        {
            testInstance.Find(item).Assert().IsNotNull();
        }
    }

    [Theory, RandomData]
    internal static void Find_IgnoresSpecialChars(DataRandom testInstance)
    {
        testInstance
            .Find("_" + Tools.Gen.NextItem(DataRandom.SupportedProperties))
            .Assert()
            .IsNotNull();
    }

    [Theory, RandomData]
    internal static void Find_NullWithMissingName(DataRandom testInstance, string name)
    {
        testInstance.Find(name).Assert().IsNull();
    }

    [Theory, RandomData]
    internal static void Find_FieldNamesWork(DataRandom testInstance)
    {
        Type type = typeof(SampleNameData);
        testInstance.Find(type.GetField(nameof(SampleNameData._firstName))).Assert().IsNotNull();
        testInstance.Find(type.GetField(nameof(SampleNameData._lastName))).Assert().IsNotNull();
        testInstance.Find(type.GetField(nameof(SampleNameData._bad))).Assert().IsNull();
    }

    [Theory, RandomData]
    internal static void Find_PropertyNamesWork(DataRandom testInstance)
    {
        Type type = typeof(SampleNameData);
        testInstance.Find(type.GetProperty(nameof(SampleNameData.FirstName))).Assert().IsNotNull();
        testInstance.Find(type.GetProperty(nameof(SampleNameData.LastName))).Assert().IsNotNull();
        testInstance.Find(type.GetProperty(nameof(SampleNameData.Bad))).Assert().IsNull();
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
