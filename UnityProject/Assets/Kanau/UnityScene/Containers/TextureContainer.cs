using Assets.Kanau.Utils;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Kanau.UnityScene.Containers {
    public class TextureContainer
    {
        public Texture Tex { get; private set; }
        public string InstanceId { get { return Tex.GetInstanceID().ToString(); } }

        public TextureContainer(Texture tex) {
            this.Tex = tex;
        }

        public FilterMode FilterMode { get { return Tex.filterMode; } }
        public TextureWrapMode WrapMode { get { return Tex.wrapMode; } }
        public int AnisoLevel { get { return Tex.anisoLevel; } }
        public string Name { get { return Tex.name; } }

        public string Guid
        {
            get
            {
#if UNITY_EDITOR
                var assetpath = AssetDatabase.GetAssetPath(Tex);
                var guid = AssetDatabase.AssetPathToGUID(assetpath);
                return guid;
#else
                return "";
#endif
            }
        }

        public string FilePath
        {
            get
            {
#if UNITY_EDITOR
                return AssetDatabase.GetAssetPath(Tex);
#else
                return "";
#endif
            }
        }

        public string FileName { get { return Path.GetFileName(FilePath); } }

#if UNITY_EDITOR
        public TextureImporter Asset {
            get { return (TextureImporter)AssetImporter.GetAtPath(FilePath); }
        }
        public TextureImporterFormat Format { get { return Asset.textureFormat; } }
#endif
        private Texture2D Tex2d { get { return Tex as Texture2D; } }
        public bool IsImage { get { return Tex is Texture2D; } }

        private byte[] Bytes {
            get {
                return Tex2d.EncodeToPNG();
            }
        }

        public void Save(string path) {
#if UNITY_EDITOR
            if (IsImage) {
                if (!Asset.isReadable) {
                    File.Delete(path);
                    File.Copy(FilePath, path);
                } else {
                    File.WriteAllBytes(path + FileName, Bytes);
                    Report.Instance.Log("Exporting texture '{0}' to {2}", Name, path + FileName);
                }
            }
#endif
        }
    }
}
