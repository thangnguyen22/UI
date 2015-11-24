using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Xml;
using System.Text;
using System.Diagnostics;
using UI.Utils;
using System.Threading.Tasks;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UI.UCs
{
    public sealed partial class ucSettings : UserControl
    {
        private const string fileName = "Settings.xml";

        public ucSettings()
        {
            this.InitializeComponent();
            SetEnable(false);
            ReadData();
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            if(btn.Content.ToString() == "Edit")
            {
                btn.Content = "Save";
                SetEnable(true);
            }
            else
            {
                btn.Content = "Edit";
                SetEnable(false);
                WriteData();
            }
        }

        private void SetEnable(bool value)
        {
            tbxEmail.IsEnabled = value;
            tbxPassword.IsEnabled = value;
            tbxSASKey.IsEnabled = value;
            tbxSASKeyName.IsEnabled = value;
            tbxTopicName.IsEnabled = value;
            tbxUserName.IsEnabled = value;
        }

        private void ReadData()
        {
            XmlDocument xml = null;
            Task task = Task.Run(async () =>
            {
                xml = await Utils.XmlHelper.ReadFile(fileName);
                
            });
            task.Wait();
            XmlNodeList xnList = xml.GetElementsByTagName("UserName");
            tbxUserName.Text = xnList[0].InnerText;
            xnList = xml.GetElementsByTagName("Password");
            tbxPassword.Text = xnList[0].InnerText;
            xnList = xml.GetElementsByTagName("Email");
            tbxEmail.Text = xnList[0].InnerText;
            xnList = xml.GetElementsByTagName("TopicName");
            tbxTopicName.Text = xnList[0].InnerText;
            xnList = xml.GetElementsByTagName("SASKeyName");
            tbxSASKeyName.Text = xnList[0].InnerText;
            xnList = xml.GetElementsByTagName("SASKey");
            tbxSASKey.Text = xnList[0].InnerText;
        }

        private void WriteData()
        {
            StringBuilder content = new StringBuilder();
            using (XmlWriter xmlWriter = XmlWriter.Create(content))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Settings");
                xmlWriter.WriteElementString("UserName", tbxUserName.Text.Trim());
                xmlWriter.WriteElementString("Password", tbxPassword.Text.Trim());
                xmlWriter.WriteElementString("Email", tbxEmail.Text.Trim());
                xmlWriter.WriteElementString("TopicName", tbxTopicName.Text.Trim());
                xmlWriter.WriteElementString("SASKeyName", tbxSASKeyName.Text.Trim());
                xmlWriter.WriteElementString("SASKey", tbxSASKey.Text.Trim());
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
            Task.Run(async () =>
            {
                await XmlHelper.WriteData(content, fileName);
            });
        }
    }
}
