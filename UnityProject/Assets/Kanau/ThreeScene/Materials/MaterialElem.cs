using Assets.Kanau.ThreeScene.Textures;
using Assets.Kanau.UnityScene.Containers;
using Assets.Kanau.Utils;
using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Kanau.ThreeScene.Materials {
    /// <summary>
    /// https://github.com/mrdoob/three.js/blob/master/src/materials/Material.js
    /// </summary>
    public class MaterialElem : BaseElem
    {
        struct MaterialMappingTuple {
            public string unity;
            public string threejs;
            public string aframe;

            public MaterialMappingTuple(string unity, string threejs, string aframe) {
                this.unity = unity;
                this.threejs = threejs;
                this.aframe = aframe;
            }
        }
        // index=0 -> default
        readonly MaterialMappingTuple[] PredefinedMaterialTable = new MaterialMappingTuple[]
        {
            new MaterialMappingTuple("Standard", "MeshStandardMaterial", "Standard"),
            new MaterialMappingTuple("Standard (Specular setup)", "MeshStandardMaterial", "Standard"),
            new MaterialMappingTuple("Mobile/Diffuse", "MeshPhongMaterial", "Diffuse"),
            new MaterialMappingTuple("Mobile/VertexLit", "MeshLambertMaterial", "Diffuse"),
            new MaterialMappingTuple("Mobile/Bumped Diffuse", "MeshPhongMaterial", "Diffuse"),
            new MaterialMappingTuple("Mobile/Bumped Specular", "MeshPhongMaterial", "Diffuse"),
            new MaterialMappingTuple("Unlit/Texture", "MeshBasicMaterial", "UnlitTexture"),
            new MaterialMappingTuple("Unlit/Color", "MeshBasicMaterial", "UnlitColor"),
        };


        MaterialMappingTuple materialType;
        public override string Type { get { return materialType.threejs; } }

        MaterialContainer container;
        public Material Material { get { return container.Material; } }
        public MaterialContainer Container { get { return container; } }


        public MaterialElem(MaterialContainer c) {
            this.container = c;
            Name = c.Name;

            materialType = PredefinedMaterialTable[0];
            var shadername = c.Material.shader.name;
            foreach (var mtl in PredefinedMaterialTable) {
                if(mtl.unity == shadername) {
                    materialType = mtl;
                    break;
                }
            }
        }

        // Texture
        public TextureElem Map { get; set; }
        public TextureElem LightMap { get; set; }
        public TextureElem BumpMap { get; set; }
        public TextureElem SpecularMap { get; set; }
        public TextureElem NormalMap { get; set; }
        public TextureElem EmissiveMap { get; set; }
        public TextureElem RoughnessMap { get; set; }
        public TextureElem MetalnessMap { get; set; }
        public TextureElem AlphaMap { get; set; }
        public TextureElem EnvMap { get; set; }
        public TextureElem AoMap { get; set; }
        public TextureElem DisplacementMap { get; set; }
        

        public override void ExportJson(JsonWriter writer) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                scope.WriteKeyValue("uuid", Uuid);
                scope.WriteKeyValue("name", Name);
                scope.WriteKeyValue("type", Type);

                var w = FindThreeJSWriter(Type);
                w.Write(this, scope);
            }
        }

        IMaterialWriter[] threejsWriters = new IMaterialWriter[]
        {
            new MeshBasicMaterialWriter(),
            new MeshLambertMaterialWriter(),
            new MeshPhongMaterialWriter(),
            new MeshStandardMaterialWriter(),
        };
        IMaterialWriter FindThreeJSWriter(string type) {
            foreach(var writer in threejsWriters) {
                if(writer.GetType().ToString().Contains(type)) { 
                    return writer;
                }
            }
            Debug.Assert(false, "cannot find writer for " + type);
            return null;
        }

        Dictionary<string, IAFrameMaterialFactory> aframeMaterialFactory = new Dictionary<string, IAFrameMaterialFactory>()
        {
            { "Standard", new StandardAFrameMaterialFactory() },
            { "UnlitColor", new UnlitColorAFrameMaterialFactory() },
            { "UnlitTexture", new UnlitTextureAFrameMaterialFactory() },
            { "Diffuse", new DiffuseAFrameMaterialFactory() },
        };
        IAFrameMaterialFactory FindAFrameMaterialFactory(string type) {
            IAFrameMaterialFactory factory;
            if(aframeMaterialFactory.TryGetValue(type, out factory)) {
                return factory;
            } else {
                const string defaulttype = "Standard";
                return aframeMaterialFactory[defaulttype];
            }
        }


        public override AFrameNode ExportAFrame() {
            return null;
        }
        public IProperty GetAFrameMaterial() {
            var factory = FindAFrameMaterialFactory(materialType.aframe);
            var mtl = factory.Create(this);
            return mtl.CreateProperty();
        }
    }
}
