/*
 * https://github.com/umiyuki/UnityAFrameExporter/blob/master/Assets/CombineMeshes/ObjExporter.cs
 */
using System.IO;
using System.Text;
using UnityEngine;

public class ObjExporter {

    public static string SkinnedMeshToString(SkinnedMeshRenderer skinnedMeshRenderer, bool flip_x) {
        Mesh m = skinnedMeshRenderer.sharedMesh;
        Material[] mats = skinnedMeshRenderer.sharedMaterials;

        StringBuilder sb = new StringBuilder();

        sb.Append("g ").Append(m.name).Append("\n");
        foreach (Vector3 v in m.vertices) {
            sb.Append(string.Format("v {0} {1} {2}\n", (flip_x ? -v.x : v.x), v.y, v.z));
        }
        sb.Append("\n");
        foreach (Vector3 v in m.normals) {
            sb.Append(string.Format("vn {0} {1} {2}\n", (flip_x ? -v.x : v.x), (flip_x ? -v.y : v.y), (flip_x ? -v.z : v.z)));
        }
        sb.Append("\n");
        foreach (Vector3 v in m.uv) {
            sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
        }
        for (int material = 0; material < m.subMeshCount; material++) {
            sb.Append("\n");
            sb.Append("usemtl ").Append(mats[material].name).Append("\n");
            sb.Append("usemap ").Append(mats[material].name).Append("\n");

            int[] triangles = m.GetTriangles(material);
            for (int i = 0; i < triangles.Length; i += 3) {
                sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                                        triangles[i] + 1, triangles[i + 1] + 1, triangles[i + 2] + 1));
            }
        }
        return sb.ToString();
    }

    public static void SkinnedMeshToFile(SkinnedMeshRenderer skineedMeshRenderer, string filename, bool flip_x = false) {
        using (StreamWriter sw = new StreamWriter(filename)) {
            sw.Write(SkinnedMeshToString(skineedMeshRenderer, flip_x));
        }
    }

    public static string MeshToString(MeshFilter mf, bool flip_x) {
        Mesh m = mf.sharedMesh;
        Material[] mats = mf.GetComponent<Renderer>().sharedMaterials;

        StringBuilder sb = new StringBuilder();

        sb.Append("g ").Append(mf.name).Append("\n");
        foreach (Vector3 v in m.vertices) {
            sb.Append(string.Format("v {0} {1} {2}\n", (flip_x ? -v.x : v.x), v.y, v.z));
        }
        sb.Append("\n");
        foreach (Vector3 v in m.normals) {
            sb.Append(string.Format("vn {0} {1} {2}\n", (flip_x ? -v.x : v.x), (flip_x ? -v.y : v.y), (flip_x ? -v.z : v.z)));
        }
        sb.Append("\n");
        foreach (Vector3 v in m.uv) {
            sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
        }
        for (int material = 0; material < m.subMeshCount; material++) {
            sb.Append("\n");
            sb.Append("usemtl ").Append(mats[material].name).Append("\n");
            sb.Append("usemap ").Append(mats[material].name).Append("\n");

            int[] triangles = m.GetTriangles(material);
            for (int i = 0; i < triangles.Length; i += 3) {
                sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                                        triangles[i] + 1, triangles[i + 1] + 1, triangles[i + 2] + 1));
            }
        }
        return sb.ToString();
    }

    public static string MeshToString(Mesh m) {
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("mtllib {0}.mtl", m.name).AppendLine();
        sb.AppendFormat("usemtl {0}", m.name).AppendLine();
        sb.AppendLine();

        sb.Append("g ").Append(m.name).Append("\n");
        foreach (Vector3 v in m.vertices) {
            sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, -v.z));
        }
        sb.Append("\n");
        foreach (Vector3 v in m.normals) {
            sb.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, -v.z));
        }
        sb.Append("\n");
        foreach (Vector3 v in m.uv) {
            sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
        }

        int[] triangles = m.triangles;
        for (int i = 0; i < triangles.Length; i += 3) {
            sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                                    triangles[i] + 1, triangles[i + 2] + 1, triangles[i + 1] + 1));
        }
        return sb.ToString();
    }

    public static void MeshToFile(MeshFilter mf, string filename, bool flip_x = false) {
        using (StreamWriter sw = new StreamWriter(filename)) {
            sw.Write(MeshToString(mf, flip_x));
        }
    }
    public static void MeshToFile(Mesh m, string filename) {
        using (StreamWriter sw = new StreamWriter(filename)) {
            sw.Write(MeshToString(m));
        }
    }
}