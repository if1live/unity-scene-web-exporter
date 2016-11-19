using Assets.Kanau.UnityScene.SceneGraph;
using UnityEngine;

namespace Assets.Kanau.ThreeScene.Lights {
    public abstract class LightElem : Object3DElem {
        public uint Color
        {
            get { return Three.UnityColorToThreeColorInt(UnityColor); }
        }
        public float Intensity { get; set; }
        public Color UnityColor { get; set; }
        public float Distance { get; set; }
        public float Decay { get; set; }

        public LightElem() {
            Intensity = 1.0f;
        }

        public LightElem(LightNode node) {
            // 빛 관련 공통속성
            this.Intensity = node.Intensity;
            this.UnityColor = node.Color;

            // 유니티로는 point light 거리 무한을 표현할수 없어서
            // 일정범위 이상을 무한으로 취급함
            if (node.Distance >= LightNode.InfiniteDistance) {
                this.Distance = 0;
            } else {
                this.Distance = node.Distance;
            }

            // TODO 유니티에는 Decay에 대응되는게 없어서 적당히 설정
            this.Decay = 1;

            // TODO shadow : cast
        }
    }
}
