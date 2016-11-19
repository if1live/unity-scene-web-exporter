using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Kanau.UnityScene.Containers {
    public class MeshContainer
    {
        public Mesh Mesh { get; private set; }
        public string InstanceId { get { return Mesh.GetInstanceID().ToString(); } }

        public MeshContainer(Mesh m) {
            this.Mesh = m;
        }

        public string Name { get { return Mesh.name; } }
        public int VertexCount { get { return Mesh.vertexCount; } }
        public int TriangleCount { get { return Mesh.triangles.Length / 3; } }

        public string Guid
        {
            get
            {
#if UNITY_EDITOR
                var assetpath = AssetDatabase.GetAssetPath(Mesh);
                var guid = AssetDatabase.AssetPathToGUID(assetpath);
                return guid;
#else
                return "";
#endif
            }
        }
    }
}
