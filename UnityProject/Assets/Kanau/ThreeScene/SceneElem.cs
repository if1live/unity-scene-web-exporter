using Assets.Kanau.Utils;
using LitJson;

namespace Assets.Kanau.ThreeScene {
    public class SceneElem : Object3DElem
    {
        public override string Type { get { return "Scene"; } }

        public override void ExportJson(JsonWriter writer) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                WriteCommonObjectNode(writer, scope);
            }
        }

        public override AFrameNode ExportAFrame() {
            var node = new AFrameNode("a-scene");
            WriteCommonAFrameNode(node);

            return node;
        }
    }
}
