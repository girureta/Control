using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Control.Tests
{
    public static class TestUtils
    {
        public static string GetXmlString(XmlElement element)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.NewLineHandling = NewLineHandling.None;

            using (var stream = new MemoryStream())
            {
                System.Text.Encoding encoding;
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    if (writer == null)
                    {
                        throw new InvalidOperationException("writer is null");
                    }

                    encoding = writer.Settings.Encoding;
                    var ser = new XmlSerializer(element.GetType());
                    ser.Serialize(writer, element);
                }

                stream.Position = 0;
                using (var reader = new StreamReader(stream, encoding, true))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}