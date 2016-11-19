using UnityEngine;

namespace Assets.Kanau {
    /*
    https://github.com/mrdoob/three.js/blob/master/src/Three.js
    */

    public class Three
    {
        // MATERIAL CONSTANTS

        // side
        public const int FrontSide = 0;
        public const int BackSide = 1;
        public const int DoubleSide = 2;

        // shading
        public const int FlatShading = 1;
        public const int SmoothShading = 2;

        // colors
        public const int NoColors = 0;
        public const int FaceColors = 1;
        public const int VertexColors = 2;

        // Mapping modes

        // Wrapping modes
        public const int RepeatWrapping = 1000;
        public const int ClampToEdgeWrapping = 1001;
        public const int MirroredRepeatWrapping = 1002;

        // Filters
        public const int NearestFilter = 1003;
        public const int NearestMipMapNearestFilter = 1004;
        public const int NearestMipMapLinearFilter = 1005;
        public const int LinearFilter = 1006;
        public const int LinearMipMapNearestFilter = 1007;
        public const int LinearMipMapLinearFilter = 1008;

        public static string UnityColorToThreeColorString(Color c) {
            byte r = (byte)(c.r * 255);
            byte g = (byte)(c.g * 255);
            byte b = (byte)(c.b * 255);
            return string.Format("0x{0:X}{1:X}{2:X}", r, g, b);
        }

        public static uint UnityColorToThreeColorInt(Color c) {
            byte r = (byte)(c.r * 255);
            byte g = (byte)(c.g * 255);
            byte b = (byte)(c.b * 255);
            return (uint)(r << 16) | (uint)(g << 8) | (uint)(b << 0);
        }

        public static string UnityColorToHexColor(Color c) {
            byte r = (byte)(c.r * 255);
            byte g = (byte)(c.g * 255);
            byte b = (byte)(c.b * 255);

            var hex = r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
            return hex;
        }
    }
}
