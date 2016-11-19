namespace Assets.Kanau.ThreeScene.Materials {
    public class MeshBasicMaterialWriter : AbstractMaterialWriter
    {
        public override string[] GetAttributes() {
            return new string[]
            {
                "color",
                "map",
                "aoMap",
                "aoMapIntensity",
                "specularMap",
                "alphaMap",
                "envMap",
                "combine",
                "reflectivity",
                "refractionRatio",
                "fog",
                "shading",
                "wireframe",
                "wireframeLinewidth",
                "wireframeLinecap",
                "wireframeLinejoin",
                "vertexColors",
                "skinning",
                "morphTargets"
            };
        }
    }
}
