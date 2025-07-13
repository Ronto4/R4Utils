using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace R4Utils.ValueEqualityCollections;

file static class Helpers
{
    // Source: https://stackoverflow.com/a/3670089/13849454
    public static bool ScrambledEquals<T>(ICollection<T> collection1, ICollection<T> collection2) where T : notnull
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
}

/// <summary>
/// A wrapper around an <see cref="ICollection{T}"/> that uses value equality of its elements for equality of the collections.
/// <remarks>Note that this type does not guarantee any behaviour when using it in async code.</remarks>
/// </summary>
public class ValueEqualityCollection<T> : ICollection<T>, IEquatable<ValueEqualityCollection<T>>,
    IEqualityOperators<ValueEqualityCollection<T>, ValueEqualityCollection<T>, bool> where T : IEquatable<T>
{
    /// <summary>
    /// Defines strategies of dealing with ordering when comparing two instances.
    /// </summary>
    public enum OrderMode
    {
        /// <summary>
        /// Consider ordering when comparing this instance with another one.
        /// </summary>
        Consider,

        /// <summary>
        /// Ignore ordering when comparing this instance with another one.
        /// <br/><br/>
        /// If any of the compared instances has this set, the ordering will be ignored.
        /// </summary>
        Ignore,
    }

    private ICollection<T> Collection { get; }

    /// <summary>
    /// Whether to consider ordering when comparing this instance with another one.
    /// <br/><br/>
    /// Regardless of this property, ordering will only be considered if both collections implement <see cref="IList{T}"/>.
    /// </summary>
    public OrderMode Ordering { get; }

    /// <summary>
    /// An assumed unique ID for each instance of this type.
    /// </summary>
    private Guid Guid { get; }

    /// <summary>
    /// Create a new wrapper around <paramref name="collection"/> that uses items equality for instance equality.
    /// </summary>
    public ValueEqualityCollection(ICollection<T> collection, OrderMode ordering)
    {
        Collection = collection;
        Ordering = ordering;
        Guid = Guid.CreateVersion7();
    }

    public override string ToString() =>
        $"{nameof(ValueEqualityCollection<T>)}[{nameof(Ordering)}={Ordering}]({Collection})";

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

    private HashSet<Guid> AlreadyComparing { get; } = [];

    private bool EqualityDispatch(ValueEqualityCollection<T> other)
    {
        if (!AlreadyComparing.Add(other.Guid))
        {
            // In case we are already comparing these two instances, we can safely return true as the first comparison will fail if they don't match.
            return true;
        }

        try
        {
            if (Ordering is OrderMode.Consider && other.Ordering is OrderMode.Consider &&
                Collection is IList<T> list1 &&
                other.Collection is IList<T> list2)
            {
                return OrderedEquality(list1, list2);
            }

            return Helpers.ScrambledEquals(Collection, other.Collection);
        }
        finally
        {
            AlreadyComparing.Remove(other.Guid);
        }
    }

    private static bool OrderedEquality(IList<T> list1, IList<T> list2) => list1.SequenceEqual(list2);

    // ICollection<T> implementation

    public IEnumerator<T> GetEnumerator() => Collection.GetEnumerator();

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((ValueEqualityCollection<T>)obj);
    }

    private bool IsHashing { get; set; } = false;

    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
        if (IsHashing)
        {
            // We are already being hashed in an upper level. Return a constant as we don't want to recurse.
            return 0;
        }

        IsHashing = true;
        try
        {
            return Collection.Aggregate(0, (current, elem) => current ^ elem.GetHashCode());
        }
        finally
        {
            IsHashing = false;
        }
    }

    public void Add(T item) => Collection.Add(item);

    public void Clear() => Collection.Clear();

    public bool Contains(T item) => Collection.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => Collection.CopyTo(array, arrayIndex);

    public bool Remove(T item) => Collection.Remove(item);

    public int Count => Collection.Count;

    public bool IsReadOnly => Collection.IsReadOnly;
}

