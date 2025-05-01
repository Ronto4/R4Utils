using System.Collections;
using System.Numerics;

namespace R4Utils.ValueEqualityCollections;

/// <summary>
/// A wrapper around an <see cref="ICollection{T}"/> that uses value equality of its elements for equality of the collections.
/// </summary>
public class ValueEqualityCollection<T> : ICollection<T>, IEquatable<ValueEqualityCollection<T>>,
    IEqualityOperators<ValueEqualityCollection<T>, ValueEqualityCollection<T>, bool> where T : IEquatable<T>
{
    private ICollection<T> Collection { get; }

    /// <summary>
    /// Whether to consider ordering when comparing this instance with another one.
    /// <br/><br/>
    /// If any of the two compared collections has this set to <c>true</c>, ordering will be ignored.
    /// <br/><br/>
    /// Regardless of this property, ordering will only be considered if both collections implement <see cref="IList{T}"/>.
    /// </summary>
    public bool IgnoreOrdering { get; }

    /// <summary>
    /// Create a new wrapper around <paramref name="collection"/> that uses items equality for instance equality.
    /// </summary>
    public ValueEqualityCollection(ICollection<T> collection, bool ignoreOrdering = false)
    {
        Collection = collection;
        IgnoreOrdering = ignoreOrdering;
    }

    public override string? ToString() => Collection.ToString();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)Collection).GetEnumerator();
    }

    public static bool operator ==(ValueEqualityCollection<T>? left, ValueEqualityCollection<T>? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(ValueEqualityCollection<T>? left, ValueEqualityCollection<T>? right) =>
        !(left == right);

    public bool Equals(ValueEqualityCollection<T>? other)
    {
        if (other is null) return false;
        return ReferenceEquals(this, other) || EqualityDispatch(other);
    }

    private bool EqualityDispatch(ValueEqualityCollection<T> other)
    {
        if (!IgnoreOrdering && !other.IgnoreOrdering && Collection is IList<T> list1 &&
            other.Collection is IList<T> list2)
        {
            return OrderedEquality(list1, list2);
        }

        return ScrambledEquals(Collection, other.Collection);
    }

    private static bool OrderedEquality(IList<T> list1, IList<T> list2) => list1.SequenceEqual(list2);

    // Source: https://stackoverflow.com/a/3670089/13849454
    private static bool ScrambledEquals(ICollection<T> collection1, ICollection<T> collection2)
    {
        var cnt = new Dictionary<T, int>();
        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var s in collection1)
        {
            if (!cnt.TryAdd(s, 1))
            {
                cnt[s]++;
            }
        }

        foreach (var s in collection2)
        {
            if (cnt.TryGetValue(s, out var value))
            {
                cnt[s] = --value;
            }
            else
            {
                return false;
            }
        }

        return cnt.Values.All(c => c == 0);
    }


    // ICollection<T> implementation

    public IEnumerator<T> GetEnumerator()
    {
        return Collection.GetEnumerator();
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((ValueEqualityCollection<T>)obj);
    }

    public override int GetHashCode()
    {
        return Collection.Aggregate(0, HashCode.Combine);
    }

    public void Add(T item)
    {
        Collection.Add(item);
    }

    public void Clear()
    {
        Collection.Clear();
    }

    public bool Contains(T item)
    {
        return Collection.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        Collection.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        return Collection.Remove(item);
    }

    public int Count => Collection.Count;

    public bool IsReadOnly => Collection.IsReadOnly;
}
