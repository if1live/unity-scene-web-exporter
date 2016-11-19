namespace Assets.Kanau.ThreeScene {
    public class SceneElem : Object3DElem
    {
        public override string Type { get { return "Scene"; } }
        public override void Accept(IVisitor v) { v.Visit(this); }
    }
}
