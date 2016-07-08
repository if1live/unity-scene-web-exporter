using Assets.Kanau.UnityScene.Containers;
using Assets.Kanau.Utils;
using UnityEngine;

namespace Assets.Kanau.ThreeScene.Geometries {
    public abstract class AbstractGeometryElem : BaseElem
    {

    }


    /// <summary>
    /// http://threejs.org/docs/#Reference/Core/BufferGeometry
    /// 다른 종류의 메시까지 일일이 만들거같지 않아서 하나로 때운다
    /// </summary>
    public class BufferGeometryElem : AbstractGeometryElem
    {
        public override string Type { get { return "BufferGeometry"; } }
        public override void Accept(IVisitor v) { v.Visit(this); }

        public float[] Vertices { get; set; }
        public float[] Normals { get; set; }
        public int[] Faces { get; set; }
        public float[] Colors { get; set; }

        public float[] UV { get; set; }
        public float[] UV2 { get; set; }
        public float[] UV3 { get; set; }
        public float[] UV4 { get; set; }

        public BufferGeometryElem(MeshContainer c) {
            Mesh mesh = c.Mesh;

            // vertices
            if (mesh.vertices.Length > 0) {
                Vertices = FlattenHelper.Flatten(GLConverter.ConvertDxVectorToGlVector(mesh.vertices));
            }

            // faces
            if (mesh.triangles.Length > 0) {
                Faces = GLConverter.ConvertDxTrianglesToGlTriangles(mesh.triangles);
            }

            // normals
            if (mesh.normals.Length > 0) {
                Normals = FlattenHelper.Flatten(GLConverter.ConvertDxVectorToGlVector(mesh.normals));
            }

            // colors
            if(mesh.colors.Length > 0) {
                Colors = new float[mesh.colors.Length * 3];
                for(int i = 0; i < mesh.colors.Length; i++) {
                    var color = mesh.colors[i];
                    Colors[i * 4 + 0] = color.r;
                    Colors[i * 4 + 1] = color.g;
                    Colors[i * 4 + 2] = color.b;
                }
            }

            // uvs
            int uvCount = 0;

            if (mesh.uv.Length > 0) {
                uvCount++;
                UV = FlattenHelper.Flatten(GLConverter.ConvertDxTexcoordToGlTexcoord(mesh.uv));
            }

            if (mesh.uv2.Length > 0) {
                uvCount++;
                UV2 = FlattenHelper.Flatten(GLConverter.ConvertDxTexcoordToGlTexcoord(mesh.uv2));
            }

            if (mesh.uv3.Length > 0) {
                uvCount++;
                UV3 = FlattenHelper.Flatten(GLConverter.ConvertDxTexcoordToGlTexcoord(mesh.uv3));
            }

            if (mesh.uv4.Length > 0) {
                uvCount++;
                UV4 = FlattenHelper.Flatten(GLConverter.ConvertDxTexcoordToGlTexcoord(mesh.uv4));
            }
        }
    }    
}
