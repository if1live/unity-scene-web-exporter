using Assets.Kanau.ThreeScene.Materials;

namespace Assets.Kanau.AFrameScene.Materials {
    public class DiffuseMaterialFactory : IMaterialFactory {
        public AFrameMaterial Create(MaterialElem elem) {
            var container = elem.Container;

            var src = "";
            if (elem.Map != null) {
                src = elem.Map.ImagePath;
            }

            var shader = new FlatAFrameShader()
            {
                Color = container.Color,
                Repeat = container.MainTextureScale,
                Src = src,
            };
            var output = new AFrameMaterial()
            {
                Shader = shader,
                Transparent = container.Transparent,
                Side = (container.Color.a == 1) ? MaterialSide.Front: MaterialSide.Double,
            };
            return output;
        }
    }
}
