using NUnit.Framework;
using R4Utils.Uri;

namespace R4UtilsTester.Uri;

[TestFixture]
public class TestR4Uri
{
    [Test]
    public void TestConversion()
    {
        var uri = new System.Uri("https://example.com/dir/page.html");
        var r4Uri = R4Uri.Create(uri);
        Assert.AreEqual(uri.AbsoluteUri, ((System.Uri)r4Uri).AbsoluteUri);
    }

    [Test]
    public void TestAppendingPath()
    {
        var targetUri = new System.Uri("https://example.com/dir/page.html");
        var r4Uri = R4Uri.Create("https://example.com") / "dir" / "page.html";
        Assert.AreEqual(targetUri.AbsoluteUri, ((System.Uri)r4Uri).AbsoluteUri);
    }

    [Test]
    public void TestQuery()
    {
        const string targetUri = "https://example.com/dir/page.html?param=value&other=123";
        var r4Uri = R4Uri.Create("https://example.com") / "dir" / "page.html" & (param => "value") & (other => 123);
        Assert.AreEqual(targetUri, r4Uri.ToString());
    }
}
