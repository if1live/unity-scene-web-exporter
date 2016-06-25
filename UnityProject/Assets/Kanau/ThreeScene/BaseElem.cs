using LitJson;
using System;

namespace Assets.Kanau.ThreeScene
{
    public abstract class BaseElem : IThreeElem, IJsonExportable
    {
        protected Guid guid = Guid.NewGuid();
        public string Uuid { get { return guid.ToString().ToUpper(); } }

        public virtual string Name { get; set; }

        public abstract string Type { get; }

        public abstract void ExportJson(JsonWriter writer);
    }
}
