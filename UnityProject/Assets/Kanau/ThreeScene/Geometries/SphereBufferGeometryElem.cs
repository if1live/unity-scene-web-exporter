using Assets.Kanau.UnityScene.Containers;
using Assets.Kanau.Utils;
using LitJson;

namespace Assets.Kanau.ThreeScene.Geometries {
    public class SphereBufferGeometryElem : AbstractGeometryElem {
        public override string Type { get { return "SphereBufferGeometry"; } }

        float radius;

        public SphereBufferGeometryElem(MeshContainer c) {
            radius = 0.5f;
        }

        public override void ExportJson(JsonWriter writer) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                scope.WriteKeyValue("uuid", Uuid);
                scope.WriteKeyValue("type", Type);

                scope.WriteKeyValue("radius", radius.ToString());
                scope.WriteKeyValue("widthSegments", 16);
                scope.WriteKeyValue("heightSegments", 16);
            }
        }

        public override AFrameNode ExportAFrame() {
            var node = new AFrameNode("a-sphere");
            node.AddAttribute("radius", radius.ToString());
            return node;
        }
    }
}
