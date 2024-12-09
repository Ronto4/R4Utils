using System.Linq.Expressions;
using System.Web;

namespace R4Utils.Uri;

public class R4Uri
{
    protected string CurrentUri = string.Empty;

    protected R4Uri()
    {
    }

    protected R4Uri(string uri)
    {
        CurrentUri = uri;
    }

    protected R4Uri(System.Uri uri)
    {
        CurrentUri = uri.ToString();
    }

    public static R4UriPath Create() => new();
    public static R4UriPath Create(string uri) => new(uri);
    public static R4UriPath Create(System.Uri uri) => new(uri);

    public override string ToString() => ((System.Uri)this).ToString();

    // public static implicit operator string(R4Uri uri) => HttpUtility.UrlEncode(uri.CurrentUri);
    public static implicit operator System.Uri(R4Uri uri) => new(uri.CurrentUri);

    public static implicit operator R4Uri(string uri) => new(uri);
    public static implicit operator R4Uri(System.Uri uri) => new(uri);
}

public class R4UriPath : R4Uri
{
    internal R4UriPath() : base()
    {
    }

    internal R4UriPath(string uri) : this(new System.Uri(uri))
    {
    }

    internal R4UriPath(System.Uri uri) : base(uri)
    {
        if (!string.IsNullOrWhiteSpace(uri.Query)) throw new ArgumentException("Uri cannot contain query strings.");
    }

    public static R4UriPath operator /(R4UriPath uri, System.Uri relativeUri) => uri / relativeUri.ToString();

    public static R4UriPath operator /(R4UriPath uri, string relativeUri)
    {
        uri.CurrentUri = $"{uri.CurrentUri.TrimEnd('/')}/{HttpUtility.UrlEncode(relativeUri.TrimStart('/'))}";
        return uri;
    }

    public static R4UriQuery operator &(R4UriPath uri, QueryParameter queryParameter) =>
        new R4UriQuery(uri) & queryParameter;
    
    public static R4UriQuery operator &(R4UriPath uri, Expression<Func<object?, object?>> lambda) => uri & (QueryParameter)lambda;
}

public class QueryParameter
{
    private readonly string _name;
    private readonly string _value;
    internal readonly bool Skip;

    private QueryParameter(string name, string? value)
    {
        _name = name;
        _value = value ?? string.Empty;
        Skip = value is null;
    }

    public override string ToString() => $"{_name}={System.Uri.EscapeDataString(_value)}";

    public static QueryParameter FromExpression(Expression<Func<object?, object?>> lambda) => new(
        lambda.Parameters[0].Name ?? throw new NullReferenceException("Lambda's first parameter has no name."),
        lambda.Compile().Invoke(null)?.ToString());

    public static implicit operator QueryParameter(Expression<Func<object?, object?>> lambda) => FromExpression(lambda);
}

public class R4UriQuery : R4Uri
{
    internal R4UriQuery(R4UriPath uri) : base(uri)
    {
        CurrentUri += "?";
    }

    public static R4UriQuery operator &(R4UriQuery uri, QueryParameter queryParameter)
    {
        if (queryParameter.Skip) return uri;
        if (!uri.CurrentUri.EndsWith('?')) uri.CurrentUri += '&';
        uri.CurrentUri += queryParameter;
        return uri;
    }

    public static R4UriQuery operator &(R4UriQuery uri, Expression<Func<object?, object?>> lambda) => uri & (QueryParameter)lambda;
}
