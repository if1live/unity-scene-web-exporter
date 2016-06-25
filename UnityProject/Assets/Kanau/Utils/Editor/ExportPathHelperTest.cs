using NUnit.Framework;

namespace Assets.Kanau.Utils
{
    public class ExportPathHelperTest
    {
        ExportPathHelper helper;

        [SetUp]
        public void Init()
        {
            string targetFilePath = @"c:\hello\world\sample.json";
            helper = new ExportPathHelper(targetFilePath);
        }

        [Test]
        public void PropertiesTest()
        {
            Assert.AreEqual(@"c:\hello\world", helper.RootPath);
            Assert.AreEqual(@"json", helper.Extension);
            Assert.AreEqual(@"sample", helper.Prefix);

            Assert.AreEqual(@"sample.json", helper.SceneFileName);
            Assert.AreEqual(@"c:\hello\world\sample.json", helper.SceneFilePath);
        }
    }
}
