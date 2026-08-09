using Werecodent.CreateAndFake.Samples.SingleValue;

namespace Werecodent.CreateAndFake.Samples.DoubleValue;

[ValidSample]
public interface IReadableHolder<out T, out TOther> : IReadableHolder<T>
{
    TOther OtherValue { get; }

    TOther ReadOtherValue();
}
