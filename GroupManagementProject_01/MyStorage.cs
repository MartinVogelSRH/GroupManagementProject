using System;
using System.IO;
using System.Xml.Serialization;

namespace GroupManagementProject_01
{
    public class MyStorage
    {
        public static void storeXML<T>(T data, string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            FileStream stream;
            stream = new FileStream(filename, FileMode.Create);
            serializer.Serialize(stream, data);
            stream.Close();
        }

        internal static T readXML<T>(string filename)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(sr);
                }
            }
            catch
            {
                
                return default(T);
            }
        }
    }
}