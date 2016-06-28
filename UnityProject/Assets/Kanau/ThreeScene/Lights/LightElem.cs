using Assets.Kanau.UnityScene.SceneGraph;
using Assets.Kanau.Utils;
using UnityEngine;

namespace Assets.Kanau.ThreeScene.Lights {
    public abstract class LightElem : Object3DElem {
        // a-frame default values
        // https://aframe.io/docs/master/primitives/a-light.html
        protected const float AFrameAngle = 60;
        protected readonly Color AFrameColor = new UnityEngine.Color(1, 1, 1);
        protected const float AFrameDecay = 1.0f;
        protected const float AFrameDistance = 0.0f;
        protected const float AFrameExponent = 10.0f;
        protected readonly Color AFrameGroundColor = new UnityEngine.Color(1, 1, 1);
        protected const float AFrameIntensity = 1.0f;

        public uint Color
        {
            get { return Three.UnityColorToThreeColorInt(UnityColor); }
        }
        public float Intensity { get; set; }
        public Color UnityColor { get; set; }
        public float Distance { get; set; }
        public float Decay { get; set; }

        public LightElem() {
            Intensity = AFrameIntensity;
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


        protected void WriteColor(JsonScopeObjectWriter scope) {
            scope.WriteKeyValue("color", Color);
        }
        protected void WriteIntensity(JsonScopeObjectWriter scope) {
            scope.WriteKeyValue("intensity", Intensity);
        }
        protected void WriteDistance(JsonScopeObjectWriter scope) {
            scope.WriteKeyValue("distance", Distance);
        }
        protected void WriteDecay(JsonScopeObjectWriter scope) {
            scope.WriteKeyValue("decay", Decay);
        }

        protected void WriteColor(AFrameNode node) {
            if (UnityColor != AFrameColor) {
                node.AddAttribute("color", new SimpleProperty<string>("#" + Three.UnityColorToHexColor(UnityColor)));
            }
        }
        protected void WriteIntensity(AFrameNode node) {
            WriteCommonAttribute(node, "intensity", Intensity, AFrameIntensity);
        }
        protected void WriteDecay(AFrameNode node) {
            WriteCommonAttribute(node, "decay", Decay, AFrameDecay);
        }
        protected void WriteDistance(AFrameNode node) {
            WriteCommonAttribute(node, "distance", Distance, AFrameDistance);
        }
        void WriteCommonAttribute<T>(AFrameNode node, string key, T val, T defaultval) {
            if (!val.Equals(defaultval)) {
                node.AddAttribute(key, new SimpleProperty<T>(val));
            }
        }
    }
}
