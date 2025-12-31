namespace CreateAndFake.Design.Tooling;

/// <summary>Flag to mark the attached property as a value settable via configuration.</summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class ConfigurableOptionAttribute : Attribute;
