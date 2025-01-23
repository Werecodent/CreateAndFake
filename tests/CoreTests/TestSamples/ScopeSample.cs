using System.Diagnostics.CodeAnalysis;

namespace CreateAndFakeTests.TestSamples;

[ExcludeFromCodeCoverage]
public class ScopeSample
{
    public virtual string PublicProp { get; set; }

    internal virtual string InternalProp { get; set; }

    protected internal virtual string ProIntProp { get; set; }

    protected virtual string ProtectProp { get; set; }

    public virtual string InternalGet { internal get; set; }

    public virtual string ProIntGet { protected internal get; set; }

    public virtual string ProtectGet { protected get; set; }

    public virtual string PrivateGet { private get; set; }

    public virtual string InternalSet { get; internal set; }

    public virtual string ProIntSet { get; protected internal set; }

    public virtual string ProtectSet { get; protected set; }

    public virtual string PrivateSet { get; private set; }

    public virtual string PublicMethod() { return "Value"; }

    internal virtual string InternalMethod() { return "Value"; }

    protected internal virtual string ProIntMethod() { return "Value"; }

    protected virtual string ProtectMethod() { return "Value"; }

    public string CallProtectProp()
    {
        return ProtectProp;
    }

    public void SetProtectProp(string data)
    {
        ProtectProp = data;
    }

    public string CallProtectGet()
    {
        return ProtectGet;
    }

    public void SetProtectSet(string data)
    {
        ProtectSet = data;
    }

    public string CallProtectMethod()
    {
        return ProtectMethod();
    }
}
