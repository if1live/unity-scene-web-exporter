using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Kanau.AFrameScene {
    public class SimpleProperty<T> : IProperty {
        public T Value { get; private set; }
        public SimpleProperty(T val) {
            this.Value = val;
        }
        public string MakeString() {
            return Value.ToString();
        }
    }
}
