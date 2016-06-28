using Assets.Kanau.Utils;
using LitJson;
using System;

namespace Assets.Kanau.ThreeScene {
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

        public AFrameNode ExportAFrame() {
            throw new NotImplementedException("A-Frame은 메타데이터가 따로 없다");
        }
    }
}
