using Werecodent.CreateAndFake.Design.Tooling;

namespace Werecodent.CreateAndFake.FakerTool.Engine;

/// <summary>Provides a callback into <see cref="IFaker"/> to fake child values.</summary>
public interface IFakerChainer : IFaker, IToolChainer<FakerOptions, IFakeHint>;
