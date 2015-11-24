using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UI.Data
{
    class RoomData
    {
        public const string tagName = "RoomData";
        public string RoomName;
        public Dictionary<string, DeviceData> devices;
        
        public RoomData(string _roomName)
        {
            RoomName = _roomName;
            devices = new Dictionary<string, DeviceData>();
        }

        public List<string> GetListDeviceName()
        {
            List<string> name = new List<string>();
            foreach (var item in devices)
            {
                name.Add(item.Key);
            }
            return name;
        }

        public DeviceData GetDevice(string _deviceName)
        {
            if (devices.ContainsKey(_deviceName))
            {
                return devices[_deviceName];
            }
            return null;
        }

        public bool AddNewDevice(string _deviceName, int _pinGPIO, string _keySpeech, bool _state,
            bool _useSensor = false, int _pinSensor = 0)
        {
            if (devices.ContainsKey(_deviceName))
            {
                return false;
            }
            devices.Add(_deviceName, new DeviceData(_deviceName, _pinGPIO, _keySpeech, _state, _useSensor, _pinSensor));
            return true;
        }

        public bool AddNewDevice(DeviceData _device)
        {
            if (devices.ContainsKey(_device.DeviceName))
            {
                return false;
            }
            devices.Add(_device.DeviceName, new DeviceData(_device.DeviceName, _device.PinGPIO, _device.KeySpeech, 
                _device.StateCurrent, _device.UsingSensor, _device.PinSensor));
            return true;
        }

        public string AddNewDevice()
        {
            string _deviceName = "My Device";
            if (devices.ContainsKey(_deviceName))
            {
                _deviceName = this.CreateName();
            }
            DeviceData device = new DeviceData(_deviceName);
            devices.Add(_deviceName, device);
            return _deviceName;
        }

        private string CreateName()
        {
            string _name = "My Device";
            int index = 1;
            while (devices.ContainsKey(_name + " (" + index.ToString() + ")"))
            {
                index++;
            }
            return _name + " (" + index.ToString() + ")";
        }

        public bool RemoveDevice(string _deviceName)
        {
            if (devices.ContainsKey(_deviceName))
            {
                devices.Remove(_deviceName);
                return true;
            }
            return false;
        }

        public bool EditDevice(string _deviceName, DeviceData _device)
        {
            if (devices.ContainsKey(_deviceName))
            {
                devices.Remove(_deviceName);
                devices.Add(_device.DeviceName, _device);
                return true;
            }
            return false;
        }

        public bool EditDevice(string _deviceName,bool _state)
        {
            if (devices.ContainsKey(_deviceName))
            {
                devices[_deviceName].ChangeState(_state);
                return true;
            }
            return false;
        }

        public void Read(XmlNodeList xnList)
        {
            DeviceData device = null;
            foreach (XmlNode xn in xnList)
            {
                device = new DeviceData(xn.Attributes["name"].Value);
                device.Read(xn);
                devices.Add(device.DeviceName, device);
            }
        }

        public string Write(XmlWriter xmlWriter, int _ID)
        {
            StringBuilder content = new StringBuilder();
            //using (XmlWriter xmlWriter = XmlWriter.Create(content))
            {
                xmlWriter.WriteStartElement(tagName);
                xmlWriter.WriteAttributeString("ID", _ID.ToString());
                xmlWriter.WriteAttributeString("name", RoomName);
                int ID = 0;
                foreach (var item in devices)
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
