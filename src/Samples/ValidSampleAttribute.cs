namespace Werecodent.CreateAndFake.Samples;

[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface,
    Inherited = false
)]
public sealed class ValidSampleAttribute : Attribute;
