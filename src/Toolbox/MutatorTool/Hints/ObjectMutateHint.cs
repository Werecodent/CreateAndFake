using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using CreateAndFake.Design;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Types;
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

        object getNewValue(Type memberType, object? originalValue, string? smartValue)
        {
            return (smartValue?.Equals(originalValue as string, StringComparison.Ordinal) != false)
                ? chainer.Variant(memberType, originalValue)
                : smartValue;
        }

        foreach (
            PropertyInfo prop in chainer.Options.Gen.NextSequence(
                InheritanceTracker
                    .For(type)
                    .Properties.SetAndGetable.Where(p =>
                        p.GetSetMethod()
                            ?.ReturnParameter.GetRequiredCustomModifiers()
                            .Contains(typeof(IsExternalInit)) == false
                    )
            )
        )
        {
            if (modified && chainer.Options.Gen.Next<bool>())
            {
                break;
            }
            try
            {
                prop.SetValue(
                    instance,
                    getNewValue(prop.PropertyType, prop.GetValue(instance), data.Find(prop))
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
                InheritanceTracker.For(type).Fields.Writable
            )
        )
        {
            if (modified && chainer.Options.Gen.Next<bool>())
            {
                break;
            }

            field.SetValue(
                instance,
                getNewValue(field.FieldType, field.GetValue(instance), data.Find(field))
            );
            modified = true;
        }

        return modified;
    }
}
