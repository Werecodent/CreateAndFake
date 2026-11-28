using System.Diagnostics.CodeAnalysis;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.Samples.SingleValue;

namespace Werecodent.CreateAndFake.Samples.Scenarios;

[ValidSample]
public sealed class GoodOrBadSampleHolder<T>(T instance) : Holder<T>(instance)
    where T : IIsGoodOrBadSample;
