using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UI.Utils;

namespace UI.Data
{
    class AppData
    {
        public const string fileName = "myAppDataFile.xml";
        public const string tagName = "AppData";
        private Dictionary<string, HouseData> houses = null;

        private static AppData instance = null;

        public static AppData Instance()
        {
            if (instance == null)
            {
                instance = new AppData(tagName);
            }
            return instance;
        }

        public AppData(string _name)
        {
            houses = new Dictionary<string, HouseData>();
        }

        public HouseData GetHouse(string _name)
        {
            if (houses.ContainsKey(_name))
            {
                return houses[_name];
            }
            return new HouseData(tagName);
        }

        public bool AddHouse(string _name)
        {
            if (!houses.ContainsKey(_name))
            {
                houses.Add(_name, new HouseData(_name));
                return true;
            }
            return false;
        }

        public string AddHouse()
        {
            string _name = "My House";
            if (houses.ContainsKey(_name))
            {
                _name = this.CreateName();
            }
            houses.Add(_name, new HouseData(_name));
            return _name;
        }

        private string CreateName()
        {
            string _name = "My House";
            int index = 1;
            while (houses.ContainsKey(_name + " (" + index.ToString() + ")"))
            {
                index++;
            }
            return _name + " (" + index.ToString() + ")";
        }

        public void Read()
        {
            XmlDocument xml = null;
            Task task = Task.Run(async () =>
            {
                xml = await Utils.XmlHelper.ReadFile(fileName);
            });
            task.Wait();
            if (xml != null)
            {
                XmlNodeList xnList = xml.GetElementsByTagName(tagName);
                HouseData house = null;
                foreach (XmlNode xn in xnList)
                {
                    house = new HouseData(xn.Attributes["name"].Value);
                    house.Read(xn.ChildNodes);
                    houses.Add(house.HouseName, house);
                }
            }
        }

        public void Write()
        {
            StringBuilder content = new StringBuilder();
            using (XmlWriter xmlWriter = XmlWriter.Create(content))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement(tagName);
                int ID = 0;
                foreach (var item in houses)
                {
                    //xmlWriter.WriteString(item.Value.Write());
                    item.Value.Write(xmlWriter, ID);
                    ID++;
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }

            Debug.WriteLine(content);
            //Task.Run(async () =>
            //{
            //    await XmlHelper.WriteData(content, fileName);
            //});
        }
    }
}
