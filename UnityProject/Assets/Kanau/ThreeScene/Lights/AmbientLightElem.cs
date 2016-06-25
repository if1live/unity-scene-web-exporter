using Assets.Kanau.UnityScene;
using Assets.Kanau.Utils;
using LitJson;

namespace Assets.Kanau.ThreeScene.Lights
{
    public class AmbientLightElem : LightElem
    {
        public override string Type { get { return "AmbientLight"; } }

        public AmbientLightElem(ProjectSettings settings) {
            this.UnityColor = settings.AmbientColor;
            this.Name = "AmbientLight";
            this.Intensity = 1;
        }

        public override void ExportJson(JsonWriter writer) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                WriteCommonObjectNode(writer, scope);
                scope.WriteKeyValue("color", Color);
                scope.WriteKeyValue("intensity", Intensity);
            }
        }
    }
}
