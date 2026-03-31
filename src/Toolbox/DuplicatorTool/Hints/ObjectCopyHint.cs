using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Types;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning objects for <see cref="IDuplicator"/> .</summary>
public sealed class ObjectCopyHint : CopyHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CopyPriority.ObjectHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [];

    /// <inheritdoc/>
    public sealed override CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source, duplicator);

        object? result = Copy(source, duplicator);
        return (result != null) ? new(result) : CopyHintResult.None;
    }

    /// <inheritdoc cref="CopyHint{T}.CopyHint"/>
    private static object? Copy(object source, IDuplicatorChainer duplicator)
    {
        object? dupe = CreateNew(source, duplicator);
        if (dupe == null)
        {
            return dupe;
        }
        duplicator.AddToHistory(source, dupe);

        foreach (
            FieldInfo field in TypeDescriber
                .For(source.GetType())
                .Fields.OnlyPublic.Where(f => !f.IsInitOnly && !f.IsLiteral)
        )
        {
            object? value = field.GetValue(source);
            field.SetValue(dupe, duplicator.Copy(value));
        }

        foreach (
            PropertyInfo property in TypeDescriber
                .For(source.GetType())
                .Properties.OnlyPublic.Where(p => p.CanRead && p.CanWrite)
                .Where(p => p.GetIndexParameters().Length == 0)
        )
        {
            object? value = property.GetValue(source);

            try
            {
                property.SetValue(dupe, duplicator.Copy(value));
            }
            catch (Exception)
            {
                // Bad setter.
            }
        }

        return dupe;
    }

    /// <summary>Creates an instance of <paramref name="source"/>'s <see cref="Type"/>.</summary>
    /// <param name="source">Object whose <see cref="Type"/> is to be created.</param>
    /// <param name="duplicator">Handles callback behavior for child values.</param>
    /// <returns>The created instance.</returns>
    private static object? CreateNew(object source, IDuplicatorChainer duplicator)
    {
        TypeDescriber describer = TypeDescriber.For(source.GetType());

        return describer
            .Constructors.PublicOrInternal.OrderByDescending(c => c.GetParameters().Length)
            .Cast<MethodBase>()
            .Concat(
                describer.Factories.PublicOrInternal.OrderByDescending(c =>
                    c.GetParameters().Length
                )
            )
            .Select(m =>
                TryCreate(source, duplicator, m, describer.Properties.All, describer.Fields.All)
            )
            .FirstOrDefault(o => o != null);
    }

    /// <summary>Attempts to create an instance using a <paramref name="maker"/>.</summary>
    /// <param name="source">Object being cloned.</param>
    /// <param name="duplicator">Handles callback behavior for child values.</param>
    /// <param name="maker">Constructor/factory on <paramref name="source"/> to use.</param>
    /// <param name="props">Properties on <paramref name="source"/>.</param>
    /// <param name="fields">Fields on <paramref name="source"/>.</param>
    /// <returns>Null if failed; created instance otherwise.</returns>
    private static object? TryCreate(
        object source,
        IDuplicatorChainer duplicator,
        MethodBase maker,
        IEnumerable<PropertyInfo> props,
        IEnumerable<FieldInfo> fields
    )
    {
        List<PropertyInfo> propList = [.. props];
        List<FieldInfo> fieldList = [.. fields];

        // Attempts to match members with parameters in the constructor.
        List<MemberInfo> matchedMembers = [];
        foreach (ParameterInfo param in maker.GetParameters())
        {
            PropertyInfo[] potentialProps =
            [
                .. propList.Where(p => p.PropertyType.Inherits(param.ParameterType)),
            ];
            if (potentialProps.Length != 0)
            {
                PropertyInfo? directPropMatch = potentialProps.FirstOrDefault(p =>
                    p.Name.Equals(param.Name, StringComparison.OrdinalIgnoreCase)
                );
                if (directPropMatch != null)
                {
                    _ = propList.Remove(directPropMatch);
                    matchedMembers.Add(directPropMatch);
                }
                else
                {
                    _ = propList.Remove(potentialProps[0]);
                    matchedMembers.Add(potentialProps[0]);
                }
                continue;
            }

            FieldInfo[] potentialFields =
            [
                .. fieldList.Where(f => f.FieldType.Inherits(param.ParameterType)),
            ];
            if (potentialFields.Length != 0)
            {
                FieldInfo? directFieldMatch = potentialFields.FirstOrDefault(f =>
                    f.Name.Equals(param.Name, StringComparison.OrdinalIgnoreCase)
                );
                if (directFieldMatch != null)
                {
                    _ = fieldList.Remove(directFieldMatch);
                    matchedMembers.Add(directFieldMatch);
                }
                else
                {
                    _ = fieldList.Remove(potentialFields[0]);
                    matchedMembers.Add(potentialFields[0]);
                }
                continue;
            }

            return null;
        }

        object?[] finalParameters =
        [
            .. matchedMembers.Select(m => CopyMember(m, source, duplicator)),
        ];

        return (maker is ConstructorInfo constructor)
            ? constructor.Invoke(finalParameters)
            : maker.Invoke(null, finalParameters);
    }

    /// <summary>Copies the value of <paramref name="member"/> on <paramref name="source"/>.</summary>
    /// <param name="member">Property or field to copy.</param>
    /// <param name="source">Instance containing the member.</param>
    /// <param name="duplicator">Duplicator handling the cloning.</param>
    /// <returns>The duplicate object.</returns>
    private static object? CopyMember(
        MemberInfo member,
        object source,
        IDuplicatorChainer duplicator
    )
    {
        if (member is PropertyInfo prop)
        {
            return duplicator.Copy(prop.GetValue(source));
        }
        else
        {
            return duplicator.Copy(((FieldInfo)member).GetValue(source));
        }
    }
}
