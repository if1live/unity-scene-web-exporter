using Assets.Kanau.ThreeScene;
using Assets.Kanau.ThreeScene.Cameras;
using Assets.Kanau.ThreeScene.Geometries;
using Assets.Kanau.ThreeScene.Lights;
using Assets.Kanau.ThreeScene.Materials;
using Assets.Kanau.ThreeScene.Objects;
using Assets.Kanau.ThreeScene.Textures;
using System;
using System.Diagnostics;

namespace Assets.Kanau.AFrameScene {
    public class AFrameExportVisitor : IVisitor {
        public AFrameNode Node { get; private set; }

        AFrameNodeFactory factory;
        public AFrameExportVisitor(IThreeNodeTable sharedNodeTable) {
            factory = new AFrameNodeFactory(sharedNodeTable);
        }

        public void Visit(QuadBufferGeometry el) { Node = factory.Create(el); }
        public void Visit(AmbientLightElem el) { Node = factory.Create(el); }
        public void Visit(PointLightElem el) { Node = factory.Create(el); }

        public void Visit(MeshElem el) { Node = factory.Create(el); }

        public void Visit(TextureElem el) {
            Debug.Assert(false, "do not reach");
            throw new NotImplementedException();
        }

        public void Visit(SceneElem el) { Node = factory.Create(el); }
        public void Visit(GroupElem el) { Node = factory.Create(el); }

        public void Visit(MetadataElem el) {
            Debug.Assert(false, "do not reach");
            throw new NotImplementedException();
        }

        public void Visit(ImageElem el) {
            Debug.Assert(false, "do not reach");
            throw new NotImplementedException();
        }

        public void Visit(MaterialElem el) {
            Debug.Assert(false, "do not reach");
            throw new NotImplementedException();
        }

        public void Visit(DirectionalLightElem el) { Node = factory.Create(el); }
        public void Visit(SphereBufferGeometryElem el) { Node = factory.Create(el); }
        public void Visit(CylinderBufferGeometryElem el) { Node = factory.Create(el); }
        public void Visit (BoxBufferGeometryElem el) { Node = factory.Create(el); }
        public void Visit (BufferGeometryElem el) { Node = factory.Create(el); }
        public void Visit(PerspectiveCameraElem el) { Node = factory.Create(el); }
    }
}
