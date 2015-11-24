using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UI.Utils;

namespace UI.Data
{
    class HouseData
    {
        public const string tagName = "HouseData";
        public string HouseName;
        public Dictionary<string, RoomData> rooms;

        public HouseData(string _name)
        {
            HouseName = _name;
            rooms = new Dictionary<string, RoomData>();
        }

        public List<string> GetListRoomName()
        {
            List<string> name = new List<string>();
            foreach (var item in rooms)
            {
                name.Add(item.Key);
            }
            return name;
        }

        public List<string> GetListDeviceName(string _roomName)
        {
            List<string> name = new List<string>();
            if (rooms.ContainsKey(_roomName))
            {
                return rooms[_roomName].GetListDeviceName();
            }
            return name;
        }

        public bool AddNewRoom(string _roomName)
        {
            if (rooms.ContainsKey(_roomName))
            {
                return false;
            }
            RoomData room = new RoomData(_roomName);
            rooms.Add(_roomName, room);
            return true;
        }

        public string AddNewRoom()
        {
            string _roomName = "My Room";
            if (rooms.ContainsKey(_roomName))
            {
                _roomName = this.CreateName();
            }
            RoomData room = new RoomData(_roomName);
            room.AddNewDevice();
            room.AddNewDevice();
            rooms.Add(_roomName, room);
            return _roomName;
        }

        private string CreateName()
        {
            string _name = "My Room";
            int index = 1;
            while (rooms.ContainsKey(_name + " (" + index.ToString() + ")"))
            {
                index++;
            }
            return _name + " (" + index.ToString() + ")";
        }

        public bool RemoveRoom(string _roomName)
        {
            if (rooms.ContainsKey(_roomName))
            {
                rooms.Remove(_roomName);
                return true;
            }
            return false;
        }

        public bool AddNewDevice(string _roomName, DeviceData _device)
        {
            if (rooms.ContainsKey(_roomName))
            {
                return rooms[_roomName].AddNewDevice(_device);
            }
            return false;
        }

        public string AddNewDevice(string _roomName)
        {
            return rooms[_roomName].AddNewDevice();
        }

        public bool RemoveDevice(string _roomName, string _deviceName)
        {
            if (rooms.ContainsKey(_roomName))
            {
                return rooms[_roomName].RemoveDevice(_deviceName);
            }
            return false;
        }

        public bool EditDevice(string _roomName, string _deviceName, DeviceData _device)
        {
            if (rooms.ContainsKey(_roomName))
            {
                return rooms[_roomName].EditDevice(_deviceName, _device);
            }
            return false;
        }

        public bool EditDevice(string _roomName, string _deviceName, bool _state)
        {
            if (rooms.ContainsKey(_roomName))
            {
                return rooms[_roomName].EditDevice(_deviceName, _state);
            }
            return false;
        }

        public RoomData GetRoom(string _roomName)
        {
            if (rooms.ContainsKey(_roomName))
            {
                return rooms[_roomName];
            }
            return null;
        }

        public DeviceData GetDevice(string _roomName, string _deviceName)
        {
            if (rooms.ContainsKey(_roomName))
            {
                return rooms[_roomName].GetDevice(_deviceName);
            }
            return null;
        }

        public void Read(XmlNodeList xnList)
        {
            RoomData room = null;
            foreach (XmlNode xn in xnList)
            {
                room = new RoomData(xn.Attributes["name"].Value);
                room.Read(xn.ChildNodes);
                rooms.Add(room.RoomName, room);
            }
        }

        public string Write(XmlWriter xmlWriter, int _ID)
        {
            StringBuilder content = new StringBuilder();
            //using (XmlWriter xmlWriter = XmlWriter.Create(content))
            {
                xmlWriter.WriteStartElement(tagName);
                xmlWriter.WriteAttributeString("ID", _ID.ToString());
                xmlWriter.WriteAttributeString("name", HouseName);
                int ID = 0;
                foreach (var item in rooms)
                {
                    //xmlWriter.WriteString(item.Value.Write());
                    item.Value.Write(xmlWriter, ID);
                    ID++;
                }
                xmlWriter.WriteEndElement();
            }
            return content.ToString();
        }
    }
}
