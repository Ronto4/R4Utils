namespace R4Utils.ValueEqualityCollections;

public static class Extensions
{
    /// <summary>
    /// Transform this collection into a collection that performs value equality on its items for deducing collection equality.
    /// </summary>
    public static ValueEqualityCollection<T> AsValueEqualityCollection<T>(this IList<T> collection,
        bool ignoreOrdering = false) where T : IEquatable<T> => new(collection, ignoreOrdering);

    /// <summary>
    /// Transform this collection into a collection that performs value equality on its items for deducing collection equality.
    /// <br/><br/>
    /// As the underlying <paramref name="collection"/> does not support ordering (does not inherit <see cref="IList{T}"/>),
    /// the resulting <see cref="ValueEqualityCollection{T}"/> will ignore ordering whenever it is compared.
    /// </summary>
    public static ValueEqualityCollection<T> AsValueEqualityCollection<T>(this ICollection<T> collection) where T : IEquatable<T> => new(collection);
}
