using Assets.Kanau.UnityScene.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Kanau.ThreeScene.Geometries {
    public class BoxBufferGeometryElem : AbstractGeometryElem {
        public override string Type { get { return "BoxBufferGeometry"; } }
        public override void Accept (IVisitor v) { v.Visit(this); }

        public float Width { get; private set; }
        public float Height { get; private set; }
        public float Depth { get; private set; }

        public BoxBufferGeometryElem(MeshContainer c) {
            this.Width = 1;
            this.Height = 1;
            this.Depth = 1;
        }
    }
}
