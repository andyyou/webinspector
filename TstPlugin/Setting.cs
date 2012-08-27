using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace PxP
{
    class Setting
    {
        private XmlDocument docXmlSettings = null;
        private string xmlFileName;
        string appDir;
        public Setting(string FileName)
        {
            appDir = Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\..\\Parameter Files\\PXP\\";
            try
            {
                this.docXmlSettings = new XmlDocument();
                this.xmlFileName = appDir + FileName;
                if (!File.Exists(this.xmlFileName))
                {
                    XmlElement docNode = this.docXmlSettings.CreateElement("myAPP");
                    this.docXmlSettings.AppendChild(docNode);
                    this.docXmlSettings.Save(appDir + FileName);
                }

                this.docXmlSettings.Load(this.xmlFileName);
            }
            catch
            {
                throw new FileNotFoundException("Error phrasing file :" + this.xmlFileName);
            }

        }

        public bool writeXMLString(string parent, string key, string value)
        {
            try
            {
                XmlElement rootElement = this.docXmlSettings.DocumentElement;
                XmlNode xmlKeyNode = rootElement.SelectSingleNode("/myAPP/" + parent + "/" + key);
                if (xmlKeyNode != null)
                {
                    xmlKeyNode.InnerText = value;
                    this.docXmlSettings.Save(this.xmlFileName);
                }
                else
                {
                    XmlNode xmlNewNode;
                    XmlNode XmlParentNode = rootElement.SelectSingleNode("/myAPP/" + parent);
                    if (XmlParentNode == null)
                    {
                        XmlParentNode = docXmlSettings.DocumentElement;
                        xmlNewNode = docXmlSettings.CreateElement(parent);
                        XmlParentNode.AppendChild(xmlNewNode);
                    }
                    XmlParentNode = rootElement.SelectSingleNode("/myAPP/" + parent);
                    xmlNewNode = docXmlSettings.CreateElement(key);
                    xmlNewNode.InnerText = value;
                    XmlParentNode.AppendChild(xmlNewNode);

                    this.docXmlSettings.Save(this.xmlFileName);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string readXMLString(string parent, string key, string mdefault)
        {
            try
            {
                XmlElement rootElement = this.docXmlSettings.DocumentElement;
                XmlNode xmlKeyNode = rootElement.SelectSingleNode("/myAPP/" + parent + "/" + key);
                if (xmlKeyNode != null)
                {
                    return xmlKeyNode.InnerText;
                }
                return mdefault;
            }
            catch
            {
                return mdefault;
            }
        }

        public bool writeXMLStringForSeries(string id, string color, string LetterOrShape, string mdefault)
        {
            try
            {
                XmlElement root = docXmlSettings.DocumentElement, SeriesSettingElement = null;
                XmlElement Issue = null, idValue = null, colorValue = null, ShowValue = null;
                if (root.SelectSingleNode("SeriesSetting") == null)
                {
                    SeriesSettingElement = docXmlSettings.CreateElement("SeriesSetting");
                    root.AppendChild(SeriesSettingElement);
                }
                SeriesSettingElement = (XmlElement)root.SelectSingleNode("SeriesSetting");

                XmlNodeList GetList = SeriesSettingElement.ChildNodes;
                XmlNodeList GetNode = null;

                foreach (XmlNode Node in GetList)
                {
                    XmlElement NodeElement = (XmlElement)Node;

                    XmlNodeList list = NodeElement.ChildNodes;

                    if (list.Item(0).InnerText == id)
                    {
                        GetNode = list;
                        break;
                    }
                }
                if (GetNode != null)
                {

                    GetNode.Item(1).InnerText = color;
                    GetNode.Item(2).InnerText = LetterOrShape;
                }
                else
                {
                    Issue = docXmlSettings.CreateElement("Issue");
                    idValue = docXmlSettings.CreateElement("Name");
                    idValue.InnerText = id;
                    Issue.AppendChild(idValue);

                    colorValue = docXmlSettings.CreateElement("Color");
                    colorValue.InnerText = color;
                    Issue.AppendChild(colorValue);

                    ShowValue = docXmlSettings.CreateElement("Value");
                    ShowValue.InnerText = LetterOrShape;
                    Issue.AppendChild(ShowValue);

                    SeriesSettingElement.AppendChild(Issue);

                }
                docXmlSettings.Save(this.xmlFileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public XmlNodeList readXmlStringForSeries(string id)
        {
            try
            {
                XmlElement root = docXmlSettings.DocumentElement, SeriesSettingElement = null;
                //XmlElement Issue = null, idValue = null, colorValue = null, ShowValue = null;
                SeriesSettingElement = (XmlElement)root.SelectSingleNode("SeriesSetting");
                
                XmlNodeList GetList = SeriesSettingElement.ChildNodes;
                foreach (XmlNode Node in GetList)
                {
                    XmlElement NodeElement = (XmlElement)Node;

                    XmlNodeList list = NodeElement.ChildNodes;

                    if (list.Item(0).InnerText == id)
                    {
                        return list;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public bool SaveAs(String name)
        {
            try
            {
                docXmlSettings.Save(appDir + name + ".xml");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

