using Assets.Kanau.Utils;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
            var format = SceneExporter.SceneFormat.ThreeJS;
            ExportCommon(extension, format);
        }

        public void ExportAFrame() {
            var extension = "html";
            var format = SceneExporter.SceneFormat.AFrame;
            ExportCommon(extension, format);
        }

        void ExportCommon(string extension, SceneExporter.SceneFormat fmt) {
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

            var report = Report.Instance();
            report.Level = ReportLevel.All;
            report.UseConsole = true;

            Debug.Log(string.Format("Platform: {0}", SystemInfo.operatingSystem));
            Debug.Log(string.Format("Unity player: {0}", Application.unityVersion));

            var gameObjects = GetExportGameObjects();
            var exporter = new SceneExporter(gameObjects, targetFilePath);
            exporter.Export(fmt);
        }
        

        GameObject[] GetExportGameObjects() {
            GameObject[] gameObjects = null;
            if (Selection.gameObjects.Length > 0) {
                gameObjects = Selection.gameObjects;
            } else {
                gameObjects = GetRootGameObjects().ToArray();
            }

            Array.Sort(gameObjects, new GameObjectIndexComparer());
            return gameObjects;
        }

        List<GameObject> GetRootGameObjects() {
            List<GameObject> rootObjects = new List<GameObject>();
            foreach (Transform xform in UnityEngine.Object.FindObjectsOfType<Transform>()) {
                if (xform.parent == null) {
                    rootObjects.Add(xform.gameObject);
                }
            }
            return rootObjects;
        }
    }
}
