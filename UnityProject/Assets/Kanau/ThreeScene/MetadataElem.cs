using Assets.Kanau.Utils;
using LitJson;

namespace Assets.Kanau.ThreeScene
{
    public class MetadataElem : IJsonExportable
    {
        public float Version { get { return 1f; } }
        public string Type { get { return "Object"; } }
        public string Generator { get { return "Kanau"; } }

        public void ExportJson(JsonWriter writer) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                scope.WriteKeyValue("version", Version);
                scope.WriteKeyValue("type", Type);
                scope.WriteKeyValue("generator", Generator);
            }
        }
    }
}
