using Assets.Kanau.UnityScene.Containers;
using Assets.Kanau.UnityScene.SceneGraph;
using Assets.Kanau.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Kanau.UnityScene {
    public class UnitySceneRoot
    {
        readonly ProjectSettings settings;
        readonly INodeTable<string> containerTable;
        readonly INodeTable<string> graphNodeTable;

        public ProjectSettings Settings { get { return settings; } }
        public INodeTable<string> ContainerTable { get { return containerTable; } }
        public INodeTable<string> GraphNodeTable { get { return graphNodeTable; } }

        public UnitySceneRoot() {
            settings = new ProjectSettings();
            containerTable = CreateContainerTable();
            graphNodeTable = CreateGraphNodeTable();            
        }

        public void Add(GameObject go) {
            // containers
            VisitToCreateLightmap();
            VisitToCreateMesh_r(go);
            VisitToCreateMaterial_r(go);
            VisitToCreateTexture(containerTable.GetEnumerable<MaterialContainer>());

            // game object
            VisitToCreateGameObjectNode_r(go);

            // nodes - for component
            VisitToCreateNode_r<Camera, CameraNode>(go);
            VisitToCreateNode_r<Light, LightNode>(go);
            VisitToCreateRenderNode_r(go);
            VisitToCreateScriptNode_r(go);

            // script variable에서 참조하는 객체
           
            foreach (var node in graphNodeTable.GetEnumerable<ScriptNode>()) {
                foreach (var val in node.VariableEnumerable) {
                    Material mtl = val.GetMaterial();
                    if (mtl && !containerTable.Contains<MaterialContainer>(mtl.GetInstanceID().ToString())) {
                        var c = new MaterialContainer(mtl);
                        containerTable.Add(c.InstanceId, c);
                    }

                    Texture tex = val.GetTexture();
                    if (tex && !containerTable.Contains<TextureContainer>(tex.GetInstanceID().ToString())) {
                        var c = new TextureContainer(tex);
                        containerTable.Add(c.InstanceId, c);
                    }
                }
            }
            // 새로운 material이 추가되면 연관된 texture가 생겼을지 모른다
            VisitToCreateTexture(containerTable.GetEnumerable<MaterialContainer>());
        }

        void VisitToCreateLightmap() {
            for (int i = 0; i < LightmapSettings.lightmaps.Length; i++) {
                var key = i.ToString();
                if (containerTable.Contains<LightmapContainer>(key) == false) {
                    var container = new LightmapContainer(i);
                    containerTable.Add(key, container);
                }
            }
        }

        void VisitToCreateMesh_r(GameObject go) {
            if (go.GetComponent<Renderer>()) {
                var mesh = ComponentHelper.GetMesh(go);
                if(!containerTable.Contains<MeshContainer>(mesh.GetInstanceID().ToString())) {
                    var c = new MeshContainer(mesh);
                    containerTable.Add(c.InstanceId, c);
                }
            }

            for (int i = 0; i < go.transform.childCount; i++) {
                VisitToCreateMesh_r(go.transform.GetChild(i).gameObject);
            }
        }

        void VisitToCreateMaterial_r(GameObject go) {
            if (go.GetComponent<Renderer>()) {
                var material = go.GetComponent<Renderer>().sharedMaterial;
                if(!containerTable.Contains<MaterialContainer>(material.GetInstanceID().ToString())) {
                    var c = new MaterialContainer(material);
                    containerTable.Add(c.InstanceId, c);
                }
            }

            for (int i = 0; i < go.transform.childCount; i++) {
                VisitToCreateMaterial_r(go.transform.GetChild(i).gameObject);
            }
        }

        void VisitToCreateTexture(IEnumerable<MaterialContainer> materials) {
            foreach (var material in materials) {
                foreach (var tex in material.Textures) {
                    if (!containerTable.Contains<TextureContainer>(tex.GetInstanceID().ToString())) {
                        var c = new TextureContainer(tex);
                        containerTable.Add(c.InstanceId, c);
                    }
                }
            }
        }

        void VisitToCreateNode_r<Comp, Node>(GameObject go)
            where Comp : Component
            where Node : class, IUnityNode, new() {

            var comp = go.GetComponent<Comp>();

            var isValid = true;
            if(typeof(Comp) == typeof(Light) && comp != null) {
                // TODO remove if light is mixed or realtime
                // baked light is not need in scene
                var light = (Light)(object)comp;
                int a = 1;
            }

            if (comp && isValid) {
                var node = new Node();
                node.Initialize(comp, containerTable);

                if (!graphNodeTable.Contains<Node>(node.InstanceId)) {
                    graphNodeTable.Add(node.InstanceId, node);
                }
            }

            for (int i = 0; i < go.transform.childCount; i++) {
                VisitToCreateNode_r<Comp, Node>(go.transform.GetChild(i).gameObject);
            }
        }

        void VisitToCreateScriptNode_r(GameObject go) {
            MonoBehaviour[] scripts = go.GetComponents<MonoBehaviour>() as MonoBehaviour[];
            foreach (MonoBehaviour script in scripts) {
                var node = new ScriptNode();
                node.Initialize(script, containerTable);

                if(!graphNodeTable.Contains<ScriptNode>(node.InstanceId)) {
                    graphNodeTable.Add(node.InstanceId, node);
                }
            }

            for (int i = 0; i < go.transform.childCount; i++) {
                VisitToCreateScriptNode_r(go.transform.GetChild(i).gameObject);
            }
        }


        void VisitToCreateRenderNode_r(GameObject go) {
            var renderer = go.GetComponent<Renderer>();
            if (renderer) {
                var node = new RenderNode();
                node.Initialize(renderer, containerTable);

                if(!graphNodeTable.Contains<RenderNode>(node.InstanceId)) {
                    graphNodeTable.Add(node.InstanceId, node);
                }
            }

            for (int i = 0; i < go.transform.childCount; i++) {
                VisitToCreateRenderNode_r(go.transform.GetChild(i).gameObject);
            }
        }

        void VisitToCreateGameObjectNode_r(GameObject go) {
            // hierarchy를 펼쳐놓고 객체 추가하면 넣은거 또 넣을수도 있다
            // 그래서 assert대신 단순 검사로
            var node = new GameObjectNode(go);
            if(!graphNodeTable.Contains<GameObjectNode>(node.InstanceId)) {
                graphNodeTable.Add(node.InstanceId, node);
            }

            for (int i = 0; i < go.transform.childCount; i++) {
                VisitToCreateGameObjectNode_r(go.transform.GetChild(i).gameObject);
            }
        }

        INodeTable<string> CreateContainerTable() {
            var table = new NodeTable<string>();
            table.Register(new SingleTypeNodeTable<string, MeshContainer>());
            table.Register(new SingleTypeNodeTable<string, TextureContainer>());
            table.Register(new SingleTypeNodeTable<string, MaterialContainer>());
            table.Register(new SingleTypeNodeTable<string, LightmapContainer>());
            return table;
        }

        INodeTable<string> CreateGraphNodeTable() {
            var table = new NodeTable<string>();
            table.Register(new SingleTypeNodeTable<string, CameraNode>());
            table.Register(new SingleTypeNodeTable<string, LightNode>());

            table.Register(new SingleTypeNodeTable<string, RenderNode>());
            table.Register(new SingleTypeNodeTable<string, ScriptNode>());

            table.Register(new SingleTypeNodeTable<string, GameObjectNode>());

            return table;
        }
    }
}
