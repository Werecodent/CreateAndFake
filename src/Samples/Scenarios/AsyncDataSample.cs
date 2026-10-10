using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Samples.Scenarios;

[ValidSample]
public class AsyncDataSample
{
    public string? StringValue { get; set; }

    public Task<int>? NumberValue { get; set; }

    public IAsyncEnumerable<string?>? CollectionValue { get; set; }

    public async Task WriteToStringValueAsync(Task<string?> newString)
    {
        ArgumentGuard.ThrowIfNull(newString);

        StringValue = await newString.ConfigureAwait(false);
    }

    public async Task<int> ReadFromNumberValueAsync()
    {
        return NumberValue == null ? default : await NumberValue.ConfigureAwait(false);
    }

    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
