using Assets.Kanau.AFrameScene.Materials;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Kanau.AFrameScene {
    class MtlExporter {
        public static void ToFile (AFrameMaterial mtl, string name, string filename) {
            var exporter = new MtlExporter();
            using (StreamWriter sw = new StreamWriter(filename)) {
                var txt = exporter.MaterialToString(mtl, name);
                sw.Write(txt);
            }
        }

        public string MaterialToString(AFrameMaterial mtl, string name) {
            var sb = new StringBuilder();
            sb.AppendFormat("newmtl {0}", name).AppendLine();

            var standand = mtl.Shader as StandardAFrameShader;
            var flat = mtl.Shader as FlatAFrameShader;
            if(standand != null) {
                ForStandard(mtl, standand, sb);
            } else if(flat != null) {
                ForFlat(mtl, flat, sb);
            }

            return sb.ToString();
        }

        void ForStandard(AFrameMaterial mtl, StandardAFrameShader shader, StringBuilder sb) {
            WriteColor(shader.Color, sb);
            WriteTexture(shader.Src, sb);
        }

        void ForFlat(AFrameMaterial mtl, FlatAFrameShader shader, StringBuilder sb) {
            WriteColor(shader.Color, sb);
            WriteTexture(shader.Src, sb);
        }

        void WriteColor(Color c, StringBuilder sb) {
            if (c != Color.white) {
                sb.AppendFormat("Kd {0} {1} {2}", c.r, c.g, c.b).AppendLine();
            }
        }
        void WriteTexture(string filename, StringBuilder sb) {
            if (filename != "") {
                var path = "../" + filename;
                sb.AppendFormat("map_Kd {0}", path).AppendLine();
            }
        }

        public bool IsBlank(AFrameMaterial mtl) {
            var standard = mtl.Shader as StandardAFrameShader;
            var flat = mtl.Shader as FlatAFrameShader;

            var checks = new List<bool>();
            if (standard != null) {
                checks.Add(standard.Color != StandardAFrameShader.DefaultColor);
                checks.Add(standard.Src != "");
            }
            if(flat != null) {
                checks.Add(standard.Color != FlatAFrameShader.DefaultColor);
                checks.Add(standard.Src != "");
            }

            foreach(var x in checks) {
                if(x == true) {
                    return false;
                }
            }
            return true;
        }
    }
}
