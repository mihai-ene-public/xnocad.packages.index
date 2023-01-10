using System.Xml.Serialization;

namespace IndexBuilder.Helpers;

public class XmlHelper
{
    public static T GetObjectfromStream<T>(Stream stream)
    {
        using (StreamReader sr = new StreamReader(stream))
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            var o = ser.Deserialize(sr);

            return (T)o;
        }
    }
}