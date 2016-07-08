using Assets.Kanau.AFrameScene;
using System;

namespace Assets.Kanau.ThreeScene {
    public class SceneElem : Object3DElem
    {
        public override string Type { get { return "Scene"; } }
        public override void Accept(IVisitor v) { v.Visit(this); }

        public override AFrameNode ExportAFrame() {
            var node = new AFrameNode("a-scene");
            WriteCommonAFrameNode(node);

            return node;
        }
    }
}
