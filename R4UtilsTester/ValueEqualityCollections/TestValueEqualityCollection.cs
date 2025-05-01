using System.Collections.Generic;
using NUnit.Framework;
using R4Utils.ValueEqualityCollections;

namespace R4UtilsTester.ValueEqualityCollections;

[TestFixture]
public class TestValueEqualityCollection
{
    [Test]
    public void TestEqualArraysEqual()
    {
        int[] data1 = [1, 2, 3];
        int[] data2 = [1, 2, 3];
        var wrapper1 = data1.AsValueEqualityCollection();
        var wrapper2 = data2.AsValueEqualityCollection();
        Assert.AreEqual(wrapper1, wrapper2);
        Assert.IsTrue(wrapper1.Equals(wrapper2));
        Assert.IsTrue(wrapper1 == wrapper2);
    }

    [Test]
    public void TestConsidersOrderingOfElements()
    {
        int[] data1 = [1, 2, 3];
        int[] data2 = [2, 1, 3];
        var wrapper1 = data1.AsValueEqualityCollection();
        var wrapper2 = data2.AsValueEqualityCollection();
        Assert.AreNotEqual(wrapper1, wrapper2);
        Assert.IsFalse(wrapper1.Equals(wrapper2));
        Assert.IsFalse(wrapper1 == wrapper2);
    }

    [Test]
    public void TestHandlesSetsCorrectly()
    {
        HashSet<int> data1 = [1, 2, 3];
        HashSet<int> data2 = [2, 1, 3];
        var wrapper1 = data1.AsValueEqualityCollection();
        var wrapper2 = data2.AsValueEqualityCollection();
        Assert.AreEqual(wrapper1, wrapper2);
        Assert.IsTrue(wrapper1.Equals(wrapper2));
        Assert.IsTrue(wrapper1 == wrapper2);
    }

    [Test]
    public void TestHandlesSetsMixedWithArraysCorrectly()
    {
        HashSet<int> data1 = [1, 2, 3];
        int[] data2 = [2, 1, 3];
        var wrapper1 = data1.AsValueEqualityCollection();
        var wrapper2 = data2.AsValueEqualityCollection();
        Assert.AreEqual(wrapper1, wrapper2);
        Assert.IsTrue(wrapper1.Equals(wrapper2));
        Assert.IsTrue(wrapper1 == wrapper2);
    }

    [Test]
    public void TestIgnoreOrdering()
    {
        bool[] bools = [true, false];
        foreach (var data1IgnoreOrdering in bools)
        {
            foreach (var data2IgnoreOrdering in bools)
            {
                int[] data1 = [1, 2, 3];
                int[] data2 = [2, 1, 3];
                var wrapper1 = data1.AsValueEqualityCollection(data1IgnoreOrdering);
                var wrapper2 = data2.AsValueEqualityCollection(data2IgnoreOrdering);
                Assert.AreEqual(wrapper1.IgnoreOrdering, data1IgnoreOrdering);
                Assert.AreEqual(wrapper2.IgnoreOrdering, data2IgnoreOrdering);
                if (data1IgnoreOrdering || data2IgnoreOrdering)
                {
                    Assert.AreEqual(wrapper1, wrapper2);
                    Assert.IsTrue(wrapper1.Equals(wrapper2));
                    Assert.IsTrue(wrapper1 == wrapper2);
                }
                else
                {
                    Assert.AreNotEqual(wrapper1, wrapper2);
                    Assert.IsFalse(wrapper1.Equals(wrapper2));
                    Assert.IsFalse(wrapper1 == wrapper2);
                }
            }
        }
    }

    [Test]
    public void TestRecursiveWrapper()
    {
        HashSet<int> data1 = [1, 2, 3];
        int[] data2 = [2, 1, 3];
        var wrapper1 = data1.AsValueEqualityCollection().AsValueEqualityCollection();
        var wrapper2 = data2.AsValueEqualityCollection();
        Assert.AreEqual(wrapper1, wrapper2);
        Assert.IsTrue(wrapper1.Equals(wrapper2));
        Assert.IsTrue(wrapper1 == wrapper2);
    }

    [Test]
    public void TestDuplicateElements()
    {
        int[] data1 = [1, 2, 3];
        int[] data2 = [1, 2, 3, 3];
        var wrapper1 = data1.AsValueEqualityCollection(true);
        var wrapper2 = data2.AsValueEqualityCollection(true);
        Assert.AreNotEqual(wrapper1, wrapper2);
        Assert.IsFalse(wrapper1.Equals(wrapper2));
        Assert.IsFalse(wrapper1 == wrapper2);
    }
}
