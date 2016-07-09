using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Kanau.Utils {
    public class GameObjectHelper {

#if UNITY_EDITOR
        public static GameObject[] GetExportGameObjects() {
            GameObject[] gameObjects = null;
            if (Selection.gameObjects.Length > 0) {
                gameObjects = Selection.gameObjects;
            } else {
                gameObjects = GetRootGameObjects().ToArray();
            }

            Array.Sort(gameObjects, new GameObjectIndexComparer());
            return gameObjects;
        }
#endif

        public static List<GameObject> GetRootGameObjects() {
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
