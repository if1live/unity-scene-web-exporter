using Assets.Kanau.Utils;
using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Kanau {
    [Serializable]
    public class ExportSettings {
        public static ExportSettings Instance;

        [Header("A-Frame")]
        public AFrameSettings aframe;

        public SkySettings sky;
        public CameraSettings camera;
        public CursorSettings cursor;

        // 파일 저장 좌표, 포맷등등
        public DestinationSettings destination;

        public LogSettings log;
    }

    public class DestinationSettings {
        public string extension;
        public SceneFormat format;

        public string rootPath;
        public string imageDirectory = "images";
        public string modelDirectory = "models";
    }

    [Serializable]
    public class LogSettings {
        public ReportLevel level = ReportLevel.All;
        public bool useConsole = false;
    }

    [Serializable]
    public class AFrameSettings {
        public string title = "Hello world!";

        // a-frame 최신버전은 언제 구현이 바뀔지 모른다. 그래서 0.2.0로 고정시킴
        // https://aframe.io/releases/latest/aframe.min.js
        public string libraryAddress = "https://aframe.io/releases/0.2.0/aframe.min.js";
        public bool enablePerformanceStatistics = false;

        public string exporterPath = "Assets/Kanau";

        public string templateHeadFilename = "template_head.txt";
        public string templateAppendFilename = "template_append.txt";
        public string templateEndFilename = "template_end.txt";

        public string TemplateHead { get { return GetTemplateContent(templateHeadFilename); } }
        public string TemplateAppend { get { return GetTemplateContent(templateAppendFilename); } }
        public string TemplateEnd { get { return GetTemplateContent(templateEndFilename); } }

        string GetTemplateContent(string templateFile) {
#if UNITY_EDITOR
            TextAsset content = AssetDatabase.LoadAssetAtPath<TextAsset>(exporterPath + "/" + templateFile);
            return content.text;
#else
            return "";
#endif
        }
    }

    [Serializable]
    public class CursorSettings {
        public bool enabled = true;

        public bool fuse = false;
        public float maxDistance = 1000f;
        public float timeout = 1500f;
    }

    [Serializable]
    public class CameraSettings {
        public bool wasdControlsEnabled = true;
        public bool lookControlsEnabled = true;
    }

    [Serializable]
    public class SkySettings {
        public bool enableSky = false;
        public bool skyColorFromMainCameraBackground = true;
        public Color skyColor = Color.white;
        public Texture skyTexture;
    }
}
