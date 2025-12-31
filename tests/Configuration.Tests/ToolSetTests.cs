namespace CreateAndFake.Configuration.Tests;

public static class ToolSetTests
{
    [Fact]
    internal static void CreateViaConfig_Testing()
    {
        ToolSet.CreateViaConfig().Gen.InitialSeed.Assert().Is(2);
    }

    [Theory, RandomData]
    internal static void FindEnvironmentName_PrioritizesSetValue(string value1, string value2)
    {
        ToolSet.FindEnvironmentName().Assert().Is("Production");
        TestEnvironmentName("DOTNET_ENVIRONMENT", value1);
        TestEnvironmentName("ASPNETCORE_ENVIRONMENT", value2);
        ToolSet.FindEnvironmentName().Assert().Is("Production");
    }

    private static void TestEnvironmentName(string name, string value)
    {
        string originalValue = Environment.GetEnvironmentVariable(name);

        Environment.SetEnvironmentVariable(name, value);
        try
        {
            ToolSet.FindEnvironmentName().Assert().Is(value);
        }
        finally
        {
            Environment.SetEnvironmentVariable(name, originalValue);
        }
    }
}
