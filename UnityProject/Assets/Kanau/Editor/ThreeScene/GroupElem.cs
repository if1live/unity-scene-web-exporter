using Assets.Kanau.UnityScene.SceneGraph;

namespace Assets.Kanau.ThreeScene {
    public class GroupElem : Object3DElem
    {
        public override string Type { get { return "Group"; } }
        public override void Accept(IVisitor v) { v.Visit(this); }

        public GroupElem(IGameObjectNode n) {
            this.Name = n.ToString();

            var go = n.CurrentObject;
            if(go != null) {
                var tr = go.transform;
                this.SetTransform(tr);

                this.Visible = n.ActiveSelf;
            }
        }
    }
}
