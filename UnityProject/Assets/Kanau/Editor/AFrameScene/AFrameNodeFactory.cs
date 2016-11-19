using Assets.Kanau.AFrameScene.Materials;
using Assets.Kanau.ThreeScene;
using Assets.Kanau.ThreeScene.Cameras;
using Assets.Kanau.ThreeScene.Geometries;
using Assets.Kanau.ThreeScene.Lights;
using Assets.Kanau.ThreeScene.Materials;
using Assets.Kanau.ThreeScene.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Kanau.AFrameScene {
    class AFrameNodeFactory {
        IThreeNodeTable sharedNodeTable;
        public AFrameNodeFactory(IThreeNodeTable sharedNodeTable) {
            this.sharedNodeTable = sharedNodeTable;
        }

        void WriteCommonAFrameNode(Object3DElem el, AFrameNode node) {
            // http://math.stackexchange.com/questions/237369/given-this-transformation-matrix-how-do-i-decompose-it-into-translation-rotati
            var m = el.Matrix;
            var pos = Vector3Property.MakePosition(new Vector3(m[12], m[13], m[14]));
            node.AddAttribute("position", pos);

            var sx = new Vector3(m[0], m[1], m[2]).magnitude;
            var sy = new Vector3(m[4], m[5], m[6]).magnitude;
            var sz = new Vector3(m[8], m[9], m[10]).magnitude;
            var scale = Vector3Property.MakeScale(new Vector3(sx, sy, sz));
            node.AddAttribute("scale", scale);

            var forward = el.UnityMatrix.MultiplyVector(Vector3.forward);
            var upward = el.UnityMatrix.MultiplyVector(Vector3.up);
            var q = Quaternion.LookRotation(forward, upward);
            var rot = Vector3Property.MakeRotation(q);
            node.AddAttribute("rotation", rot);            

            node.AddAttribute("name", el.Name);
            if (el.HasTag) { node.AddAttribute("tag", el.Tag); }
            if (el.HasLayer) { node.AddAttribute("layer", el.Layer); }


            var visitor = new AFrameExportVisitor(sharedNodeTable);
            foreach (var child in el.Children) {
                child.Accept(visitor);
                node.AddChild(visitor.Node);
            }
        }

        
        public AFrameNode Create(PerspectiveCameraElem el) {
            var container = new AFrameNode("a-entity");
            WriteCommonAFrameNode(el, container);

            var cam = new AFrameNode("a-camera");
            cam.AddAttribute("far", new SimpleProperty<float>(el.Far));
            cam.AddAttribute("near", new SimpleProperty<float>(el.Near));

            var camsettings = ExportSettings.Instance.camera;
            cam.AddAttribute("look-controls-enabled", camsettings.lookControlsEnabled.ToString().ToLower());
            cam.AddAttribute("wasd-controls-enabled", camsettings.wasdControlsEnabled.ToString().ToLower());

            container.AddChild(cam);

            var cursorsettings = ExportSettings.Instance.cursor;
            if (cursorsettings.enabled) {
                var cursor = Create(cursorsettings);
                cam.AddChild(cursor);
            }
            return container;
        }
        
        public AFrameNode Create(CursorSettings settings) {
            var cursor = new AFrameNode("a-cursor");
            cursor.AddAttribute("fuse", settings.fuse.ToString());
            cursor.AddAttribute("max-distance", settings.maxDistance.ToString());
            cursor.AddAttribute("timeout", settings.timeout.ToString());
            return cursor;
        }

        public AFrameNode Create(SkySettings settings) {
            var node = new AFrameNode("a-sky");

            node.AddAttribute("radius", settings.radius);

            if(settings.mode == SkySettings.SkyMode.Color) {
                node.AddAttribute("color", "#" + Three.UnityColorToHexColor(settings.skyColor));
            } else if(settings.mode == SkySettings.SkyMode.MainCameraBackground) {
                var cam = Camera.main;
                node.AddAttribute("color", "#" + Three.UnityColorToHexColor(cam.backgroundColor));
            } else if(settings.mode == SkySettings.SkyMode.Texture) {
                throw new NotImplementedException();
                //node.AddAttribute("src", "TODO")
            }
            return node;
        }

        #region Lights
        class LightHelper {
            // a-frame default values
            // https://aframe.io/docs/master/primitives/a-light.html
            const float DefaultAngle = 60;
            readonly Color DefaultColor = new UnityEngine.Color(1, 1, 1);
            const float DefaultDecay = 1.0f;
            const float DefaultDistance = 0.0f;
            const float DefaultExponent = 10.0f;
            public readonly Color DefaultGroundColor = new UnityEngine.Color(1, 1, 1);
            const float DefaultIntensity = 1.0f;

            AFrameNode node;
            LightElem el;

            public LightHelper(AFrameNode node, LightElem el) {
                this.node = node;
                this.el = el;
            }

            public void WriteColor() {
                if (el.UnityColor != DefaultColor) {
                    node.AddAttribute("color", new SimpleProperty<string>("#" + Three.UnityColorToHexColor(el.UnityColor)));
                }
            }
            public void WriteIntensity() {
                WriteCommonAttribute("intensity", el.Intensity, DefaultIntensity);
            }
            public void WriteDecay() {
                WriteCommonAttribute("decay", el.Decay, DefaultDecay);
            }
            public void WriteDistance() {
                WriteCommonAttribute("distance", el.Distance, DefaultDistance);
            }
            void WriteCommonAttribute<T>(string key, T val, T defaultval) {
                if (!val.Equals(defaultval)) {
                    node.AddAttribute(key, new SimpleProperty<T>(val));
                }
            }
        }
        public AFrameNode Create(DirectionalLightElem el) {
            var node = new AFrameNode("a-light");
            WriteCommonAFrameNode(el, node);
            node.AddAttribute("type", "directional");

            var helper = new LightHelper(node, el);
            helper.WriteColor();
            helper.WriteIntensity();
            return node;
        }
        public AFrameNode Create(AmbientLightElem el) {
            var node = new AFrameNode("a-light");
            WriteCommonAFrameNode(el, node);
            node.AddAttribute("type", "ambient");

            var helper = new LightHelper(node, el);
            helper.WriteColor();
            return node;
        }

        public AFrameNode Create(PointLightElem el) {
            var node = new AFrameNode("a-light");
            WriteCommonAFrameNode(el, node);
            node.AddAttribute("type", "point");

            var helper = new LightHelper(node, el);
            helper.WriteColor();
            helper.WriteIntensity();
            helper.WriteDecay();
            return node;
        }
        #endregion

        #region Geometries
        public AFrameNode Create(QuadBufferGeometry el) {
            var node = new AFrameNode("a-plane");
            node.AddAttribute("width", el.Width);
            node.AddAttribute("height", el.Height);
            return node;
        }

        public AFrameNode Create(SphereBufferGeometryElem el) {
            var node = new AFrameNode("a-sphere");
            node.AddAttribute("radius", el.Radius);
            return node;
        }

        public AFrameNode Create(CylinderBufferGeometryElem el) {
            var node = new AFrameNode("a-cylinder");
            node.AddAttribute("height", el.Height);
            node.AddAttribute("radius", el.Radius);
            return node;
        }
        public AFrameNode Create (BoxBufferGeometryElem el) {
            var node = new AFrameNode("a-box");
            node.AddAttribute("width", el.Width);
            node.AddAttribute("height", el.Height);
            node.AddAttribute("depth", el.Depth);
            return node;
        }

        public AFrameNode Create(BufferGeometryElem el) {
            throw new NotImplementedException();
        }
        #endregion

        public AFrameNode Create(SceneElem el) {
            var node = new AFrameNode("a-scene");

            var aframe = ExportSettings.Instance.aframe;
            if (aframe.enablePerformanceStatistics) {
                node.AddAttribute("stats", "true");
            }

            // assets
            var assetsNode = new AFrameNode("a-assets");
            // export mesh
            foreach (var elem in sharedNodeTable.GetEnumerable<AbstractGeometryElem>()) {
                // TODO 타입에 따라서 obj 굽는게 바뀔텐데
                var bufferGeom = elem as BufferGeometryElem;
                if (bufferGeom == null) { continue; }

                var assetNode = new AFrameNode("a-asset-item");
                assetNode.AddAttribute("id", bufferGeom.CreateSafeMeshName("-obj"));
                string filepath = "./models/" + bufferGeom.CreateMeshFileName(".obj");
                assetNode.AddAttribute("src", filepath);
                assetsNode.AddChild(assetNode);

                var assetNode_mtl = new AFrameNode("a-asset-item");
                assetNode_mtl.AddAttribute("id", bufferGeom.CreateSafeMeshName("-mtl"));
                string filepath_mtl = "./models/" + bufferGeom.CreateMeshFileName(".mtl");
                assetNode_mtl.AddAttribute("src", filepath_mtl);
                assetsNode.AddChild(assetNode_mtl);
            }
            node.AddChild(assetsNode);

            var sky = ExportSettings.Instance.sky;
            if (sky.mode != SkySettings.SkyMode.None) {
                var skyNode = Create(sky);
                node.AddChild(skyNode);
            }

            WriteCommonAFrameNode(el, node);

            return node;
        }

        public AFrameNode Create(GroupElem el) {
            var node = new AFrameNode("a-entity");
            WriteCommonAFrameNode(el, node);
            return node;
        }

        public AFrameNode Create(MeshElem el) {
            var node = new AFrameNode("a-entity");
            WriteCommonAFrameNode(el, node);

            AFrameNode geometryNode = null;
            var mtl = MaterialFacade.Instance.CreateMaterial(el.Material);

            // TODO 타입 하드코딩해서 분기하는거 제거하기
            if (el.Geometry.Type != "BufferGeometry") {
                var v = new AFrameExportVisitor(sharedNodeTable);
                el.Geometry.Accept(v);
                geometryNode = v.Node;

                // v0.3.2
                var standard = mtl.Shader as StandardAFrameShader;
                var flat = mtl.Shader as FlatAFrameShader;

                var props = new Dictionary<string, IProperty>();
                if(standard != null) {
                    var shader = standard;
                    props["shader"] = new SimpleProperty<string>(shader.ShaderName);
                    props["color"] = new ColorProperty(shader.Color, StandardAFrameShader.DefaultColor);
                    props["roughness"] = new SimpleProperty<float>(shader.Roughness, StandardAFrameShader.DefaultRoughness);
                    props["metalness"] = new SimpleProperty<float>(shader.Metalness, StandardAFrameShader.DefaultMetalness);
                    props["repeat"] = new Vector2Property(shader.Repeat, StandardAFrameShader.DefaultRepeat);
                    props["src"] = new SimpleProperty<string>(shader.Src, "");
                }

                if(flat != null) {
                    var shader = flat;
                    props["shader"] = new SimpleProperty<string>(shader.ShaderName);
                    props["color"] = new ColorProperty(shader.Color, StandardAFrameShader.DefaultColor);
                    props["repeat"] = new Vector2Property(shader.Repeat, StandardAFrameShader.DefaultRepeat);
                    props["src"] = new SimpleProperty<string>(shader.Src, "");
                }
                
                foreach(var kv in props) {
                    geometryNode.AddAttribute(kv.Key, kv.Value);
                }

                node.AddChild(geometryNode);

            } else {
                var geom = el.Geometry as BufferGeometryElem;
                var sb = new StringBuilder();
                sb.Append("obj: ");

                sb.Append("#" + geom.CreateSafeMeshName("-obj"));
                sb.Append("; ");

                var exporter = new MtlExporter();
                if(!exporter.IsBlank(mtl)) {
                    sb.Append("mtl: ");
                    sb.Append("#" + geom.CreateSafeMeshName("-mtl"));
                }

                node.AddAttribute("obj-model", sb.ToString());
            }

            return node;
        }
    }
}
