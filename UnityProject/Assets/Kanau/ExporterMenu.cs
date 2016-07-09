using Assets.Kanau.Utils;
using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Kanau {
    [Serializable]
    public class CursorSettings {
        public bool enabled = true;

        public bool fuse = false;
        public float maxDistance = 1000f;
        public float timeout = 1500f;

        public static CursorSettings Instance;
    }

    [Serializable]
    public class CameraSettings {
        public bool wasdControlsEnabled = true;
        public bool lookControlsEnabled = true;

        public static CameraSettings Instance;
    }

    [Serializable]
    public class SkySettings {
        public bool enableSky = false;
        public bool skyColorFromMainCameraBackground = true;
        public Color skyColor = Color.white;
        public Texture skyTexture;

        public static SkySettings Instance;
    }

#if UNITY_EDITOR
    public class ExporterMenu : ScriptableObject {
        // Header에 필요한 정보가 생기면 추가하기
        //[Header("General")]
        //[Header("Three.js")]

        [Header("A-Frame")]
        public string title = "Hello world!";
        public string libraryAddress = "https://aframe.io/releases/latest/aframe.min.js";
        public bool enablePerformanceStatistics = false;
        public SkySettings sky;
        public CameraSettings camera;
        public CursorSettings cursor;

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

        void ExportCommon(string extension, SceneFormat fmt) {
            // 현재 설정값을 전역변수로 연결
            // 그래야 밖에서 조회하기 쉽다
            CameraSettings.Instance = camera;
            CursorSettings.Instance = cursor;
            SkySettings.Instance = sky;

            string targetFilePath = EditorUtility.SaveFilePanel("Save scene", FileHelper.lastExportPath, "", extension);
            if (targetFilePath == "") {
                // cancel 버튼 처리
                return;
            }

            var pathHelper = new ExportPathHelper(targetFilePath);
            FileHelper.lastExportPath = pathHelper.RootPath;
            Report.rootPath = pathHelper.RootPath;

            var report = Report.Instance;
            report.Level = ReportLevel.All;
            report.UseConsole = true;

            Debug.LogFormat("Platform: {0}", SystemInfo.operatingSystem);
            Debug.LogFormat("Unity player: {0}", Application.unityVersion);

            var gameObjects = GameObjectHelper.GetExportGameObjects();
            var exporter = new SceneExporter(gameObjects, targetFilePath);
            exporter.Export(fmt);

            Debug.LogFormat("Export to {0} finish.", fmt);
        }  
    }
#endif
}
