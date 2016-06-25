using UnityEngine;

namespace Assets.Kanau.Utils
{
    public class GLConverter
    {
        public static Vector2[] ConvertDxTexcoordToGlTexcoord(Vector2[] un_uvs) {
            // dx UV
            // +---
            // |
            // |

            // gl UV
            // |
            // |
            // +----
            var th_uvs = new Vector2[un_uvs.Length];
            for (int i = 0; i < un_uvs.Length; i++) {
                var un_uv = un_uvs[i];
                th_uvs[i] = new Vector2()
                {
                    x = un_uv.x,
                    //y = -un_uv.y + 1,
                    y = un_uv.y,
                };
            }
            return th_uvs;
        }

        public static Vector3[] ConvertDxVectorToGlVector(Vector3[] un_vecs) {
            var th_vecs = new Vector3[un_vecs.Length];
            for (int i = 0; i < un_vecs.Length; i++) {
                var un_vec = un_vecs[i];
                th_vecs[i] = new Vector3()
                {
                    x = un_vec.x,
                    y = un_vec.y,
                    z = -un_vec.z,
                };
            }
            return th_vecs;
        }

        public static int[] ConvertDxTrianglesToGlTriangles(int[] un_tris) {
            var th_tris = new int[un_tris.Length];
            var triCount = un_tris.Length / 3;
            for (int i = 0; i < triCount; i++) {
                var a = un_tris[i * 3 + 0];
                var b = un_tris[i * 3 + 1];
                var c = un_tris[i * 3 + 2];

                th_tris[i * 3 + 0] = a;
                th_tris[i * 3 + 1] = c;
                th_tris[i * 3 + 2] = b;
            }
            return th_tris;
        }
    }
}
