using UnityEngine;

namespace Assets.Kanau.UnityScene.SceneGraph
{
    public interface IUnityNode
    {
        int InstanceId { get; }

        GameObject CurrentObject { get; }
        GameObject ParentObject { get; }

        void Initialize<T>(T comp, INodeTable<int> containerTable) where T : Component;
    }
}
