using Assets.Kanau.AFrameScene;
using Assets.Kanau.ThreeScene.Geometries;
using Assets.Kanau.ThreeScene.Materials;
using Assets.Kanau.ThreeScene.Textures;
using Assets.Kanau.Utils;
using LitJson;

namespace Assets.Kanau.ThreeScene {
    public class ThreeSceneRoot
    {
        readonly IThreeNodeTable sharedNodeTable;
        readonly MetadataElem metadata;
        Object3DElem root;

        public MetadataElem Metadata { get { return metadata; } }
        public IThreeNodeTable SharedNodeTable { get { return sharedNodeTable; } }
        public Object3DElem Root {
            get { return root; }
            set { root = value; }
        }

        ThreeNodeTable CreateSharedNodeTable() {
            var table = new ThreeNodeTable();
            table.Register(new SingleTypeThreeNodeTable<AbstractGeometryElem>());
            table.Register(new SingleTypeThreeNodeTable<TextureElem>());
            table.Register(new SingleTypeThreeNodeTable<ImageElem>());
            table.Register(new SingleTypeThreeNodeTable<MaterialElem>());
            return table;
        }

        public void ExportJson(JsonWriter writer) {
            var visitor = new ThreeSceneExportVisitor(writer);

            using (var s = new JsonScopeObjectWriter(writer)) {
                writer.WritePropertyName("metadata");
                metadata.Accept(visitor);

                writer.WritePropertyName("geometries");
                using (var s1 = new JsonScopeArrayWriter(writer)) {
                    foreach (var geometry in SharedNodeTable.GetEnumerable<AbstractGeometryElem>()) {
                        geometry.Accept(visitor);
                    }
                }

                writer.WritePropertyName("materials");
                using (var s1 = new JsonScopeArrayWriter(writer)) {
                    foreach (var material in SharedNodeTable.GetEnumerable<MaterialElem>()) {
                        material.Accept(visitor);
                    }
                }

                if (root != null) {
                    writer.WritePropertyName("object");
                    root.Accept(visitor); 
                }

                writer.WritePropertyName("images");
                using (var s1 = new JsonScopeArrayWriter(writer)) {
                    foreach (var image in SharedNodeTable.GetEnumerable<ImageElem>()) {
                        image.Accept(visitor);
                    }
                }

                writer.WritePropertyName("textures");
                using (var s1 = new JsonScopeArrayWriter(writer)) {
                    foreach (var tex in SharedNodeTable.GetEnumerable<TextureElem>()) {
                        tex.Accept(visitor);
                    }
                }
            }
        }

        public AFrameNode ExportAFrame() {
            var visitor = new AFrameExportVisitor();
            root.Accept(visitor);
            var rootNode = visitor.Node;

            var assetsNode = new AFrameNode("a-assets");
            // export mesh
            var pathHelper = ExportPathHelper.Instance;
            foreach (var el in SharedNodeTable.GetEnumerable<AbstractGeometryElem>()) {
                // TODO 타입에 따라서 obj 굽는게 바뀔텐데
                var bufferGeom = el as BufferGeometryElem;
                if (bufferGeom == null) { continue; }
                var mesh = bufferGeom.Mesh;

                var node = new AFrameNode("a-asset-item");
                node.AddAttribute("id", mesh.name + "-obj");

                string filepath = "./models/" + mesh.name + ".obj";
                node.AddAttribute("src", filepath);

                assetsNode.AddChild(node);
            }
            rootNode.AddChild(assetsNode);

            return rootNode;
        }

        public ThreeSceneRoot() {
            metadata = new MetadataElem();
            sharedNodeTable = CreateSharedNodeTable();
        }
    }
}
