using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.MSTest.v3.Tests;

[TestClass]
public sealed class AutoTesterTests() : AutoTester(typeof(DataSample));
