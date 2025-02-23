namespace CreateAndFake;

/// <summary>Flag to create the attached <c>object</c> as a stub.</summary>
/// <seealso cref="RandomizerTool.IRandomizer.CreateFor(System.Reflection.MethodBase, IEnumerable{object?}?)"/>
/// <seealso cref="FakerTool.IFaker.Stub(Type,IEnumerable{Type})"/>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public abstract class BaseStubAttribute : Attribute { }