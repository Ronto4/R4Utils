namespace R4Utils.ValueEqualityCollections;

public static class ValueEqualityCollectionsExtensions
{
    /// <summary>
    /// Transform this collection into a collection that performs value equality on its items for deducing collection equality.
    /// <br/><br/>
    /// The resulting will consider ordering whenever compared to another instance also considering ordering.
    /// </summary>
    public static ValueEqualityCollection<T> AsOrderedValueEqualityCollection<T>(this IList<T> collection)
        where T : IEquatable<T> => new(collection, ValueEqualityCollection<T>.OrderMode.Consider);

    /// <summary>
    /// Transform this collection into a collection that performs value equality on its items for deducing collection equality.
    /// <br/><br/>
    /// As the underlying <paramref name="collection"/> does not support ordering (does not inherit <see cref="IList{T}"/>),
    /// the resulting <see cref="ValueEqualityCollection{T}"/> will ignore ordering whenever it is compared.
    /// </summary>
    public static ValueEqualityCollection<T> AsValueEqualityCollection<T>(this ICollection<T> collection)
        where T : IEquatable<T> => new(collection, ValueEqualityCollection<T>.OrderMode.Ignore);
}
