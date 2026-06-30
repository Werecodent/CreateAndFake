using CreateAndFake.Design;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.FakerTool.Engine;

/// <summary>Provides a callback into <see cref="IFaker"/> to fake child values.</summary>
public sealed class FakerChainer
    : ToolChainer<FakerChainer, IFakerEngine, FakerOptions, IFakeHint>,
        IFakerChainer
{
    /// <inheritdoc/>
    public FakerChainer(FakerOptions options, IFakerEngine engine)
        : base(options, engine) { }

    /// <inheritdoc/>
    private FakerChainer(FakerOptions options, FakerChainer prevChainer)
        : base(options, prevChainer) { }

    /// <inheritdoc/>
    protected override FakerChainer CreateSubChainer(FakerOptions subOptions)
    {
        return new FakerChainer(subOptions, this);
    }

    /// <inheritdoc/>
    public bool Supports<T>(FakerMod? optionConfiguration = null)
    {
        return Supports(typeof(T), optionConfiguration);
    }

    /// <inheritdoc/>
    public bool Supports(Type type, FakerMod? optionConfiguration = null)
    {
        return Engine.Supports(type, GetSubChainer(optionConfiguration));
    }

    /// <inheritdoc/>
    public Fake<T> Mock<T>(params IEnumerable<Type> interfaces)
    {
        return Mock<T>(interfaces, null);
    }

    /// <inheritdoc/>
    public Fake<T> Mock<T>(IEnumerable<Type> interfaces, FakerMod? optionConfiguration)
    {
        return new Fake<T>(Engine.Mock(typeof(T), interfaces, GetSubChainer(optionConfiguration)));
    }

    /// <inheritdoc/>
    public Fake Mock(Type parent, params IEnumerable<Type> interfaces)
    {
        return Mock(parent, interfaces, null);
    }

    /// <inheritdoc/>
    public Fake Mock(Type parent, IEnumerable<Type> interfaces, FakerMod? optionConfiguration)
    {
        return Engine.Mock(parent, interfaces, GetSubChainer(optionConfiguration));
    }

    /// <inheritdoc/>
    public Fake<T> Stub<T>(params IEnumerable<Type> interfaces)
    {
        return Stub<T>(interfaces, null);
    }

    /// <inheritdoc/>
    public Fake<T> Stub<T>(IEnumerable<Type> interfaces, FakerMod? optionConfiguration)
    {
        Fake<T> fake = Mock<T>(interfaces, optionConfiguration);
        fake.ThrowByDefault = false;
        return fake;
    }

    /// <inheritdoc/>
    public Fake Stub(Type parent, params IEnumerable<Type> interfaces)
    {
        return Stub(parent, interfaces, null);
    }

    /// <inheritdoc/>
    public Fake Stub(Type parent, IEnumerable<Type> interfaces, FakerMod? optionConfiguration)
    {
        return Engine.Stub(parent, interfaces, GetSubChainer(optionConfiguration));
    }

    /// <inheritdoc/>
    public Injected<T> InjectMocks<T>(FakerMod? optionConfiguration = null)
    {
        return InjectMocks<T>([], optionConfiguration);
    }

    /// <inheritdoc/>
    public Injected<T> InjectMocks<T>(
        IEnumerable<object> values,
        FakerMod? optionConfiguration = null
    )
    {
        return Engine.InjectMocks<T>(values, GetSubChainer(optionConfiguration));
    }

    /// <inheritdoc/>
    public Injected<T> InjectStubs<T>(FakerMod? optionConfiguration = null)
    {
        return InjectStubs<T>([], optionConfiguration);
    }

    /// <inheritdoc/>
    public Injected<T> InjectStubs<T>(
        IEnumerable<object> values,
        FakerMod? optionConfiguration = null
    )
    {
        return Engine.InjectStubs<T>(values, GetSubChainer(optionConfiguration));
    }

    /// <inheritdoc/>
    public IFaker WithOptions(FakerMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new Faker(optionConfiguration.Invoke(Options));
    }
}
