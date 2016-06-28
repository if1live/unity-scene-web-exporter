using Assets.Kanau.UnityScene.Containers;
using Assets.Kanau.Utils;
using LitJson;

namespace Assets.Kanau.ThreeScene.Geometries {
    public class QuadBufferGeometry : AbstractGeometryElem {
        float width;
        float height;

        public QuadBufferGeometry(MeshContainer c) {
            width = 1;
            height = 1;
        }

        public override string Type { get { return "PlaneBufferGeometry"; } }

        public override void ExportJson(JsonWriter writer) {
            using(var scope = new JsonScopeObjectWriter(writer)) {
                scope.WriteKeyValue("uuid", Uuid);
                scope.WriteKeyValue("type", Type);

                scope.WriteKeyValue("width", width);
                scope.WriteKeyValue("height", height);
            }
        }

        public override AFrameNode ExportAFrame() {
            var node = new AFrameNode("a-plane");
            node.AddAttribute("width", width.ToString());
            node.AddAttribute("height", height.ToString());
            return node;
        }
    }
}
