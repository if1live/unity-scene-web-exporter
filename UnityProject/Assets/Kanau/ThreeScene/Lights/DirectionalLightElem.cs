using Assets.Kanau.UnityScene.SceneGraph;
using Assets.Kanau.Utils;
using LitJson;
using UnityEngine;

namespace Assets.Kanau.ThreeScene.Lights
{
    public class DirectionalLightElem : LightElem
    {
        public override string Type { get { return "DirectionalLight"; } }

        public DirectionalLightElem(LightNode node) {
            //this.Name = node.Value.name;
            this.Name = string.Format("{0}_{1}", node.Value.name, Type);

            //var dir = node.Direction;
            this.UnityMatrix = Matrix4x4.identity;

            this.Intensity = node.Intensity;
            this.UnityColor = node.Color;
            
            // TODO shadow : cast
        }

        public override void ExportJson(JsonWriter writer) {
            // matrix 조작
            // 유니티에서는 회전된 방향으로 빛을 쏘지만 three.js에서는 카메라의 위치에서 빚을 쏜다
            var forward = Vector3.forward;
            var direction = UnityMatrix.MultiplyVector(forward);
            var mat = Matrix4x4.TRS(-direction, Quaternion.identity, Vector3.one);
            this.UnityMatrix = mat;

            using (var scope = new JsonScopeObjectWriter(writer)) {
                WriteCommonObjectNode(writer, scope);
                scope.WriteKeyValue("color", Color);
                scope.WriteKeyValue("intensity", Intensity);
            }
        }
    }
}
