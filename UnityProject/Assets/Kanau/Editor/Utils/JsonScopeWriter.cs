using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Kanau.Utils
{
    public abstract class AbstractJsonScopeWriter
    {
        protected JsonWriter writer;
        public AbstractJsonScopeWriter(JsonWriter writer)
        {
            this.writer = writer;
        }

        public void Write(float[] vals)
        {
            using (var scope = new JsonScopeArrayWriter(writer))
            {
                foreach (var x in vals)
                {
                    writer.Write(x);
                }
            }
        }
        public void Write(string[] vals)
        {
            using (var scope = new JsonScopeArrayWriter(writer))
            {
                foreach (var x in vals)
                {
                    writer.Write(x);
                }
            }
        }
        public void Write(int[] vals)
        {
            using (var scope = new JsonScopeArrayWriter(writer))
            {
                foreach (var x in vals)
                {
                    writer.Write(x);
                }
            }
        }

        public void Write(Color color)
        {
            using (var scope = new JsonScopeObjectWriter(writer))
            {
                scope.WriteKeyValue("r", color.r);
                scope.WriteKeyValue("g", color.g);
                scope.WriteKeyValue("b", color.b);
                scope.WriteKeyValue("a", color.a);
            }
        }
        public void Write(Bounds bounds)
        {
            using (var scope = new JsonScopeObjectWriter(writer))
            {
                scope.WriteKeyValue("min", bounds.min);
                scope.WriteKeyValue("max", bounds.max);
            }
        }
    }

    public class JsonScopeArrayWriter : AbstractJsonScopeWriter, IDisposable
    {
        public JsonScopeArrayWriter(JsonWriter writer)
            : base(writer)
        {
            writer.WriteArrayStart();
        }

        public void Dispose()
        {
            writer.WriteArrayEnd();
        }
    }

    public class JsonScopeObjectWriter : AbstractJsonScopeWriter, IDisposable
    {
        public JsonScopeObjectWriter(JsonWriter writer)
            : base(writer)
        {
            writer.WriteObjectStart();
        }

        public void Dispose()
        {
            writer.WriteObjectEnd();
        }

        #region WriteKeyValue - Single Value
        public void WriteKeyValue(string key, bool val)
        {
            writer.WritePropertyName(key);
            writer.Write(val);
        }

        public void WriteKeyValue(string key, string val)
        {
            writer.WritePropertyName(key);
            writer.Write(val);
        }
        public void WriteKeyValue(string key, int val)
        {
            writer.WritePropertyName(key);
            writer.Write(val);
        }
        public void WriteKeyValue(string key, uint val) {
            writer.WritePropertyName(key);
            writer.Write(val);
        }

        public void WriteKeyValue(string key, float val)
        {
            writer.WritePropertyName(key);
            writer.Write(val);
        }
        public void WriteKeyValue(string key, double val) {
            writer.WritePropertyName(key);
            writer.Write(val);
        }
        public void WriteKeyValue(string key, Color color)
        {
            writer.WritePropertyName(key);
            Write(color);
        }
        public void WriteKeyValue(string key, Vector3 val)
        {
            writer.WritePropertyName(key);
            float[] vals = new float[] { val.x, val.y, val.z };
            Write(vals);
        }
        public void WriteKeyValue(string key, Vector2 val)
        {
            writer.WritePropertyName(key);
            float[] vals = new float[] { val.x, val.y };
            Write(vals);
        }
        public void WriteKeyValue(string key, Bounds bounds)
        {
            writer.WritePropertyName(key);
            Write(bounds);
        }
        public void WriteKeyValue(string key, IEnumerable<float> e)
        {
            writer.WritePropertyName(key);
            using (var scope = new JsonScopeArrayWriter(writer))
            {
                foreach(float v in e)
                {
                    writer.Write(v);
                }
            }
        }
        #endregion

        #region WriteKeyValue - Array
        public void WriteKeyValue(string key, float[] vals)
        {
            writer.WritePropertyName(key);
            Write(vals);
        }
        public void WriteKeyValue(string key, string[] vals)
        {
            writer.WritePropertyName(key);
            Write(vals);
        }
        public void WriteKeyValue(string key, int[] vals)
        {
            writer.WritePropertyName(key);
            Write(vals);
        }
        #endregion
    }
}
