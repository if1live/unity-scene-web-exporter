using Assets.Kanau.ThreeScene;
using Assets.Kanau.ThreeScene.Textures;
using Assets.Kanau.UnityScene;
using Assets.Kanau.UnityScene.SceneGraph;
using Assets.Kanau.Utils;
using LitJson;
using System.Text;
using UnityEngine;

namespace Assets.Kanau {
    public class SceneExporter {
        readonly UnitySceneRoot unitySceneRoot;

        public SceneExporter(GameObject[] gos) {
            unitySceneRoot = BuildUnitySceneRoot(gos);
        }

        public void Export(SceneFormat fmt) {
            // 유니티 씬을 제대로 읽었는지 확인
            bool useDump = false;
            if(useDump) {
                DumpUnitySceneRoot(unitySceneRoot);
            }

            // 유니티씬을 threejs 씬으로 변환
            ThreeSceneRoot threeSceneRoot = BuildThreeSceneRoot(unitySceneRoot);

            WriteTextureFiles(threeSceneRoot, fmt);
            WriteModelFiles(threeSceneRoot, fmt);

            
            switch(fmt) {
                case SceneFormat.AFrame:
                    WriteToAFrame(threeSceneRoot);
                    break;
                case SceneFormat.ThreeJS:
                    WriteToThreeJS(threeSceneRoot);
                    break;
                default:
                    Debug.Assert(false, "unknown format");
                    break;
            }
        }

        void WriteTextureFiles(ThreeSceneRoot root, SceneFormat fmt) {
            var pathHelper = ExportPathHelper.Instance;
            // 일반 텍스쳐 export
            foreach (var el in root.SharedNodeTable.GetEnumerable<ImageElem>()) {
                el.ExpoortImageFile(pathHelper);
            }
        }

        void WriteModelFiles(ThreeSceneRoot root, SceneFormat fmt) {
            // three.js는 단일 파일로 출력하는게 목적이라서 model 파일을 생성하지 않는다
            if (fmt == SceneFormat.AFrame) {
                // TODO 모델 export
            }
        }

        void WriteExportedTextFile(string text) {
            var pathHelper = ExportPathHelper.Instance;
            FileHelper.SaveContentsAsFile(text, pathHelper.SceneFilePath);
        }

        ThreeSceneRoot BuildThreeSceneRoot(UnitySceneRoot root) {
            var report = new Report("ThreeSceneConvert");
            report.UseConsole = false;
            var visitor = new ThreeSceneConvertVisitor(report);
            return visitor.Run(unitySceneRoot);
        }

        void DumpUnitySceneRoot(UnitySceneRoot root) {
            var report = new Report("unity-scene-dump");
            report.UseConsole = false;

            var dumpVisitor = new DumpVisitor(report);
            dumpVisitor.Run(root);

            report.SaveReport("unity-scene.log");
        }

        UnitySceneRoot BuildUnitySceneRoot(GameObject[] gos) {
            var root = new UnitySceneRoot();
            foreach (var go in gos) {
                root.Add(go);
            }
            return root;
        }

        void WriteToAFrame(ThreeSceneRoot root) {
            var sb = new StringBuilder();
            var node = root.ExportAFrame();
            node.BuildSource(sb);

            var aframe = ExportSettings.Instance.aframe;
            var text = aframe.TemplateHead + sb.ToString() + aframe.TemplateAppend + aframe.TemplateEnd;

            text = text.Replace("&TITLE&", aframe.title);
            text = text.Replace("&LIBRARY&", aframe.libraryAddress);
            if (aframe.enablePerformanceStatistics) {
                text = text.Replace("a-scene", "a-scene stats=\"true\"");
            }

            WriteExportedTextFile(text);
        }

        void WriteToThreeJS(ThreeSceneRoot root) {
            // wrilte to text file
            var sb = new StringBuilder();
            var writer = new JsonWriter(sb);
            writer.PrettyPrint = true;
            writer.IndentValue = 2;

            root.ExportJson(writer);
            var content = sb.ToString();
            WriteExportedTextFile(content);
        }
    }
}
