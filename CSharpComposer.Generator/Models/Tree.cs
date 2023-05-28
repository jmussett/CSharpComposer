using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CSharpComposer.Generator.Models;

[XmlRoot]
public class Tree
{
    [XmlAttribute]
    public string? Root { get; set; }

    [XmlElement(ElementName = "Node", Type = typeof(Node))]
    [XmlElement(ElementName = "AbstractNode", Type = typeof(AbstractNode))]
    [XmlElement(ElementName = "PredefinedNode", Type = typeof(PredefinedNode))]
    public List<TreeType> Types { get; set; } = new();
}
