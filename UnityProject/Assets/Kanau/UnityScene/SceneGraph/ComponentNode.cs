using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Kanau.UnityScene.SceneGraph {
    public class ComponentNode<T> : IUnityNode where T : Component
    {
        // camera, light, ... 컴포넌트의 기본 클래스
        // T를 Component로 쓰면 enabled가 없다
        // T를 Behaviour로 쓰면 enabled가 있지만 Renderer는 상속관계가 아니다
        // Renderer까지 하나로 취급하려고 Component 기반으로 변경
        public T Value { get; protected set; }
        public Transform Transform {
            get { return Value.transform; }
        }

        protected INodeTable<string> ContainerTable { get; set; }

        public string Guid
        {
            get
            {
#if UNITY_EDITOR
                var assetpath = AssetDatabase.GetAssetPath(Value);
                var guid = AssetDatabase.AssetPathToGUID(assetpath);
                return guid;
#else
                return "";
#endif
            }
        }

        public GameObject CurrentObject { get { return Value.gameObject; } }
        public GameObject ParentObject {
            get {
                var tr = Value.transform;
                if (tr.parent == null) {
                    return null;
                } else {
                    return tr.parent.gameObject;
                }
            }
        }
        public string InstanceId { get { return Value.GetInstanceID().ToString(); } }

        public virtual void Initialize<T1>(T1 comp, INodeTable<string> containerTable) where T1 : Component {
            Debug.Assert(typeof(T) == typeof(T1));
            this.Value = (T)Convert.ChangeType(comp, comp.GetType());
            this.ContainerTable = containerTable;
        }

        // transform는 모든 객체에 있잖아?
        // 그렇다면 모든 컴포넌트에서 접근할수 있도록 하면 구조가 단순해지지 않을까
        public bool HasPosition {
            get { return (Vector3.zero != Transform.localPosition); }
        }

        public bool HasRotation {
            get {
                Quaternion r = Transform.localRotation;
                return (r != Quaternion.identity);
            }
        }

        public bool HasScale {
            get {
                Vector3 s = Transform.localScale;
                bool x = (s.x == 1.0f);
                bool y = (s.y == 1.0f);
                bool z = (s.z == 1.0f);
                bool hs = x && y && z;
                return !hs;
            }
        }
    }
}
