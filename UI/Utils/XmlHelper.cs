using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;

namespace UI.Utils
{
    class XmlHelper
    {
        public static async Task WriteData(StringBuilder content, string fileName)
        {
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            StringBuilder _content = content;

            // Create a new folder name DataFolder.
            var dataFolder = await local.CreateFolderAsync("DataFolder",
                CreationCollisionOption.OpenIfExists);

            // Create a new file named DataFile.txt.
            var file = await dataFolder.CreateFileAsync(fileName,
            CreationCollisionOption.ReplaceExisting);



            //using (XmlWriter xmlWriter = XmlWriter.Create(fileName))
            //{
            //    xmlWriter.WriteStartDocument();
            //    xmlWriter.WriteStartElement("Settings");
            //    xmlWriter.WriteElementString("UserName", tbxUserName.Text.Trim());
            //    xmlWriter.WriteElementString("Password", tbxPassword.Text.Trim());
            //    xmlWriter.WriteElementString("Email", tbxEmail.Text.Trim());
            //    xmlWriter.WriteElementString("TopicName", tbxTopicName.Text.Trim());
            //    xmlWriter.WriteElementString("SASKeyName", tbxSASKeyName.Text.Trim());
            //    xmlWriter.WriteElementString("SASKey", tbxSASKey.Text.Trim());
            //    xmlWriter.WriteEndElement();
            //    xmlWriter.WriteEndDocument();
            //}
            
            await FileIO.WriteTextAsync(file, _content.ToString());

        }

        public static async Task<XmlDocument> ReadFile(string fileName)
        {
            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
            XmlDocument xml = null;
            if (local != null)
            {
                try
                {
                    // Get the DataFolder folder.
                    var dataFolder = await local.GetFolderAsync("DataFolder");

                    StorageFile sampleFile = await dataFolder.GetFileAsync(fileName);

                    string readText = await FileIO.ReadTextAsync(sampleFile);

                    // suppose that myXmlString contains "<Names>...</Names>"

                    xml = new XmlDocument();
                    xml.LoadXml(readText);
                }
                catch
                {
                    return null;
                }


                //XmlNodeList xnList = xml.GetElementsByTagName("UserName");
                //tbxUserName.Text = xnList[0].InnerText;
                //xnList = xml.GetElementsByTagName("Password");
                //tbxPassword.Text = xnList[0].InnerText;
                //xnList = xml.GetElementsByTagName("Email");
                //tbxEmail.Text = xnList[0].InnerText;
                //xnList = xml.GetElementsByTagName("TopicName");
                //tbxTopicName.Text = xnList[0].InnerText;
                //xnList = xml.GetElementsByTagName("SASKeyName");
                //tbxSASKeyName.Text = xnList[0].InnerText;
                //xnList = xml.GetElementsByTagName("SASKey");
                //tbxSASKey.Text = xnList[0].InnerText;
            }

            return xml;
        }
    }
}
