using System.Globalization;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;
using Werecodent.CreateAndFake.DuplicatorTool.Hints;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class HandlerCopyHintTests : CopyHintTestBase<HandlerCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        .. new HandlerCopyHint().SupportedTypes.Except([typeof(object)]),
    ];

    public HandlerCopyHintTests()
        : base(_ValidTypes) { }

    [Fact]
    internal void TryCopy_HandlesBaseObject()
    {
        object data = new();
        CopyHintResult result = TestInstance.TryCopy(data, CreateChainer());

        result.Assert().Is(new CopyHintResult(data));
        result.Data.Assert().ReferenceEqual(data);
    }

    [Fact]
    internal void TryCopy_HandlesDateTimeFormatInfoReadability()
    {
        DateTimeFormatInfo readable;
        do
        {
            readable = Tools.Randomizer.Create<DateTimeFormatInfo>();
        } while (!readable.IsReadOnly);

        CopyHintResult readableResult = TestInstance.TryCopy(readable, CreateChainer());
        readableResult.Data.Assert().Is(readable);

        DateTimeFormatInfo unreadable = DateTimeFormatInfo.ReadOnly(readable);
        unreadable.IsReadOnly.Assert().Is(true);

        CopyHintResult unreadableResult = TestInstance.TryCopy(unreadable, CreateChainer());
        unreadableResult.Data.Assert().Is(unreadable);
    }

    [Fact]
    internal void TryCopy_HandlesNumberFormatInfoReadability()
    {
        NumberFormatInfo readable;
        do
        {
            readable = Tools.Randomizer.Create<NumberFormatInfo>();
        } while (!readable.IsReadOnly);

        CopyHintResult readableResult = TestInstance.TryCopy(readable, CreateChainer());
        readableResult.Data.Assert().Is(readable);

        NumberFormatInfo unreadable = NumberFormatInfo.ReadOnly(readable);
        unreadable.IsReadOnly.Assert().Is(true);

        CopyHintResult unreadableResult = TestInstance.TryCopy(unreadable, CreateChainer());
        unreadableResult.Data.Assert().Is(unreadable);
    }

    [Fact]
    internal void TryCopy_HandlesCultureInfoReadability()
    {
        CultureInfo readable;
        do
        {
            readable = Tools.Randomizer.Create<CultureInfo>();
        } while (!readable.IsReadOnly);

        CopyHintResult readableResult = TestInstance.TryCopy(readable, CreateChainer());
        readableResult.Data.Assert().Is(readable);

        CultureInfo unreadable = CultureInfo.ReadOnly(readable);
        unreadable.IsReadOnly.Assert().Is(true);

        CopyHintResult unreadableResult = TestInstance.TryCopy(unreadable, CreateChainer());
        unreadableResult.Data.Assert().Is(unreadable);
    }

    [Fact]
    internal void TryCopy_CancellationTokenSourceSupport()
    {
        using CancellationTokenSource source = new();

        using CancellationTokenSource result = (CancellationTokenSource)
            TestInstance.TryCopy(source, CreateChainer()).Data;

        result.IsCancellationRequested.Assert().Is(false);

        source.Cancel();
        result.IsCancellationRequested.Assert().Is(false);

        using CancellationTokenSource canceledResult = (CancellationTokenSource)
            TestInstance.TryCopy(source, CreateChainer()).Data;

        canceledResult.IsCancellationRequested.Assert().Is(true);
    }
}
