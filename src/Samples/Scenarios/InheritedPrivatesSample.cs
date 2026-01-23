using System;

namespace CreateAndFake.Samples.Scenarios;

[ValidSample]
public sealed class InheritedPrivatesSample(string stringValue)
    : PrivateValuerEquatableSample(stringValue) { }
