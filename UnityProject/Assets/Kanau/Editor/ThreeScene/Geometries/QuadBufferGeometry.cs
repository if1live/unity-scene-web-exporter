using Assets.Kanau.UnityScene.Containers;

namespace Assets.Kanau.ThreeScene.Geometries {
    public class QuadBufferGeometry : AbstractGeometryElem {
        public float Width { get; private set; }
        public float Height { get; private set; }

        public QuadBufferGeometry(MeshContainer c) {
            this.Width = 1;
            this.Height = 1;
        }

        public override string Type { get { return "PlaneBufferGeometry"; } }
        public override void Accept(IVisitor v) { v.Visit(this); }
    }
}
