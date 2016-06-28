using Assets.Kanau.UnityScene.SceneGraph;
using Assets.Kanau.Utils;
using LitJson;

namespace Assets.Kanau.ThreeScene.Lights {
    public class PointLightElem : LightElem
    {
        public override string Type { get { return "PointLight"; } }

        

        public PointLightElem(LightNode node) : base(node){
            //this.Name = node.Value.name;
            this.Name = string.Format("{0}_{1}", node.Value.name, Type);            
        }

        public override void ExportJson(JsonWriter writer) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                WriteCommonObjectNode(writer, scope);

                WriteColor(scope);
                WriteIntensity(scope);
                WriteDistance(scope);
                WriteDecay(scope);
            }
        }

        public override AFrameNode ExportAFrame() {
            var node = new AFrameNode("a-light");
            WriteCommonAFrameNode(node);

            node.AddAttribute("type", "point");
            WriteColor(node);
            WriteIntensity(node);
            WriteDecay(node);

            return node;
        }
    }
}
