using Assets.Kanau.ThreeScene.Textures;
using Assets.Kanau.Utils;
using UnityEngine;

namespace Assets.Kanau.ThreeScene.Materials {
    public interface IMaterialWriter
    {
        void Write(MaterialElem mtl, JsonScopeObjectWriter scope);
    }

    public abstract class AbstractMaterialWriter : IMaterialWriter
    {
        MaterialElem elem;
        JsonScopeObjectWriter scope;

        public void Write(MaterialElem elem, JsonScopeObjectWriter scope) {
            this.elem = elem;
            this.scope = scope;

            var attrs = GetAttributes();
            foreach(var attr in attrs) {
                WriteCommonProperty(attr);
            }

            WriteProperty("transparent", elem.Transparent, false);
        }

        protected void WriteProperty(string key, int value, int defaultval) {
            if(value != defaultval) {
                scope.WriteKeyValue(key, value);
            }
        }
        protected void WriteProperty(string key, bool value, bool defaultval) {
            if (value != defaultval) {
                scope.WriteKeyValue(key, value);
            }
        }
        protected void WriteProperty(string key, float value, float defaultval) {
            if (value != defaultval) {
                scope.WriteKeyValue(key, value);
            }
        }
        protected void WriteProperty(string key, uint value, uint defaultval) {
            if (value != defaultval) {
                scope.WriteKeyValue(key, value);
            }
        }
        protected void WriteProperty(string key, float[] value, float[] defaultval) {
            bool equal = false;
            if(value.Length == defaultval.Length) {
                for(int i = 0; i < value.Length; i++) {
                    if(defaultval[i] != value[i]) {
                        equal = false;
                    }
                }
            }

            if(!equal) {
                scope.WriteKeyValue(key, value);
            }
        }

        protected void WriteProperty(string key, Color value, Color defaultval) {
            if (value != defaultval) {
                uint colorval = Three.UnityColorToThreeColorInt(value);
                scope.WriteKeyValue(key, colorval);
            }
        }
        protected void WriteProperty(string key, TextureElem value) {
            if(value != null) {
                scope.WriteKeyValue(key, value.Uuid);
            }
        }
        

        public abstract string[] GetAttributes();


        bool WriteLightmapProperty(string key) {
            switch (key) {
                case "lightMap":
                    // lightMap — Set light map.Default is null.
                    WriteProperty(key, elem.LightMap);
                    break;
                case "lightMapIntensity":
                    // lightMapIntensity — Set light map intensity. Default is 1.
                    break;
                default:
                    return false;
            }
            return true;
        }
        bool WriteEmissiveProperty(string key) {
            var container = elem.Container;
            switch (key) {
                // emissive
                case "emissive":
                    // emissive - Set emissive color.Default is 0x000000.
                    var TheDefaultEmissiveColor = new Color(0, 0, 0);
                    WriteProperty(key, container.EmissionColor, TheDefaultEmissiveColor);
                    break;
                case "emissiveMap":
                    // emissiveMap — Set emissive map.Default is null.
                    WriteProperty(key, elem.EmissiveMap);
                    break;
                case "emissiveIntensity":
                    // emissiveIntensity — Set emissive map intensity. Default is 1.
                    WriteProperty(key, 1, 1);
                    break;
                default:
                    return false;
            }
            return true;
        }

        bool WriteBumpMapProperty(string key) {
            var container = elem.Container;
            switch (key) {
                // bump map
                case "bumpMap":
                    // bumpMap — Set bump map.Default is null.
                    WriteProperty(key, elem.BumpMap);
                    break;
                case "bumpScale":
                    //bumpScale — Set bump map scale. Default is 1.
                    WriteProperty(key, container.BumpScale / 10, 1);
                    break;
                case "bumpMapScale":
                    // bumpMapScale — Set bump map scale. Default is 1.
                    WriteProperty(key, container.BumpScale / 10, 1);
                    break;
                default:
                    return false;
            }
            return true;
        }

        bool WriteNormalMapProperty(string key) {
            var container = elem.Container;
            switch (key) {
                // normal
                case "normalMap":
                    //normalMap — Set normal map.Default is null.
                    WriteProperty(key, elem.NormalMap);
                    break;
                case "normalScale":
                    // normalScale — Set normal map scale. Default is (1, 1).
                    WriteProperty(key, new float[] { container.BumpScale/10, container.BumpScale/10 }, new float[] { 1, 1 });
                    break;
                case "normalMapScale":
                    // normalMapScale — Set normal map scale. Default is (1, 1).
                    WriteProperty(key, new float[] { container.BumpScale/10, container.BumpScale/10 }, new float[] { 1, 1 });
                    break;
                default:
                    return false;
            }
            return true;
        }

        bool WriteWireframeProperty(string key) {
            switch (key) {
                // wireframe
                case "wireframe":
                    // wireframe — render geometry as wireframe.Default is false.
                    WriteProperty(key, false, false);
                    break;
                case "wireframeLinewidth":
                    // wireframeLinewidth — Line thickness. Default is 1.
                    WriteProperty(key, 1, 1);
                    break;
                case "wireframeLinecap":
                    // wireframeLinecap — Define appearance of line ends.Default is 'round'.
                    break;
                case "wireframeLinejoin":
                    // wireframeLinejoin — Define appearance of line joints.Default is 'round'.
                    break;
                default:
                    return false;
            }
            return true;
        }

