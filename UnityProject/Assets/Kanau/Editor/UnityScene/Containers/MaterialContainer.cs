using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Kanau.UnityScene.Containers {
    public class MaterialContainer
    {
        public Material Material { get; private set; }
        public string InstanceId { get { return Material.GetInstanceID().ToString(); } }

        public MaterialContainer(Material mtl) {
            this.Material = mtl;
        }

        public string Name { get { return Material.name; } }

        public Vector2 MainTextureOffset { get { return Material.mainTextureOffset; } }
        public Vector2 MainTextureScale { get { return Material.mainTextureScale; } }

        public string Guid
        {
            get
            {
                var assetpath = AssetDatabase.GetAssetPath(Material);
                var guid = AssetDatabase.AssetPathToGUID(assetpath);
                return guid;
            }
        }

        // colors
        public Color Color { get { return GetColor("_Color", Color.white); } }
        public Color EmissionColor { get { return GetColor("_EmissionColor", Color.black); } }
        public Color SpecularColor 
        {
            get
            {
                // Mobile/Bumped Specular 같은 경우 _SpecColor가 없다
                // 이런 경우에는 _Color를 대신 사용
                if (Material.HasProperty("_SpecColor")) {
                    return GetColor("_SpecColor", Color.black);
                }
                // else..
                return Color;
            }
        }

        // textures
        public Texture MainTexture { get { return GetTexture("_MainTex", null); } }
        public Texture BumpMap { get { return GetTexture("_BumpMap", null); } }
        public Texture DetailNormalMap { get { return GetTexture("_DetailNormalMap", null); } }
        public Texture ParallaxMap { get { return GetTexture("_ParallaxMap", null); } }
        public Texture OcclusionMap { get { return GetTexture("_OcclusionMap", null); } }
        public Texture EmissionMap { get { return GetTexture("_EmissionMap", null); } }
        public Texture DetailMask { get { return GetTexture("_DetailMask", null); } }
        public Texture DetailAlbedoMap { get { return GetTexture("_DetailAlbedoMap", null); } }
        public Texture MetallicGlossMap { get { return GetTexture("_MetallicGlossMap", null); } }
        public Texture SpecGlossMap { get { return GetTexture("_SpecGlossMap", null); } }

        // float
        public float Shininess { get { return GetFloat("_Shininess", 0); } }
        public float SrcBlend { get { return GetFloat("_SrcBlend", 0); } }
        public float DstBlend { get { return GetFloat("_DstBlend", 0); } }
        public float Cutoff { get { return GetFloat("_Cutoff", 0); } }
        public float Parallax { get { return GetFloat("_Parallax", 0); } }
        public float ZWrite { get { return GetFloat("_ZWrite", 0); } }
        public float Glossiness { get { return GetFloat("_Glossiness", 0); } }
        public float BumpScale { get { return GetFloat("_BumpScale", 1); } }
        public float OcclusionStrength { get { return GetFloat("_OcclusionStrength", 0); } }
        public float DetailNormalMapScale { get { return GetFloat("_DetailNormalMapScale", 0); } }
        public float UVSec { get { return GetFloat("_UVSec", 0); } }
        public float Mode { get { return GetFloat("_Mode", 0); } }
        public float Metallic { get { return GetFloat("_Metallic", 0); } }

        // from unity aframe exporter
        public bool Transparent { get { return Mode == 3 ? true : false; } }

        public float Roughness { get { return 1 - Glossiness; } }

        private float GetFloat(string key, float defaultValue) {
            if (Material.HasProperty(key)) {
                return Material.GetFloat(key);
            } else {
                return defaultValue;
            }
        }

        private Color GetColor(string key, Color defaultValue) {
            if (Material.HasProperty(key)) {
                return Material.GetColor(key);
            } else {
                return defaultValue;
            }
        }

        private Texture GetTexture(string key, Texture defaultValue) {
            if (Material.HasProperty(key)) {
                return Material.GetTexture(key);
            } else {
                return defaultValue;
            }
        }

        public Texture[] Textures {
            get {
                List<Texture> allTextures = new List<Texture>
                {
                    MainTexture,
                    BumpMap,
                    DetailNormalMap,
                    ParallaxMap,
                    OcclusionMap,
                    EmissionMap,
                    DetailMask,
                    DetailAlbedoMap,
                    MetallicGlossMap,
                    SpecGlossMap,
                };
                var texs = from tex in allTextures where tex != null select tex;
                return texs.ToArray();
            }
        }
    }
}
