using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CSFlowDocumentTanulo
{
    public class SerializeMethods
    {

        public TanuloKontener Deserialize(string xml)
        {
            if (!xml.Equals("") && !xml.Equals(null))
            {
                TanuloKontener result = null;
                XmlSerializer serializer = new XmlSerializer(typeof(TanuloKontener));
                using (TextReader reader = new StringReader(xml))
                {
                    result = (TanuloKontener)serializer.Deserialize(reader);
                }
                return result;
            }
            return null;
        }

        public string serialize(Object taroltEljaras, Type anyType)
        {
            XmlSerializer serializer = new XmlSerializer(anyType);

            var xml = "";

            using (var writer = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(writer))
                {
                    serializer.Serialize(writer, taroltEljaras);
                    xml = writer.ToString(); // Your XML
                }
            }
            //System.IO.File.WriteAllText(path, xml);

            return xml;

        }
    }
}
