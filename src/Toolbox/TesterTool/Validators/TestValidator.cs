using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.CompilerServices;
using CreateAndFake.Design;
using CreateAndFake.Design.Types;

namespace CreateAndFake.TesterTool.Validators;

/// <summary>Automates common tests.</summary>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <exception cref="ArgumentNullException">If given a <see langword="null"/> parameter.</exception>
internal sealed class TestValidator(TesterOptions options)
{
    /// <inheritdoc cref="Tester.Options"/>
    internal TesterOptions Options { get; } =
        options ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc cref="ITester.ProvidesTestClassCoverage"/>
    public void ProvidesTestClassCoverage(Assembly codeAssembly, Assembly testAssembly)
    {
        ArgumentGuard.ThrowIfNull(codeAssembly, testAssembly);

        FrozenSet<string> testClasses = testAssembly.GetTypes().Select(t => t.Name).ToFrozenSet();

        Options.Asserter.IsEmpty(
            ScopeChecker
                .FindLoadedClassTypes(codeAssembly)
                .Where(t => !t.IsAbstract || t.IsSealed)
                .Where(t => ScopeChecker.IsVisible(t, testAssembly.GetName()))
                .Where(t => FindPossibleTestClassNames(t).All(name => !testClasses.Contains(name)))
                .Where(t => !Options.TestClassCoverageExceptions.Contains(t.Name))
                .Where(t => !t.Inherits<Delegate>())
                .Where(t =>
                    !t.Namespace!.StartsWith("Microsoft.CodeCoverage", StringComparison.Ordinal)
                )
                .Where(t =>
                    !t.Namespace!.StartsWith(
                        "Coverlet.Core.Instrumentation.Tracker",
                        StringComparison.Ordinal
                    )
                ),
            $"Missing tests for classes from {codeAssembly} in {testAssembly}."
        );
    }

    private IEnumerable<string> FindPossibleTestClassNames(Type codeClass)
    {
        if (
            codeClass.IsGenericTypeDefinition
            && codeClass.Name.Contains("`", StringComparison.Ordinal)
        )
        {
            string baseName = codeClass.Name.Substring(
                0,
                codeClass.Name.IndexOf("`", StringComparison.Ordinal)
            );

            return Options.TestClassNameSuffixes.SelectMany(suffix =>
                Options.TestClassNameGenericSubstitutes.Select(sub => baseName + sub + suffix)
            );
        }
        else
        {
            return Options.TestClassNameSuffixes.Select(suffix => codeClass.Name + suffix);
        }
    }

    /// <inheritdoc cref="ITester.VerifyTestMethodNaming"/>
    internal void VerifyTestMethodNaming(
        IEnumerable<Type> testMarkers,
        Assembly codeAssembly,
        Assembly testAssembly
    )
    {
        ArgumentGuard.ThrowIfNull(testMarkers, codeAssembly, testAssembly);

        List<Type> markers = [.. testMarkers];

        static string stripGeneric(Type codeClass)
        {
            if (
                codeClass.IsGenericTypeDefinition
                && codeClass.Name.Contains("`", StringComparison.Ordinal)
            )
            {
                return codeClass.Name.Substring(
                    0,
                    codeClass.Name.IndexOf("`", StringComparison.Ordinal)
                );
            }
            else
            {
                return codeClass.Name;
            }
        }

        static string getTarget(string testName)
        {
            string cleanedName = testName.StartsWith("Debug_", StringComparison.Ordinal)
                ? testName.Substring(6)
                : testName;

            if (cleanedName.Contains("_", StringComparison.Ordinal))
            {
                return cleanedName.Substring(0, cleanedName.IndexOf("_", StringComparison.Ordinal));
            }
            else
            {
                return cleanedName;
            }
        }

        static IEnumerable<string> getAllTypeNames(Type? codeClass)
        {
            if (codeClass == null || codeClass == typeof(object))
            {
                yield return "Object";
            }
            else
            {
                yield return codeClass.Name;
                foreach (string name in getAllTypeNames(codeClass.BaseType))
                {
                    yield return name;
                }
            }
        }

        ImmutableHashSet<string> globalValidTargets =
        [
            .. Options.TestMethodNameAllowedTargets,
            "New",
            codeAssembly.GetName()!.Name!,
            testAssembly.GetName()!.Name!,
        ];

        Dictionary<string, List<string>> testsByClass = ScopeChecker
            .FindLoadedClassTypes(testAssembly)
            .Where(t => t != null)
            .Where(t =>
                Options.TestClassNameSuffixes.Any(suffix =>
                    t.Name.Contains(suffix, StringComparison.Ordinal)
                )
            )
            .Select(TypeDescriber.For)
            .ToDictionary(
                t => stripGeneric(t.SupportedType!),
                t =>
                    t.Methods.PublicOrInternal.Concat(t.StaticMethods.PublicOrInternal)
                        .Where(m => markers.Exists(marker => Attribute.IsDefined(m, marker)))
                        .Select(m => m.Name)
                        .Where(n =>
                        {
                            string target = getTarget(n);
                            return !(
                                t.SupportedType!.Name.Contains(target, StringComparison.Ordinal)
                                || globalValidTargets.Contains(target)
                            );
                        })
                        .ToList()
            );

        foreach (
            Type codeClass in codeAssembly
                .GetTypes()
                .Where(t => t != null)
                .Where(t => !Attribute.IsDefined(t, typeof(CompilerGeneratedAttribute)))
        )
        {
            ImmutableHashSet<string> methods = codeClass.IsEnum
                ? [.. Enum.GetNames(codeClass), .. getAllTypeNames(codeClass), "Values"]
                :
                [
                    .. getAllTypeNames(codeClass),
                    .. codeClass
                        .GetMembers(
                            BindingFlags.Instance
                                | BindingFlags.Static
                                | BindingFlags.Public
                                | BindingFlags.NonPublic
                        )
                        .Select(m => m.Name)
                        .Select(n =>
                            n.Contains("`", StringComparison.Ordinal)
                                ? n.Substring(0, n.IndexOf("`", StringComparison.Ordinal))
                                : n
                        ),
                ];

            foreach (
                string name in FindPossibleTestClassNames(codeClass)
                    .SelectMany(name =>
                        name.StartsWith("I", StringComparison.Ordinal)
                            ? new List<string>() { name, name.Substring(1) }
                            : [name]
                    )
            )
            {
                if (testsByClass.TryGetValue(name, out List<string>? tests))
                {
                    _ = tests.RemoveAll(n => methods.Contains(getTarget(n)));
                }
            }
        }

        Options.Asserter.IsEmpty(
            testsByClass.SelectMany(t => t.Value.Select(v => t.Key + " - " + v)),
            $"Invalid test methods for classes from {codeAssembly} in {testAssembly}."
        );
    }
}
