using System;

namespace Assets.Kanau.ThreeScene {
    public class MetadataElem : IThreeElem
    {
        public float Version { get { return 1f; } }
        public string Type { get { return "Object"; } }
        public string Generator { get { return "Kanau"; } }

        public string Uuid { get { throw new NotImplementedException(); } }
        public void Accept(IVisitor v) { v.Visit(this); }
    }
}
