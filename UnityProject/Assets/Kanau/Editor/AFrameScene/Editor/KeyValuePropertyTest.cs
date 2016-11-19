using NUnit.Framework;

namespace Assets.Kanau.AFrameScene.Editor {
    class KeyValuePropertyTest {
        [Test]
        public void AddTest() {
            var p = new KeyValueProperty();
            p.Add("primitive", "box");
            p.Add("width", 1);
            var expected = "primitive: box; width: 1";
            Assert.AreEqual(expected, p.MakeString());
        }

        [Test]
        public void EmptyTest() {
            var w = new KeyValueProperty();
            Assert.AreEqual("", w.MakeString());
        }
    }

    
}
