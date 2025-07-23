namespace CreateAndFake.Samples;

[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface,
    Inherited = false
)]
public sealed class InvalidSampleAttribute : Attribute;
