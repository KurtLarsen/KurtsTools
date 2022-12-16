using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.Xml;

namespace NSKurtsTools;

[SupportedOSPlatform("windows")]
public static partial class KurtsTools{
    public static XmlDocument StringToXmlDocument([StringSyntax(StringSyntaxAttribute.Xml)]string xmlString){
        XmlDocument doc = new();
        doc.LoadXml(xmlString);
        return doc;
    }
}