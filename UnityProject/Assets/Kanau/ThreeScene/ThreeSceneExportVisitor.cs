using Assets.Kanau.ThreeScene.Cameras;
using Assets.Kanau.ThreeScene.Geometries;
using Assets.Kanau.ThreeScene.Lights;
using Assets.Kanau.ThreeScene.Materials;
using Assets.Kanau.ThreeScene.Objects;
using Assets.Kanau.ThreeScene.Textures;
using Assets.Kanau.UnityScene.SceneGraph;
using Assets.Kanau.Utils;
using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Kanau.ThreeScene {
    class ThreeSceneExportVisitor : IVisitor {
        JsonWriter writer;
        public ThreeSceneExportVisitor(JsonWriter writer) {
            this.writer = writer;
        }

        #region Common
        public void WriteCommonObjectNode(JsonScopeObjectWriter scope, Object3DElem el) {
            scope.WriteKeyValue("uuid", el.Uuid);
            scope.WriteKeyValue("type", el.Type);
            scope.WriteKeyValue("name", el.Name);
            scope.WriteKeyValue("matrix", el.Matrix);
            scope.WriteKeyValue("visible", el.Visible);

            if (el.HasUserData()) {
                writer.WritePropertyName("userData");
                WriteUserdata(el);
            }

            if (el.ChildCount > 0) {
                writer.WritePropertyName("children");
                using (var s = new JsonScopeArrayWriter(writer)) {
                    foreach (var child in el.Children) {
                        child.Accept(this);
                    }
                }
            }
        }
        void WriteUserdata(Object3DElem el) {
            using (var s1 = new JsonScopeObjectWriter(writer)) {
                if (el.HasTag) {
                    s1.WriteKeyValue("tag", el.Tag);
                }
                if (el.HasLayer) {
                    s1.WriteKeyValue("layer", el.Layer);
                }

                s1.WriteKeyValue("isStatic", el.IsStatic);

                foreach (var kv in el.VarGroupDict) {
                    writer.WritePropertyName(kv.Key);
                    WriteScriptVariableGroup(kv.Value);
                }
            }
        }

        void WriteScriptVariableGroup(List<ScriptVariable> vars) {
            var helper = new ScriptVariableHelper(writer);

            using (var s2 = new JsonScopeObjectWriter(writer)) {
                foreach (var val in vars) {
                    writer.WritePropertyName(val.key);
                    helper.WriterScriptNodeVariable(val);
                }
            }
        }

        public class ScriptVariableHelper {
            JsonWriter writer;

            public ScriptVariableHelper(JsonWriter writer) {
                this.writer = writer;
            }

            public void WriterScriptNodeVariable(ScriptVariable val) {
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
        }
        #endregion

        #region Buffer Geometry

        void ExportBufferGeometryAttributes<T>(string name, T[] arr, int itemSize, string arrtype, JsonWriter writer) {
            if (arr == null) {
                return;
            }

            writer.WritePropertyName(name);
            using (var s = new JsonScopeObjectWriter(writer)) {
                s.WriteKeyValue("itemSize", itemSize);
                s.WriteKeyValue("type", arrtype);
                if (typeof(T) == typeof(float)) {
                    s.WriteKeyValue("array", (float[])Convert.ChangeType(arr, typeof(float[])));
                } else if (typeof(T) == typeof(int)) {
                    s.WriteKeyValue("array", (int[])Convert.ChangeType(arr, typeof(int[])));
                }
            }
        }

        public void Visit(BufferGeometryElem el) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                scope.WriteKeyValue("uuid", el.Uuid);
                scope.WriteKeyValue("type", el.Type);

                writer.WritePropertyName("data");
                using (var s1 = new JsonScopeObjectWriter(writer)) {
                    ExportBufferGeometryAttributes("index", el.Faces, 1, "Uint16Array", writer);

                    writer.WritePropertyName("attributes");
                    using (var s2 = new JsonScopeObjectWriter(writer)) {
                        ExportBufferGeometryAttributes("position", el.Vertices, 3, "Float32Array", writer);
                        ExportBufferGeometryAttributes("normal", el.Normals, 3, "Float32Array", writer);
                        ExportBufferGeometryAttributes("color", el.Colors, 3, "Float32Array", writer);
                        ExportBufferGeometryAttributes("uv", el.UV, 2, "Float32Array", writer);
                        ExportBufferGeometryAttributes("uv2", el.UV2, 2, "Float32Array", writer);
                        ExportBufferGeometryAttributes("uv3", el.UV3, 2, "Float32Array", writer);
                        ExportBufferGeometryAttributes("uv4", el.UV4, 2, "Float32Array", writer);
                    }
                }
            }
        }
        #endregion


        public void Visit (BoxBufferGeometryElem el) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                scope.WriteKeyValue("uuid", el.Uuid);
                scope.WriteKeyValue("type", el.Type);

                scope.WriteKeyValue("width", el.Width);
                scope.WriteKeyValue("height", el.Height);
                scope.WriteKeyValue("depth", el.Depth);
            }
        }

        public void Visit(QuadBufferGeometry el) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                scope.WriteKeyValue("uuid", el.Uuid);
                scope.WriteKeyValue("type", el.Type);

                scope.WriteKeyValue("width", el.Width);
                scope.WriteKeyValue("height", el.Height);
            }
        }
        

        #region Lights
        class LightHelper {
            JsonScopeObjectWriter scope;
            LightElem el;

            public LightHelper(JsonScopeObjectWriter scope, LightElem el) {
                this.scope = scope;
                this.el = el;
            }

            public void WriteColor() {
                scope.WriteKeyValue("color", el.Color);
            }
            public void WriteIntensity() {
                scope.WriteKeyValue("intensity", el.Intensity);
            }
            public void WriteDistance() {
                scope.WriteKeyValue("distance", el.Distance);
            }
            public void WriteDecay() {
                scope.WriteKeyValue("decay", el.Decay);
            }
        }
        public void Visit(AmbientLightElem el) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                WriteCommonObjectNode(scope, el);

                var helper = new LightHelper(scope, el);
                helper.WriteColor();
            }
        }
        public void Visit(PointLightElem el) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                WriteCommonObjectNode(scope, el);

                var helper = new LightHelper(scope, el);
                helper.WriteColor();
                helper.WriteIntensity();
                helper.WriteDistance();
                helper.WriteDecay();
            }
        }
        public void Visit(DirectionalLightElem el) {
            // matrix 조작
            // 유니티에서는 회전된 방향으로 빛을 쏘지만 three.js에서는 카메라의 위치에서 빚을 쏜다
            var forward = Vector3.forward;
            var direction = el.UnityMatrix.MultiplyVector(forward);
            var mat = Matrix4x4.TRS(-direction, Quaternion.identity, Vector3.one);
            el.UnityMatrix = mat;

            using (var scope = new JsonScopeObjectWriter(writer)) {
                WriteCommonObjectNode(scope, el);

                var helper = new LightHelper(scope, el);
                helper.WriteColor();
                helper.WriteIntensity();
            }
        }

        #endregion




        public void Visit(MeshElem el) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                WriteCommonObjectNode(scope, el);
                scope.WriteKeyValue("geometry", el.Geometry.Uuid);
                scope.WriteKeyValue("material", el.Material.Uuid);
            }
        }

        public void Visit(TextureElem el) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                scope.WriteKeyValue("uuid", el.Uuid);
                scope.WriteKeyValue("offset", el.Offset);
                scope.WriteKeyValue("repeat", el.Repeat);
                scope.WriteKeyValue("magFilter", el.MagFilter);
                scope.WriteKeyValue("minFilter", el.MinFilter);
                scope.WriteKeyValue("wrap", el.Wrap);
                scope.WriteKeyValue("image", el.ImageUuid);
                scope.WriteKeyValue("name", el.ImagePath);
                scope.WriteKeyValue("anisotropy", el.Anisotropy);
            }
        }

        public void Visit(SceneElem el) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                WriteCommonObjectNode(scope, el);
            }
        }

        public void Visit(GroupElem el) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                WriteCommonObjectNode(scope, el);
            }
        }

        public void Visit(MetadataElem el) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                scope.WriteKeyValue("version", el.Version);
                scope.WriteKeyValue("type", el.Type);
                scope.WriteKeyValue("generator", el.Generator);
            }
        }

        public void Visit(MaterialElem el) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                scope.WriteKeyValue("uuid", el.Uuid);
                scope.WriteKeyValue("name", el.Name);
                scope.WriteKeyValue("type", el.Type);

                var w = FindThreeJSWriter(el.Type);
                w.Write(el, scope);
            }
        }
        IMaterialWriter[] threejsWriters = new IMaterialWriter[]
        {
            new MeshBasicMaterialWriter(),
            new MeshLambertMaterialWriter(),
            new MeshPhongMaterialWriter(),
            new MeshStandardMaterialWriter(),
        };
        IMaterialWriter FindThreeJSWriter(string type) {
            foreach (var writer in threejsWriters) {
                if (writer.GetType().ToString().Contains(type)) {
                    return writer;
                }
            }
            Debug.Assert(false, "cannot find writer for " + type);
            return null;
        }



        public void Visit(SphereBufferGeometryElem el) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                scope.WriteKeyValue("uuid", el.Uuid);
                scope.WriteKeyValue("type", el.Type);

                scope.WriteKeyValue("radius", el.Radius.ToString());
                scope.WriteKeyValue("widthSegments", 16);
                scope.WriteKeyValue("heightSegments", 16);
            }
        }

        public void Visit(CylinderBufferGeometryElem el) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                scope.WriteKeyValue("uuid", el.Uuid);
                scope.WriteKeyValue("type", el.Type);

                scope.WriteKeyValue("radiusTop", el.Radius);
                scope.WriteKeyValue("radiusBottom", el.Radius);
                scope.WriteKeyValue("height", el.Height);
                scope.WriteKeyValue("radiusSegments", 16);
            }
        }

        public void Visit(PerspectiveCameraElem el) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                WriteCommonObjectNode(scope, el);

                scope.WriteKeyValue("fov", el.FOV);
                scope.WriteKeyValue("zoom", 1);
                scope.WriteKeyValue("near", el.Near);
                scope.WriteKeyValue("far", el.Far);
                scope.WriteKeyValue("focus", 10);
                scope.WriteKeyValue("aspect", 1);
                scope.WriteKeyValue("filmGauge", 35);
                scope.WriteKeyValue("filmOffset", 0);
            }
        }

        public void Visit(ImageElem el) {
            using (var scope = new JsonScopeObjectWriter(writer)) {                
                scope.WriteKeyValue("url", el.URL);
                scope.WriteKeyValue("uuid", el.Uuid);
                scope.WriteKeyValue("name", el.Name);
            }
        }
    }
}
