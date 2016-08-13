using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Kanau.UnityScene.SceneGraph {
    public struct ScriptVariable
    {
        public const string TheGameObject = "gameobject";
        public const string TheTransform = "transform";
        public const string TheMaterial = "material";
        public const string TheTexture = "texture";
        public const string TheMesh = "mesh";
        public const string TheUnknown = "unknown";
            
        public string fieldType;
        public string key;
        public object value;

        public static readonly ScriptVariable Null;

        static ScriptVariable() {
            Null = new ScriptVariable()
            {
                fieldType = "",
                key = "",
                value = null,
            };
        }

        public string ShortFieldType {
            get {
                // system.boolean 보다 boolean이 읽기 쉬우니까
                if(fieldType.StartsWith("system.")) {
                    return fieldType.Replace("system.", "");
                }

                return fieldType;
            }
        }

        public Material GetMaterial() { return GetByGeneric<Material>(TheMaterial); }
        public Texture GetTexture() { return GetByGeneric<Texture>(TheTexture); }
        public Mesh GetMesh() { return GetByGeneric<Mesh>(TheMesh); }
        public GameObject GetGameObject() { return GetByGeneric<GameObject>(TheGameObject); }
        public Transform GetTransform() { return GetByGeneric<Transform>(TheTransform); }

        T GetByGeneric<T>(string type) where T : class{
            if (fieldType == type) {
                return value as T;
            }
            return null;
        }
    }


    public class ScriptNode : ComponentNode<MonoBehaviour>
    {
        List<ScriptVariable> variables = new List<ScriptVariable>();
        public string BehaviourName { get; private set; }

        public IEnumerable<ScriptVariable> VariableEnumerable {
            get { return variables; }
        }


        static HashSet<Type> simpleTypeSet = new HashSet<Type>()
        {
            // primitive type
            typeof(byte),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(bool),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(char),
            typeof(string),

            // basic type
            typeof(DateTime),
        };

        static string GetFieldTypeFromObject(object o) {
            var type = o.GetType();
            if (simpleTypeSet.Contains(type)) {
                return type.ToString().ToLower();
            }
            if(o is Material) {
                return ScriptVariable.TheMaterial;
            }
            if (o is Texture) {
                return ScriptVariable.TheTexture;
            }
            if (o is Mesh) {
                return ScriptVariable.TheMesh;
            }
            if(o is Transform) {
                return ScriptVariable.TheTransform;
            }
            if (o is GameObject) {
                return ScriptVariable.TheGameObject;
            }

            // fail-over
            return ScriptVariable.TheUnknown;
        }

        public override void Initialize<T1>(T1 comp, INodeTable<string> containerTable) {
            base.Initialize(comp, containerTable);

            this.BehaviourName = comp.GetType().ToString();

            var type = Value.GetType();
            var fieldInfos = from info in type.GetFields() where info.IsPublic select info;
            foreach (var field in fieldInfos) {
                var key = field.Name;
                var val = field.GetValue(Value);
                var fieldType = GetFieldTypeFromObject(val);
                var kv = new ScriptVariable()
                {
                    fieldType = fieldType,
                    key = key,
                    value = val,
                };
                variables.Add(kv);
            }
        }
    }
}
