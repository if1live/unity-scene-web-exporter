using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Assets.Kanau.UnityScene
{
    public interface INodeTable<KeyType>
    {
        Type ValueType { get; }
        bool Contains<T>(KeyType key);
        T Get<T>(KeyType key) where T : class;
        void Add<T>(KeyType key, T value);
        IEnumerable<T> GetEnumerable<T>();
    }

    class SingleTypeNodeTable<KeyType, T> : INodeTable<KeyType> where T : class
    {
        // 추가한 순서를 보장하지 않으면 transform 객체가 섞여서 순서를 믿을수 없다
        Dictionary<KeyType, T> table = new Dictionary<KeyType, T>();
        List<T> list = new List<T>();

        public Type ValueType { get { return typeof(T); } }

        public void Add<T1>(KeyType key, T1 node){
            Debug.Assert(typeof(T1) == typeof(T));
            T elem = node as T;
            table[key] = elem;
            list.Add(elem);
        }

        public bool Contains<T1>(KeyType key) {
            Debug.Assert(typeof(T1) == typeof(T));
            return table.ContainsKey(key);
        }

        public T1 Get<T1>(KeyType key) where T1 : class {
            Debug.Assert(typeof(T1) == typeof(T));
            return table[key] as T1;
        }

        public IEnumerable<T1> GetEnumerable<T1>() {
            Debug.Assert(typeof(T1) == typeof(T));
            return list as IEnumerable<T1>;
        }
    }

    public class NodeTable<KeyType> : INodeTable<KeyType>
    {
        readonly Dictionary<Type, INodeTable<KeyType>> tables = new Dictionary<Type, INodeTable<KeyType>>();

        public Type ValueType {
            get {
                throw new InvalidOperationException("do not use");
            }
        }

        public void Register(INodeTable<KeyType> table) {
            Debug.Assert(tables.ContainsKey(table.ValueType) == false, "중복 추가 방지");
            tables[table.ValueType] = table;
        }

        public T Get<T>(KeyType key) where T : class {
            var table = tables[typeof(T)];
            return table.Get<T>(key);
        }

        public void Add<T>(KeyType key, T node) {
            var table = tables[typeof(T)];
            table.Add(key, node);
        }

        public bool Contains<T>(KeyType key) {
            var table = tables[typeof(T)];
            return table.Contains<T>(key);
        }

        public IEnumerable<T> GetEnumerable<T>() {
            var table = tables[typeof(T)];
            return table.GetEnumerable<T>();
        }
    }
}
