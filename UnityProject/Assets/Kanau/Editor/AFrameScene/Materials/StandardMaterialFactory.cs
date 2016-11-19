using Assets.Kanau.ThreeScene.Materials;

namespace Assets.Kanau.AFrameScene.Materials {
    public class StandardMaterialFactory : IMaterialFactory {
        public AFrameMaterial Create(MaterialElem elem) {
            var container = elem.Container;

            var src = "";
            if (elem.Map != null) {
                src = elem.Map.ImagePath;
            }

            var shader = new StandardAFrameShader()
            {
                Color = container.Color,
                Metalness = container.Metallic,
                Roughness = container.Roughness,
                Repeat = container.MainTextureScale,
                Src = src,
            };

            var side = (container.Color.a == 1) ? MaterialSide.Front : MaterialSide.Double;
            var output = new AFrameMaterial()
            {
                Shader = shader,
                Transparent = container.Transparent,
                Opacity = container.Color.a,
                Side = side,
            };
            return output;
        }
    }
}
