using Assets.Kanau.Utils;
using LitJson;

namespace Assets.Kanau {
    public interface IJsonExportable
    {
        void ExportJson(JsonWriter writer);
        // TODO - visitor pattern으로 뺴기전 야매 하드코딩

        AFrameNode ExportAFrame();
    }
}
