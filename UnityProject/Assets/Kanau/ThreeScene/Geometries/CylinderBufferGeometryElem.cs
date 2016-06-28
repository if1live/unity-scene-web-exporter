using Assets.Kanau.UnityScene.Containers;
using Assets.Kanau.Utils;
using LitJson;

namespace Assets.Kanau.ThreeScene.Geometries {
    public class CylinderBufferGeometryElem : AbstractGeometryElem {
        public override string Type { get { return "CylinderBufferGeometry"; } }

        float height;
        float radius;

        public CylinderBufferGeometryElem(MeshContainer c) {
            height = 2;
            radius = 0.5f;
        }

        public override void ExportJson(JsonWriter writer) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                scope.WriteKeyValue("uuid", Uuid);
                scope.WriteKeyValue("type", Type);

                scope.WriteKeyValue("radiusTop", radius);
                scope.WriteKeyValue("radiusBottom", radius);
                scope.WriteKeyValue("height", height);
                scope.WriteKeyValue("radiusSegments", 16);
            }
        }

        public override AFrameNode ExportAFrame() {
            var node = new AFrameNode("a-cylinder");
            node.AddAttribute("height", height.ToString());
            node.AddAttribute("radius", radius.ToString());
            return node;
        }
    }
}
