using UnityEngine;

namespace Assets.Kanau.ThreeScene.Lights
{
    public abstract class LightElem : Object3DElem
    {
        public uint Color {
            get { return Three.UnityColorToThreeColorInt(UnityColor); }
        }
        public float Intensity { get; set; }

        public Color UnityColor { get; set; }
    }
}
