using UnityEngine;

namespace Assets.Kanau.Utils
{
    public class FlattenHelper
    {
        public static float[] Flatten(Vector2[] data) {
            const int stride = 2;
            var retval = new float[data.Length * stride];
            for (int i = 0; i < data.Length; i++) {
                var p = data[i];
                for(int j = 0; j < stride; j++) {
                    retval[i * stride + j] = p[j];
                }
            }
            return retval;
        }

        public static float[] Flatten(Vector3[] data) {
            const int stride = 3;
            var retval = new float[data.Length * stride];
            for (int i = 0; i < data.Length; i++) {
                var p = data[i];
                for (int j = 0; j < stride; j++) {
                    retval[i * stride + j] = p[j];
                }
            }
            return retval;
        }

        public static float[] Flatten(Vector4[] data) {
            const int stride = 4;
            var retval = new float[data.Length * stride];
            for (int i = 0; i < data.Length; i++) {
                var p = data[i];
                for (int j = 0; j < stride; j++) {
                    retval[i * stride + j] = p[j];
                }
            }
            return retval;
        }
    }
}
