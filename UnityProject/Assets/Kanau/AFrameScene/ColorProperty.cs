using System;
using UnityEngine;

namespace Assets.Kanau.AFrameScene {
    public class ColorProperty : IProperty {
        public Color Value { get; private set; }

        bool useDefaultValue;
        Color defaultValue;

        public ColorProperty (Color value, Color defaultval) {
            this.Value = value;
            this.defaultValue = defaultval;
            this.useDefaultValue = true;
        }
        public ColorProperty (Color value) {
            this.Value = value;
            this.useDefaultValue = false;
        }

        public string MakeString () {
            if (useDefaultValue && Value == defaultValue) {
                return "";
            }

            return "#" + Three.UnityColorToHexColor(Value);
        }
    }
}
