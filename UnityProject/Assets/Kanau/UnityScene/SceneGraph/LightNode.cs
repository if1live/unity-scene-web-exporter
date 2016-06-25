using UnityEngine;

namespace Assets.Kanau.UnityScene.SceneGraph
{
    public class LightNode : ComponentNode<Light>
    {
        public const float InfiniteDistance = 10000;
        public LightType Type { get { return Value.type; } }
        public Color Color { get { return Value.color; } }
        public float Intensity { get { return Value.intensity; } }
        public float Distance { get { return Value.range; } }

        public Vector3 Direction {
            get {
                return Value.transform.forward;
            }
        }
    }
}
