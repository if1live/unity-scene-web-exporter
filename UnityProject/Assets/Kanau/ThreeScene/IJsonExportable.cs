using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Kanau.ThreeScene {
    public interface IJsonExportable {
        void ExportJson(JsonWriter writer);
        // TODO - visitor pattern으로 뺴기전 야매 하드코딩
    }
}
