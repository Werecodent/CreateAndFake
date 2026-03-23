using System.Collections.Immutable;

namespace CreateAndFake.Design.Randomization.Handlers;

#pragma warning disable CA2263 // Not available in all .NET versions.

/// <inheritdoc/>
internal sealed class DateTimeValueHandler : ValueHandler<DateTime>
{
    /// <summary>All possible <see cref="DateTimeKind"/>s.</summary>
    private static readonly ImmutableArray<DateTimeKind> _Kinds =
    [
        .. Enum.GetValues(typeof(DateTimeKind)).Cast<DateTimeKind>(),
    ];

    /// <inheritdoc/>
    protected override DateTime Create(IRandom gen)
    {
        return Create(gen, DateTime.MinValue, DateTime.MaxValue);
    }

    /// <inheritdoc/>
    protected override DateTime Create(IRandom gen, DateTime min, DateTime max)
    {
        return new DateTime(gen.Next(min.Ticks, max.Ticks), gen.NextItem(_Kinds));
    }
}

#pragma warning restore CA2263
