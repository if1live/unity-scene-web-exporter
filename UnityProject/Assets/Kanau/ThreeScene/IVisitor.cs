using Assets.Kanau.ThreeScene.Cameras;
using Assets.Kanau.ThreeScene.Geometries;
using Assets.Kanau.ThreeScene.Lights;
using Assets.Kanau.ThreeScene.Materials;
using Assets.Kanau.ThreeScene.Objects;
using Assets.Kanau.ThreeScene.Textures;

namespace Assets.Kanau.ThreeScene {
    public interface IVisitor {
        void Visit(PerspectiveCameraElem el);

        void Visit(BufferGeometryElem el);
        void Visit (BoxBufferGeometryElem el);
        void Visit(CylinderBufferGeometryElem el);
        void Visit(QuadBufferGeometry el);
        void Visit(SphereBufferGeometryElem el);

        void Visit(AmbientLightElem el);
        void Visit(DirectionalLightElem el);
        void Visit(PointLightElem el);

        void Visit(MaterialElem el);

        void Visit(MeshElem el);

        void Visit(ImageElem el);
        void Visit(TextureElem el);

        void Visit(MetadataElem el);
        void Visit(SceneElem el);
        void Visit(GroupElem el);
    }
}
