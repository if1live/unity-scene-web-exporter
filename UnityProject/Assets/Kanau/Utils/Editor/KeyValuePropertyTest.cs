using NUnit.Framework;
using UnityEngine;

namespace Assets.Kanau.Utils.Editor {
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

    class Vector3PropertyTest {
        [Test]
        public void SameWithDefaultValue() {
            var p = new Vector3Property(Vector3.one, Vector3.one);
            var expected = "";
            Assert.AreEqual(expected, p.MakeString());
        }

        [Test]
        public void DifferentWithDefaultValue() {
            var p = new Vector3Property(Vector3.one, Vector3.zero);
            var expected = "1 1 1";
            Assert.AreEqual(expected, p.MakeString());
        }
        
        [Test]
        public void NotUseDefaultValue() {
            var p = new Vector3Property(Vector3.zero);
            var expected = "0 0 0";
            Assert.AreEqual(expected, p.MakeString());
        }
    }
}
