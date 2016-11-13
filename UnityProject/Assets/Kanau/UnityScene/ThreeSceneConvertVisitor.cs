using Assets.Kanau.ThreeScene;
using Assets.Kanau.ThreeScene.Cameras;
using Assets.Kanau.ThreeScene.Geometries;
using Assets.Kanau.ThreeScene.Lights;
using Assets.Kanau.ThreeScene.Materials;
using Assets.Kanau.ThreeScene.Objects;
using Assets.Kanau.ThreeScene.Textures;
using Assets.Kanau.UnityScene.Containers;
using Assets.Kanau.UnityScene.SceneGraph;
using Assets.Kanau.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Kanau.UnityScene {
    public class ThreeSceneConvertVisitor
    {
        Report report;
        ThreeSceneRoot root;

        Dictionary<string, Object3DElem> objNodeTable = new Dictionary<string, Object3DElem>();

        public ThreeSceneConvertVisitor(Report report) {
            this.report = report;
            this.root = new ThreeSceneRoot();
        }

        public ThreeSceneRoot Run(UnitySceneRoot unityscene) {
            // three.js는 scene밑에 모든게 들어간다
            // 그래서 기본적인 계층 구조를 미리 만들어야한다
            // 계층 구조 - begin
            foreach (var n in unityscene.GraphNodeTable.GetEnumerable<GameObjectNode>()) {
                n.BuildHierarchy(unityscene.GraphNodeTable);
            }
            var objroot = new RootGameObjectNode();
            objroot.BuildHierarchy(unityscene.GraphNodeTable);
            root.Root = VisitGameObjectNode_r(objroot);
            // 계층 구조 - end

            // ambient light 같은거 + 기타 속성
            Visit(unityscene.Settings);

            // 공유 속성
            foreach (var n in unityscene.ContainerTable.GetEnumerable<TextureContainer>()) {
                RegisterTexture(n);
            }
            foreach(var n in unityscene.ContainerTable.GetEnumerable<LightmapContainer>()) {
                RegisterLightmap(n);
            }

            // lightmap이 없는 경우
            foreach (var n in unityscene.ContainerTable.GetEnumerable<MeshContainer>()) {
                RegisterMeshWithoutLightmap(n);
            }

            // 게임오브젝트마다 별도의 Mesh를 등록하는게 가능하도록
            // 왜냐하면 lightmap 좌표 정보는 lightmapScaleOffset는 Renderer에 저장되기때문
            // lightmap이 있는 경우
            foreach (var n in unityscene.GraphNodeTable.GetEnumerable<GameObjectNode>()) {
                if(!n.HasLightmap) { continue; }
                var renderer = n.CurrentObject.GetComponent<Renderer>();
                var meshfilter = n.CurrentObject.GetComponent<MeshFilter>();
                if(renderer == null) { continue; }
                if(meshfilter == null) { continue; }

                var meshcontainerkey = meshfilter.sharedMesh.GetInstanceID().ToString();
                var meshcontainer = unityscene.ContainerTable.Get<MeshContainer>(meshcontainerkey);
                AbstractGeometryElem geo = new BufferGeometryElem(meshcontainer, n.HasLightmap, n.lightmapScaleOffset);
                var geokey = ConvertKeyForBufferGeometryElem(renderer);
                root.SharedNodeTable.Add(geo, geokey);
            }

            foreach (var n in unityscene.ContainerTable.GetEnumerable<MaterialContainer>()) {
                var mtl = CreateMaterialElem(n);
                root.SharedNodeTable.Add(mtl, n.InstanceId);
            }

            foreach (var n in unityscene.GraphNodeTable.GetEnumerable<GameObjectNode>()) {
                if (!n.HasLightmap) { continue; }
                var renderer = n.CurrentObject.GetComponent<Renderer>();
                if (renderer == null) { continue; }

                var mtl = renderer.sharedMaterial;
                var mtlcontainer = unityscene.ContainerTable.Get<MaterialContainer>(mtl.GetInstanceID().ToString());
                var mtlelem = CreateMaterialElem(mtlcontainer);

                var lightmapkey = renderer.lightmapIndex.ToString();
                var lightmapelem = root.SharedNodeTable.Get<TextureElem>(lightmapkey);
                mtlelem.LightMap = lightmapelem;

                var key = ConvertKeyForMaterialELem(renderer);
                root.SharedNodeTable.Add(mtlelem, key);

            }

            // Object3D와 관련된 객체
            var lightelems = new List<LightElem>();
            foreach (var n in unityscene.GraphNodeTable.GetEnumerable<LightNode>()) {
                var lightelem = RegisterToThreeScene(n);
                lightelems.Add(lightelem);
            }

            var camelems = new List<CameraElem>();
            foreach(var n in unityscene.GraphNodeTable.GetEnumerable<CameraNode>()) {
                var camelem = RegisterToThreeScene(n);
                camelems.Add(camelem);
            }

            var meshelems = new List<MeshElem>();
            foreach(var n in unityscene.GraphNodeTable.GetEnumerable<RenderNode>()) {
                var meshelem = RegisterToThreeScene(n);
                meshelems.Add(meshelem);

                root.SharedNodeTable.Add(meshelem, meshelem.Uuid);
            }

            // 연관된 스크립트 변수 등록
            foreach (var n in unityscene.GraphNodeTable.GetEnumerable<ScriptNode>()) {
                var objnode = objNodeTable[n.CurrentObject.GetInstanceID().ToString()];
                RegisterScriptVariables(unityscene, objnode, n);
            }

            // Optimize scene graph
            // 특정 객체를 포함하는 Group에 다른 객체가 등록되어있지 않은 경우, cam을 위로 올릴수 있다
            // 처음부터 객체를 바로 생성하지 않고 자식이 1개인 경우에만 끌어올리는 이유는
            // 유니티 game object안에는 light, mesh 같은게 동시에 들어갈수 있지만 three.js에서는 불가능하기때문
            foreach (var cam in camelems) {
                UpcastInSceneThreeObject3D(cam);
            }
            foreach (var light in lightelems) {
                UpcastInSceneThreeObject3D(light);
            }
            foreach (var child in meshelems) {
                UpcastInSceneThreeObject3D(child);
            }
            
            return root;
        }

        public bool UpcastInSceneThreeObject3D(Object3DElem child) {
            bool isSimpleGroup = (child.Parent.Type == "Group") && (child.Parent.ChildCount == 1);
            if (isSimpleGroup) {
                var parent = child.Parent;
                var grandparent = parent.Parent;

                parent.RemoveChild(child);
                child.CopyAttributes(parent);

                grandparent.ReplaceChild(parent, child);
            }
            return isSimpleGroup;
        }

        Object3DElem VisitGameObjectNode_r(IGameObjectNode n) {
            Debug.Assert(n != null);

            Object3DElem elem = null;
            if(n.SuperRoot) {
                elem = new SceneElem();
                elem.Name = "KanauScene";
            } else {
                elem = new GroupElem(n);
            }

            if (n.HasTag) { elem.Tag = n.Tag; }
            if (n.HasLayer) { elem.Layer = n.Layer; }
            elem.IsStatic = n.IsStatic;

            objNodeTable[n.InstanceId] = elem;

            foreach (var child in n.Children) {
                var childelem = VisitGameObjectNode_r(child);
                elem.AddChild(childelem);
            }
            return elem;
        }

        CameraElem RegisterToThreeScene(CameraNode n) {
            var node = new PerspectiveCameraElem(n);
            var parent = objNodeTable[n.CurrentObject.GetInstanceID().ToString()];
            parent.AddChild(node);
            return node;
        }
        
        void RegisterLightmap(LightmapContainer n) {
            // create image
            var imageNode = new ImageElem(n);
            root.SharedNodeTable.Add(imageNode, n.InstanceId);

            var texNode = new TextureElem(n);
            root.SharedNodeTable.Add(texNode, n.InstanceId);

            texNode.Image = imageNode;
        }

        LightElem RegisterToThreeScene(LightNode n) {
            LightElem node = null;
            if(n.Type == LightType.Point) {
                node = new PointLightElem(n);
            } if(n.Type == LightType.Directional) {
                node = new DirectionalLightElem(n);
            }

            if (node != null) {
                var parent = objNodeTable[n.CurrentObject.GetInstanceID().ToString()];
                parent.AddChild(node);
                return node;
            } else {
                throw new NotImplementedException();
            }
        }

        MaterialElem CreateMaterialElem(MaterialContainer n) {
            var mtl = new MaterialElem(n);

            if (n.MainTexture) {
                var instanceId = n.MainTexture.GetInstanceID().ToString();
                mtl.Map = root.SharedNodeTable.Get<TextureElem>(instanceId);
            }

            if (n.BumpMap) {
                var instanceId = n.BumpMap.GetInstanceID().ToString();
                mtl.BumpMap = root.SharedNodeTable.Get<TextureElem>(instanceId);
            }

            if (n.DetailNormalMap) {
                var uid = n.DetailNormalMap.GetInstanceID().ToString();
                mtl.NormalMap = root.SharedNodeTable.Get<TextureElem>(uid);
            }

            if (n.ParallaxMap) {
                var uid = n.ParallaxMap.GetInstanceID().ToString();
                mtl.DisplacementMap = root.SharedNodeTable.Get<TextureElem>(uid);
            }
            if (n.OcclusionMap) {
                var uid = n.OcclusionMap.GetInstanceID().ToString();
                mtl.AoMap = root.SharedNodeTable.Get<TextureElem>(uid);
            }
            if (n.EmissionMap) {
                var uid = n.EmissionMap.GetInstanceID().ToString();
                mtl.EmissiveMap = root.SharedNodeTable.Get<TextureElem>(uid);
            }

            if (n.MetallicGlossMap) {
                var uid = n.MetallicGlossMap.GetInstanceID().ToString();
                mtl.MetalnessMap = root.SharedNodeTable.Get<TextureElem>(uid);
            }

            if (n.SpecGlossMap) {
                var uid = n.SpecGlossMap.GetInstanceID().ToString();
                mtl.SpecularMap = root.SharedNodeTable.Get<TextureElem>(uid);
            }

            return mtl;
        }

        void RegisterMeshWithoutLightmap(MeshContainer n) {
            AbstractGeometryElem geo = null;
            if (n.Mesh.name == "Sphere") {
                geo = new SphereBufferGeometryElem(n);
            } else if (n.Mesh.name == "Cylinder") {
                geo = new CylinderBufferGeometryElem(n);
            } else if (n.Mesh.name == "Quad") {
                geo = new QuadBufferGeometry(n);
            } else if(n.Mesh.name == "Cube") {
                geo = new BoxBufferGeometryElem(n);
            } else {
                geo = new BufferGeometryElem(n, false, Vector4.zero);
            } 
            root.SharedNodeTable.Add(geo, n.InstanceId);
        }

        void RegisterScriptVariables(UnitySceneRoot unityscene, Object3DElem obj, ScriptNode script) {
            foreach(var v in script.VariableEnumerable) {
                var val = v;

                Texture tex = val.GetTexture();
                if(tex) {
                    var elem = root.SharedNodeTable.Get<TextureElem>(tex.GetInstanceID().ToString());
                    val.value = elem.Uuid;
                }

                Material mtl = val.GetMaterial();
                if (mtl) {
                    var elem = root.SharedNodeTable.Get<MaterialElem>(mtl.GetInstanceID().ToString());
                    val.value = elem.Uuid;
                }

                Transform tr = val.GetTransform();
                if(tr) {
                    var elem = objNodeTable[tr.gameObject.GetInstanceID().ToString()];
                    val.value = elem.Uuid;
                }

                GameObject go = val.GetGameObject();
                if(go) {
                    var elem = objNodeTable[go.GetInstanceID().ToString()];
                    val.value = elem.Uuid;
                }

                obj.AddScriptVariable(script.BehaviourName, val);
            }
        }

        void Visit(ProjectSettings n) {
            // ambient light - 유니티에서는 프로젝트 설정이지만 three.js에서는 요소
            var ambientlight = new AmbientLightElem(n);
            var parent = objNodeTable[GameObjectNode.RootInstanceId];
            parent.AddChild(ambientlight);
        }
        
        void RegisterTexture(TextureContainer n) {
            // create image
            var imageNode = new ImageElem(n);
            root.SharedNodeTable.Add(imageNode, n.InstanceId);

            // create texture
            var texNode = new TextureElem(n);
            texNode.Image = imageNode;
            root.SharedNodeTable.Add(texNode, n.InstanceId);
        }

        MeshElem RegisterToThreeScene(RenderNode n) {
            var hasLightmap = (n.Value.lightmapIndex >= 0);
            var geoKey = n.Mesh.InstanceId;
            var mtlKey = n.Material.InstanceId;
            if(hasLightmap) {
                geoKey = ConvertKeyForBufferGeometryElem(n.Value);
                mtlKey = ConvertKeyForMaterialELem(n.Value);
            }

            var geometryNode = root.SharedNodeTable.Get<AbstractGeometryElem>(geoKey);
            var materialNode = root.SharedNodeTable.Get<MaterialElem>(mtlKey);

            Debug.Assert(geometryNode != null);
            Debug.Assert(materialNode != null);

            var meshNode = new MeshElem(n)
            {
                Geometry = geometryNode,
                Material = materialNode,
            };
            var parent = objNodeTable[n.CurrentObject.GetInstanceID().ToString()];
            parent.AddChild(meshNode);
            return meshNode;
        }

        string ConvertKeyForBufferGeometryElem(Renderer renderer) {
            var meshfilter = renderer.GetComponent<MeshFilter>();
            var mesh = meshfilter.sharedMesh;
            var basicKey = meshfilter.sharedMesh.GetInstanceID().ToString();

            if (renderer.lightmapIndex >= 0) {
                var attr = renderer.lightmapScaleOffset;
                var key = string.Format("{0}_{1},{2},{3},{4}", basicKey, attr.x, attr.y, attr.z, attr.w);
                return key;
            } else {
                // mesh container . instance id
                return basicKey;
            }
        }

        string ConvertKeyForMaterialELem(Renderer renderer) {
            var mtl = renderer.sharedMaterial;
            var basicKey = mtl.GetInstanceID().ToString();

            if(renderer.lightmapIndex >= 0) {
                var attr = renderer.lightmapScaleOffset;
                var key = string.Format("{0}_{1},{2},{3},{4}", basicKey, attr.x, attr.y, attr.z, attr.w);
                return key;

            } else {
                return basicKey;
            }
        }

    }
}