using RingingBloom.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RingingBloom
{
    public class Options
    {
        public string defaultImport { get; set; }
        public string defaultExport { get; set; }
        public SupportedGames defaultGame { get; set; }
        public string wwisePath { get; set; }
        public string defaultProjectPath { get; set; }

        public Options()
        {
            defaultImport = null;
            defaultExport = null;
            defaultGame = SupportedGames.MHWorld;
            wwisePath = null;
            defaultProjectPath = null;
        }

        public Options(string dImport, string dExport, SupportedGames dGame, string dWwise, string dProject)
        {
            defaultImport = dImport;
            defaultExport = dExport;
            defaultGame = dGame;
            wwisePath = dWwise;
            defaultProjectPath = dProject;
        }

        public Options(XmlReader xml)
        {
            string content;
            while (xml.Read())
            {
                switch (xml.Name)
                {
                    case "DefaultImportPath":
                        content = xml.ReadElementContentAsString();
                        if ( content != "")
                        {
                            defaultImport = content;
                        }
                        break;
                    case "DefaultExportPath":
                        content = xml.ReadElementContentAsString();
                        if (content != "")
                        {
                            defaultExport = content;
                        }
                        break;
                    case "DefaultGame":
                        switch (xml.ReadElementContentAsString())
                        {
                            case "MHWorld":
                                defaultGame = SupportedGames.MHWorld;
                                break;
                            case "RE2/DMC5":
                                defaultGame = SupportedGames.RE2DMC5;
                                break;
                            case "RE3R":
                                defaultGame = SupportedGames.RE3R;
                                break;
                            case "MHRise":
                                defaultGame = SupportedGames.MHRise;
                                break;
                            case "RE8":
                                defaultGame = SupportedGames.RE8;
                                break;
                            default:
                                break;
                        }
                        break;
                    case "WwisePath":
                        content = xml.ReadElementContentAsString();
                        if (content != "")
                        {
                            wwisePath = content;
                        }
                        break;
                    case "DefaultProjectPath":
                        content = xml.ReadElementContentAsString();
                        if (content != "")
                        {
                            defaultProjectPath = content;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public void WriteOptions()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;
            settings.OmitXmlDeclaration = true;
            XmlWriter xml = XmlWriter.Create("Options.xml",settings);//this will mean any time the function is invoked, the old is replaced
            xml.WriteStartElement("RingingBloomOptions");
            xml.WriteElementString("DefaultImportPath", defaultImport);
            xml.WriteElementString("DefaultExportPath", defaultExport);
            xml.WriteElementString("DefaultGame", defaultGame.ToString());
            xml.WriteElementString("WwisePath", wwisePath);
            xml.WriteElementString("DefaultProjectPath", defaultProjectPath);
            xml.WriteEndElement();
            xml.Flush();
            xml.Close();

        }
    }
}
