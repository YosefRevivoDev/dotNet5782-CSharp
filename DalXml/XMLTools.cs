using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DAL
{
    class XMLTools
    {
        static string dir = @"xml\";

        static XMLTools()
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        public static void SaveListToXMLElement(XElement rootElem , string filePath)
        {
            try
            {
                rootElem.Save(dir + filePath);
            }
            catch (Exception ex)
            {

                throw new XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }

        public static XElement LoadListFromXMLElement(string filePath)
        {
            try
            {
                if (File.Exists(dir+filePath))
                {
                    return XElement.Load(dir + filePath);
                }
                else
                {
                    XElement rootElem = new XElement(dir + filePath);
                    rootElem.Save(dir + filePath);
                    return rootElem;
                }
            }
            catch (Exception ex)
            {
                throw new XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }

        public static void SaveListToXMLSerializer<T>(List<T> list , string filePath)
        {
            try
            {
                FileStream file = new FileStream(dir + filePath, FileMode.Create);
                XmlSerializer xml = new XmlSerializer(list.GetType());
                xml.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }

        public static List<T> LoadListToXMLSerializer<T>(string filePath)
        {
            try
            {
                if (File.Exists(dir+filePath))
                {
                    List<T> list;
                    XmlSerializer xml = new XmlSerializer(typeof(List<T>));
                    FileStream file = new FileStream(dir + filePath, FileMode.Open);
                    list = (List<T>)xml.Deserialize(file);
                    file.Close();
                    return list;
                }
                else
                {
                    return new List<T>();
                }

            }
            catch (Exception ex)
            {
                throw new XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
    }
}