/// <summary>
/// A wrapper around a <typeparamref name="TCollection"/> that uses value equality of its elements for equality of the collections.
/// <remarks>Note that this type does not guarantee any behaviour when using it in async code.</remarks>
/// </summary>
public class ValueEqualityCollection<T, TCollection> : ICollection<T>,
    IEquatable<ValueEqualityCollection<T, TCollection>>,
    IEqualityOperators<ValueEqualityCollection<T, TCollection>, ValueEqualityCollection<T, TCollection>, bool>
    where T : IEquatable<T> where TCollection : ICollection<T>
{
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ValueEqualityCollection<T, TCollection>)obj);
    }

    private bool IsHashing { get; set; } = false;

    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
        if (IsHashing)
        {
            // We are already being hashed in an upper level. Return a constant as we don't want to recurse.
            return 0;
        }

        IsHashing = true;
        try
        {
            return Underlying.Aggregate(0, (current, elem) => current ^ elem.GetHashCode());
        }
        finally
        {
            IsHashing = false;
        }
    }

    /// <summary>
    /// Defines strategies of dealing with ordering when comparing two instances.
    /// </summary>
    public enum OrderMode
    {
        /// <summary>
        /// Consider ordering when comparing this instance with another one.
        /// </summary>
        Consider,

        /// <summary>
        /// Ignore ordering when comparing this instance with another one.
        /// <br/><br/>
        /// If any of the compared instances has this set, the ordering will be ignored.
        /// </summary>
        Ignore,
    }

    /// <summary>
    /// The collection used to create this instance.
    /// </summary>
    public TCollection Underlying { get; }

    /// <summary>
    /// Whether to consider ordering when comparing this instance with another one.
    /// <br/><br/>
    /// This is always <see cref="OrderMode.Ignore"/> unless <typeparamref name="TCollection"/> inherits <see cref="IList{T}"/>.
    /// </summary>
    public OrderMode Ordering { get; }

    /// <summary>
    /// An assumed unique ID for each instance of this type.
    /// </summary>
    private Guid Guid { get; }

    internal ValueEqualityCollection(TCollection underlying, OrderMode ordering)
    {
        Underlying = underlying;
        Ordering = underlying is IList<T> ? ordering : OrderMode.Ignore;
        Guid = Guid.CreateVersion7();
    }

    // Equality
    public bool Equals(ValueEqualityCollection<T, TCollection>? other)
    {
        if (other is null) return false;
        return ReferenceEquals(this, other) || EqualityDispatch(other);
    }

    private HashSet<Guid> AlreadyComparing { get; } = [];

    private bool EqualityDispatch(ValueEqualityCollection<T, TCollection> other)
    {
        if (!AlreadyComparing.Add(other.Guid))
        {
            // In case we are already comparing these two instances, we can safely return true as the first comparison will fail if they don't match.
            return true;
        }

        try
        {
            if (Ordering is OrderMode.Consider && other.Ordering is OrderMode.Consider &&
                Underlying is IList<T> list1 &&
                other.Underlying is IList<T> list2)
            {
                return OrderedEquality(list1, list2);
            }

            return Helpers.ScrambledEquals(Underlying, other.Underlying);
        }
        finally
        {
            AlreadyComparing.Remove(other.Guid);
        }
    }

    private static bool OrderedEquality(IList<T> list1, IList<T> list2) => list1.SequenceEqual(list2);

    public static bool operator ==(ValueEqualityCollection<T, TCollection>? left,
        ValueEqualityCollection<T, TCollection>? right) => right?.Equals(left) ?? left is null;

    public static bool operator !=(ValueEqualityCollection<T, TCollection>? left,
        ValueEqualityCollection<T, TCollection>? right) => !(left == right);

    // Collection
    public IEnumerator<T> GetEnumerator() => Underlying.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Underlying).GetEnumerator();

    public void Add(T item) => Underlying.Add(item);

    public void Clear() => Underlying.Clear();

    public bool Contains(T item) => Underlying.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => Underlying.CopyTo(array, arrayIndex);

    public bool Remove(T item) => Underlying.Remove(item);

    public int Count => Underlying.Count;

    public bool IsReadOnly => Underlying.IsReadOnly;
}
