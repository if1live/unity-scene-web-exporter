using Assets.Kanau.Utils;
using System;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Kanau.UnityScene.Containers {
    public class LightmapContainer
    {
        const float JpegQuality = 75.0f;
        const float LightmapBrightness = 8.0f;
        const float LightmapContrast = 1.1f;

        public int Index { get; private set; }
        public string InstanceId { get { return Index.ToString(); } }

        public string Guid
        {
            get
            {
                return string.Format("lightmap-{0}", Index);
            }
        }

        public LightmapContainer(int index) {
            this.Index = index;
        }

        public string ExrAssetPath {
            get {
#if UNITY_EDITOR
                // http://answers.unity3d.com/questions/1114251/lightmappingcompleted-callback-occurs-before-light.html
                string curScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().path;
                string[] parts = curScene.Split('/', '\\');
                string sceneName = parts[parts.Length - 1].Split('.')[0];
                string lightmapPath = Path.GetDirectoryName(curScene) + "/" + sceneName + "/";
                return lightmapPath + ExrAssetFileName;
#else
                return "";
#endif
            }
        }

        public string ExrAssetFileName {
            get {
                string filename = "Lightmap-" + Index.ToString() + "_comp_light.exr";
                return filename;
            }
        }

        public string ExportedFileName(string extension) {
            return ExrAssetFileName.Replace(".exr", extension);
        }

        public void EnableReadable() {
#if UNITY_EDITOR
            TextureImporter texImporter = (TextureImporter)AssetImporter.GetAtPath(ExrAssetPath);
            if (!texImporter.isReadable) {
                texImporter.isReadable = true;
                texImporter.SaveAndReimport();
            }
#endif
        }

        public Texture2D LightmapFar {
            get { return LightmapSettings.lightmaps[Index].lightmapColor; }
        }

        public FilterMode FilterMode { get { return LightmapFar.filterMode; } }
        public TextureWrapMode WrapMode { get { return LightmapFar.wrapMode; } }
        public int AnisoLevel { get { return LightmapFar.anisoLevel; } }

        public void Save(ExportPathHelper pathHelper) {
            var filepath = pathHelper.ToImagePath(ExportedFileName(".jpg"));
            Save(filepath, JpegQuality, LightmapBrightness, LightmapContrast);
        }

        public void Save(string path, float jpegQuality, float lightmapMult, float lightmapPower) {
#if UNITY_EDITOR
            // http://answers.unity3d.com/questions/1114251/lightmappingcompleted-callback-occurs-before-light.html
            string curScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().path;
            string[] parts = curScene.Split('/', '\\');
            string sceneName = parts[parts.Length - 1].Split('.')[0];
            string lightmapPath = Path.GetDirectoryName(curScene) + "/" + sceneName + "/";
            string filepath = lightmapPath + "Lightmap-" + "0" + "_comp_light.exr";

            //TODO 어디에서 파일명을 얻을 것인가?
            //string filepath = "Assets/Scenes/simple/Lightmap-0_comp_light.exr";

            // readable 설정 켜주기
            TextureImporter texImporter = (TextureImporter)AssetImporter.GetAtPath(filepath);
            if (!texImporter.isReadable) {
                texImporter.isReadable = true;
                texImporter.SaveAndReimport();
            }

            Texture2D ti = LightmapSettings.lightmaps[Index].lightmapColor;

            Texture2D tf = new Texture2D(ti.width, ti.height, TextureFormat.ARGB32, false);
            Color32[] c = ti.GetPixels32();

            for (int j = 0; j < c.Length; j++) {
                float af = c[j].a / 255f;
                float rf = c[j].r / 255f;
                float gf = c[j].g / 255f;
                float bf = c[j].b / 255f;

                float ur = Mathf.Pow(rf * af, lightmapPower) * 255f * lightmapMult;
                float ug = Mathf.Pow(gf * af, lightmapPower) * 255f * lightmapMult;
                float ub = Mathf.Pow(bf * af, lightmapPower) * 255f * lightmapMult;

                ur = Mathf.Clamp(ur, 0, 255);
                ug = Mathf.Clamp(ug, 0, 255);
                ub = Mathf.Clamp(ub, 0, 255);

                c[j].r = Convert.ToByte(ur);
                c[j].g = Convert.ToByte(ug);
                c[j].b = Convert.ToByte(ub);
                c[j].a = 255;
            }

            tf.SetPixels32(c);

            JPGEncoder je = new JPGEncoder(tf, jpegQuality, "", true);
            byte[] bytes = je.GetBytes();
            File.WriteAllBytes(path, bytes);

            Texture2D.DestroyImmediate(tf);
#endif
        }
    }
}
