using Assets.Kanau.AFrameScene;
using System;

namespace Assets.Kanau.ThreeScene {
    public class MetadataElem : IThreeElem, IAFrameExportable
    {
        public float Version { get { return 1f; } }
        public string Type { get { return "Object"; } }
        public string Generator { get { return "Kanau"; } }

        public AFrameNode ExportAFrame() {
            throw new NotImplementedException("A-Frame은 메타데이터가 따로 없다");
        }

        public string Uuid { get { throw new NotImplementedException(); } }
        public void Accept(IVisitor v) { v.Visit(this); }
    }
}
