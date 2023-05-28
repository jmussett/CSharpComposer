using System.Xml;
using System.Xml.Serialization;

namespace CSharpComposer.Generator.Models;

public class Comment
{
    [XmlAnyElement]
    public XmlElement[]? Body { get; set; }
}
