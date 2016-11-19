using System.Collections.Generic;
using UnityEngine;

namespace Assets.Kanau.UnityScene
{
    // 프로젝트 속성은 씬 그래프와 분리
    public class ProjectSettings
    {
        public Color BackgroundColor {
            get {
                if (Camera.main != null) {
                    return Camera.main.backgroundColor;
                } else {
                    return Color.black;
                }
            }
        }

        public Color AmbientColor {
            get { return RenderSettings.ambientLight; }
        }

        public string[] Layers {
            get {
                List<string> layerNames = new List<string>();
                //user defined layers start with layer 8 and unity supports 31 layers
                for (int i = 0; i <= 31; i++) {
                    layerNames.Add(LayerMask.LayerToName(i));
                }
                return layerNames.ToArray();
            }
        }
    }
}
