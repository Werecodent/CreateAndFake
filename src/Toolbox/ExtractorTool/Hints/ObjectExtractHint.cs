using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.ExtractorTool.Engine;

namespace CreateAndFake.ExtractorTool.Hints;

/// <summary>Handles extracting objects for <see cref="IExtractor"/>.</summary>
public sealed class ObjectExtractHint : ExtractHint<object>
{
    /// <inheritdoc/>
    protected override bool Extract(object value, IExtractorChainer extractor)
    {
        ArgumentGuard.ThrowIfNull(extractor);

        if (extractor.AddFoundValue(value))
        {
            Type type = value.GetType();
            foreach (
                PropertyInfo property in TypeDescriber
                    .GetAllProperties(type, !extractor.Options.ExtractPrivateMembers)
                    .Where(p => p.CanRead)
            )
            {
                _ = extractor.InnerExtract(property.GetValue(value));
            }
            foreach (
                FieldInfo field in TypeDescriber.GetAllFields(
                    type,
                    !extractor.Options.ExtractPrivateMembers
                )
            )
            {
                _ = extractor.InnerExtract(field.GetValue(value));
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
