using Assets.Kanau.UnityScene.SceneGraph;
using Assets.Kanau.Utils;
using LitJson;

namespace Assets.Kanau.ThreeScene.Cameras {
    public class PerspectiveCameraElem : CameraElem
    {
        public override string Type { get { return "PerspectiveCamera"; } }
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

        public override void ExportJson(JsonWriter writer) {
            using (var scope = new JsonScopeObjectWriter(writer)) {
                WriteCommonObjectNode(writer, scope);

                scope.WriteKeyValue("fov", FOV);
                scope.WriteKeyValue("zoom", 1);
                scope.WriteKeyValue("near", Near);
                scope.WriteKeyValue("far", Far);
                scope.WriteKeyValue("focus", 10);
                scope.WriteKeyValue("aspect", 1);
                scope.WriteKeyValue("filmGauge", 35);
                scope.WriteKeyValue("filmOffset", 0);
            }
        }

        public override AFrameNode ExportAFrame() {
            var container = new AFrameNode("a-entity");
            WriteCommonAFrameNode(container);

            var cam = new AFrameNode("a-camera");
            cam.AddAttribute("far", new SimpleProperty<float>(Far));
            cam.AddAttribute("near", new SimpleProperty<float>(Near));

            var camsettings = CameraSettings.Instance;
            cam.AddAttribute("look-controls-enabled", camsettings.lookControlsEnabled.ToString());
            cam.AddAttribute("wasd-controls-enabled", camsettings.wasdControlsEnabled.ToString());

            container.AddChild(cam);

            var cursorsettings = CursorSettings.Instance;
            if (cursorsettings.enabled) {
                var cursor = CreateCursorNode(cursorsettings);
                cam.AddChild(cursor);
            }
            return container;
        }

        AFrameNode CreateCursorNode(CursorSettings settings) {
            var cursor = new AFrameNode("a-cursor");
            cursor.AddAttribute("fuse", settings.fuse.ToString());
            cursor.AddAttribute("max-distance", settings.maxDistance.ToString());
            cursor.AddAttribute("timeout", settings.timeout.ToString());
            return cursor;
        }
    }
}
