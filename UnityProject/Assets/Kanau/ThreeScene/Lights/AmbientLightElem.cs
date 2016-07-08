using Assets.Kanau.AFrameScene;
using Assets.Kanau.UnityScene;

namespace Assets.Kanau.ThreeScene.Lights {
    public class AmbientLightElem : LightElem
    {
        public override string Type { get { return "AmbientLight"; } }
        public override void Accept(IVisitor v) { v.Visit(this); }

        public AmbientLightElem(ProjectSettings settings) : base() {
            this.UnityColor = settings.AmbientColor;
            this.Name = "AmbientLight";
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
