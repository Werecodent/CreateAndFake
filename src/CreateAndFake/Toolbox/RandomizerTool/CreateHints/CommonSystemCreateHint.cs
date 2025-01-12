using System.Collections.Frozen;
using System.Globalization;
using System.Reflection;
using System.Text;
using CreateAndFake.Design;

namespace CreateAndFake.Toolbox.RandomizerTool.CreateHints;

/// <summary>Handles randomizing common instances for <see cref="IRandomizer"/>.</summary>
public sealed class CommonSystemCreateHint : CreateHint
{
    /// <summary>Supported types and the methods used to generate them.</summary>
    private static readonly FrozenDictionary<Type, Func<RandomizerChainer, object>> _Gens
        = new Dictionary<Type, Func<RandomizerChainer, object>>()
        {
            { typeof(CultureInfo), rand => rand.Options.Gen.NextItem(CultureInfo.GetCultures(CultureTypes.AllCultures)) },
            { typeof(TimeSpan), rand => new TimeSpan(rand.Options.Gen.Next<long>()) },
            { typeof(DateTime), rand => new DateTime(rand.Options.Gen.Next(DateTime.MinValue.Ticks, DateTime.MaxValue.Ticks)) },
            { typeof(DateTimeOffset), rand => new DateTimeOffset(rand.Create<DateTime>()) },
            { typeof(Guid), rand => new Guid(Enumerable.Range(0, 16).Select(i => rand.Create<byte>()).ToArray()) },

            { typeof(Assembly), rand => rand.Options.Gen.NextItem(AppDomain.CurrentDomain.GetAssemblies()) },
            { typeof(AssemblyName), rand => rand.Create<Assembly>()!.GetName() },
            { typeof(Type).GetType(), rand => rand.Create<Type>()! },
            { typeof(Type), rand => rand.Options.Gen.NextItem(Assembly.GetExecutingAssembly().GetTypes()) },

            { typeof(Uri), rand => rand.Create<UriBuilder>()!.Uri },
            { typeof(UriBuilder), rand => new UriBuilder(
                rand.Create<bool>() ? "http" : "https", rand.Create<string>(), rand.Options.Gen.Next(-1, 65535)) },

            { typeof(StringBuilder), rand => new StringBuilder(rand.Create<string>()) },

            { typeof(ConstructorInfo), rand => FindTypeInfo(rand, t => t.GetConstructors()) },
            { typeof(PropertyInfo), rand => FindTypeInfo(rand, t => t.GetProperties()) },
            { typeof(MethodInfo), rand => FindTypeInfo(rand, t => t.GetMethods()) },
            { typeof(MemberInfo), rand => FindTypeInfo(rand, t => t.GetMembers()) },
            { typeof(FieldInfo), rand => FindTypeInfo(rand, t => t.GetFields()) },
            { typeof(ParameterInfo), rand => FindTypeInfo(rand,
                t => t.GetMethods().SelectMany(m => m.GetParameters()).ToArray()) },
            { typeof(MethodBase), rand => FindTypeInfo(rand,
                t => t.GetConstructors().Cast<MethodBase>().Concat(t.GetMethods()).ToArray()) },
        }.ToFrozenDictionary();

    /// <inheritdoc/>
    protected internal override (bool, object?) TryCreate(Type type, RandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));

        if (type != null && _Gens.TryGetValue(type, out Func<RandomizerChainer, object?>? gen))
        {
            return (true, gen.Invoke(randomizer));
        }
        else
        {
            return (false, null);
        }
    }

    /// <summary>Finds a random member info.</summary>
    /// <typeparam name="T"><c>Type</c> being found.</typeparam>
    /// <param name="randomizer">Handles randomizing child values.</param>
    /// <param name="grabber">How members are found on a <c>Type</c>.</param>
    /// <returns>The found member.</returns>
    private static T FindTypeInfo<T>(RandomizerChainer randomizer, Func<Type, T[]> grabber)
    {
        T[] result;
        do
        {
            result = grabber.Invoke((Type)_Gens[typeof(Type)].Invoke(randomizer));
        } while (result.Length == 0);

        return randomizer.Options.Gen.NextItem(result);
    }
}
