using Assets.Kanau.UnityScene.Containers;
using System;
using UnityEngine;

namespace Assets.Kanau.ThreeScene.Textures {
    public class TextureElem : BaseElem
    {
        public TextureElem(LightmapContainer c) {
            SetFilterMode(c.FilterMode);
            SetWrapMode(c.WrapMode);
            Anisotropy = c.AnisoLevel;
        }

        public TextureElem(TextureContainer c) {
            SetFilterMode(c.FilterMode);
            SetWrapMode(c.WrapMode);
            Anisotropy = c.AnisoLevel;
        }

        void SetWrapMode(TextureWrapMode m) {
            // wrap mode
            switch (m) {
                case TextureWrapMode.Clamp:
                    WrapMode = Three.ClampToEdgeWrapping;
                    break;
                case TextureWrapMode.Repeat:
                    WrapMode = Three.RepeatWrapping;
                    break;
                default:
                    Debug.Assert(false, "do not reach");
                    break;
            }
        }
        void SetFilterMode(FilterMode m) {
            switch (m) {
                case FilterMode.Point:
                    MagFilter = Three.NearestFilter;
                    MinFilter = Three.NearestMipMapNearestFilter;
                    break;
                case FilterMode.Bilinear:
                    MagFilter = Three.LinearFilter;
                    MinFilter = Three.LinearMipMapLinearFilter;
                    break;
                case FilterMode.Trilinear:
                    MagFilter = Three.LinearFilter;
                    MinFilter = Three.LinearMipMapLinearFilter;
                    break;
                default:
                    Debug.Assert(false, "do not reach");
                    break;
            }
        }

        public override void Accept(IVisitor v) { v.Visit(this); }

        public override string Type {
            get {
                throw new NotImplementedException("not need");
            }
        }

        // TODO
        public float[] Offset {
            get { return new float[] { 0, 0 }; }
        }
        // TODO
        public float[] Repeat {
            get { return new float[] { 1, 1 }; }
        }

        public int MagFilter { get; set; }
        public int MinFilter { get; set; }

        // 유니티에서는 wrap mode를 s, t축 따로 지정하는게 불가능하다
        // 그래서 하나만 받도록해도 문제 없을듯
        public int WrapMode { get; set; }
        public int[] Wrap {
            get {
                return new int[] { WrapMode, WrapMode };
            }
        }

        public string ImageUuid {
            get {
                if (Image == null) {
                    return "";
                }
                return Image.Uuid;
            }
        }

        public string ImageName {
            get {
                if (Image == null) {
                    return "";
                }
                return Image.Name;
            }
        }
        public string ImagePath
        {
            get
            {
                var name = ImageName;
                if(name == "") { return name; }
                return string.Format("./{0}/{1}", ExportSettings.Instance.destination.imageDirectory, name);
            }
        }

        public int Anisotropy { get; set; }

        public ImageElem Image { get; set; }
    }
}
