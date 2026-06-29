using CreateAndFake.Design;
using CreateAndFake.FakerTool.Engine;

namespace CreateAndFake.FakerTool;

/// <inheritdoc cref="IFaker"/>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
public sealed class Faker(FakerOptions options) : IFaker
{
    /// <summary>Handles hint based faking.</summary>
    private static readonly FakerEngine _engine = new();

    /// <inheritdoc/>
    public FakerOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public IEnumerable<Type> SupportedTypes => _engine.SupportedTypes;

    /// <inheritdoc/>
    public bool Supports<T>(FakerMod? optionConfiguration = null)
    {
        return new FakerChainer(Options, _engine).Supports<T>(optionConfiguration);
    }

    /// <inheritdoc/>
    public bool Supports(Type type, FakerMod? optionConfiguration = null)
    {
        return new FakerChainer(Options, _engine).Supports(type, optionConfiguration);
    }

    /// <inheritdoc/>
    public Fake<T> Mock<T>(params IEnumerable<Type> interfaces)
    {
        return new FakerChainer(Options, _engine).Mock<T>(interfaces);
    }

    /// <inheritdoc/>
    public Fake<T> Mock<T>(IEnumerable<Type> interfaces, FakerMod? optionConfiguration)
    {
        return new FakerChainer(Options, _engine).Mock<T>(interfaces, optionConfiguration);
    }

    /// <inheritdoc/>
    public Fake Mock(Type parent, params IEnumerable<Type> interfaces)
    {
        return new FakerChainer(Options, _engine).Mock(parent, interfaces);
    }

    /// <inheritdoc/>
    public Fake Mock(Type parent, IEnumerable<Type> interfaces, FakerMod? optionConfiguration)
    {
        return new FakerChainer(Options, _engine).Mock(parent, interfaces, optionConfiguration);
    }

    /// <inheritdoc/>
    public Fake<T> Stub<T>(params IEnumerable<Type> interfaces)
    {
        return new FakerChainer(Options, _engine).Stub<T>(interfaces);
    }

    /// <inheritdoc/>
    public Fake<T> Stub<T>(IEnumerable<Type> interfaces, FakerMod? optionConfiguration)
    {
        return new FakerChainer(Options, _engine).Stub<T>(interfaces, optionConfiguration);
    }

    /// <inheritdoc/>
    public Fake Stub(Type parent, params IEnumerable<Type> interfaces)
    {
        return new FakerChainer(Options, _engine).Stub(parent, interfaces);
    }

    /// <inheritdoc/>
    public Fake Stub(Type parent, IEnumerable<Type> interfaces, FakerMod? optionConfiguration)
    {
        return new FakerChainer(Options, _engine).Stub(parent, interfaces, optionConfiguration);
    }

    /// <inheritdoc/>
    public Injected<T> InjectMocks<T>(FakerMod? optionConfiguration = null)
    {
        return new FakerChainer(Options, _engine).InjectMocks<T>(optionConfiguration);
    }

    /// <inheritdoc/>
    public Injected<T> InjectMocks<T>(
        IEnumerable<object> values,
        FakerMod? optionConfiguration = null
    )
    {
        return new FakerChainer(Options, _engine).InjectMocks<T>(values, optionConfiguration);
    }

    /// <inheritdoc/>
    public Injected<T> InjectStubs<T>(FakerMod? optionConfiguration = null)
    {
        return new FakerChainer(Options, _engine).InjectStubs<T>(optionConfiguration);
    }

    /// <inheritdoc/>
    public Injected<T> InjectStubs<T>(
        IEnumerable<object> values,
        FakerMod? optionConfiguration = null
    )
    {
        return new FakerChainer(Options, _engine).InjectStubs<T>(values, optionConfiguration);
    }

    /// <inheritdoc/>
    public IFaker WithOptions(FakerMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new Faker(optionConfiguration.Invoke(Options));
    }
}
