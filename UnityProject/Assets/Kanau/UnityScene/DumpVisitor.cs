using Assets.Kanau.UnityScene.Containers;
using Assets.Kanau.Utils;
using System;

namespace Assets.Kanau.UnityScene.SceneGraph {
    class DumpVisitor
    {
        Report report;

        public DumpVisitor(Report report) {
            this.report = report;
        }

        public void Run(UnitySceneRoot root) {
            
            report.Log("# Project Settings");
            Visit(root.Settings);
            report.Log("");
            
            // containers
            report.Log("# Material");
            foreach(var n in root.ContainerTable.GetEnumerable<MaterialContainer>()) { 
                Visit(n);
                report.Log("");
            }

            report.Log("# Texture");
            foreach (var n in root.ContainerTable.GetEnumerable<TextureContainer>()) {
                Visit(n);
                report.Log("");
            }

            /*
            TODO
            report.Log("# Lightmap");
            foreach (var x in n.LightmapEnumerable) {
                Visit(x.Value);
                report.Log("");
            }
            */

            // scene graph
            report.Log("# Mesh");
            foreach (var n in root.ContainerTable.GetEnumerable<MeshContainer>()) {
                Visit(n);
                report.Log("");
            }

            // component
            report.Log("# Render");
            foreach(var n in root.GraphNodeTable.GetEnumerable<RenderNode>()) {
                Visit(n);
                report.Log("");
            }

            report.Log("# Camera");
            foreach (var n in root.GraphNodeTable.GetEnumerable<CameraNode>()) {
                Visit(n);
                report.Log("");
            }

            report.Log("# Light");
            foreach (var n in root.GraphNodeTable.GetEnumerable<LightNode>()) {
                Visit(n);
                report.Log("");
            }

            report.Log("# Script");
            foreach (var n in root.GraphNodeTable.GetEnumerable<ScriptNode>()) {
                Visit(n);
                report.Log("");
            }

            // gameobject
            report.Log("# GameObject");
            foreach (var n in root.GraphNodeTable.GetEnumerable<GameObjectNode>()) {
                Visit(n);
                report.Log("");
            }
        }

        
        public void Visit(CameraNode n)
        {
            report.Log("## InstanceId: " + n.InstanceId);
            report.Log("Type: " + n.Type);
            report.Log("FieldOfView: " + n.FieldOfView);
            report.Log("Near: " + n.Near);
            report.Log("Far: " + n.Far);
            report.Log("GameObject: " + n.CurrentObject.GetInstanceID());
        }
        public void Visit(GameObjectNode n)
        {
            report.Log("## InstanceId: " + n.InstanceId);
            report.Log("Name: " + n.Name);
            if (n.HasLayer) { report.Log("LayerId: " + n.LayerId); }
            if (n.HasTag) { report.Log("Tag: " + n.Tag); }
            report.Log("IsStatic: " + n.IsStatic);
            report.Log("ActiveInHierarchy: " + n.ActiveInHierarchy);
            report.Log("ActiveSelf: " + n.ActiveSelf);
        }

        public void Visit(RenderNode n) {
            report.Log("## InstanceId: " + n.InstanceId);
            report.Log("GameObject: " + n.CurrentObject.GetInstanceID());
            report.Log("Mesh: " + n.Mesh.InstanceId);
            report.Log("Material: " + n.Material.InstanceId);
        }

        /*
        TODO
        public void Visit(LightmapNode n)
        {
           report.Log("## InstanceId: " + n.InstanceId);
           report.Log("Index: " + n.Index);
           report.Log("Exr: " + n.ExrAssetFileName);
        }
        */

        public void Visit(LightNode n)
        {
            report.Log("## InstanceId: " + n.InstanceId);
            report.Log("Type: " + n.Type);
            report.Log("Color: " + n.Color);
            report.Log("Direction: " + n.Direction);
            report.Log("GameObject: " + n.CurrentObject.GetInstanceID());
        }
        public void Visit(MaterialContainer n)
        {
            report.Log("## InstanceId: " + n.InstanceId);
            report.Log("Name: " + n.Name);
        }
        public void Visit(MeshContainer n)
        {
            report.Log("## InstanceId: " + n.InstanceId);
            report.Log("Name: " + n.Name);
            report.Log("VertexCount: " + n.VertexCount);
            report.Log("TriangleCount: " + n.TriangleCount);
        }

        public void Visit(ScriptNode n)
        {
            report.Log("## InstanceId: " + n.InstanceId);
            report.Log("GameObject: " + n.CurrentObject.GetInstanceID());
        }
        public void Visit(ProjectSettings n)
        {
            report.Log("AmbientColor: " + n.AmbientColor.ToString());
            report.Log("BackgroundColor: " + n.BackgroundColor.ToString());
            report.Log("Layers : " + String.Join(",", n.Layers));
        }
        
        public void Visit(TextureContainer n)
        {
            report.Log("## InstanceId: " + n.InstanceId);
        }
    }
}
