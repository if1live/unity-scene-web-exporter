using System;
using Assets.Kanau.AFrameScene;
using Assets.Kanau.UnityScene.SceneGraph;
using Assets.Kanau.Utils;
using LitJson;

namespace Assets.Kanau.ThreeScene {
    public class GroupElem : Object3DElem
    {
        public override string Type { get { return "Group"; } }
        public override void Accept(IVisitor v) { v.Visit(this); }

        public override AFrameNode ExportAFrame() {
            var node = new AFrameNode("a-entity");
            WriteCommonAFrameNode(node);
            return node;
        }

        public GroupElem(IGameObjectNode n) {
            this.Name = n.ToString();

            var go = n.CurrentObject;
            if(go != null) {
                var tr = go.transform;
                this.SetTransform(tr);

                this.Visible = n.ActiveSelf;
            }
        }
    }
}
