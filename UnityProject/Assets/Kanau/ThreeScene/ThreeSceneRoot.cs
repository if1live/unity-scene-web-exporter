using Assets.Kanau.ThreeScene.Geometries;
using Assets.Kanau.ThreeScene.Materials;
using Assets.Kanau.ThreeScene.Textures;
using Assets.Kanau.Utils;
using LitJson;
using System;
using System.Text;

namespace Assets.Kanau.ThreeScene {
    public class ThreeSceneRoot : IJsonExportable
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
            using (var s = new JsonScopeObjectWriter(writer)) {
                writer.WritePropertyName("metadata");
                metadata.ExportJson(writer);

                writer.WritePropertyName("geometries");
                using (var s1 = new JsonScopeArrayWriter(writer)) {
                    foreach (var geometry in SharedNodeTable.GetEnumerable<AbstractGeometryElem>()) {
                        geometry.ExportJson(writer);
                    }
                }

                writer.WritePropertyName("materials");
                using (var s1 = new JsonScopeArrayWriter(writer)) {
                    foreach (var material in SharedNodeTable.GetEnumerable<MaterialElem>()) {
                        material.ExportJson(writer);
                    }
                }

                if (root != null) {
                    writer.WritePropertyName("object");
                    root.ExportJson(writer); 
                }

                writer.WritePropertyName("images");
                using (var s1 = new JsonScopeArrayWriter(writer)) {
                    foreach (var image in SharedNodeTable.GetEnumerable<ImageElem>()) {
                        image.ExportJson(writer);
                    }
                }

                writer.WritePropertyName("textures");
                using (var s1 = new JsonScopeArrayWriter(writer)) {
                    foreach (var tex in SharedNodeTable.GetEnumerable<TextureElem>()) {
                        tex.ExportJson(writer);
                    }
                }
            }
        }

        public AFrameNode ExportAFrame() {
            return root.ExportAFrame();
        }

        public ThreeSceneRoot() {
            metadata = new MetadataElem();
            sharedNodeTable = CreateSharedNodeTable();
        }
    }
}
