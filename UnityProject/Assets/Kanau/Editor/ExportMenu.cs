using Assets.Kanau.Utils;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Kanau
{
    public class ExportMenu : ScriptableWizard
    {
        public bool useConsole = true;
        public bool generateReport = false;
        public ReportLevel level = ReportLevel.All;

        public GameObject[] gameObjects;

        [MenuItem("Kanau/Export")]
        static void Export() {
            ScriptableWizard.DisplayWizard("Export Scene", typeof(ExportMenu), "Export");
        }

        void OnWizardUpdate() {
            if (Selection.gameObjects.Length > 0) {
                gameObjects = Selection.gameObjects;
            } else {
                gameObjects = GetRootGameObjects().ToArray();
            }

            Array.Sort(gameObjects, new GameObjectIndexComparer());
        }

        void OnWizardCreate() {
            string targetFilePath = EditorUtility.SaveFilePanel("Save scene", FileHelper.lastExportPath, "", "json");
            if (targetFilePath == "") {
                // cancel 버튼 처리
                return;
            }

            var pathHelper = new ExportPathHelper(targetFilePath);
            FileHelper.lastExportPath = pathHelper.RootPath;
            Report.rootPath = pathHelper.RootPath;

            var report = Report.Instance();
            report.Level = level;
            report.UseConsole = useConsole;

            //Debug.Log(string.Format("Exporting scene: {0}", EditorApplication.currentScene));
            Debug.Log(string.Format("Platform: {0}", SystemInfo.operatingSystem));
            Debug.Log(string.Format("Unity player: {0}", Application.unityVersion));

            var exporter = new SceneExporter(gameObjects, targetFilePath);
            exporter.Export();
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
