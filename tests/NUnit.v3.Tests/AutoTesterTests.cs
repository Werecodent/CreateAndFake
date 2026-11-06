using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.NUnit.v3.Tests;

[TestFixture]
public sealed class AutoTesterTests() : AutoTester(typeof(DataSample));
