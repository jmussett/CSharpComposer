using System.Xml;
using System.Xml.Serialization;

namespace CSharpComposer.Generator.Models;

public class Node : TreeType
{
    [XmlAttribute]
    public string? Root { get; set; }

    [XmlAttribute]
    public string? Errors { get; set; }

    [XmlElement(ElementName = "Kind", Type = typeof(Kind))]
    public List<Kind> Kinds = new();

    public readonly List<Field> Fields = new();
}
