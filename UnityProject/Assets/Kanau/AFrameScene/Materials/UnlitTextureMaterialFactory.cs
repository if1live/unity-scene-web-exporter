using Assets.Kanau.ThreeScene.Materials;

namespace Assets.Kanau.AFrameScene.Materials {
    public class UnlitTextureMaterialFactory : IMaterialFactory {
        public AFrameMaterial Create(MaterialElem elem) {
            var container = elem.Container;

            var src = "";
            if (elem.Map != null) {
                src = elem.Map.ImagePath;
            }

            var shader = new FlatAFrameShader()
            {
                Repeat = container.MainTextureScale,
                Src = src,
            };
            var output = new AFrameMaterial()
            {
                Shader = shader,
                Side = MaterialSide.Front,
            };
            return output;
        }
    }
}
