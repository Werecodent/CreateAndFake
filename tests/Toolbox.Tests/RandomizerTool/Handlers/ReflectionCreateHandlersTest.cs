using System.Reflection;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Types;
using CreateAndFake.RandomizerTool.Handlers;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.RandomizerTool.Handlers;

public static class ReflectionCreateHandlersTests
{
    [Fact]
    internal static void ReflectionCreateHandlers_InternalOnly()
    {
        typeof(ReflectionCreateHandlers).IsPublic.Assert().Is(false);
    }

    [Fact]
    internal static async Task PossibleMethods_AllSupported()
    {
        Dictionary<string, Exception> failures = [];
        foreach (MethodBase method in ReflectionCreateHandlers.PossibleMethods)
        {
            try
            {
                object instance = method.ReflectedType.CreateRandomInstance();
                MethodCallWrapper wrapper = Tools.Runner.CreateFor(
                    method,
                    TestContext.Current.CancellationToken
                );
                wrapper.CreateDeepClone().Assert().Is(wrapper);

                await Disposer.CleanupAsync(
                    await Tools.Runner.RunAsync(
                        instance,
                        wrapper,
                        TestContext.Current.CancellationToken
                    )
                );
            }
            catch (Exception e)
            {
                if (e is not UnsupportedException)
                {
                    failures.TryAdd(
                        $"{method} - {TypeDescriber.ExpandedName(method.ReflectedType)}",
                        e
                    );
                }
            }
        }
        failures.Assert().IsEmpty();
    }
}
