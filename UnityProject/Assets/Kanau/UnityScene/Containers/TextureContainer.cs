using Assets.Kanau.Utils;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets.Kanau.UnityScene.Containers
{
    public class TextureContainer
    {
        public Texture Tex { get; private set; }
        public int InstanceId { get { return Tex.GetInstanceID(); } }

        public TextureContainer(Texture tex) {
            this.Tex = tex;
        }

        public FilterMode FilterMode { get { return Tex.filterMode; } }
        public TextureWrapMode WrapMode { get { return Tex.wrapMode; } }
        public int AnisoLevel { get { return Tex.anisoLevel; } }
        public string Name { get { return Tex.name; } }

        public string FilePath { get { return AssetDatabase.GetAssetPath(Tex); } }
        public string FileName { get { return Path.GetFileName(FilePath); } }

        public TextureImporter Asset {
            get { return (TextureImporter)AssetImporter.GetAtPath(FilePath); }
        }
        public TextureImporterFormat Format { get { return Asset.textureFormat; } }
        private Texture2D Tex2d { get { return Tex as Texture2D; } }
        public bool IsImage { get { return Tex is Texture2D; } }

        private byte[] Bytes {
            get {
                return Tex2d.EncodeToPNG();
            }
        }

        public void Save(string path) {
            if (IsImage) {
                var report = Report.Instance();
                if (!Asset.isReadable) {
                    //report.Log("Texture not exported. '" + Name + "' not marked as readable. so use copy mode.");
                    File.Delete(path);
                    File.Copy(FilePath, path);
                } else {
                    File.WriteAllBytes(path + FileName, Bytes);
                    report.Log("Exporting texture " + Name + " to " + path + FileName);
                }
            }
        }
    }
}
