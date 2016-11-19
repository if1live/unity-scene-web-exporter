using Assets.Kanau.UnityScene.SceneGraph;

namespace Assets.Kanau.ThreeScene.Cameras {
    public class PerspectiveCameraElem : CameraElem
    {
        public override string Type { get { return "PerspectiveCamera"; } }
        public override void Accept(IVisitor v) { v.Visit(this); }

        public float FOV { get; set; }
        public float Near { get; set; }
        public float Far { get; set; }

        public PerspectiveCameraElem(CameraNode cam) {
            //this.Name = cam.Value.name;
            this.Name = string.Format("{0}_{1}", cam.Value.name, Type);

            this.FOV = cam.FieldOfView;
            this.Near = cam.Near;
            this.Far = cam.Far;
            // shadow : cast + receive
        }
    }
}
