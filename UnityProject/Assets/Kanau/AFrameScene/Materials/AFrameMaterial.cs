namespace Assets.Kanau.AFrameScene.Materials {
    public enum MaterialSide {
        Front,
        Back,
        Double
    }

    /// <summary>
    /// https://aframe.io/docs/master/components/material.html#properties
    /// </summary>
    public class AFrameMaterial {
        const bool DefaultDepthTest = true;
        const float DefaultOpacity = 1.0f;
        const bool DefaultTransparent = false;
        const MaterialSide DefaultSide = MaterialSide.Front;

        public bool DepthTest { get; set; }
        public float Opacity { get; set; }
        public bool Transparent { get; set; }
        public MaterialSide Side { get; set; }

        public BaseAFrameShader Shader { get; set; }

        public AFrameMaterial() {
            this.DepthTest = DefaultDepthTest;
            this.Opacity = DefaultOpacity;
            this.Transparent = DefaultTransparent;
            this.Side = DefaultSide;
        }

        public KeyValueProperty CreateProperty() {
            var p = new KeyValueProperty();
            if (DepthTest != DefaultDepthTest) { p.Add("depthTest", DepthTest); }
            if (Opacity != DefaultOpacity) { p.Add("opacity", Opacity); }
            if (Transparent != DefaultTransparent) { p.Add("transparent", Transparent); }
            if (Side != DefaultSide) { p.Add("side", Side.ToString().ToLower()); }

            Shader.FillProperty(p);
            return p;
        }
    }
}
