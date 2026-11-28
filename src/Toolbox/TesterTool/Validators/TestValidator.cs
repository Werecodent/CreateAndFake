using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.CompilerServices;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.TesterTool.Validators;

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
                .FindLoadedSpecificTypes(codeAssembly)
                .Where(t => !t.IsAbstract || t.IsSealed)
                .Where(t => t.Namespace != null)
                .Where(t => ScopeChecker.IsVisible(t, testAssembly.GetName()))
                .Where(t => FindPossibleTestClassNames(t).All(name => !testClasses.Contains(name)))
                .Where(t => !Options.ClassCoverageExclusions.Contains(t.Name))
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

    /// <summary>
    ///     Creates all potential test class names that would qualify
    ///     as code coverage for the <paramref name="codeClass"/>.
    /// </summary>
    /// <param name="codeClass"><see cref="Type"/> to check coverage for.</param>
    /// <returns>The test class names.</returns>
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

        static string stripGeneric(string memberName)
        {
            if (memberName.Contains("`", StringComparison.Ordinal))
            {
                return memberName.Substring(0, memberName.IndexOf("`", StringComparison.Ordinal));
            }
            else
            {
                return memberName;
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
            else if (testName.Contains("_", StringComparison.Ordinal))
            {
                return testName.Substring(0, testName.IndexOf("_", StringComparison.Ordinal));
            }
            else
            {
                return "?";
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
            .. Options.AllowedTestNamePrefixes,
            "New",
            "Tester",
            codeAssembly.GetName()!.Name!,
            testAssembly.GetName()!.Name!,
        ];

        Dictionary<string, List<string>> findTestsByTarget(TypeDescriber t)
        {
            return t
                .Methods.PublicOrInternal.Concat(t.StaticMethods.PublicOrInternal)
                .Where(m => testMarkers.Any(marker => Attribute.IsDefined(m, marker)))
                .Select(m => m.Name)
                .GroupBy(getTarget)
                .Where(g =>
                    !(
                        t.SupportedType!.Name.Contains(g.Key, StringComparison.Ordinal)
                        || globalValidTargets.Contains(g.Key)
                        || (g.Key == "?" && !Options.RequirePrefixInTestName)
                    )
                )
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        List<Tuple<string, Dictionary<string, List<string>>>> testsByTargetByClass =
        [
            .. ScopeChecker
                .FindLoadedSpecificTypes(testAssembly)
                .Select(TypeDescriber.For)
                .Select(t =>
                    Tuple.Create(stripGeneric(t.SupportedType!.Name), findTestsByTarget(t))
                ),
        ];

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
                        .Select(m => stripGeneric(m.Name)),
                ];

            List<string> possibleNames =
            [
                .. FindPossibleTestClassNames(codeClass)
                    .SelectMany(name =>
                        name.StartsWith("I", StringComparison.Ordinal)
                            ? new List<string>() { name, name.Substring(1) }
                            : [name]
                    ),
            ];

            for (int i = 0; i < testsByTargetByClass.Count; i++)
            {
                if (possibleNames.Contains(testsByTargetByClass[i].Item1))
                {
                    foreach (string validTarget in methods)
                    {
                        _ = testsByTargetByClass[i].Item2.Remove(validTarget);
                    }
                }
            }
        }

        Options.Asserter.IsEmpty(
            testsByTargetByClass.SelectMany(t =>
                t.Item2.Values.SelectMany(x => x).Select(v => t.Item1 + " - " + v)
            ),
            $"Invalid test methods for classes from {codeAssembly} in {testAssembly}."
        );
    }
}
