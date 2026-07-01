using System.Collections.Immutable;
using CreateAndFake.Design;
using CreateAndFake.Design.Types;

namespace CreateAndFake.FakerTool;

/// <summary>Instance with dynamically injected fakes.</summary>
/// <typeparam name="T"><see cref="Type"/> being injected by fakes.</typeparam>
public sealed class Injected<T>
{
    /// <summary>Faked implementation.</summary>
    public T Dummy { get; }

    /// <summary>Fakes injected into the dummy.</summary>
    public ImmutableArray<Fake> Fakes { get; }

    /// <inheritdoc cref="Injected{T}"/>
    /// <param name="dummy">Faked implementation.</param>
    /// <param name="fakes">Fakes injected into the dummy.</param>
    public Injected(T dummy, IEnumerable<Fake> fakes)
    {
        ArgumentGuard.ThrowIfNull(dummy, fakes);

        Dummy = dummy;
        Fakes = [.. fakes];
    }

    /// <summary>Finds the <typeparamref name="TInject"/> fake.</summary>
    /// <typeparam name="TInject"><see cref="Type"/> of fake to look for.</typeparam>
    /// <param name="skip">How many fakes of the <typeparamref name="TInject"/> to ignore first.</param>
    /// <returns>The found <typeparamref name="TInject"/> fake.</returns>
    public Fake<TInject> Fake<TInject>(int skip = 0)
    {
        Fake<TInject>? direct = Fakes.OfType<Fake<TInject>>().Skip(skip).FirstOrDefault();

        return direct
            ?? Fakes
                .Where(f => f.Dummy.GetType().Inherits<TInject>())
                .Skip(skip)
                .First()
                .ToFake<TInject>();
    }

    /// <summary>Finds the fake owner of the given dummy.</summary>
    /// <typeparam name="TInject"><see cref="Type"/> of fake to look for.</typeparam>
    /// <param name="dummy">Dummy of a fake to search on.</param>
    /// <returns>The found fake.</returns>
    public Fake<TInject> Fake<TInject>(TInject dummy)
    {
        return Fakes.First(f => ReferenceEquals(f.Dummy, dummy)).ToFake<TInject>();
    }

    /// <summary>Verifies all behaviors in all fakes were called as expected.</summary>
    public void Verify()
    {
        foreach (Fake fake in Fakes)
        {
            fake.Verify();
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return GenericTypeConverter.ExpandedName(GetType());
    }
}
