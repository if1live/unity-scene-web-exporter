using Assets.Kanau.ThreeScene.Materials;
using UnityEngine;

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

    public abstract class BaseAFrameShader {
        public abstract void FillProperty(KeyValueProperty p);

        protected void Add(KeyValueProperty p, string key, Color color) {
            p.Add(key, "#" + Three.UnityColorToHexColor(color));
        }
        protected void Add(KeyValueProperty p, string key, Vector2 vec) {
            var repeat = string.Format("{0} {1}", vec.x, vec.y);
            p.Add(key, repeat);
        }
        protected void AddTextureSrc(KeyValueProperty p, string key, string src) {
            const string path = "./";
            var attr = string.Format("url({0}{1})", path, src);
            p.Add(key, attr);
        }
    }

    /// <summary>
    /// https://aframe.io/docs/master/core/shaders.html#properties
    /// </summary>
    public class StandardAFrameShader : BaseAFrameShader {
        readonly Color DefaultColor = new Color(1, 1, 1);
        // TODO height
        const bool DefaultFog = true;
        const float DefaultMetalness = 0.5f;
        readonly Vector2 DefaultRepeat = new Vector2(1, 1);
        const float DefaultRoughness = 0.5f;
        // TODO width

        public Color Color { get; set; }
        public bool Fog { get; set; }
        public float Metalness { get; set; }
        public Vector2 Repeat { get; set; }
        public float Roughness { get; set; }
        public string Src { get; set; }

        public StandardAFrameShader() {
            this.Color = DefaultColor;
            this.Fog = DefaultFog;
            this.Metalness = DefaultMetalness;
            this.Repeat = DefaultRepeat;
            this.Roughness = DefaultRoughness;
        }

        public override void FillProperty(KeyValueProperty p) { 
            p.Add("shader", "standard");

            if (Color != DefaultColor) { Add(p, "color", Color); }
            if (Fog != DefaultFog) { p.Add("fog", Fog); }
            if (Metalness != DefaultMetalness) { p.Add("metalness", Metalness); }
            if (Repeat != DefaultRepeat) { Add(p, "repeat", Repeat); }
            if (Roughness != DefaultRoughness) { p.Add("roughness", Roughness); }
            if (Src != "") { AddTextureSrc(p, "src", Src); }
        }
    }

    /// <summary>
    /// https://aframe.io/docs/master/core/shaders.html#properties-1
    /// </summary>
    public class FlatAFrameShader : BaseAFrameShader {
        readonly Color DefaultColor = new Color(1, 1, 1);
        const bool DefaultFog = true;
        // TODO height
        readonly Vector2 DefaultRepeat = new Vector2(1, 1);
        // TODO width

        public Color Color { get; set; }
        public bool Fog { get; set; }
        public Vector2 Repeat { get; set; }
        public string Src { get; set; }

        public FlatAFrameShader() {
            this.Color = DefaultColor;
            this.Fog = DefaultFog;
            this.Repeat = DefaultRepeat;
        }

        public override void FillProperty(KeyValueProperty p) {
            p.Add("shader", "flat");

            if (Color != DefaultColor) { Add(p, "color", Color); }
            if (Fog != DefaultFog) { p.Add("fog", Fog); }
            if (Repeat != DefaultRepeat) { Add(p, "repeat", Repeat); }
            if (Src != "") { AddTextureSrc(p, "src", Src); }
        }
    }

    public interface IAFrameMaterialFactory {
        AFrameMaterial Create(MaterialElem elem);
    }

    public class StandardAFrameMaterialFactory : IAFrameMaterialFactory {
        public AFrameMaterial Create(MaterialElem elem) {
            var container = elem.Container;

            var src = "";
            if(elem.Map != null) {
                src = elem.Map.ImageName;
            }

            var shader = new StandardAFrameShader()
            {
                Color = container.Color,
                Metalness = container.Metallic,
                Roughness = container.Roughness,
                Repeat = container.MainTextureScale,
                Src = src,
            };

            var side = (container.Color.a == 1) ? MaterialSide.Double : MaterialSide.Front;
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

    public class UnlitColorAFrameMaterialFactory : IAFrameMaterialFactory {
        public AFrameMaterial Create(MaterialElem elem) {
            var container = elem.Container;

            var shader = new FlatAFrameShader()
            {
                Color = container.Color,
            };

            var output = new AFrameMaterial()
            {
                Side = MaterialSide.Double,
                Shader = shader,
            };
            return output;
        }
    }

    public class UnlitTextureAFrameMaterialFactory : IAFrameMaterialFactory {
        public AFrameMaterial Create(MaterialElem elem) {
            var container = elem.Container;

            var src = "";
            if (elem.Map != null) {
                src = elem.Map.ImageName;
            }

            var shader = new FlatAFrameShader()
            {
                Repeat = container.MainTextureScale,
                Src = src,
            };
            var output = new AFrameMaterial()
            {
                Shader = shader,
                Side = MaterialSide.Double,
            };
            return output;
        }
    }

    public class DiffuseAFrameMaterialFactory : IAFrameMaterialFactory {         
        public AFrameMaterial Create(MaterialElem elem) {
            var container = elem.Container;

            var src = "";
            if (elem.Map != null) {
                src = elem.Map.ImageName;
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
                Side = (container.Color.a == 1) ? MaterialSide.Double : MaterialSide.Front,
            };
            return output;
        }
    }
}
