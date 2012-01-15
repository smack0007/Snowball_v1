using System;
using NUnit.Framework;

namespace Snowball.Tests
{
    [TestFixture]
    public class MathHelperTests
    {
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(3, 4)]
        [TestCase(5, 8)]
        [TestCase(10, 16)]
        [TestCase(27, 32)]
        [TestCase(63, 64)]
        [TestCase(65, 128)]
        public void NextPowerOf2TestCases(int input, int expected)
        {
            Assert.AreEqual(expected, (int)MathHelper.NextPowerOf2((uint)input));
        }
    }
}
