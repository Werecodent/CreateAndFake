namespace CreateAndFake.Samples.DoubleValue;

[ValidSample]
public interface IHolder<T, TOther> : IReadableHolder<T, TOther>, IWriteableHolder<T, TOther>;
