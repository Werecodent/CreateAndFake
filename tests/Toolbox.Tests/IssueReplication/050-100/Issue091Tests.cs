using Werecodent.CreateAndFake.FakerTool;

namespace Werecodent.CreateAndFake.Tests.IssueReplication;

public static class Issue091Tests
{
    internal sealed class Api(ILayer layer)
    {
        public Item FindItem(int key)
        {
            return layer.GetItem(key);
        }

        public Item FindItem(Guid id)
        {
            return layer.GetItem(id);
        }

        public Item FindItem(object key)
        {
            return layer.GetItem(key);
        }
    }

    public interface ILayer
    {
        Item GetItem(int key);

        Item GetItem(Guid id);

        Item GetItem(object key);
    }

    public class Item
    {
        public Guid Id { get; set; }
    }

    [Theory, RandomData]
    internal static void Issue091_FluentArgMatchesInt(
        [Fake] ILayer layer,
        [Inject] Api api,
        Item sample,
        int key
    )
    {
        layer.GetItem(Arg.Any<int>()).SetupReturn(sample, Times.Once);

        api.FindItem(key).Assert().Is(sample);

        layer.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Issue091_FluentArgMatchesGuid(
        [Fake] ILayer layer,
        [Inject] Api api,
        Item sample
    )
    {
        layer.GetItem(Arg.Any<Guid>()).SetupReturn(sample, Times.Once);

        api.FindItem(sample.Id).Assert().Is(sample);

        layer.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Issue091_FluentArgMatchesObject(
        [Fake] ILayer layer,
        [Inject] Api api,
        Item sample,
        object key
    )
    {
        layer.GetItem(Arg.Any<object>()).SetupReturn(sample, Times.Once);

        api.FindItem(key).Assert().Is(sample);

        layer.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Issue091_FluentArgMatchesNull(
        [Fake] ILayer layer,
        [Inject] Api api,
        Item sample
    )
    {
        layer.GetItem(null).SetupReturn(sample, Times.Once);

        api.FindItem(null).Assert().Is(sample);

        layer.Assert().Called();
    }
}
