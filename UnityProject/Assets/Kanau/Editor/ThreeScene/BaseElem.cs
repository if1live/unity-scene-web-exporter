using System;

namespace Assets.Kanau.ThreeScene {
    public abstract class BaseElem : IThreeElem
    {
        protected Guid guid = Guid.NewGuid();

        public string _uuid = null;
        public string Uuid
        {
            get
            {
                if(_uuid == null) {
                    _uuid = guid.ToString().ToUpper();
                }
                return _uuid;
            }
            set { _uuid = value; }
        }

        public virtual string Name { get; set; }

        public abstract string Type { get; }

        public abstract void Accept(IVisitor v);
    }
}
