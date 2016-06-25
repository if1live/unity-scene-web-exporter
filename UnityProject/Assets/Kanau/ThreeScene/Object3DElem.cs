using Assets.Kanau.UnityScene.SceneGraph;
using Assets.Kanau.Utils;
using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Kanau.ThreeScene
{
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

        Dictionary<string, List<ScriptVariable>> varGroupDict = new Dictionary<string, List<ScriptVariable>>();
        public void AddScriptVariable(string group, ScriptVariable val) {
            List<ScriptVariable> vars = null;
            if(!varGroupDict.TryGetValue(group, out vars)) {
                varGroupDict[group] = new List<ScriptVariable>();
                vars = varGroupDict[group];
            }

            vars.Add(val);
        }

        void WriteUserdata(JsonWriter writer) {
            using (var s1 = new JsonScopeObjectWriter(writer)) {
                foreach (var kv in varGroupDict) {
                    writer.WritePropertyName(kv.Key);
                    WriteScriptVariableGroup(writer, kv.Value);
                }
            }
        }

        void WriteScriptVariableGroup(JsonWriter writer, List<ScriptVariable> vars) {
            using (var s2 = new JsonScopeObjectWriter(writer)) {
                foreach (var val in vars) {
                    writer.WritePropertyName(val.key);
                    WriterScriptNodeVariable(writer, val);
                }
            }
        }

        void WriterScriptNodeVariable(JsonWriter writer, ScriptVariable val) {
            using (var s1 = new JsonScopeObjectWriter(writer)) {
                s1.WriteKeyValue("key", val.key);
                s1.WriteKeyValue("type", val.ShortFieldType);
                WriteObjectVariable(s1, val.value);
            }
        }

        void WriteObjectVariable(JsonScopeObjectWriter s, object o) {
            var type = o.GetType();
            // primitive types
            if (WriteObjectVariable_Decimal(s, o)) { return; }
            if (WriteObjectVariable_Integer(s, o)) { return; }
            if (WriteObjectVariable_String(s, o)) { return; }
            if (type == typeof(bool)) {
                s.WriteKeyValue("value", (bool)o);
            }

            s.WriteKeyValue("value", o.ToString());
        }

        bool WriteObjectVariable_Decimal(JsonScopeObjectWriter s, object o) {
            var type = o.GetType();
            if (type == typeof(float)) {
                s.WriteKeyValue("value", (float)o);
            } else if (type == typeof(double)) {
                s.WriteKeyValue("value", (double)o);
            } else {
                return false;
            }

            return true;
        }
        bool WriteObjectVariable_Integer(JsonScopeObjectWriter s, object o) {
            var type = o.GetType();
            if (type == typeof(int)) {
                s.WriteKeyValue("value", (int)o);
            } else if (type == typeof(short)) {
                s.WriteKeyValue("value", (short)o);
            } else if (type == typeof(byte)) {
                s.WriteKeyValue("value", (byte)o);
            } else if (type == typeof(long)) {
                s.WriteKeyValue("value", (long)o);
            } else {
                return false;
            }

            return true;
        }

        bool WriteObjectVariable_String(JsonScopeObjectWriter s, object o) {
            var type = o.GetType();
            if (type == typeof(char)) {
                s.WriteKeyValue("value", (char)o);
            } else if (type == typeof(string)) {
                s.WriteKeyValue("value", (string)o);
            } else {
                return false;
            }

            return true;
        }
        #endregion

        public void CopyAttributes(Object3DElem other) {
            this.guid = other.guid;
            this.Visible = other.Visible;
            this.UnityMatrix = other.UnityMatrix;
            this.Name = other.Name;
            this.userdata = new Dictionary<string, object>(other.Userdata);

            this.varGroupDict = new Dictionary<string, List<ScriptVariable>>();
            foreach(var group in other.varGroupDict) {
                this.varGroupDict[group.Key] = new List<ScriptVariable>(group.Value);
            }
        }

        public void WriteCommonObjectNode(JsonWriter writer, JsonScopeObjectWriter scope) {
            scope.WriteKeyValue("uuid", Uuid);
            scope.WriteKeyValue("type", Type);
            scope.WriteKeyValue("name", Name);
            scope.WriteKeyValue("matrix", Matrix);
            scope.WriteKeyValue("visible", Visible);

            if((userdata.Count + varGroupDict.Count) > 0) {
                writer.WritePropertyName("userdata");
                WriteUserdata(writer);
            }

            if (ChildCount > 0) {
                writer.WritePropertyName("children");
                using (var s = new JsonScopeArrayWriter(writer)) {
                    foreach (var child in Children) {
                        child.ExportJson(writer);
                    }
                }
            }
        }
    }
}
