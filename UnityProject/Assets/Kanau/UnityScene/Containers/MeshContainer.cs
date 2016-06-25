using UnityEngine;

namespace Assets.Kanau.UnityScene.Containers
{
    public class MeshContainer
    {
        public Mesh Mesh { get; private set; }
        public int InstanceId { get { return Mesh.GetInstanceID(); } }

        public MeshContainer(Mesh m) {
            this.Mesh = m;
        }

        public string Name { get { return Mesh.name; } }
        public int VertexCount { get { return Mesh.vertexCount; } }
        public int TriangleCount { get { return Mesh.triangles.Length / 3; } }
    }
}
