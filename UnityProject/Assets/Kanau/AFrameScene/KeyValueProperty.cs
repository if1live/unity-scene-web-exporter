using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Kanau.AFrameScene {
    public class KeyValueProperty : IProperty {
        struct KeyValueTuple {
            public string key;
            public string value;
        }

        List<KeyValueTuple> data = new List<KeyValueTuple>();

        public void Add<T>(string key, T value) {
            var tuple = new KeyValueTuple()
            {
                key = key,
                value = value.ToString(),
            };
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
}
