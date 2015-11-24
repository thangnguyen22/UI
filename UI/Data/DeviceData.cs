using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UI.Data
{
    class DeviceData
    {
        public const string tagName = "MyDevice";
        public const string deviceName = "DeviceName";
        public const string keySpeech = "KeySpeech";
        public const string pinGPIO = "PinGPIO";
        public const string stateCurrent = "StateCurrent";
        public const string usingSensor = "UsingSensor";
        public const string pinSensor = "PinSensor";

        public string DeviceName;
        public int PinGPIO;
        public string KeySpeech;
        public bool StateCurrent;
        public bool UsingSensor;
        public int PinSensor;

        public DeviceData(string _deviceName, int _pinGPIO, string _keySpeech, bool _state, 
            bool _useSensor = false, int _pinSensor = 0)
        {
            DeviceName = _deviceName;
            PinGPIO = _pinGPIO;
            KeySpeech = _keySpeech;
            StateCurrent = _state;
            UsingSensor = _useSensor;
            PinSensor = _pinSensor;
        }

        public DeviceData(string _deviceName)
        {
            DeviceName = _deviceName;
        }

        public void ChangeState(bool _state)
        {
            StateCurrent = _state;
        }

        public void Read(XmlNode xn)
        {
            DeviceName = xn.Attributes[deviceName].Value;
            if(!int.TryParse(xn.Attributes[stateCurrent].Value, out PinGPIO))
            {
                PinGPIO = 0;
            }
            KeySpeech = xn.Attributes[keySpeech].Value;
            StateCurrent = xn.Attributes[stateCurrent].Value == "true" ? true : false;
            UsingSensor = xn.Attributes[usingSensor].Value == "true" ? true : false;
            if (!int.TryParse(xn.Attributes[stateCurrent].Value, out PinSensor))
            {
                PinSensor = 0;
            }
        }

        public string Write(XmlWriter xmlWriter, int ID)
        {
            StringBuilder content = new StringBuilder();
            //using (XmlWriter xmlWriter = XmlWriter.Create(content))
            {
                xmlWriter.WriteStartElement(tagName);
                xmlWriter.WriteAttributeString("ID", ID.ToString());
                xmlWriter.WriteAttributeString(deviceName, DeviceName);
                xmlWriter.WriteAttributeString(pinGPIO, PinGPIO.ToString());
                xmlWriter.WriteAttributeString(keySpeech, KeySpeech);
                xmlWriter.WriteAttributeString(stateCurrent, StateCurrent.ToString());
                xmlWriter.WriteAttributeString(usingSensor, UsingSensor.ToString());
                xmlWriter.WriteAttributeString(pinSensor, PinSensor.ToString());
                xmlWriter.WriteEndElement();
            }
            return content.ToString();
        }
    }
}
