namespace CreateAndFake.Toolbox.FakerTool.Proxy;

/// <summary>Represents a faked implementation.</summary>
public interface IFaked
{
    /// <summary>Handles the behavior of the faked object.</summary>
    FakeMetaProvider FakeMeta { get; }
}
