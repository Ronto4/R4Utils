using System;
using System.Collections.Generic;
using NUnit.Framework;
using R4Utils.ValueEqualityCollections;

namespace R4UtilsTester.ValueEqualityCollections;

[TestFixture]
public class TestValueEqualityCollection
{
    public class NonGeneric
    {
        private static void AssertEqual<T>(ValueEqualityCollection<T> wrapper1, ValueEqualityCollection<T> wrapper2,
            bool unequal = false) where T : IEquatable<T>
        {
            if (unequal)
            {
                Assert.AreNotEqual(wrapper1, wrapper2);
                Assert.IsFalse(wrapper1.Equals(wrapper2));
                Assert.IsFalse(wrapper1 == wrapper2);
                Assert.IsTrue(wrapper1 != wrapper2);
            }
            else
            {
                Assert.AreEqual(wrapper1, wrapper2);
                Assert.IsTrue(wrapper1.Equals(wrapper2));
                Assert.IsTrue(wrapper1 == wrapper2);
                Assert.IsFalse(wrapper1 != wrapper2);
            }
        }

        [Test]
        public void TestEqualArraysEqual()
        {
            int[] data1 = [1, 2, 3];
            int[] data2 = [1, 2, 3];
            var wrapper1 = data1.AsOrderedValueEqualityCollection();
            var wrapper2 = data2.AsOrderedValueEqualityCollection();
            AssertEqual(wrapper1, wrapper2);
        }

        [Test]
        public void TestConsidersOrderingOfElements()
        {
            int[] data1 = [1, 2, 3];
            int[] data2 = [2, 1, 3];
            var wrapper1 = data1.AsOrderedValueEqualityCollection();
            var wrapper2 = data2.AsOrderedValueEqualityCollection();
            AssertEqual(wrapper1, wrapper2, true);
        }

        [Test]
        public void TestHandlesSetsCorrectly()
        {
            HashSet<int> data1 = [1, 2, 3];
            HashSet<int> data2 = [2, 1, 3];
            var wrapper1 = data1.AsValueEqualityCollection();
            var wrapper2 = data2.AsValueEqualityCollection();
            AssertEqual(wrapper1, wrapper2);
        }

        [Test]
        public void TestHandlesSetsMixedWithArraysCorrectly()
        {
            HashSet<int> data1 = [1, 2, 3];
            int[] data2 = [2, 1, 3];
            var wrapper1 = data1.AsValueEqualityCollection();
            var wrapper2 = data2.AsOrderedValueEqualityCollection();
            AssertEqual(wrapper1, wrapper2);
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
                    var wrapper1 = data1IgnoreOrdering
                        ? data1.AsValueEqualityCollection()
                        : data1.AsOrderedValueEqualityCollection();
                    var wrapper2 = data2IgnoreOrdering
                        ? data2.AsValueEqualityCollection()
                        : data2.AsOrderedValueEqualityCollection();
                    Assert.AreEqual(wrapper1.Ordering,
                        data1IgnoreOrdering
                            ? ValueEqualityCollection<int>.OrderMode.Ignore
                            : ValueEqualityCollection<int>.OrderMode.Consider);
                    Assert.AreEqual(wrapper2.Ordering,
                        data2IgnoreOrdering
                            ? ValueEqualityCollection<int>.OrderMode.Ignore
                            : ValueEqualityCollection<int>.OrderMode.Consider);
                    if (data1IgnoreOrdering || data2IgnoreOrdering)
                    {
                        AssertEqual(wrapper1, wrapper2);
                    }
                    else
                    {
                        AssertEqual(wrapper1, wrapper2, true);
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
            var wrapper2 = data2.AsOrderedValueEqualityCollection();
            AssertEqual(wrapper1, wrapper2);
        }

        [Test]
        public void TestDuplicateElements()
        {
            bool[] bools = [true, false];
            foreach (var data1IgnoreOrdering in bools)
            {
                foreach (var data2IgnoreOrdering in bools)
                {
                    int[] data1 = [1, 2, 3];
                    int[] data2 = [1, 2, 3, 3];
                    var wrapper1 = data1IgnoreOrdering
                        ? data1.AsValueEqualityCollection()
                        : data1.AsOrderedValueEqualityCollection();
                    var wrapper2 = data2IgnoreOrdering
                        ? data2.AsValueEqualityCollection()
                        : data2.AsOrderedValueEqualityCollection();
                    Assert.AreEqual(wrapper1.Ordering,
                        data1IgnoreOrdering
                            ? ValueEqualityCollection<int>.OrderMode.Ignore
                            : ValueEqualityCollection<int>.OrderMode.Consider);
                    Assert.AreEqual(wrapper2.Ordering,
                        data2IgnoreOrdering
                            ? ValueEqualityCollection<int>.OrderMode.Ignore
                            : ValueEqualityCollection<int>.OrderMode.Consider);
                    AssertEqual(wrapper1, wrapper2, true);
                }
            }
        }
    }

