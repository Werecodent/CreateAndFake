# IRandom

The `IRandom` classes provide primitive value type randomization without reflection:

```c#
IRandom gen = new FastRandom();
double value1 = gen.Next<double>();
int value2 = gen.Next<int>(0, 5);
```

Provided are three default implementations:

* `FastRandom` - Uses a base that is quick but not cryptographically secure.
* `SecureRandom` - Uses a base that is cryptographically secure but slow.
* `SeededRandom` - Generates deterministic values.

By default only valid values are generated (not NaN or infinities), but that can be changed through the constructor.

## Deterministic

Providing an initial seed and using the `SeededRandom` enables one to get the same random numbers generated between runs, appearing random but giving consistent results:

```c#
/// <summary>Verifies values are deterministic based upon seed.</summary>
[Fact]
public void Seed_Deterministic()
{
    SeededRandom gen = new SeededRandom(5);
    
    Tools.Asserter.Is(-825260579, gen.Next<int>());
    Tools.Asserter.Is(1292641934, gen.Next<int>());
    Tools.Asserter.Is(140109521, gen.Next<int>());

    Tools.Asserter.Is(5, gen.InitialSeed);
    Tools.Asserter.Is(1338796877, gen.Seed);
}
```