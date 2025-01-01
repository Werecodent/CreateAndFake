using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Toolbox.AsserterTool;
using CreateAndFake.Toolbox.DuplicatorTool;
using CreateAndFake.Toolbox.FakerTool.Proxy;
using CreateAndFake.Toolbox.RandomizerTool;

namespace CreateAndFake.Toolbox.TesterTool;

/// <summary>Configuration for controlling automated testing behavior.</summary>
public record TesterOptions : IToolOptions
{
    /// <summary>Core value random handler.</summary>
    public required IRandom Gen { get; init; }

    /// <summary>Creates objects and populates them with random values.</summary>
    public required IRandomizer Randomizer { get; init; }

    /// <summary>Deep clones objects.</summary>
    public required IDuplicator Duplicator { get; init; }

    /// <summary>Handles common test scenarios.</summary>
    public required IAsserter Asserter { get; init; }

    /// <summary>Retries tests if timeout is reached.</summary>
    public Limiter Limiter { get; init; } = Limiter.Few;

    /// <summary>How long to wait for tests to complete.</summary>
    public TimeSpan Timeout { get; init; } = new(0, 0, 6);

    /// <summary>Values to inject into called methods.</summary>
    public ImmutableArray<object?> InjectionValues { get; init; } = [];

    /// <summary>If constructors are included when running tests on classes.</summary>
    public bool IncludeConstructors { get; init; } = true;

    /// <summary>If class methods are included when running tests on classes.</summary>
    public bool IncludeInstanceMethods { get; init; } = true;

    /// <summary>If static methods are included when running tests on classes.</summary>
    public bool IncludeStaticMethods { get; init; } = true;

    /// <summary>Exceptions that are safe to ignore when running tests on classes.</summary>
    public FrozenSet<Type> IgnorableExceptions { get; init; } = FrozenSet.ToFrozenSet([
        typeof(TargetParameterCountException),
        typeof(ArgumentOutOfRangeException),
        typeof(InvalidOperationException),
        typeof(NotImplementedException),
        typeof(MissingMethodException),
        typeof(NullReferenceException),
        typeof(TaskSchedulerException),
        typeof(ArgumentNullException),
        typeof(MemberAccessException),
        typeof(NotSupportedException),
        typeof(KeyNotFoundException),
        typeof(InvalidCastException),
        typeof(FakeVerifyException),
        typeof(ArgumentException),
        typeof(FakeCallException),
        typeof(OverflowException),
        typeof(TimeoutException),
        typeof(AssertException),
        typeof(TargetException)]);
}