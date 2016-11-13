using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Assets.Kanau.AFrameScene {
    public class SimpleProperty<T> : IProperty {
        public T Value { get; private set; }
        T defaultValue;
        bool useDefaultValue;

        public SimpleProperty(T val) {
            Debug.Assert(val != null);
            this.Value = val;
            this.useDefaultValue = false;
            this.defaultValue = default(T);
        }
        public SimpleProperty (T val, T defaultValue) {
            Debug.Assert(val != null);
            this.Value = val;
            this.defaultValue = defaultValue;
            this.useDefaultValue = true;
        }
        public string MakeString() {
            if(useDefaultValue) {
                if(Value.Equals(defaultValue)) {
                    return "";
                }
                return Value.ToString();

            } else {
                return Value.ToString();
            }
        }
    }
}
