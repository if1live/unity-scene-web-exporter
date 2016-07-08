using Assets.Kanau.AFrameScene;
using Assets.Kanau.UnityScene.SceneGraph;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Kanau.ThreeScene {
    public abstract class Object3DElem : BaseElem {
        public float[] Matrix {
            get {
                var m = UnityMatrix;
                return new float[16]
                {
                    m[0, 0], m[1, 0], -m[2, 0], m[3, 0],
                    m[0, 1], m[1, 1], -m[2, 1], m[3, 1],
                    -m[0, 2], -m[1, 2], m[2, 2], m[3, 2],
                    m[0, 3], m[1, 3], -m[2, 3], m[3, 3],
                };
            }
        }
        public bool Visible { get; set; }

        public Object3DElem() {
            UnityMatrix = Matrix4x4.identity;
            Visible = true;
            Parent = null;
        }

        public Object3DElem Parent { get; set; }
        public Object3DElem[] Children { get { return children.ToArray(); } }
        List<Object3DElem> children = new List<Object3DElem>();

        public void AddChild(Object3DElem n) {
            Debug.Assert(n.Parent == null);
            children.Add(n);
            n.Parent = this;
        }
        public void RemoveChild(Object3DElem n) {
            Debug.Assert(n.Parent == this);
            n.Parent = null;
            children.Remove(n);
        }
        public void ReplaceChild(Object3DElem prev, Object3DElem next) {
            Debug.Assert(next.Parent == null);
            Debug.Assert(prev.Parent == this);

            var idx = children.FindIndex(delegate (Object3DElem el) { return el == prev; });
            children[idx] = next;
            prev.Parent = null;
            next.Parent = this;
        }
        public int ChildCount { get { return children.Count; } }

        public Matrix4x4 UnityMatrix { get; set; }
        public void SetTransform(Transform tr) {
            var m = Matrix4x4.TRS(tr.localPosition, tr.localRotation, tr.localScale);
            this.UnityMatrix = m;
        }

        public override string Type { get { return "Object3D"; } }

        #region Userdata
        Dictionary<string, object> userdata = new Dictionary<string, object>();
        public Dictionary<string, object> Userdata { get { return userdata; } }

        public Dictionary<string, List<ScriptVariable>> VarGroupDict { get { return varGroupDict; } }
        Dictionary<string, List<ScriptVariable>> varGroupDict = new Dictionary<string, List<ScriptVariable>>();
        public void AddScriptVariable(string group, ScriptVariable val) {
            List<ScriptVariable> vars = null;
            if(!varGroupDict.TryGetValue(group, out vars)) {
                varGroupDict[group] = new List<ScriptVariable>();
                vars = varGroupDict[group];
            }

            vars.Add(val);
        }
        #endregion

        public string Tag { get; set; }
        public string Layer { get; set; }
        public bool HasTag { get { return (Tag != null && Tag != ""); } }
        public bool HasLayer { get { return (Layer != null && Layer != ""); } }

        public bool IsStatic { get; set; }

        public void CopyAttributes(Object3DElem other) {
            this.guid = other.guid;
            this.Visible = other.Visible;
            this.UnityMatrix = other.UnityMatrix;
            this.Name = other.Name;
            this.userdata = new Dictionary<string, object>(other.Userdata);

            this.Tag = other.Tag;
            this.Layer = other.Layer;
            this.IsStatic = other.IsStatic;

            this.varGroupDict = new Dictionary<string, List<ScriptVariable>>();
            foreach(var group in other.varGroupDict) {
                this.varGroupDict[group.Key] = new List<ScriptVariable>(group.Value);
            }
        }

        public bool HasUserData() {
            if(userdata.Count > 0) { return true; }
            if(varGroupDict.Count > 0) { return true; }
            if(HasLayer) { return true; }
            if(HasTag) { return true; }
            return false;
        }

        public void WriteCommonAFrameNode(AFrameNode node) {
            var m = Matrix;
            var pos = Vector3Property.MakePosition(new Vector3(m[12], m[13], m[14]));
            node.AddAttribute("position", pos);

            var scale = Vector3Property.MakeScale(new Vector3(m[0], m[5], m[10]));
            node.AddAttribute("scale", scale);

            var forward = UnityMatrix.MultiplyVector(Vector3.forward);
            var upward = UnityMatrix.MultiplyVector(Vector3.up);
            var q = Quaternion.LookRotation(forward, upward);
            var rot = Vector3Property.MakeRotation(q);
            node.AddAttribute("rotation", rot);

            node.AddAttribute("name", Name);
            if (HasTag) { node.AddAttribute("tag", Tag); }
            if (HasLayer) { node.AddAttribute("layer", Layer); }


            foreach (var child in Children) {
                node.AddChild(child.ExportAFrame());
            }
        }
    }
}
