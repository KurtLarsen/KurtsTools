using System.Xml;

namespace KurtsToolsLibrary.XmlTools;

public static class XmlTools{
    public static XmlDocument StringToXmlDocument(string xmlString){
        // if (xmlString == null) return null;

        XmlDocument doc = new();
        doc.LoadXml(xmlString);
        return doc;
    }
}