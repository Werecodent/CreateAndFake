namespace Werecodent.CreateAndFake.Samples.SingleValue;

[ValidSample]
public interface IHolder<T> : IReadableHolder<T>, IWriteableHolder<T>;
