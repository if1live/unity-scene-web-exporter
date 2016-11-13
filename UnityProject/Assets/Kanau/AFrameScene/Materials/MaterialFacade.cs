using Assets.Kanau.ThreeScene.Materials;
using System.Collections.Generic;

namespace Assets.Kanau.AFrameScene.Materials {
    class MaterialFacade {
        public static readonly MaterialFacade Instance;
        static MaterialFacade() {
            Instance = new MaterialFacade();
        }

        class MaterialMappingTuple {
            public string from;
            public string to;

            public MaterialMappingTuple(string from, string to) {
                this.to = to;
                this.from = from;
            }
        }
        // index=0 -> default
        readonly MaterialMappingTuple[] PredefinedMaterialTable = new MaterialMappingTuple[]
        {
            new MaterialMappingTuple("Standard", "Standard"),
            new MaterialMappingTuple("Standard (Specular setup)", "Standard"),
            new MaterialMappingTuple("Mobile/Diffuse", "Diffuse"),
            new MaterialMappingTuple("Mobile/VertexLit", "Diffuse"),
            new MaterialMappingTuple("Mobile/Bumped Diffuse", "Diffuse"),
            new MaterialMappingTuple("Mobile/Bumped Specular", "Diffuse"),
            new MaterialMappingTuple("Unlit/Texture", "UnlitTexture"),
            new MaterialMappingTuple("Unlit/Color", "UnlitColor"),
        };

        Dictionary<string, IMaterialFactory> aframeMaterialFactory = new Dictionary<string, IMaterialFactory>()
        {
            { "Standard", new StandardMaterialFactory() },
            { "UnlitColor", new UnlitColorMaterialFactory() },
            { "UnlitTexture", new UnlitTextureMaterialFactory() },
            { "Diffuse", new DiffuseMaterialFactory() },
        };

        IMaterialFactory FindMaterialFactory(string type) {
            IMaterialFactory factory;
            if (aframeMaterialFactory.TryGetValue(type, out factory)) {
                return factory;
            } else {
                const string defaulttype = "Standard";
                return aframeMaterialFactory[defaulttype];
            }
        }

        public AFrameMaterial CreateMaterial(MaterialElem el) {
            var materialType = PredefinedMaterialTable[0];
            var shadername = el.Material.shader.name;
            foreach (var m in PredefinedMaterialTable) {
                if (m.from == shadername) {
                    materialType = m;
                    break;
                }
            }

            var factory = FindMaterialFactory(materialType.to);
            return factory.Create(el);
        }

        public IProperty GetMaterialProperty(MaterialElem el) {
            var mtl = CreateMaterial(el);
            return mtl.CreateProperty();
        }
    }
}
