using Assets.Kanau.Utils;
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Kanau.Editor {
    public class BaseExportMenu : ScriptableWizard {
        protected string lastExportPath;

        [SerializeField]
        public ExportSettings settings;

        protected void ExportCommon(string extension, SceneFormat fmt) {
            string targetFilePath = EditorUtility.SaveFilePanel("Save scene", lastExportPath, "", extension);
            if (targetFilePath == "") {
                // cancel 버튼 처리
                return;
            }

            var pathHelper = ExportPathHelper.Instance;
            pathHelper.UpdateTargetFilePath(targetFilePath);

            lastExportPath = pathHelper.RootPath;

            // 설정값을 전역변수로 연결
            // 그래야 밖에서 조회하기 쉽다
            ExportSettings.Instance = settings;
            ExportSettings.Instance.destination = new DestinationSettings()
            {
                extension = extension,
                format = fmt,
                rootPath = pathHelper.RootPath,
            };
            Report.Instance.Level = settings.log.level;
            Report.Instance.UseConsole = settings.log.useConsole;

            Debug.LogFormat("Platform: {0}", SystemInfo.operatingSystem);
            Debug.LogFormat("Unity player: {0}", Application.unityVersion);

            var gameObjects = GameObjectHelper.GetExportGameObjects();
            var exporter = new SceneExporter(gameObjects);
            exporter.Export(fmt);

            Debug.LogFormat("Export to {0} finish.", fmt);
        }
    }

    public class ExportMenuForThreeJS : BaseExportMenu {
        [MenuItem("Edit/Kanau/Export Three.js")]
        static void ExportThreeJS() {
            ScriptableWizard.DisplayWizard("Export to Three.js", typeof(ExportMenuForThreeJS), "Export");
        }

        void OnWizardUpdate() {
        }

        void OnWizardCreate() {
            var extension = "json";
            var format = SceneFormat.ThreeJS;
            ExportCommon(extension, format);
        }


    }

    public class ExporterMenuForAFrame : BaseExportMenu {
        [MenuItem("Edit/Kanau/Export AFrame")]
        static void Export() {
            ScriptableWizard.DisplayWizard("Export to AFrame", typeof(ExporterMenuForAFrame), "Export");
        }

        void OnWizardUpdate() {
        }

        void OnWizardCreate() {
            var extension = "html";
            var format = SceneFormat.AFrame;
            ExportCommon(extension, format);
        }
    }

    public class SampleSceneExporter : ScriptableWizard {
        [Serializable]
        public class ExportTask {
            public SceneFormat format;
            public string scene;
            public string outputdir;

            public string OutputFileName
            {
                get
                {
                    switch (format) {
                        case SceneFormat.AFrame:
                            return "index.html";
                        case SceneFormat.ThreeJS:
                            return "scene.json";
                        default:
                            Debug.Assert(false, "unknown scene format");
                            return "";
                    }
                }
            }

            public string Extension
            {
                get
                {
                    switch (format) {
                        case SceneFormat.AFrame:
                            return "html";
                        case SceneFormat.ThreeJS:
                            return "json";
                        default:
                            Debug.Assert(false, "unknown scene format");
                            return "";
                    }
                }
            }
        }

        [SerializeField]
        public ExportSettings settings;

        public ExportTask[] tasks = new ExportTask[]
        {
            // a-frame
            new ExportTask() { format = SceneFormat.AFrame, outputdir = "sample-aframe/simple-scene", scene="Assets/Scenes/DemoSimpleScene.unity" },
            // threejs 
            new ExportTask() { format = SceneFormat.ThreeJS, outputdir = "sample-threejs/5minlab",  scene = "Assets/Scenes/Demo5minlab.unity" },
            new ExportTask() { format = SceneFormat.ThreeJS, outputdir = "sample-threejs/lightmap", scene = "Assets/Scenes/DemoLightmap.unity" },
            new ExportTask() { format = SceneFormat.ThreeJS, outputdir = "sample-threejs/materials", scene = "Assets/Scenes/DemoMaterials.unity" },
            new ExportTask() { format = SceneFormat.ThreeJS, outputdir = "sample-threejs/models", scene = "Assets/Scenes/DemoModels.unity" },
            new ExportTask() { format = SceneFormat.ThreeJS, outputdir = "sample-threejs/script-variables", scene = "Assets/Scenes/DemoScriptVariables.unity" },
            new ExportTask() {format = SceneFormat.ThreeJS, outputdir = "sample-threejs/simple-scene", scene = "Assets/Scenes/DemoSimpleScene.unity" },
        };

        [MenuItem("Edit/Kanau/Export Sample Scene")]
        static void Export() {
            ScriptableWizard.DisplayWizard("Export Sample Scene", typeof(SampleSceneExporter), "Export");
        }

        void OnWizardUpdate() {
        }

        string GetProjectPath() {
            // http://answers.unity3d.com/questions/245993/getting-the-absolute-path-of-current-project.html
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            //var resourcePath = Path.GetFullPath(new Uri(Path.Combine(Path.GetDirectoryName(path), "../../Assets/Resources")).AbsolutePath);
            //var assetPath = Path.GetFullPath(new Uri(Path.Combine(Path.GetDirectoryName(path), "../../Assets")).AbsolutePath);
            var rootPath = Path.GetFullPath(new Uri(Path.Combine(Path.GetDirectoryName(path), "../../")).AbsolutePath);
            return rootPath;
        }

        void OnWizardCreate() {
            var projpath = GetProjectPath();
            var repopath = Path.GetFullPath(new Uri(Path.Combine(projpath, "..")).AbsolutePath);
            var rootpath = Path.Combine(repopath, "SimpleViewer");

            foreach (var t in tasks) {
                EditorSceneManager.OpenScene(t.scene);

                var outputdir = Path.Combine(rootpath, t.outputdir);
                var filepath = Path.Combine(outputdir, t.OutputFileName);

                // remove old directory
                try {
                    Directory.Delete(outputdir, true);
                } catch (DirectoryNotFoundException) {
                }

                // create new directory;
                Directory.CreateDirectory(outputdir);

                var pathHelper = ExportPathHelper.Instance;
                pathHelper.UpdateTargetFilePath(filepath);

                ExportSettings.Instance = settings;
                ExportSettings.Instance.destination = new DestinationSettings()
                {
                    extension = t.Extension,
                    format = t.format,
                    rootPath = rootpath,
                };

                var gameObjects = GameObjectHelper.GetExportGameObjects();
                var exporter = new SceneExporter(gameObjects);
                exporter.Export(t.format);
            }

            Debug.Log("Exporting Sample Scene completed!");
        }
    }
}
