namespace Assets.Kanau.ThreeScene.Materials {
    public class MeshLambertMaterialWriter : AbstractMaterialWriter
    {
        public override string[] GetAttributes() {
            return new string[]
            {
                "color",
                "map",
                "lightMap",
                "lightMapIntensity",
                "aoMap",
                "aoMapIntensity",
                "emissive",
                "emissiveMap",
                "emissiveIntensity",
                "specularMap",
                "alphaMap",
                "envMap",
                "combine",
                "reflectivity",
                "refractionRatio",
                "fog",
                "wireframe",
                "wireframeLinewidth",
                "wireframeLinecap",
                "wireframeLinejoin",
                "vertexColors",
                "skinning",
                "morphTargets",
                "morphNormals",
            };
        }
    }
}
