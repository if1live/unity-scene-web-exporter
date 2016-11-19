using NUnit.Framework;

namespace Assets.Kanau.Utils {
    public class ExportPathHelperTest
    {
        [SetUp]
        public void Init()
        {
            string targetFilePath = @"c:\hello\world\sample.json";
            ExportPathHelper.Instance.UpdateTargetFilePath(targetFilePath);
        }

        [Test]
        public void PropertiesTest()
        {
            var helper = ExportPathHelper.Instance;

            Assert.AreEqual(@"c:\hello\world", helper.RootPath);
            Assert.AreEqual(@"json", helper.Extension);
            Assert.AreEqual(@"sample", helper.Prefix);

            Assert.AreEqual(@"sample.json", helper.SceneFileName);
            Assert.AreEqual(@"c:\hello\world\sample.json", helper.SceneFilePath);
        }
    }
}
