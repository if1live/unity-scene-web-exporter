using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Assets.Kanau.ThreeScene {
    public interface IThreeNodeTable
    {
        Type ValueType { get; }

        void Add<T>(T node, string instanceId) where T : class, IThreeElem;

        T Get<T>(string instanceId) where T : class, IThreeElem;
        IEnumerable<T> GetEnumerable<T>();
    }

    class SingleTypeThreeNodeTable<T> : IThreeNodeTable
        where T : IThreeElem
    {
        List<T> list = new List<T>();
        Dictionary<string, T> uuid_table = new Dictionary<string, T>();
        Dictionary<string, T> instanceid_table = new Dictionary<string, T>();

        public const string InvalidInstanceId = "";

        public Type ValueType { get { return typeof(T); } }

        public void Add<T1>(T1 node) where T1 : class, IThreeElem {
            Add(node, InvalidInstanceId);
        }
        public void Add<T1>(T1 node, string instanceId) where T1 : class, IThreeElem {
            var n = (T)Convert.ChangeType(node, node.GetType());

            uuid_table[node.Uuid] = n;
            list.Add(n);

            if (instanceId != InvalidInstanceId) {
                instanceid_table[instanceId] = n;
            }
        }

        public T1 Get<T1>(string instanceId) where T1 : class, IThreeElem {
            var obj = instanceid_table[instanceId];
            var val = (T1)Convert.ChangeType(obj, obj.GetType());
            return val;
        }

        public IEnumerable<T1> GetEnumerable<T1>() {
            return list as IEnumerable<T1>;
        }
    }

    public class ThreeNodeTable : IThreeNodeTable
    {
        Dictionary<Type, IThreeNodeTable> tables = new Dictionary<Type, IThreeNodeTable>();

        public Type ValueType {
            get {
                throw new InvalidOperationException("do not use");
            }
        }

        public void Register(IThreeNodeTable t) {
            Debug.Assert(tables.ContainsKey(t.ValueType) == false, "중복 추가 방지");
            tables[t.ValueType] = t;
        }

        public void Add<T>(T node, string instanceId) where T : class, IThreeElem {
            var table = tables[typeof(T)];
            table.Add(node, instanceId);
        }

        public T Get<T>(string instanceId) where T :class, IThreeElem {
            var table = tables[typeof(T)];
            return table.Get<T>(instanceId);
        }
        public IEnumerable<T> GetEnumerable<T>() {
            var table = tables[typeof(T)];
            return table.GetEnumerable<T>();
        }
    }
}
