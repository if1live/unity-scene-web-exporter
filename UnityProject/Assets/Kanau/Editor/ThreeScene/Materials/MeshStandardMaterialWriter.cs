namespace Assets.Kanau.ThreeScene.Materials {
    /// <summary>
    /// http://threejs.org/docs/index.html#Reference/Materials/MeshStandardMaterial
    /// </summary>
    public class MeshStandardMaterialWriter : AbstractMaterialWriter
    {
        /*
        text = '''....'''
        print(",\n".join(['"' + x.split('-')[0].strip() + '"' for x in text.splitlines()]))
        */

        public override string[] GetAttributes() {
            return new string[]
            {
                "color",
                "roughness",
                "metalness",
                "map",
                "lightMap",
                "lightMapIntensity",
                "aoMap",
                "aoMapIntensity",
                "emissive",
                "emissiveMap",
                "emissiveIntensity",
                "bumpMap",
                //"bumpMapScale",
                "bumpScale",
                "normalMap",
                //"normalMapScale",
                "normalScale",
                "displacementMap",
                "displacementScale",
                "displacementBias",
                "roughnessMap",
                "metalnessMap",
                "alphaMap",
                "envMap",
                "envMapIntensity",
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
                "morphNormals",
            };
        }
    }
}
