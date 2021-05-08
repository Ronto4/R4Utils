using NUnit.Framework;

namespace R4UtilsTester
{
    [TestFixture]
    public class TestDemoUtils
    {
        [Test]
        public void TestAdderAdding()
        {
            Assert.AreEqual(4, R4Utils.DemoUtils.Adder.Add(1, 3));
        }

        [Test]
        public void TestAdderNotAddingWrongly()
        {
            Assert.AreNotEqual(3, R4Utils.DemoUtils.Adder.Add(1, 4));
        }
    }
}