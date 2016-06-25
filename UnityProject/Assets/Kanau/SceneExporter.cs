using Assets.Kanau.ThreeScene;
using Assets.Kanau.ThreeScene.Textures;
using Assets.Kanau.UnityScene;
using Assets.Kanau.UnityScene.SceneGraph;
using Assets.Kanau.Utils;
using LitJson;
using System.Text;
using UnityEngine;

namespace Assets.Kanau
{
    public class SceneExporter {
        ExportPathHelper pathHelper;

        readonly UnitySceneRoot unitySceneRoot;

        public SceneExporter(GameObject[] gos, string targetFilePath) {
            pathHelper = new ExportPathHelper(targetFilePath);

            unitySceneRoot = new UnitySceneRoot();
            foreach (var go in gos) {
                unitySceneRoot.Add(go);
            }
        }

        public void Export() {
            // 유니티 씬을 제대로 읽었는지 확인
            {
                var report = new Report("UnitySceneDump");
                report.UseConsole = false;

                var dumpVisitor = new DumpVisitor(report);
                dumpVisitor.Run(unitySceneRoot);

                report.SaveReportFile("unity-scene.log");
            }

            // 유니티씬을 threejs 씬으로 변환
            ThreeSceneRoot threeSceneRoot = null;
            {
                var report = new Report("ThreeSceneConvert");
                report.UseConsole = false;
                var visitor = new ThreeSceneConvertVisitor(report);
                threeSceneRoot = visitor.Run(unitySceneRoot);
            }

            // threejs 씬을 json으로 뽑기
            { 
                var sb = new StringBuilder();
                var writer = new JsonWriter(sb);
                writer.PrettyPrint = true;
                writer.IndentValue = 2;

                threeSceneRoot.ExportJson(writer);
                var report = new Report("ThreeSceneJson");
                report.UseConsole = false;
                report.Info(sb.ToString());

                // 일반 텍스쳐 export
                foreach(var el in threeSceneRoot.SharedNodeTable.GetEnumerable<ImageElem>()) {
                    el.ExpoortImageFile(pathHelper);
                }


                report.SaveReportFile("demo.json");
            }
        }
    }
}
