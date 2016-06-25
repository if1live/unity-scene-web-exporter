using Assets.Kanau.UnityScene.Containers;
using Assets.Kanau.Utils;
using System;

namespace Assets.Kanau.UnityScene.SceneGraph
{
    class DumpVisitor
    {
        Report report;

        public DumpVisitor(Report report) {
            this.report = report;
        }

        public void Run(UnitySceneRoot root) {
            
            report.Info("# Project Settings");
            Visit(root.Settings);
            report.Info("");
            
            // containers
            report.Info("# Material");
            foreach(var n in root.ContainerTable.GetEnumerable<MaterialContainer>()) { 
                Visit(n);
                report.Info("");
            }

            report.Info("# Texture");
            foreach (var n in root.ContainerTable.GetEnumerable<TextureContainer>()) {
                Visit(n);
                report.Info("");
            }

            /*
            TODO
            report.Info("# Lightmap");
            foreach (var x in n.LightmapEnumerable) {
                Visit(x.Value);
                report.Info("");
            }
            */

            // scene graph
            report.Info("# Mesh");
            foreach (var n in root.ContainerTable.GetEnumerable<MeshContainer>()) {
                Visit(n);
                report.Info("");
            }

            // component
            report.Info("# Render");
            foreach(var n in root.GraphNodeTable.GetEnumerable<RenderNode>()) {
                Visit(n);
                report.Info("");
            }

            report.Info("# Camera");
            foreach (var n in root.GraphNodeTable.GetEnumerable<CameraNode>()) {
                Visit(n);
                report.Info("");
            }

            report.Info("# Light");
            foreach (var n in root.GraphNodeTable.GetEnumerable<LightNode>()) {
                Visit(n);
                report.Info("");
            }

            report.Info("# Script");
            foreach (var n in root.GraphNodeTable.GetEnumerable<ScriptNode>()) {
                Visit(n);
                report.Info("");
            }

            // gameobject
            report.Info("# GameObject");
            foreach (var n in root.GraphNodeTable.GetEnumerable<GameObjectNode>()) {
                Visit(n);
                report.Info("");
            }
        }

        
        public void Visit(CameraNode n)
        {
            report.Info("## InstanceId: " + n.InstanceId);
            report.Info("Type: " + n.Type);
            report.Info("FieldOfView: " + n.FieldOfView);
            report.Info("Near: " + n.Near);
            report.Info("Far: " + n.Far);
            report.Info("GameObject: " + n.CurrentObject.GetInstanceID());
        }
        public void Visit(GameObjectNode n)
        {
            report.Info("## InstanceId: " + n.InstanceId);
            report.Info("Name: " + n.Name);
            if (n.HasLayer) { report.Info("LayerId: " + n.LayerId); }
            if (n.HasTag) { report.Info("Tag: " + n.Tag); }
            report.Info("IsStatic: " + n.IsStatic);
            report.Info("ActiveInHierarchy: " + n.ActiveInHierarchy);
            report.Info("ActiveSelf: " + n.ActiveSelf);
        }

        public void Visit(RenderNode n) {
            report.Info("## InstanceId: " + n.InstanceId);
            report.Info("GameObject: " + n.CurrentObject.GetInstanceID());
            report.Info("Mesh: " + n.Mesh.InstanceId);
            report.Info("Material: " + n.Material.InstanceId);
        }

        /*
        TODO
        public void Visit(LightmapNode n)
        {
           report.Info("## InstanceId: " + n.InstanceId);
           report.Info("Index: " + n.Index);
           report.Info("Exr: " + n.ExrAssetFileName);
        }
        */

        public void Visit(LightNode n)
        {
            report.Info("## InstanceId: " + n.InstanceId);
            report.Info("Type: " + n.Type);
            report.Info("Color: " + n.Color);
            report.Info("Direction: " + n.Direction);
            report.Info("GameObject: " + n.CurrentObject.GetInstanceID());
        }
        public void Visit(MaterialContainer n)
        {
            report.Info("## InstanceId: " + n.InstanceId);
            report.Info("Name: " + n.Name);
        }
        public void Visit(MeshContainer n)
        {
            report.Info("## InstanceId: " + n.InstanceId);
            report.Info("Name: " + n.Name);
            report.Info("VertexCount: " + n.VertexCount);
            report.Info("TriangleCount: " + n.TriangleCount);
        }

        public void Visit(ScriptNode n)
        {
            report.Info("## InstanceId: " + n.InstanceId);
            report.Info("GameObject: " + n.CurrentObject.GetInstanceID());
        }
        public void Visit(ProjectSettings n)
        {
            report.Info("AmbientColor: " + n.AmbientColor.ToString());
            report.Info("BackgroundColor: " + n.BackgroundColor.ToString());
            report.Info("Layers : " + String.Join(",", n.Layers));
        }
        
        public void Visit(TextureContainer n)
        {
            report.Info("## InstanceId: " + n.InstanceId);
        }
    }
}
