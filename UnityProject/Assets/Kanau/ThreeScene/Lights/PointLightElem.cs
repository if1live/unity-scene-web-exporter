using Assets.Kanau.UnityScene.SceneGraph;
using Assets.Kanau.Utils;
using LitJson;

namespace Assets.Kanau.ThreeScene.Lights
{
    public class PointLightElem : LightElem
    {
        public override string Type { get { return "PointLight"; } }

        public float Distance { get; set; }
        public float Decay { get; set; }

        public PointLightElem(LightNode node) {
            //this.Name = node.Value.name;
            this.Name = string.Format("{0}_{1}", node.Value.name, Type);

            this.Intensity = node.Intensity;
            this.UnityColor = node.Color;

            // 유니티로는 point light 거리 무한을 표현할수 없어서
            // 일정범위 이상을 무한으로 취급함
            if (node.Distance >= LightNode.InfiniteDistance) {
                this.Distance = 0;
            } else {
                this.Distance = node.Distance;
            }
            
            // TODO 유니티에는 Decay에 대응되는게 없어서 적당히 설정
            this.Decay = 1;

            // TODO shadow : cast
        }

        public override void ExportJson(JsonWriter writer) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                WriteCommonObjectNode(writer, scope);
                scope.WriteKeyValue("color", Color);
                scope.WriteKeyValue("intensity", Intensity);
                scope.WriteKeyValue("distance", Distance);
                scope.WriteKeyValue("decay", Decay);
            }
        }
    }
}
