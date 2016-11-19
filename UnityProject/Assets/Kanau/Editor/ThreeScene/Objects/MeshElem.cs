using Assets.Kanau.ThreeScene.Geometries;
using Assets.Kanau.ThreeScene.Materials;
using Assets.Kanau.UnityScene.SceneGraph;

namespace Assets.Kanau.ThreeScene.Objects {
    public class MeshElem : Object3DElem
    {
        public override string Type { get { return "Mesh"; } }
        public override void Accept(IVisitor v) { v.Visit(this); }

        public AbstractGeometryElem Geometry { get; set; }
        public MaterialElem Material { get; set; }

        public MeshElem(RenderNode n) {
            //Name = n.CurrentObject.name;
            this.Name = string.Format("{0}_{1}", n.CurrentObject.name, Type);

            // shadow : cast + receive
        }        
    }
}
