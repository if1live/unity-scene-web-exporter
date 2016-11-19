using Assets.Kanau.ThreeScene.Textures;
using Assets.Kanau.UnityScene.Containers;
using UnityEngine;

namespace Assets.Kanau.ThreeScene.Materials {
    /// <summary>
    /// https://github.com/mrdoob/three.js/blob/master/src/materials/Material.js
    /// </summary>
    public class MaterialElem : BaseElem
    {
        class MaterialMappingTuple {
            public string from;
            public string to;

            public MaterialMappingTuple(string from, string to) {
                this.from = from;
                this.to = to;
            }
        }
        // index=0 -> default
        readonly MaterialMappingTuple[] PredefinedMaterialTable = new MaterialMappingTuple[]
        {
            new MaterialMappingTuple("Standard", "MeshStandardMaterial"),
            new MaterialMappingTuple("Standard (Specular setup)", "MeshStandardMaterial"),
            new MaterialMappingTuple("Mobile/Diffuse", "MeshPhongMaterial"),
            new MaterialMappingTuple("Mobile/VertexLit", "MeshLambertMaterial"),
            new MaterialMappingTuple("Mobile/Bumped Diffuse", "MeshPhongMaterial"),
            new MaterialMappingTuple("Mobile/Bumped Specular", "MeshPhongMaterial"),
            new MaterialMappingTuple("Unlit/Texture", "MeshBasicMaterial"),
            new MaterialMappingTuple("Unlit/Color", "MeshBasicMaterial"),
        };


        MaterialMappingTuple materialType;
        public override string Type { get { return materialType.to; } }

        MaterialContainer container;
        public Material Material { get { return container.Material; } }
        public MaterialContainer Container { get { return container; } }

        public bool Transparent { get; private set; }


        public MaterialElem(MaterialContainer c) {
            this.container = c;
            Transparent = false;
            Name = c.Name;

            materialType = PredefinedMaterialTable[0];
            var shadername = c.Material.shader.name;
            foreach (var mtl in PredefinedMaterialTable) {
                if(mtl.from == shadername) {
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

        public override void Accept(IVisitor v) {
            v.Visit(this);
        }
    }
}
