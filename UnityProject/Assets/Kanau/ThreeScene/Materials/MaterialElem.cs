using Assets.Kanau.ThreeScene.Textures;
using Assets.Kanau.UnityScene.Containers;
using Assets.Kanau.Utils;
using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Kanau.ThreeScene.Materials
{
    /// <summary>
    /// https://github.com/mrdoob/three.js/blob/master/src/materials/Material.js
    /// </summary>
    public class MaterialElem : BaseElem
    {
        

        string type;
        public override string Type { get { return type; } }

        MaterialContainer container;
        public Material Material { get { return container.Material; } }
        public MaterialContainer Container { get { return container; } }


        public MaterialElem(MaterialContainer c) {
            this.container = c;
            Name = c.Name;

            const string defaultType = "MeshStandardMaterial";
            var table = new Dictionary<string, string>()
            {
                { "Standard", "MeshStandardMaterial" },
                { "Standard (Specular setup)", defaultType },
                { "Mobile/Diffuse", "MeshPhongMaterial" },
                { "Mobile/VertexLit", "MeshLambertMaterial" },
                { "Mobile/Bumped Diffuse", "MeshPhongMaterial" },
                { "Mobile/Bumped Specular", "MeshPhongMaterial" },
                { "Unlit/Texture", "MeshBasicMaterial" },
                { "Unlit/Color", "MeshBasicMaterial" },
            };

            var shadername = c.Material.shader.name;
            string materialname;
            if(table.TryGetValue(shadername, out materialname)) {
                type = materialname;
            } else {
                type = defaultType;
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

                var w = FindWriter(type);
                w.Write(this, scope);
            }
        }

        IMaterialWriter[] writers = new IMaterialWriter[]
        {
            new MeshBasicMaterialWriter(),
            new MeshLambertMaterialWriter(),
            new MeshPhongMaterialWriter(),
            new MeshStandardMaterialWriter(),
        };

        IMaterialWriter FindWriter(string type) {
            foreach(var writer in writers) {
                if(writer.GetType().ToString().Contains(type)) { 
                    return writer;
                }
            }
            Debug.Assert(false, "cannot find writer for " + type);
            return null;
        }
    }
}
