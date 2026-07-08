namespace CreateAndFake.Design.Content;

/// <summary>
///     Prevents the <see cref="CreateAndFake"/> framework from dynamically
///     including the attached <see langword="class"/> via reflection.
/// </summary>
/// <remarks>Does not prevent use if directly specified by callers.</remarks>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class ExcludeFromCreateAndFakeAttribute : Attribute;
