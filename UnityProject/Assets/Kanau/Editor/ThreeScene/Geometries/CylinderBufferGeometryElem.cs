using Assets.Kanau.UnityScene.Containers;

namespace Assets.Kanau.ThreeScene.Geometries {
    public class CylinderBufferGeometryElem : AbstractGeometryElem {
        public override string Type { get { return "CylinderBufferGeometry"; } }
        public override void Accept(IVisitor v) { v.Visit(this); }

        public float Height { get; private set; }
        public float Radius { get; private set; }

        public CylinderBufferGeometryElem(MeshContainer c) {
            this.Height = 2;
            this.Radius = 0.5f;
        }
    }
}