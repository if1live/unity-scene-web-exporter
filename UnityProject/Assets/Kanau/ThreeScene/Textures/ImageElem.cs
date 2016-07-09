using Assets.Kanau.UnityScene.Containers;
using Assets.Kanau.Utils;
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

        public void ExpoortImageFile(ExportPathHelper pathHelper) {
            if (texcontainer != null) {
                string filename = pathHelper.ToImagePath(Name);
                texcontainer.Save(filename);
            }
        }

        public override void Accept(IVisitor v) { v.Visit(this); }
    }
}
