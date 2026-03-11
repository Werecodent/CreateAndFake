using CreateAndFake.Design.Types;
using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.FakerTool;

/// <summary>Manages faking behavior.</summary>
/// <param name="fake">Faked implementation.</param>
public class Fake(IFaked fake)
{
    /// <summary>Faked implementation.</summary>
    public IFaked Dummy { get; } = fake ?? throw new ArgumentNullException(nameof(fake));

    /// <summary>How to compare call data.</summary>
    protected FakerOptions? Options => Dummy.FakeMeta.Options;

    /// <summary>Determines behavior when missing set behavior for a call.</summary>
    public bool ThrowByDefault
    {
        get => Dummy.FakeMeta.ThrowByDefault;
        set => Dummy.FakeMeta.ThrowByDefault = value;
    }

    /// <summary>Ties a method call to fake behavior.</summary>
    /// <param name="methodName">Method name of the call.</param>
    /// <param name="args">Args to match on the call.</param>
    /// <param name="callback">Fake behavior to invoke.</param>
    /// <returns>Representation of the call.</returns>
    public void Setup(string methodName, object[] args, Behavior callback)
    {
        Setup(methodName, [], args, callback);
    }

    /// <summary>Ties a method call to fake behavior.</summary>
    /// <param name="methodName">Method name of the call.</param>
    /// <param name="generics">Generics tied to the call.</param>
    /// <param name="args">Args to match on the call.</param>
    /// <param name="callback">Fake behavior to invoke.</param>
    /// <returns>Representation of the call.</returns>
    public void Setup(string methodName, Type[] generics, object?[] args, Behavior callback)
    {
        Dummy.FakeMeta.SetCallBehavior(new CallData(methodName, generics, args, Options), callback);
    }

    /// <summary>Verifies all behaviors with associated times were called as expected.</summary>
    /// <param name="total">Expected total number of calls to test as well.</param>
    public void VerifyAll(Times? total = null)
    {
        Dummy.FakeMeta.Verify();
        if (total != null)
        {
            VerifyTotalCalls(total);
        }
    }

    /// <summary>Verifies the number of calls made.</summary>
    /// <param name="times">Expected number of calls.</param>
    /// <param name="methodName">Method name of the call.</param>
    /// <param name="args">Args to match on the call.</param>
    public void Verify(Times times, string methodName, object[] args)
    {
        Verify(times, methodName, [], args);
    }

    /// <summary>Verifies the number of calls made.</summary>
    /// <param name="times">Expected number of calls.</param>
    /// <param name="methodName">Method name of the call.</param>
    /// <param name="generics">Generics tied to the call.</param>
    /// <param name="args">Args to match on the call.</param>
    public void Verify(Times times, string methodName, Type[] generics, object?[] args)
    {
        Verify(times, new CallData(methodName, generics, args, Options));
    }

    /// <summary>Verifies the number of calls made.</summary>
    /// <param name="times">Expected number of calls.</param>
    /// <param name="call">Call to verify.</param>
    private void Verify(Times times, CallData call)
    {
        Dummy.FakeMeta.Verify(times, call);
    }

    /// <summary>Verifies the total number of calls made.</summary>
    /// <param name="times">Expected total.</param>
    public void VerifyTotalCalls(Times times)
    {
        Dummy.FakeMeta.VerifyTotalCalls(times);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return TypeHelper.ExpandedName(GetType());
    }
}
