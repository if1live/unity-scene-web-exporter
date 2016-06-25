using Assets.Kanau.ThreeScene.Geometries;
using Assets.Kanau.ThreeScene.Materials;
using Assets.Kanau.UnityScene.SceneGraph;
using Assets.Kanau.Utils;
using LitJson;

namespace Assets.Kanau.ThreeScene.Objects
{
    public class MeshElem : Object3DElem
    {
        public override string Type { get { return "Mesh"; } }

        public AbstractGeometryElem Geometry { get; set; }
        public MaterialElem Material { get; set; }

        public MeshElem(RenderNode n) {
            //Name = n.CurrentObject.name;
            this.Name = string.Format("{0}_{1}", n.CurrentObject.name, Type);

            // shadow : cast + receive
        }

        public override void ExportJson(JsonWriter writer) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                WriteCommonObjectNode(writer, scope);
                scope.WriteKeyValue("geometry", Geometry.Uuid);
                scope.WriteKeyValue("material", Material.Uuid);
            }
        }
    }
}