        bool WriteDisplacementProperty(string key) {
            var container = elem.Container;
            switch (key) {
                // displacement = height
                case "displacementMap":
                    //displacementMap — Set displacement map.Default is null.
                    WriteProperty(key, elem.DisplacementMap);
                    break;
                case "displacementScale":
                    // displacementScale — Set displacement scale.Default is 1.
                    WriteProperty(key, container.Parallax, 1);
                    break;
                case "displacementBias":
                    // displacementBias — Set displacement offset.Default is 0.
                    WriteProperty(key, 0, 0);
                    break;
                default:
                    return false;
            }
            return true;
        }

        bool WriteAoProperty(string key) {
            var container = elem.Container;
            switch (key) {
                // ao
                case "aoMap":
                    // aoMap — Set ao map.Default is null.
                    WriteProperty(key, elem.AoMap);
                    break;
                case "aoMapIntensity":
                    // aoMapIntensity — Set ao map intensity. Default is 1.
                    WriteProperty(key, container.OcclusionStrength, 1);
                    break;
                default:
                    return false;
            }
            return true;
        }

        bool WriteSpecularProperty(string key) {
            var container = elem.Container;
            switch (key) {
                case "specular":
                    //  specular — Set specular color. Default is 0x111111 .
                    var TheDefaultSpecularColor = new Color(0.067f, 0.067f, 0.067f);
                    WriteProperty(key, container.SpecularColor, TheDefaultSpecularColor);
                    break;
                case "specularMap":
                    // specularMap — Set specular map. Default is null.
                    WriteProperty(key, elem.SpecularMap);
                    break;

                case "shininess":
                    // shininess — Set shininess Default is 30.
                    // 유니티에서는 0~1라서 역수를 이용
                    float shininess = container.Shininess;
                    if(shininess > 0) {
                        shininess = 1 / shininess;
                    }
                    WriteProperty(key, shininess, 30);
                    break;

                default:
                    return false;
            }
            return true;
        }

        bool WriteStandardMaterialProperty(string key) {
            var container = elem.Container;
            switch (key) {
                case "roughness":
                    // roughness — Set roughness.Default is 0.5.
                    WriteProperty(key, container.Roughness, 0.5f);
                    break;
                case "roughnessMap":
                    //roughnessMap - Set roughness map.Default is null.
                    WriteProperty(key, elem.RoughnessMap);
                    break;

                case "metalness":
                    // metalness — Set metalness. Default is 0.5.
                    WriteProperty(key, container.Metallic, 0.5f);
                    break;
                case "metalnessMap":
                    //metalnessMap - Set metalness map.Default is null.
                    WriteProperty(key, elem.MetalnessMap);
                    break;

                default:
                    return false;
            }
            return true;
        }

        bool WriteEnvMapProperty(string key) {
            switch (key) {
                case "envMap":
                    //envMap — Set env map.Default is null.
                    WriteProperty(key, elem.EnvMap);
                    break;
                case "envMapIntensity":
                    // envMapIntensity — Set env map intensity.Default is 1.0.
                    break;
                default:
                    return false;
            }
            return true;
        }

        protected void WriteCommonProperty(string key) {
            var container = elem.Container;

            if (WriteLightmapProperty(key)) { return; }
            if (WriteEmissiveProperty(key)) { return; }
            if (WriteBumpMapProperty(key)) { return; }
            if (WriteNormalMapProperty(key)) { return; }
            if (WriteWireframeProperty(key)) { return; }
            if (WriteDisplacementProperty(key)) { return; }
            if (WriteAoProperty(key)) { return; }
            if (WriteSpecularProperty(key)) { return; }
            if (WriteStandardMaterialProperty(key)) { return; }
            if (WriteEnvMapProperty(key)) { return; }

            switch (key) {
                case "color":
                    // color — geometry color in hexadecimal.Default is 0xffffff.
                    var TheDefaultColor = new Color(1.0f, 1.0f, 1.0f);
                    WriteProperty(key, container.Color, TheDefaultColor);
                    break;
                case "map":
                    // map — Set texture map.Default is null.
                    WriteProperty(key, elem.Map);
                    break;               
                

                case "alphaMap":
                    //alphaMap — Set alpha map.Default is null.
                    WriteProperty(key, elem.AlphaMap);
                    break;
                
                case "vertexColors":
                    // vertexColors — Define how the vertices gets colored. Default is THREE.NoColors.
                    WriteProperty(key, Three.VertexColors, Three.VertexColors);
                    break;

                case "combine":
                    // combine — Set combine operation.Default is THREE.MultiplyOperation.
                    break;
                
                case "reflectivity":
                    // reflectivity — Set reflectivity. Default is 1."
                    break;
                case "refractionRatio":
                    // refractionRatio — Set refraction ratio.Default is 0.98.
                    break;

                case "shading":
                    // shading — Define shading type.Default is THREE.SmoothShading.
                    break;
                

                case "fog":
                    // fog — Define whether the material color is affected by global fog settings.Default is true.
                    WriteProperty(key, true, true);
                    break;

                case "skinning":
                    // skinning — Define whether the material uses skinning. Default is false.
                    WriteProperty(key, false, false);
                    break;
                case "morphTargets":
                    // morphTargets — Define whether the material uses morphTargets. Default is false.
                    WriteProperty(key, false, false);
                    break;
                case "morphNormals":
                    // morphNormals — Define whether the material uses morphNormals. Default is false. 
                    WriteProperty(key, false, false);
                    break;
                default:
                    Debug.Assert(false, "unknown attribute : " + key);
                    break;
            }
        }
    }
}
