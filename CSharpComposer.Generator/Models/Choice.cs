using System.Xml;
using System.Xml.Serialization;

namespace CSharpComposer.Generator.Models;

public class Choice : TreeTypeChild
{
    // Note: 'Choice's should not be children of a 'Choice'.  It's not necessary, and the child
    // choice can just be inlined into the parent.
    [XmlElement(ElementName = "Field", Type = typeof(Field))]
    [XmlElement(ElementName = "Sequence", Type = typeof(Sequence))]
    public List<TreeTypeChild> Children { get; set; } = new();

    [XmlAttribute]
    public bool Optional { get; set; }
}
