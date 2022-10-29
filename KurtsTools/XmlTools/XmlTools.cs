using System.Runtime.Versioning;
using System.Xml;

namespace NSKurtsTools;

[SupportedOSPlatform("windows")]
public static partial class KurtsTools{
    public static XmlDocument StringToXmlDocument(string xmlString){
        // if (xmlString == null) return null;

        XmlDocument doc = new();
        doc.LoadXml(xmlString);
        return doc;
    }
}