using LitJson;

namespace Assets.Kanau
{
    public interface IJsonExportable
    {
        void ExportJson(JsonWriter writer);
    }
}
