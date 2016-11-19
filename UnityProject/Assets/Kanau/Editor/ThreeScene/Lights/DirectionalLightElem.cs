using Assets.Kanau.UnityScene.SceneGraph;

namespace Assets.Kanau.ThreeScene.Lights {
    public class DirectionalLightElem : LightElem
    {
        public override string Type { get { return "DirectionalLight"; } }
        public override void Accept(IVisitor v) { v.Visit(this); }

        public DirectionalLightElem(LightNode node) : base(node) {
            //this.Name = node.Value.name;
            this.Name = string.Format("{0}_{1}", node.Value.name, Type);
        }
    }
}
