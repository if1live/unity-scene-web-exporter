using Assets.Kanau.ThreeScene.Materials;

namespace Assets.Kanau.AFrameScene.Materials {
    public class UnlitColorMaterialFactory : IMaterialFactory {
        public AFrameMaterial Create(MaterialElem elem) {
            var container = elem.Container;

            var shader = new FlatAFrameShader()
            {
                Color = container.Color,
            };

            var output = new AFrameMaterial()
            {
                Side = MaterialSide.Front,
                Shader = shader,
            };
            return output;
        }
    }
}