    public class Generic
    {
        private static void AssertEqual<T, TCollection>(ValueEqualityCollection<T, TCollection> wrapper1,
            ValueEqualityCollection<T, TCollection> wrapper2,
            bool unequal = false) where T : IEquatable<T> where TCollection : ICollection<T>
        {
            if (unequal)
            {
                Assert.AreNotEqual(wrapper1, wrapper2);
                Assert.IsFalse(wrapper1.Equals(wrapper2));
                Assert.IsFalse(wrapper1 == wrapper2);
                Assert.IsTrue(wrapper1 != wrapper2);
            }
            else
            {
                Assert.AreEqual(wrapper1, wrapper2);
                Assert.IsTrue(wrapper1.Equals(wrapper2));
                Assert.IsTrue(wrapper1 == wrapper2);
                Assert.IsFalse(wrapper1 != wrapper2);
            }
        }

        [Test]
        public void TestEqualArraysEqual()
        {
            int[] data1 = [1, 2, 3];
            int[] data2 = [1, 2, 3];
            var wrapper1 = data1.AsGenericOrderedValueEqualityCollection<int, int[]>();
            var wrapper2 = data2.AsGenericOrderedValueEqualityCollection<int, int[]>();
            AssertEqual(wrapper1, wrapper2);
        }

        [Test]
        public void TestConsidersOrderingOfElements()
        {
            int[] data1 = [1, 2, 3];
            int[] data2 = [2, 1, 3];
            var wrapper1 = data1.AsGenericOrderedValueEqualityCollection<int, int[]>();
            var wrapper2 = data2.AsGenericOrderedValueEqualityCollection<int, int[]>();
            AssertEqual(wrapper1, wrapper2, true);
        }

        [Test]
        public void TestHandlesSetsCorrectly()
        {
            HashSet<int> data1 = [1, 2, 3];
            HashSet<int> data2 = [2, 1, 3];
            var wrapper1 = data1.AsGenericValueEqualityCollection<int, HashSet<int>>();
            var wrapper2 = data2.AsGenericValueEqualityCollection<int, HashSet<int>>();
            AssertEqual(wrapper1, wrapper2);
        }

        [Test]
        public void TestHandlesSetsMixedWithArraysCorrectly()
        {
            HashSet<int> data1 = [1, 2, 3];
            int[] data2 = [2, 1, 3];
            var wrapper1 = data1.AsGenericValueEqualityCollection<int, ICollection<int>>();
            var wrapper2 = data2.AsGenericValueEqualityCollection<int, ICollection<int>>();
            AssertEqual(wrapper1, wrapper2);
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
                    var wrapper1 = data1IgnoreOrdering
                        ? data1.AsGenericValueEqualityCollection<int, int[]>()
                        : data1.AsGenericOrderedValueEqualityCollection<int, int[]>();
                    var wrapper2 = data2IgnoreOrdering
                        ? data2.AsGenericValueEqualityCollection<int, int[]>()
                        : data2.AsGenericOrderedValueEqualityCollection<int, int[]>();
                    Assert.AreEqual(wrapper1.Ordering,
                        data1IgnoreOrdering
                            ? ValueEqualityCollection<int, int[]>.OrderMode.Ignore
                            : ValueEqualityCollection<int, int[]>.OrderMode.Consider);
                    Assert.AreEqual(wrapper2.Ordering,
                        data2IgnoreOrdering
                            ? ValueEqualityCollection<int, int[]>.OrderMode.Ignore
                            : ValueEqualityCollection<int, int[]>.OrderMode.Consider);
                    if (data1IgnoreOrdering || data2IgnoreOrdering)
                    {
                        AssertEqual(wrapper1, wrapper2);
                    }
                    else
                    {
                        AssertEqual(wrapper1, wrapper2, true);
                    }
                }
            }
        }

        [Test]
        public void TestRecursiveWrapper()
        {
            HashSet<int> data1 = [1, 2, 3];
            int[] data2 = [2, 1, 3];
            var wrapper1 = data1.AsGenericValueEqualityCollection<int, ICollection<int>>()
                .AsGenericValueEqualityCollection<int, ICollection<int>>();
            var wrapper2 = data2.AsGenericValueEqualityCollection<int, ICollection<int>>();
            AssertEqual(wrapper1, wrapper2);
        }

        [Test]
        public void TestDuplicateElements()
        {
            bool[] bools = [true, false];
            foreach (var data1IgnoreOrdering in bools)
            {
                foreach (var data2IgnoreOrdering in bools)
                {
                    int[] data1 = [1, 2, 3];
                    int[] data2 = [1, 2, 3, 3];
                    var wrapper1 = data1IgnoreOrdering
                        ? data1.AsGenericValueEqualityCollection<int, int[]>()
                        : data1.AsGenericOrderedValueEqualityCollection<int, int[]>();
                    var wrapper2 = data2IgnoreOrdering
                        ? data2.AsGenericValueEqualityCollection<int, int[]>()
                        : data2.AsGenericOrderedValueEqualityCollection<int, int[]>();
                    Assert.AreEqual(wrapper1.Ordering,
                        data1IgnoreOrdering
                            ? ValueEqualityCollection<int, int[]>.OrderMode.Ignore
                            : ValueEqualityCollection<int, int[]>.OrderMode.Consider);
                    Assert.AreEqual(wrapper2.Ordering,
                        data2IgnoreOrdering
                            ? ValueEqualityCollection<int, int[]>.OrderMode.Ignore
                            : ValueEqualityCollection<int, int[]>.OrderMode.Consider);
                    AssertEqual(wrapper1, wrapper2, true);
                }
            }
        }
    }
}
