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
        LightmapContainer lightmapcontainer;

        public ImageElem(TextureContainer c) {
            this.texcontainer = c;
            this.Name = c.FileName;
        }

        public ImageElem(LightmapContainer c) {
            this.lightmapcontainer = c;
            this.Name = c.ExportedFileName(".jpg");
        }

        public void ExpoortImageFile(ExportPathHelper pathHelper) {
            if (texcontainer != null) {
                string filename = pathHelper.ToImagePath(Name);
                texcontainer.Save(filename);
            }
            if(lightmapcontainer != null) {
                lightmapcontainer.Save(pathHelper);
            }
        }

        public string URL
        {
            get
            {
                return string.Format("./{0}/{1}", ExportSettings.Instance.destination.imageDirectory, Name);
            }
        }

        public override void Accept(IVisitor v) { v.Visit(this); }
    }
}
