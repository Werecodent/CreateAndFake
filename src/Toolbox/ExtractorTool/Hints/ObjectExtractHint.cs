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
        ArgumentGuard.ThrowIfNull(extractor, nameof(extractor));

        if (extractor.AddFoundValue(value))
        {
            BindingFlags scope = extractor.Options.ExtractPrivateMembers
                ? BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic
                : BindingFlags.Public | BindingFlags.Instance;

            Type type = value.GetType();
            foreach (
                PropertyInfo property in TypeDescriber
                    .GetAllProperties(type, scope)
                    .Where(p => p.CanRead)
            )
            {
                _ = extractor.InnerExtract(property.GetValue(value));
            }
            foreach (FieldInfo field in TypeDescriber.GetAllFields(type, scope))
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
