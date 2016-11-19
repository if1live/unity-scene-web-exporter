using UnityEngine;

namespace Assets.Kanau.UnityScene.SceneGraph
{
    public interface IUnityNode
    {
        string InstanceId { get; }

        GameObject CurrentObject { get; }
        GameObject ParentObject { get; }

        void Initialize<T>(T comp, INodeTable<string> containerTable) where T : Component;
    }
}
