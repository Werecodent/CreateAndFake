using System.Reflection;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.ExtractorTool.Handlers;

/// <summary>Holds a collection of related handlers.</summary>
internal static class ReflectionExtractHandlers
{
    /// <summary>The collection of related handlers.</summary>
    internal static IEnumerable<IExtractHandler> Handlers { get; } =
    [
        new SelfExtractHandler(RuntimeDetails.RuntimeType),
        new ConvertExtractHandler(
            RuntimeDetails.RuntimeConstructorInfoType,
            data =>
            {
                ConstructorInfo info = (ConstructorInfo)data;
                return [info.Name, info.DeclaringType, info.GetParameters()];
            }
        ),
        new ConvertExtractHandler(
            RuntimeDetails.RuntimeMethodInfoType,
            data =>
            {
                MethodInfo info = (MethodInfo)data;
                return [info.Name, info.DeclaringType, info.ReturnType, info.GetParameters()];
            }
        ),
        new ConvertExtractHandler(
            RuntimeDetails.RuntimePropertyInfoType,
            data =>
            {
                PropertyInfo info = (PropertyInfo)data;
                return [info.Name, info.DeclaringType, info.PropertyType];
            }
        ),
        new ConvertExtractHandler(
            RuntimeDetails.RtFieldInfoType,
            data =>
            {
                FieldInfo info = (FieldInfo)data;
                return [info.Name, info.DeclaringType, info.FieldType];
            }
        ),
        new ConvertExtractHandler(
            RuntimeDetails.MdFieldInfoType,
            data =>
            {
                FieldInfo info = (FieldInfo)data;
                return [info.Name, info.DeclaringType, info.FieldType];
            }
        ),
        new ConvertExtractHandler(
            RuntimeDetails.RuntimeParameterInfoType,
            data =>
            {
                ParameterInfo info = (ParameterInfo)data;
                return [info.Name, info.ParameterType];
            }
        ),
        new ConvertExtractHandler(
            RuntimeDetails.RuntimeAssemblyType,
            data =>
            {
                Assembly info = (Assembly)data;
                return [info.FullName];
            }
        ),
    ];
}
