namespace CreateAndFake;

/// <summary>Flag to create the attached collection with <paramref name="count"/> items.</summary>
/// <param name="count"><inheritdoc cref="Count" path="/summary"/></param>
/// <seealso cref="Toolbox.RandomizerTool.IRandomizer.CreateFor"/>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public sealed class SizeAttribute(int count) : Attribute
{
    /// <summary>Number of items to generate and populate the attached collection with.</summary>
    public int Count { get; } = count;
}