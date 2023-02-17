using System.Reflection;

namespace Inflop.Shared.Extensions.Types;

public abstract class Enumeration<T> : IEquatable<Enumeration<T>> where T : Enumeration<T>
{
    private static readonly IDictionary<int, T> Enumerations = CreateEnumerations();

    public Enumeration(int value, string name)
    {
        Value = value;
        Name = name;
    }

    public int Value { get; protected init; }

    public string Name { get; protected init; }

    public static T FromValue(int value)
        => Enumerations.TryGetValue(value, out T @enum) ? @enum : default;

    public static T FromName(string name)
        => Enumerations.Values.SingleOrDefault(e => e.Name == name);

    public bool Equals(Enumeration<T> other)
        => other.IsNotNull() && GetType() == other.GetType() && Value == other.Value;

    public override bool Equals(object obj)
        => obj is Enumeration<T> other && Equals(other);

    public override int GetHashCode()
        => Value.GetHashCode();

    private static IDictionary<int, T> CreateEnumerations()
    {
        var type = typeof(T);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                        .Where(fi => type.IsAssignableFrom(fi.FieldType))
                        .Select(fi => (T)fi.GetValue(default));

        return fields.ToDictionary(i => i.Value);
    }
}
