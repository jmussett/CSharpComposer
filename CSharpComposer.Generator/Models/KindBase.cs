using System.Xml;
using System.Xml.Serialization;

namespace CSharpComposer.Generator.Models;

public class KindBase
{
    [XmlAttribute]
    public string Name { get; set; } = "";
}
