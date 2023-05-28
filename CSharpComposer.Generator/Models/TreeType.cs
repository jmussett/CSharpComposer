using System.Xml;
using System.Xml.Serialization;

namespace CSharpComposer.Generator.Models;

public class TreeType
{
    [XmlAttribute]
    public string? Name { get; set; }

    [XmlAttribute]
    public string? Base { get; set; }

    [XmlAttribute]
    public string? SkipConvenienceFactories { get; set; }

    [XmlElement]
    public Comment? TypeComment { get; set; }

    [XmlElement]
    public Comment? FactoryComment { get; set; }

    [XmlElement(ElementName = "Field", Type = typeof(Field))]
    [XmlElement(ElementName = "Choice", Type = typeof(Choice))]
    [XmlElement(ElementName = "Sequence", Type = typeof(Sequence))]
    public List<TreeTypeChild> Children { get; set; } = new();
}
