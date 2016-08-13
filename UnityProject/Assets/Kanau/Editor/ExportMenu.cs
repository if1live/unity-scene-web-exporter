using Assets.Kanau.Utils;
using UnityEditor;
using UnityEngine;

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
}
