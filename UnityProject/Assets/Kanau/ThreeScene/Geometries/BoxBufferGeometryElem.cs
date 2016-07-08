using Assets.Kanau.AFrameScene;
using Assets.Kanau.UnityScene.Containers;

namespace Assets.Kanau.ThreeScene.Geometries {
    public class BoxBufferGeometryElem : AbstractGeometryElem {
        public override string Type { get { return "BoxBufferGeometry"; } }
        public override void Accept(IVisitor v) { v.Visit(this); }


        public float Height;
        public float Width;
        public float Depth;

        public BoxBufferGeometryElem(MeshContainer c) {
            Height = 1;
            Width = 1;
            Depth = 1;
        }

        public override AFrameNode ExportAFrame() {
            var node = new AFrameNode("a-box");
            node.AddAttribute("width", Width.ToString());
            node.AddAttribute("height", Height.ToString());
            node.AddAttribute("depth", Depth.ToString());
            return node;
        }

    }
}
