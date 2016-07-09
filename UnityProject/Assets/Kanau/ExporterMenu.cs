using Assets.Kanau.Utils;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Kanau {
#if UNITY_EDITOR
    public class ExporterMenu : ScriptableObject {
        // Header에 필요한 정보가 생기면 추가하기
        //[Header("General")]
        //[Header("Three.js")]
        public ExportSettings settings;
        

        public void ExportThree() {
            var extension = "json";
            var format = SceneFormat.ThreeJS;
            ExportCommon(extension, format);
        }

        public void ExportAFrame() {
            var extension = "html";
            var format = SceneFormat.AFrame;
            ExportCommon(extension, format);
        }

        string lastExportPath;

        void ExportCommon(string extension, SceneFormat fmt) {
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
#endif
}
