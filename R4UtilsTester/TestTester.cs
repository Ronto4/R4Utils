using NUnit.Framework;

namespace R4UtilsTester
{
    public class TestTester
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestPass()
        {
            Assert.Pass();
        }
    }
}