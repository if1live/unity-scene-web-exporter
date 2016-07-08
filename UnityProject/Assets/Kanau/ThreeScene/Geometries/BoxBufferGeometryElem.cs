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
    }
}
