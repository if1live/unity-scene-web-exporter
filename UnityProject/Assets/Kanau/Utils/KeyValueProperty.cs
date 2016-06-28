using System.Collections.Generic;
using UnityEngine;

namespace Assets.Kanau.Utils {
    public interface IProperty {
        string MakeString();
    }

    public class KeyValueProperty : IProperty {
        struct KeyValueTuple {
            public string key;
            public string value;
        }

        List<KeyValueTuple> data = new List<KeyValueTuple>();

        public void Add<T>(string key, T value) {
            var tuple = new KeyValueTuple() { key = key, value = value.ToString() };
            data.Add(tuple);
        }

        public string MakeString() {
            var tokens = new string[data.Count];
            for (int i = 0; i < data.Count; i++) {
                var t = data[i];
                var elem = string.Format("{0}: {1}", t.key, t.value);
                tokens[i] = elem;
            }
            return string.Join("; ", tokens);
        }
    }

    public class SimpleProperty<T> : IProperty {
        public T Value { get; private set; }
        public SimpleProperty(T val) {
            this.Value = val;
        }
        public string MakeString() {
            return Value.ToString();
        }
    }

    public class Vector3Property : IProperty {
        public Vector3 Value { get; private set; }

        bool useDefaultValue;
        Vector3 defaultValue;

        public Vector3Property(Vector3 value, Vector3 defaultval) {
            this.Value = value;
            this.defaultValue = defaultval;
            this.useDefaultValue = true;
        }
        public Vector3Property(Vector3 value) {
            this.Value = value;
            this.useDefaultValue = false;
        }

        public string MakeString() {
            if (useDefaultValue && Value == defaultValue) {
                return "";
            }

            var tokens = new string[]
            {
                Value.x.ToString(),
                Value.y.ToString(),
                Value.z.ToString(),
            };
            return string.Join(" ", tokens);
        }

        public static Vector3Property MakeScale(Vector3 s) {
            return new Vector3Property(s, Vector3.one);
        }
        public static Vector3Property MakePosition(Vector3 p) {
            return new Vector3Property(p, Vector3.zero);
        }

        public static Vector3Property MakeRotation(Quaternion q) {
            var r = q.eulerAngles;
            return new Vector3Property(new Vector3(-r.x, -r.y, r.z), Vector3.zero);
        }
    }
}
