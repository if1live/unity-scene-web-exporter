using System.Collections.Generic;
using System.Text;

namespace Assets.Kanau.AFrameScene {
    public class AFrameNode {
        List<AFrameNode> children = new List<AFrameNode>();
        public string NodeType { get; private set; }
        List<Attribute> attributes = new List<Attribute>();

        class Attribute {
            public string name;
            public IProperty property;
        }

        public AFrameNode(string nodetype) {
            this.NodeType = nodetype;
        }

        public void AddChild(AFrameNode node) {
            children.Add(node);
        }


        public void AddAttribute(string name, IProperty property) {
            var attr = new Attribute() { name = name, property = property };
            attributes.Add(attr);
        }
        public void AddAttribute(string name, string value) {
            AddAttribute(name, new SimpleProperty<string>(value));
        }
        public void AddAttribute(string name, int value) {
            AddAttribute(name, new SimpleProperty<int>(value));
        }
        public void AddAttribute(string name, float value) {
            AddAttribute(name, new SimpleProperty<float>(value));
        }

        public void BuildSource(StringBuilder sb) {
            BuildSource(sb, 0);
        }

        void BuildSource(StringBuilder sb, int indentLevel) {
            PrintIndent(sb, indentLevel);
            sb.AppendFormat("<{0}", NodeType);

            foreach (var attr in attributes) {
                var value = attr.property.MakeString();
                if (value.Length == 0) { continue; }
                sb.AppendFormat(" {0}=\"{1}\"", attr.name, value);
            }
            sb.Append(">");

            foreach (var child in children) {
                sb.AppendLine();
                child.BuildSource(sb, indentLevel + 1);
            }

            if(children.Count == 0) {
                sb.AppendFormat("</{0}>", NodeType);
            } else { 
                sb.AppendLine();
                PrintIndent(sb, indentLevel);
                sb.AppendFormat("</{0}>", NodeType);
            }
        }

        void PrintIndent(StringBuilder sb, int indentLevel) {
            var aframe = ExportSettings.Instance.aframe;

            var indentCh = "";
            switch (aframe.indentStyle) {
                case AFrameSettings.IndentStyle.Space:
                    indentCh = " ";
                    break;
                case AFrameSettings.IndentStyle.Tab:
                    indentCh = "\t";
                    break;
            }
            var indentStr = "";
            for (int i = 0; i < aframe.indentSize; i++) {
                indentStr += indentCh;
            }

            for (int i = 0; i < indentLevel; i++) {
                sb.Append(indentStr);
            }
        }
    }
}
