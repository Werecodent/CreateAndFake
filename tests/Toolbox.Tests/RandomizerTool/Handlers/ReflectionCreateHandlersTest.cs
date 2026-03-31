using System.Reflection;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.RandomizerTool.Handlers;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.RandomizerTool.Handlers;

public static class ReflectionCreateHandlersTests
{
    private const int _HealthyMin = 8;

    [Fact]
    internal static void ReflectionCreateHandlers_InternalOnly()
    {
        typeof(ReflectionCreateHandlers).IsPublic.Assert().Is(false);
    }

    [Fact]
    internal static void Handlers_HealthyRandomizationPools()
    {
        ReflectionCreateHandlers.PossibleTypes.Count.Assert().GreaterThan(_HealthyMin);
        ReflectionCreateHandlers.PossibleConstructors.Count.Assert().GreaterThan(_HealthyMin);
        ReflectionCreateHandlers.PossibleMethods.Count.Assert().GreaterThan(_HealthyMin);
        ReflectionCreateHandlers.PossibleProperties.Count.Assert().GreaterThan(_HealthyMin);
        ReflectionCreateHandlers.PossibleFields.Count.Assert().GreaterThan(_HealthyMin);
        ReflectionCreateHandlers.PossibleConstants.Count.Assert().GreaterThan(_HealthyMin);
        ReflectionCreateHandlers.PossibleParameters.Count.Assert().GreaterThan(_HealthyMin);
    }

    [Fact]
    internal static async Task Handlers_AllMethodsSupported()
    {
        foreach (
            MethodBase method in ReflectionCreateHandlers
                .PossibleConstructors.Cast<MethodBase>()
                .Concat(ReflectionCreateHandlers.PossibleMethods)
        )
        {
            object instance = null;
            MethodCallWrapper wrapper = null;
            RunResult result = null;
            try
            {
                instance = method.ReflectedType.CreateRandomInstance();

                wrapper = Tools.Runner.CreateFor(method, TestContext.Current.CancellationToken);
                wrapper.CreateDeepClone().Assert().Is(wrapper);

                result = await Tools.Runner.RunAsync(
                    instance,
                    wrapper,
                    TestContext.Current.CancellationToken
                );
            }
            catch (Exception e)
            {
                throw new ToolException($"Method '{method}' encountered an issue.", e);
            }
            finally
            {
                await Disposer.CleanupAsync(instance, wrapper.Args, result.Result);
            }
        }
    }
}
