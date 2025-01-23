namespace CreateAndFake;

/// <summary>Flag to create the attached <c>object</c> as a stub.</summary>
/// <seealso cref="Toolbox.RandomizerTool.IRandomizer.CreateFor"/>
/// <seealso cref="Toolbox.FakerTool.IFaker.Stub(Type,IEnumerable{Type})"/>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public sealed class StubAttribute : Attribute { }