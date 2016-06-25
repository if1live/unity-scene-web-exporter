using NUnit.Framework;

namespace Assets.Kanau.Utils
{
    public class ConversionsTest
    {
        [Test]
        public void RoundWithDecimalPlaceTest()
        {
            Assert.AreEqual(0f, Conversions.RoundWithDecimalPlace(0.1234f, 0));
            Assert.AreEqual(0.1f, Conversions.RoundWithDecimalPlace(0.1234f, 1));
            Assert.AreEqual(0.12f, Conversions.RoundWithDecimalPlace(0.1234f, 2));
            Assert.AreEqual(0.123f, Conversions.RoundWithDecimalPlace(0.1234f, 3));
        }
    }
}
