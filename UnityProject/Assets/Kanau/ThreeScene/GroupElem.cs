using Assets.Kanau.UnityScene.SceneGraph;
using Assets.Kanau.Utils;
using LitJson;
using UnityEngine;

namespace Assets.Kanau.ThreeScene
{
    public class GroupElem : Object3DElem
    {
        public override string Type { get { return "Group"; } }
        public override void ExportJson(JsonWriter writer) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                WriteCommonObjectNode(writer, scope);
            }
        }

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
