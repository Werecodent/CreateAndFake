namespace CreateAndFake;

/// <summary>Flag to create the attached <see langword="object"/> as a stub with injected random behavior.</summary>
/// <seealso cref="RunnerTool.IRunner.CreateFor(System.Reflection.MethodBase, IEnumerable{object?}?)"/>
/// <seealso cref="RandomizerTool.IRandomizer.Inject"/>
/// <seealso cref="FakerTool.IFaker.Stub(Type,IEnumerable{Type})"/>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public abstract class BaseFakeAttribute : Attribute;
