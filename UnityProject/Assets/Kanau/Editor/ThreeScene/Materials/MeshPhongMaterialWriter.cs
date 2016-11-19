namespace Assets.Kanau.ThreeScene.Materials {
    /// <summary>
    /// http://threejs.org/docs/index.html#Reference/Materials/MeshPhongMaterial
    /// </summary>
    public class MeshPhongMaterialWriter : AbstractMaterialWriter
    {
        public override string[] GetAttributes() {
            return new string[]
            {
                "color",
                "specular",
                "shininess",
                "map",
                "lightMap",
                "lightMapIntensity",
                "aoMap",
                "aoMapIntensity",
                "emissive",
                "emissiveMap",
                "emissiveIntensity",
                "bumpMap",
                "bumpScale",
                "normalMap",
                "normalScale",
                "displacementMap",
                "displacementScale",
                "displacementBias",
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
                "morphTargets",
                "morphNormals"
            };
        }
    }
}
