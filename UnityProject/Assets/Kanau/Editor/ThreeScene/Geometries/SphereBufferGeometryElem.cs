using Assets.Kanau.UnityScene.Containers;

namespace Assets.Kanau.ThreeScene.Geometries {
    public class SphereBufferGeometryElem : AbstractGeometryElem {
        public override string Type { get { return "SphereBufferGeometry"; } }
        public override void Accept(IVisitor v) { v.Visit(this); }

        public float Radius { get; private set; }

        public SphereBufferGeometryElem(MeshContainer c) {
            Radius = 0.5f;
        }
    }
}
