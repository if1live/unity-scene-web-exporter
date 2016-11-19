using System;
using UnityEngine;

namespace Assets.Kanau.AFrameScene {
    public class Vector2Property : IProperty {
        public Vector2 Value { get; private set; }

        bool useDefaultValue;
        Vector2 defaultValue;

        public Vector2Property (Vector2 value, Vector2 defaultval) {
            this.Value = value;
            this.defaultValue = defaultval;
            this.useDefaultValue = true;
        }
        public Vector2Property (Vector2 value) {
            this.Value = value;
            this.useDefaultValue = false;
        }

        public string MakeString () {
            if (useDefaultValue && Value == defaultValue) {
                return "";
            }

            var tokens = new string[]
            {
                Value.x.ToString(),
                Value.y.ToString(),
            };
            return string.Join(" ", tokens);
        }

        public string GetValue (string key) {
            return MakeString();
        }
    }
}
