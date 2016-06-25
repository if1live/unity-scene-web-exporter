using Assets.Kanau.UnityScene.Containers;
using Assets.Kanau.Utils;
using LitJson;
using System;
using UnityEngine;

namespace Assets.Kanau.ThreeScene.Geometries
{
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

        void ExportAttributes<T>(string name, T[] arr, int itemSize, string arrtype, JsonWriter writer) {
            if (arr == null) {
                return;
            }

            writer.WritePropertyName(name);
            using (var s = new JsonScopeObjectWriter(writer)) {
                s.WriteKeyValue("itemSize", itemSize);
                s.WriteKeyValue("type", arrtype);
                if (typeof(T) == typeof(float)) {
                    s.WriteKeyValue("array", (float[])Convert.ChangeType(arr, typeof(float[])));
                } else if(typeof(T) == typeof(int)) {
                    s.WriteKeyValue("array", (int[])Convert.ChangeType(arr, typeof(int[])));
                }
            }
        }

        public override void ExportJson(JsonWriter writer) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                scope.WriteKeyValue("uuid", Uuid);
                scope.WriteKeyValue("type", Type);

                writer.WritePropertyName("data");
                using (var s1 = new JsonScopeObjectWriter(writer)) {
                    ExportAttributes("index", Faces, 1, "Uint16Array", writer);

                    writer.WritePropertyName("attributes");
                    using (var s2 = new JsonScopeObjectWriter(writer)) {
                        ExportAttributes("position", Vertices, 3, "Float32Array", writer);
                        ExportAttributes("normal", Normals, 3, "Float32Array", writer);
                        ExportAttributes("color", Colors, 3, "Float32Array", writer);
                        ExportAttributes("uv", UV, 2, "Float32Array", writer);
                        ExportAttributes("uv2", UV2, 2, "Float32Array", writer);
                        ExportAttributes("uv3", UV3, 2, "Float32Array", writer);
                        ExportAttributes("uv4", UV4, 2, "Float32Array", writer);
                    }
                }
            }
        }
    }

    public class BoxBufferGeometryElem : AbstractGeometryElem
    {
        public override string Type { get { return "BoxBufferGeometry"; } }

        public BoxBufferGeometryElem(MeshContainer c) {
        }

        public override void ExportJson(JsonWriter writer) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                scope.WriteKeyValue("uuid", Uuid);
                scope.WriteKeyValue("type", Type);

                scope.WriteKeyValue("width", 1);
                scope.WriteKeyValue("height", 1);
                scope.WriteKeyValue("depth", 1);
            }
        }
    }

    public class SphereBufferGeometryElem : AbstractGeometryElem
    {
        public override string Type { get { return "SphereBufferGeometry"; } }

        public SphereBufferGeometryElem(MeshContainer c) {
        }

        public override void ExportJson(JsonWriter writer) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                scope.WriteKeyValue("uuid", Uuid);
                scope.WriteKeyValue("type", Type);

                scope.WriteKeyValue("radius", 0.5f);
                scope.WriteKeyValue("widthSegments", 16);
                scope.WriteKeyValue("heightSegments", 16);
            }
        }
    }

    public class CylinderBufferGeometryElem : AbstractGeometryElem
    {
        public override string Type { get { return "CylinderBufferGeometry"; } }

        public CylinderBufferGeometryElem(MeshContainer c) {
        }

        public override void ExportJson(JsonWriter writer) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                scope.WriteKeyValue("uuid", Uuid);
                scope.WriteKeyValue("type", Type);

                scope.WriteKeyValue("radiusTop", 0.5f);
                scope.WriteKeyValue("radiusBottom", 0.5f);
                scope.WriteKeyValue("height", 2);
                scope.WriteKeyValue("radiusSegments", 16);
            }
        }
    }
}
