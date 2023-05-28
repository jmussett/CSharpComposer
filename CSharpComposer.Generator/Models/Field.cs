using System.Xml;
using System.Xml.Serialization;

namespace CSharpComposer.Generator.Models;

public class Field : TreeTypeChild
{
    [XmlAttribute]
    public string? Name { get; set; }

    [XmlAttribute]
    public string? Type { get; set; }

    [XmlAttribute]
    public string? Optional { get; set; }

    [XmlAttribute]
    public string? Override { get; set; }

    [XmlAttribute]
    public string? New { get; set; }

    [XmlAttribute]
    public int MinCount { get; set; }

    [XmlAttribute]
    public bool AllowTrailingSeparator;

    [XmlElement(ElementName = "ContextualKind", Type = typeof(ContextualKind))]
    [XmlElement(ElementName = "Kind", Type = typeof(Kind))]
    public List<KindBase> Kinds = new();

    [XmlElement]
    public Comment? PropertyComment { get; set; }

    public bool IsToken => Type == "SyntaxToken";
    public bool IsOptional => Optional == "true";
    public bool IsOverride => Override == "true";
}
