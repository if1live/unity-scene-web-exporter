using Assets.Kanau.UnityScene.Containers;
using Assets.Kanau.Utils;
using LitJson;
using System;

namespace Assets.Kanau.ThreeScene.Textures {
    public class ImageElem : BaseElem
    {
        public override string Type {
            get {
                throw new NotImplementedException("not need");
            }
        }

        TextureContainer texcontainer;

        public ImageElem(TextureContainer c) {
            this.texcontainer = c;
            this.Name = c.FileName;
        }

        public ImageElem(LightmapContainer c) {
            this.Name = c.ExportedFileName(".png");
        }

        public override void ExportJson(JsonWriter writer) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                scope.WriteKeyValue("url", "./" + Name);
                scope.WriteKeyValue("uuid", Uuid);
                scope.WriteKeyValue("name", Name);
            }
        }

        public void ExpoortImageFile(ExportPathHelper pathHelper) {
            if (texcontainer != null) {
                string filename = pathHelper.ToFilePath(Name);
                texcontainer.Save(filename);
            }
        }

        public override AFrameNode ExportAFrame() {
            return null;
        }
    }
}
