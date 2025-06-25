namespace CreateAndFake.RunnerTool.Attributes;

/// <summary>Flag to create the attached <see langword="object"/> as a stub.</summary>
/// <seealso cref="IRunner.CreateFor(System.Reflection.MethodBase, IEnumerable{object?}?)"/>
/// <seealso cref="FakerTool.IFaker.Stub(Type,IEnumerable{Type})"/>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public abstract class BaseStubAttribute : Attribute;
