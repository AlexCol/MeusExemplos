using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PrimeiroExemplo.src.Model;

namespace PrimeiroExemplo.src.Conversores;

public class MyXml {
    public static string ObjToXml<T>(T obj) {
        StringWriter writer = new StringWriter();
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        serializer.Serialize(writer, obj);
        return writer.ToString();
    }
    public static T XmlToObj<T>(string xml) {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        StringReader reader = new StringReader(xml);
        return (T)serializer.Deserialize(reader);
    }
}
