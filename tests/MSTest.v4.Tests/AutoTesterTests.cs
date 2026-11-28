using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.MSTest.v4.Tests;

[TestClass]
public sealed class AutoTesterTests() : AutoTester(typeof(DataSample));
