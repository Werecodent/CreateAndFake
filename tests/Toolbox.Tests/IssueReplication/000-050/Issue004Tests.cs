using CreateAndFake.FakerTool;

namespace CreateAndFake.Tests.IssueReplication;

public static class Issue004Tests
{
    public interface IBaseHolder
    {
        void GetValueUnset();
    }

    public abstract class BaseHolder
    {
        internal static readonly int _TestValue = Tools.Randomizer.Create<int>();

        public int ValueHolder { get; set; }

        public virtual int GetValueA()
        {
            return _TestValue;
        }

        public virtual int GetValueB(int input)
        {
            return input;
        }

        public virtual void GetValueC()
        {
            ValueHolder = _TestValue;
        }

        public virtual void GetValueD(int input)
        {
            ValueHolder = input;
        }

        public virtual T GetValueE<T>(T input)
        {
            return input;
        }

        public virtual void ThrowError(Exception e)
        {
            throw e;
        }

        public abstract void GetValueUnset();
    }

    [Theory, RandomData]
    internal static void Issue004_MockCanCallBaseA(Fake<BaseHolder> sample)
    {
        sample.Setup(d => d.GetValueA(), Behavior.Base<int>());
        sample.Dummy.GetValueA().Assert().Is(BaseHolder._TestValue);
        sample.Verify();
    }

    [Theory, RandomData]
    internal static void Issue004_MockCanCallBaseB(Fake<BaseHolder> sample, int value)
    {
        sample.Setup(d => d.GetValueB(value), Behavior.Base<int>());
        sample.Dummy.GetValueB(value).Assert().Is(value);
        sample.Verify();
    }

    [Theory, RandomData]
    internal static void Issue004_MockCanCallBaseC(Fake<BaseHolder> sample)
    {
        sample.Setup(d => d.GetValueC(), Behavior.Base());
        sample.Dummy.GetValueC();
        sample.Dummy.ValueHolder.Assert().Is(BaseHolder._TestValue);
        sample.Verify();
    }

    [Theory, RandomData]
    internal static void Issue004_MockCanCallBaseD(Fake<BaseHolder> sample, int value)
    {
        sample.Setup(d => d.GetValueD(value), Behavior.Base());
        sample.Dummy.GetValueD(value);
        sample.Dummy.ValueHolder.Assert().Is(value);
        sample.Verify();
    }

    [Theory, RandomData]
    internal static void Issue004_MockCanCallBaseE(Fake<BaseHolder> sample, int value)
    {
        sample.Setup(d => d.GetValueE(value), Behavior.Base<int>());
        sample.Dummy.GetValueE(value).Assert().Is(value);
        sample.Verify();
    }

    [Theory, RandomData]
    internal static void Issue004_MockCanCallBaseThrow(Fake<BaseHolder> sample, Exception e)
    {
        sample.Setup(d => d.ThrowError(e), Behavior.Base());
        sample.Dummy.Assert(x => x.ThrowError(e)).Throws<Exception>().That.Is(e);
        sample.Verify();
    }

    [Theory, RandomData]
    internal static void Issue004_MockCallBaseAbstractInvalid(Fake<BaseHolder> sample)
    {
        sample.Setup(d => d.GetValueUnset(), Behavior.Base());
        sample.Dummy.Assert(x => x.GetValueUnset()).Throws<InvalidOperationException>();
    }

    [Theory, RandomData]
    internal static void Issue004_MockCallBaseWrongTypeInvalid(Fake<IBaseHolder> sample)
    {
        sample.Setup(d => d.GetValueUnset(), Behavior.Base());
        sample.Dummy.Assert(x => x.GetValueUnset()).Throws<MissingMethodException>();
    }
}
