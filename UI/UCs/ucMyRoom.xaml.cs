using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UI.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UI.UCs
{
    public sealed partial class ucMyRoom : UserControl
    {
        public event EventHandler<EventArgs> HomeBack;
        public string roomName = null;
        public ObservableCollection<string> deviceSource = null;

        public ucMyRoom()
        {
            this.InitializeComponent();
        }

        public void ChangeRoomName(string name)
        {
            tbRoomName.Text = roomName = name;
            this.ChangeDevice(name);
        }

        private void ChangeDevice(string _roomName)
        {
            //lvDevices.Items.Clear();
            deviceSource = new ObservableCollection<string>(AppData.Instance()
                .GetHouse(HouseData.tagName).GetListDeviceName(_roomName));
            lvDevices.ItemsSource = deviceSource;
            if (deviceSource.Count > 0)
            {
                lvDevices.SelectedIndex = 0;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //Save all
            if (HomeBack != null)
            {
                AppData.Instance().Write();
                HomeBack(sender, EventArgs.Empty);
            }
        }

        private void lvDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvDevices.SelectedItems.Count > 1)
            {
                this.SetEnable(false);
                return;
            }
            else if (lvDevices.SelectedItems.Count == 1)
            {
                this.SetEnable(true);
                string dvName = lvDevices.SelectedItem.ToString();
                if (dvName != tbxDeviceName.Text)
                {
                    DeviceData dd = AppData.Instance().GetHouse(HouseData.tagName)
                        .GetDevice(roomName, lvDevices.SelectedItem.ToString());
                    if (dd != null)
                    {
                        tbxDeviceName.Text = dd.DeviceName;
                        tbxGPIOPin.Text = dd.PinGPIO.ToString();
                        if (dd.KeySpeech != null)
                            tbxKeyWord.Text = dd.KeySpeech;
                        if (dd.StateCurrent)
                        {
                            rdbTurnOn.IsChecked = true;
                        }
                        else
                        {
                            rdbTurnOff.IsChecked = true;
                        }
                        cbxUsingSensor.IsChecked = dd.UsingSensor;
                        cbbSensor.IsEnabled = dd.UsingSensor;
                    }
                }
            }
            else
            {
                this.SetEnable(false);
                if (deviceSource.Count > 0)
                {
                    lvDevices.SelectedIndex = 0;
                }
            }
        }

        private void SetEnable(bool _isEnable)
        {
            tbxDeviceName.IsEnabled = _isEnable;
            tbxGPIOPin.IsEnabled = _isEnable;
            tbxKeyWord.IsEnabled = _isEnable;
            cbxUsingSensor.IsEnabled = _isEnable;
            if (_isEnable)
            {
                if (cbxUsingSensor.IsChecked == true)
                    cbbSensor.IsEnabled = _isEnable;
            }
            else
            {
                cbbSensor.IsEnabled = _isEnable;
            }

        }

        private void cbxUsingSensor_Unchecked(object sender, RoutedEventArgs e)
        {
            cbbSensor.IsEnabled = false;
        }

        private void cbxUsingSensor_Checked(object sender, RoutedEventArgs e)
        {
            cbbSensor.IsEnabled = true;
        }

        private void btnAddDevice_Click(object sender, RoutedEventArgs e)
        {
            //lvDevices.Items.Add(AppData.Instance().GetHouse(HouseData.tagName).AddNewDevice(roomName));
            deviceSource.Add(AppData.Instance().GetHouse(HouseData.tagName).AddNewDevice(roomName));
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lvDevices.SelectedIndex >= 0)
            {
                List<object> name = new List<object>();
                var selectedItems = lvDevices.SelectedItems;
                foreach (var item in selectedItems)
                {
                    name.Add(item);
                }
                string content = "Do you want remove " + (name.Count == 1 ? "this device?" : (name.Count + " devices?"));
                MessageDialog dialog = new MessageDialog(content, "Remove");
                dialog.Commands.Add(new UICommand("Remove")
                {
                    Id = 0
                });
                dialog.Commands.Add(new UICommand("Cancel")
                {
                    Id = 1
                });
                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;
                Task task = Task.Run(async () =>
                {
                    var result = await dialog.ShowAsync();
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        if ((int)result.Id == 0)
                        {
                            HouseData house = AppData.Instance().GetHouse(HouseData.tagName);
                            for (int i = 0; i < name.Count; i++)
                            {
                                bool removed = house.RemoveDevice(roomName, name[i].ToString());
                                if (removed)
                                {
                                    deviceSource.Remove(name[i].ToString());
                                }
                            }
                        }
                    });
                });
            }
            //else
            //{
            //    string content = "Do you want remove this room?";
            //    MessageDialog dialog = new MessageDialog(content, "Remove");
            //    dialog.Commands.Add(new UICommand("Remove")
            //    {
            //        Id = 0
            //    });
            //    dialog.Commands.Add(new UICommand("Cancel")
            //    {
            //        Id = 1
            //    });
            //    dialog.DefaultCommandIndex = 0;
            //    dialog.CancelCommandIndex = 1;
            //    Task task = Task.Run(async () =>
            //    {
            //        var result = await dialog.ShowAsync();
            //        if ((int)result.Id == 0)
            //        {
            //            if (AppData.Instance().GetHouse(HouseData.tagName).RemoveRoom(tbRoomName.Text))
            //            {
            //                btnSave_Click(btnSave, null);
            //            }
            //        }
            //    });
            //}
        }

        private void rdbTurnOff_Checked(object sender, RoutedEventArgs e)
        {
            if (tbState.Text == "Turn on")
            {
                tbState.Text = "Turn off";
            }
        }

        private void rdbTurnOff_Unchecked(object sender, RoutedEventArgs e)
        {
            if (tbState.Text == "Turn off")
            {
                tbState.Text = "Turn on";
            }
        }

        private void btnSaveAll_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = lvDevices.SelectedItems;
            string content = null;
            MessageDialog dialog;

            if (selectedItems.Count >= 1)
            {
                List<object> name = null;
                Dictionary<string, DeviceData> dv = null;
                int index = 0;

                if (selectedItems.Count == 1)
                {
                    dv = new Dictionary<string, DeviceData>();
                    index = lvDevices.SelectedIndex;
                    int GPIOPin;
                    if (int.TryParse(tbxGPIOPin.Text.Trim(), out GPIOPin))
                    {
                        dv.Add(selectedItems[0].ToString(),
                            new DeviceData(tbxDeviceName.Text.Trim(), GPIOPin, tbxKeyWord.Text, 
                            (bool)rdbTurnOn.IsChecked, (bool)cbxUsingSensor.IsChecked));
                    }
                    content = "Do you want save this devices?";
                    dialog = new MessageDialog(content, (rdbTurnOff.IsChecked == true ? "Turn Off " : "Turn On "));
                }
                else
                {
                    name = new List<object>();
                    foreach (var item in selectedItems)
                    {
                        name.Add(item);
                    }

                    content = "Do you want " + (rdbTurnOff.IsChecked == true ? "turn off " : "turn on ")
                    + (name.Count == 1 ? "this device?" : (name.Count + " devices?"));
                    dialog = new MessageDialog(content, (rdbTurnOff.IsChecked == true ? "Turn Off " : "Turn On "));
                }                    
                
                dialog.Commands.Add(new UICommand("Save")
                {
                    Id = 0
                });
                dialog.Commands.Add(new UICommand("Cancel")
                {
                    Id = 1
                });
                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;
                Task task = Task.Run(async () =>
                {
                    var result = await dialog.ShowAsync();
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        if ((int)result.Id == 0)
                        {
                            HouseData house = AppData.Instance().GetHouse(HouseData.tagName);
                            if (name == null)
                            {
                                if (deviceSource.IndexOf(dv.Values.ElementAt(0).DeviceName) < 0
                                || dv.Values.ElementAt(0).DeviceName == dv.Keys.ElementAt(0))
                                {
                                    if (house.EditDevice(this.roomName, dv.Keys.ElementAt(0), dv.Values.ElementAt(0)))
                                    {
                                        //cập nhật tên...
                                        deviceSource[index] = dv.Values.ElementAt(0).DeviceName.ToString();
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < name.Count; i++)
                                {
                                    house.EditDevice(roomName, name[i].ToString(), (bool)rdbTurnOn.IsChecked);
                                }
                            }
                        }
                    });
                });
            }
        }
    }
}
