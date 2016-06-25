using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Kanau.UnityScene.SceneGraph
{
    public interface IGameObjectNode : IUnityNode
    {
        string Name { get; }

        bool SuperRoot { get; }

        string Tag { get; }
        bool HasTag { get; }

        bool HasLayer { get; }
        int LayerId { get; }
        string Layer { get; }

        bool ActiveInHierarchy { get; }
        bool ActiveSelf { get; }

        bool IsStatic { get; }

        ComponentNode<Transform> TransformNode { get; }

        void BuildHierarchy(INodeTable<int> table);
        GameObjectNode Parent { get; }
        GameObjectNode[] Children { get; }

        bool HasParent { get; }
        bool IsClockwise { get; }

        // ligthmap
        bool HasLightmap { get; }
        int LightmapIndex { get; }
        float[] LightmapTileOffset { get; }
    }

    /// <summary>
    /// 부모 -자식 관계도 만들고 컨테이너로써의 역할을 한다
    /// </summary>
    public class GameObjectNode : IGameObjectNode
    {
        public const int RootInstanceId = int.MaxValue;

        public GameObject CurrentObject { get; private set; }
        public GameObject ParentObject {
            get {
                if(CurrentObject.transform.parent == null) {
                    return null; 
                } else {
                    return CurrentObject.transform.parent.gameObject;
                }
            }
        }
        public bool HasParent { get { return ParentObject != null; } }

        public int InstanceId {
            get {
                return CurrentObject.GetInstanceID();
            }
        }

        public GameObjectNode(GameObject go) {
            this.CurrentObject = go;
        }

        public void Initialize<T>(T comp, INodeTable<int> containerTable) where T : Component {
            throw new NotImplementedException();
        }

        public string Name { get { return CurrentObject.name; } }

        public string Tag { get { return CurrentObject.tag; } }
        public bool HasTag { get { return (Tag != "" && Tag != "Untagged"); } }

        public bool HasLayer { get { return LayerId != 0; } }
        public int LayerId { get { return CurrentObject.layer; } }
        public string Layer { get { return LayerMask.LayerToName(LayerId); } }

        public bool ActiveInHierarchy { get { return CurrentObject.activeInHierarchy; } }
        public bool ActiveSelf { get { return CurrentObject.activeSelf; } }

        public bool IsStatic { get { return CurrentObject.isStatic; } }

        public ComponentNode<Transform> TransformNode {
            get {
                var node = new ComponentNode<Transform>();
                node.Initialize(CurrentObject.transform, null);
                return node;
            }
        }

        // child-parent
        public void BuildHierarchy(INodeTable<int> table) {
            if(ParentObject != null) {
                Parent = table.Get<GameObjectNode>(ParentObject.GetInstanceID());
            }

            var childlist = new List<GameObjectNode>();
            var tr = CurrentObject.transform;
            for(int i = 0; i < tr.childCount; i++) {
                var childtr = tr.GetChild(i);
                var child = table.Get<GameObjectNode>(childtr.gameObject.GetInstanceID());
                childlist.Add(child);
            }
            Children = childlist.ToArray();
        }
        public GameObjectNode Parent { get; private set; }
        public GameObjectNode[] Children { get; private set; }

        public bool SuperRoot { get { return false; } }

        public bool IsClockwise {
            get {
                float mul = 1;
                Transform t = CurrentObject.transform;
                while (t != null) {
                    var s = t.localScale;
                    mul *= (s.x * s.y * s.z);
                    t = t.transform.parent;
                }
                return (mul > 0);
            }
        }

        public override string ToString() {
            return Name;
        }

        // lightmap
        public bool HasLightmap {
            get {
                if (CurrentObject.GetComponent<Renderer>()) {
                    return CurrentObject.GetComponent<Renderer>().lightmapIndex != 255 && CurrentObject.GetComponent<Renderer>().lightmapIndex != -1;
                } else {
                    return false;
                }
            }
        }
        public int LightmapIndex {
            get { return CurrentObject.GetComponent<Renderer>().lightmapIndex; }
        }

        public float[] LightmapTileOffset {
            get {
                Vector4 p = CurrentObject.GetComponent<Renderer>().lightmapScaleOffset;
                return new float[] { p.x, p.y, p.z, p.w };
            }
        }
    }

    // 유니티에서는 최상위 부모 객체가 없다
    // 그래도 부모를 가짜로라도 만들어두면 이것저것 작업하기 좋을거같더라
    public class RootGameObjectNode : IGameObjectNode
    {
        public bool SuperRoot { get { return true; } }
        public bool HasParent { get { return false; } }

        public bool ActiveInHierarchy { get { return true; } }
        public bool ActiveSelf { get { return true; } }

        public GameObject CurrentObject { get { return null; } }
        public GameObject ParentObject { get { return null; } }

        public bool HasLayer { get { return false; } }
        public bool HasTag { get { return false; } }
        public string Tag { get { return ""; } }

        public int InstanceId { get { return GameObjectNode.RootInstanceId; } }

        public bool IsStatic { get { return false; } }

        public string Layer { get { return ""; } }
        public int LayerId { get { return 0; } }

        public string Name { get { return "[ROOT]"; } }

        public GameObjectNode Parent { get { return null; } }
        public GameObjectNode[] Children { get; private set; }

        public ComponentNode<Transform> TransformNode { get { return null; } }

        public void BuildHierarchy(INodeTable<int> table) {
            // TODO 부모 없는 모다 찾아서 붙이기
            var parentlist = new List<GameObjectNode>();
            foreach (var n in table.GetEnumerable<GameObjectNode>()) {
                if (n.ParentObject == null) {
                    parentlist.Add(n);
                }
            }
            Children = parentlist.ToArray();
        }

        public void Initialize<T>(T comp, INodeTable<int> containerTable) where T : Component {
            throw new NotImplementedException();
        }

        public override string ToString() {
            return Name;
        }
        public bool IsClockwise { get { return true; } }

        // lightmap
        public bool HasLightmap { get { return false; } }
        public int LightmapIndex { get { return -1; } }

        public float[] LightmapTileOffset { get { return new float[] { 0, 0, 0, 0 }; } }
    }
}
