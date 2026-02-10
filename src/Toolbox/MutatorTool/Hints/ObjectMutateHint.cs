using System.Collections;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Randomization;
using CreateAndFake.MutatorTool.Engine;

namespace CreateAndFake.MutatorTool.Hints;

/// <summary>Handles the mutation of data classes.</summary>
public sealed class ObjectMutateHint : MutateHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)MutatePriority.ObjectHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes { get; } = [typeof(object)];

    /// <inheritdoc/>
    protected override bool Supports(object instance)
    {
        return instance is not IEnumerable;
    }

    /// <inheritdoc/>
    protected override bool Modify(object instance, IMutatorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(instance, chainer);

        Type type = instance.GetType();
        DataRandom data = chainer.Options.Gen.NextData();
        bool modified = false;

        foreach (
            PropertyInfo property in chainer.Options.Gen.NextSequence(
                TypeDescriber
                    .GetAllProperties(type, true)
                    .Where(p => p.CanWrite && p.CanRead)
                    .Where(p => p.GetGetMethod() != null)
                    .Where(p => p.GetSetMethod() != null)
            )
        )
        {
            if (modified && chainer.Options.Gen.Next<bool>())
            {
                break;
            }
            try
            {
                property.SetValue(
                    instance,
                    data.Find(property)
                        ?? chainer.Variant(property.PropertyType, property.GetValue(instance))
                );
                modified = true;
            }
            catch (Exception)
            {
                // Failed to modify.
            }
        }

        foreach (
            FieldInfo field in chainer.Options.Gen.NextSequence(
                TypeDescriber.GetAllFields(type, true)
            )
        )
        {
            if (modified && chainer.Options.Gen.Next<bool>())
            {
                break;
            }
            try
            {
                field.SetValue(
                    instance,
                    data.Find(field) ?? chainer.Variant(field.FieldType, field.GetValue(instance))
                );
                modified = true;
            }
            catch (Exception)
            {
                // Failed to modify.
            }
        }

        return modified;
    }
}
