using UnityEditor;
using UnityEngine;

namespace Assets.Kanau.Editor {
    [CustomEditor(typeof(ExporterMenu))]
    public class ExporterMenuEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            ExporterMenu script = (ExporterMenu)target;

            if(GUILayout.Button("Export Three.js")) {
                script.ExportThree();
            }
            if(GUILayout.Button("Expurt A-Frame")) {
                script.ExportAFrame();
            }

            DrawDefaultInspector();
        }
    }
}
