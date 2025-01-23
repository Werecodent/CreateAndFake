namespace CreateAndFake;

/// <summary>Flag to create the attached <c>object</c> as a stub with injected random behavior.</summary>
/// <seealso cref="Toolbox.RandomizerTool.IRandomizer.CreateFor"/>
/// <seealso cref="Toolbox.RandomizerTool.IRandomizer.Inject"/> 
/// <seealso cref="Toolbox.FakerTool.IFaker.Stub(Type,IEnumerable{Type})"/>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public sealed class FakeAttribute : Attribute { }