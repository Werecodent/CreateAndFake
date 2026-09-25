using System.Reflection;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.RandomizerTool.Handlers;
using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Handlers;

public static class ReflectionCreateHandlersTests
{
    private const int _HealthyMin = 6;

    [Fact]
    internal static void ReflectionCreateHandlers_InternalOnly()
    {
        typeof(ReflectionCreateHandlers).IsPublic.Assert().Is(false);
    }

    [Fact]
    internal static void Handlers_HealthyRandomizationPools()
    {
        ReflectionCreateHandlers.PossibleTypes.Assert().HasCountMoreThan(_HealthyMin);
        ReflectionCreateHandlers.PossibleConstructors.Assert().HasCountMoreThan(_HealthyMin);
        ReflectionCreateHandlers.PossibleMethods.Assert().HasCountMoreThan(_HealthyMin);
        ReflectionCreateHandlers.PossibleProperties.Assert().HasCountMoreThan(_HealthyMin);
        ReflectionCreateHandlers.PossibleConstants.Assert().HasCountMoreThan(_HealthyMin);
        ReflectionCreateHandlers.PossibleParameters.Assert().HasCountMoreThan(_HealthyMin);
        ReflectionCreateHandlers
            .PossibleFields.Where(f => f.GetType() == RuntimeDetails.RtFieldInfoType)
            .Assert()
            .HasCountMoreThan(_HealthyMin);
        ReflectionCreateHandlers
            .PossibleFields.Where(f => f.GetType() == RuntimeDetails.MdFieldInfoType)
            .Assert()
            .HasCountMoreThan(_HealthyMin);
    }

    [Fact]
    internal static async Task Handlers_AllMethodsSupported()
    {
        foreach (
            MethodBase method in Enumerable
                .Empty<MethodBase>()
                .Concat(ReflectionCreateHandlers.PossibleMethods)
                .Concat(ReflectionCreateHandlers.PossibleConstructors)
        )
        {
            object instance = null;
            MethodCallWrapper wrapper = null;
            RunResult result = null;
            try
            {
                instance = method.ReflectedType.Tools().CreateRandomInstance();

                wrapper = await Tools.Runner.CreateForAsync(
                    method,
                    TestContext.Current.CancellationToken
                );
                await wrapper
                    .Tools()
                    .Copy()
                    .Assert()
                    .IsAsync(wrapper, TestContext.Current.CancellationToken);

                result = await Tools.Runner.RunAsync(
                    instance,
                    wrapper,
                    TestContext.Current.CancellationToken
                );
            }
            catch (Exception e)
            {
                throw new ToolException(
                    $"Method '{method}' on '{method.DeclaringType}' encountered an issue.",
                    e
                );
            }
            finally
            {
                await Disposer.CleanupAsync(instance, wrapper?.Args, result?.Result);
            }
        }
    }

    [Fact]
    internal static Task PossibleTypes_AllSupported()
    {
        return Tools.Tester.VerifyToolSetSupportAsync(
            ReflectionCreateHandlers.PossibleTypes,
            TestContext.Current.CancellationToken
        );
    }
}
