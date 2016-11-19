using UnityEngine;

namespace Assets.Kanau.UnityScene.SceneGraph
{
    public class CameraNode : ComponentNode<Camera>
    {
        public string Type {
            get {
                if (Value.orthographic) {
                    return "OrthographicCamera";
                } else {
                    return "PerspectiveCamera";
                }
            }
        }

        public float FieldOfView { get { return Value.fieldOfView; } }
        public float Near { get { return Value.nearClipPlane; } }
        public float Far { get { return Value.farClipPlane; } }
    }
}