using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Reflection;
using System.Text;
using CreateAndFake.AsserterTool;
using CreateAndFake.AsyncAsserterTool;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Reiteration;
using CreateAndFake.Design.Tooling;
using CreateAndFake.DuplicatorTool;
using CreateAndFake.FakerTool.Proxy;
using CreateAndFake.RandomizerTool;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.TesterTool;

/// <summary>Configuration for controlling automated testing behavior.</summary>
public sealed record TesterOptions : IToolOptions
{
    /// <summary>Core value random handler.</summary>
    public required IRandom Gen { get; init; }

    /// <summary>Creates objects and populates them with random values.</summary>
    public required IRandomizer Randomizer { get; init; }

    /// <summary>Deep clones objects.</summary>
    public required IDuplicator Duplicator { get; init; }

    /// <summary>Handles common test scenarios.</summary>
    public required IAsserter Asserter { get; init; }

    /// <summary>Handles common test scenarios.</summary>
    public required IAsyncAsserter AsyncAsserter { get; init; }

    /// <summary>Handles method generation.</summary>
    public required IRunner Runner { get; init; }

    /// <summary>Retries tests if timeout is reached.</summary>
    public Limiter Limiter { get; init; } = Limiter.Few;

    /// <summary>Values to inject into called methods.</summary>
    public ImmutableArray<object?> InjectionValues { get; init; } = [];

    /// <summary>If constructors are included when running tests on classes.</summary>
    public bool IncludeConstructors { get; init; } = true;

    /// <summary>If class methods are included when running tests on classes.</summary>
    public bool IncludeInstanceMethods { get; init; } = true;

    /// <summary>If static methods are included when running tests on classes.</summary>
    public bool IncludeStaticMethods { get; init; } = true;

    /// <summary>If internal members are included when running tests on classes.</summary>
    public bool IncludeInternals { get; init; } = true;

    /// <summary>Common suffix attached to class names to name the test classes.</summary>
    public string TestClassNameSuffix { get; init; } = "Tests";

    /// <summary>Possible strings replacing generics in a type name for coverage tests.</summary>
    public ImmutableArray<string> TestClassNameGenericSubstitutes { get; init; } = ["", "_T_"];

    /// <summary>Method used to convert parameters to a test name.</summary>
    public Func<object?, string> TestDisplayNameConverter { get; init; } = o => o?.ToString() ?? "";

    /// <summary>Types to ignore for test class coverage tests.</summary>
    public FrozenSet<string> TestClassCoverageExceptions { get; init; } =
        FrozenSet.ToFrozenSet<string>([]);

    /// <summary>Names of methods to skip when running tests on classes.</summary>
    public FrozenSet<string> MethodsToIgnore { get; init; } =
        FrozenSet.ToFrozenSet(["Finalize", "Dispose", "DisposeAsync", "PrintMembers"]);

    /// <summary>Exceptions that are safe to ignore when running tests on classes.</summary>
    public FrozenSet<Type> IgnorableExceptions { get; init; } =
        FrozenSet.ToFrozenSet([
            typeof(InsufficientExecutionStackException),
            typeof(WaitHandleCannotBeOpenedException),
            typeof(InvalidFilterCriteriaException),
            typeof(MulticastNotSupportedException),
            typeof(TargetParameterCountException),
            typeof(ArgumentOutOfRangeException),
            typeof(InsufficientMemoryException),
            typeof(UnauthorizedAccessException),
            typeof(EntryPointNotFoundException),
            typeof(OperationCanceledException),
            typeof(ArrayTypeMismatchException),
            typeof(InvalidOperationException),
            typeof(TargetInvocationException),
            typeof(AccessViolationException),
            typeof(IndexOutOfRangeException),
            typeof(EncoderFallbackException),
            typeof(DecoderFallbackException),
            typeof(DataMisalignedException),
            typeof(ObjectDisposedException),
            typeof(BadImageFormatException),
            typeof(NotImplementedException),
            typeof(ContextMarshalException),
            typeof(MissingMethodException),
            typeof(NullReferenceException),
            typeof(TaskSchedulerException),
            typeof(MissingFieldException),
            typeof(ArgumentNullException),
            typeof(MemberAccessException),
            typeof(NotSupportedException),
            typeof(EndOfStreamException),
            typeof(KeyNotFoundException),
            typeof(InvalidCastException),
            typeof(PathTooLongException),
            typeof(ThreadStateException),
            typeof(DllNotFoundException),
            typeof(FakeVerifyException),
            typeof(AggregateException),
            typeof(ArgumentException),
            typeof(FakeCallException),
            typeof(OverflowException),
            typeof(TypeLoadException),
            typeof(TimeoutException),
            typeof(FormatException),
            typeof(AssertException),
            typeof(TargetException),
            typeof(SystemException),
            typeof(RankException),
            typeof(ToolException),
        ]);

    /// <inheritdoc/>
    public override string ToString()
    {
        return nameof(TesterOptions);
    }
}
