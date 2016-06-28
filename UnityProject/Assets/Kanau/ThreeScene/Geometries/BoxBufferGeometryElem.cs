using Assets.Kanau.UnityScene.Containers;
using Assets.Kanau.Utils;
using LitJson;

namespace Assets.Kanau.ThreeScene.Geometries {
    public class BoxBufferGeometryElem : AbstractGeometryElem {
        public override string Type { get { return "BoxBufferGeometry"; } }

        float height;
        float width;
        float depth;

        public BoxBufferGeometryElem(MeshContainer c) {
            height = 1;
            width = 1;
            depth = 1;
        }

        public override void ExportJson(JsonWriter writer) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                scope.WriteKeyValue("uuid", Uuid);
                scope.WriteKeyValue("type", Type);

                scope.WriteKeyValue("width", width);
                scope.WriteKeyValue("height", height);
                scope.WriteKeyValue("depth", depth);
            }
        }

        public override AFrameNode ExportAFrame() {
            var node = new AFrameNode("a-box");
            node.AddAttribute("width", width.ToString());
            node.AddAttribute("height", height.ToString());
            node.AddAttribute("depth", depth.ToString());
            return node;
        }
    }


}
