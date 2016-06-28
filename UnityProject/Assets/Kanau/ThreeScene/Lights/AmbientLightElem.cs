using Assets.Kanau.UnityScene;
using Assets.Kanau.Utils;
using LitJson;

namespace Assets.Kanau.ThreeScene.Lights {
    public class AmbientLightElem : LightElem
    {
        public override string Type { get { return "AmbientLight"; } }

        public AmbientLightElem(ProjectSettings settings) : base() {
            this.UnityColor = settings.AmbientColor;
            this.Name = "AmbientLight";
        }

        public override void ExportJson(JsonWriter writer) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                WriteCommonObjectNode(writer, scope);
                WriteColor(scope);
            }
        }

        public override AFrameNode ExportAFrame() {
            var node = new AFrameNode("a-light");
            WriteCommonAFrameNode(node);
            node.AddAttribute("type", "ambient");
            WriteColor(node);
            return node;
        }
    }
}
