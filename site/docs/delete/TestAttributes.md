# Test Attributes

For many test frameworks, attributes can create parameter data for test methods. For example, using xUnit:

```c#
/// <summary>Verifies the option works.</summary>
[Theory, RandomData]
public static void AtLeast_Works(int value)
{
    Tools.Asserter.Is(false, Times.AtLeast(value).IsInRange(value - 1));
    Tools.Asserter.Is(true, Times.AtLeast(value).IsInRange(value));
    Tools.Asserter.Is(true, Times.AtLeast(value).IsInRange(value + 1));
}
```

Using the following attribute:

```c#
/// <summary>Populates theory data with random values.</summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class RandomDataAttribute : DataAttribute
{
    /// <summary>Number of times to test the method.</summary>
    public int Trials { get; set; } = 2;

    /// <summary>Generates data for a theory.</summary>
    /// <param name="testMethod">Theory information.</param>
    /// <returns>The generated data.</returns>
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        if (testMethod == null) throw new ArgumentNullException(nameof(testMethod));

        for (int i = 0; i < Math.Max(Trials, 0); i++)
        {
            yield return Tools.Randomizer.CreateFor(testMethod);
        }
    }
}
```
