namespace CreateAndFake.Toolbox.FakerTool;

/// <summary>Niceties for fakes.</summary>
public static class FakeExtensions
{
    /// <summary>Switches the fake to a different type.</summary>
    /// <typeparam name="T">Alternative base to cast to.</typeparam>
    /// <param name="baseFake">Created fake with the extra type.</param>
    /// <returns>Fake to test with.</returns>
    public static Fake<T> ToFake<T>(this Fake baseFake)
    {
        if (baseFake is Fake<T> fake)
        {
            return fake;
        }
        else
        {
            return new Fake<T>(baseFake);
        }
    }
}
