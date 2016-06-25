using Assets.Kanau.UnityScene.Containers;
using Assets.Kanau.Utils;
using System;
using UnityEngine;

namespace Assets.Kanau.UnityScene.SceneGraph
{
    public class RenderNode : ComponentNode<Renderer>
    {
        public MaterialContainer Material { get; private set; }
        public MeshContainer Mesh { get; private set; }
        public object MeshNode {
            get;
            internal set;
        }

        public override void Initialize<T1>(T1 comp, INodeTable<int> containerTable) {
            base.Initialize(comp, containerTable);

            // find mesh & material
            if (Value is MeshRenderer || Value is SkinnedMeshRenderer) {
                var mesh = ComponentHelper.GetMesh(Value.gameObject);
                Mesh = containerTable.Get<MeshContainer>(mesh.GetInstanceID());

                // TODO material이 여러개인 경우는?
                var mtl = Value.sharedMaterial;
                Material = containerTable.Get<MaterialContainer>(mtl.GetInstanceID());

            } else {
                throw new NotImplementedException("unknown renderer " + comp.GetType().ToString());
            }
        }
    }
}
