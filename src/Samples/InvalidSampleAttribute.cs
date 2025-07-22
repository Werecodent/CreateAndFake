namespace CreateAndFake.Samples;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
public sealed class InvalidSampleAttribute : Attribute;
