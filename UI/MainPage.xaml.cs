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
using UI.UCs;
using UI.Data;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ucMyHome _ucMyHome;
        private ucMyRoom _ucMyRoom;
        private ucSettings _ucSettings;
        private ucSupport _ucSupport;
        private Control _currentUser;

        public MainPage()
        {
            this.InitializeComponent();
            _ucMyHome = new ucMyHome();
            _ucMyHome.ViewRoomClicked += _ucMyHome_NewRoomClicked;
            _ucMyRoom = new ucMyRoom();
            _ucMyRoom.HomeBack += _ucMyRoom_HomeBack;
            _ucSettings = new ucSettings();
            _ucSupport = new ucSupport();
            _currentUser = _ucMyHome;
            gridMain.Children.Add(_ucMyHome);
            Grid.SetRow(_ucMyHome, 1);
            Grid.SetRow(_ucMyRoom, 1);
            Grid.SetRow(_ucSettings, 1);
            Grid.SetRow(_ucSupport, 1); 
        }

        private void _ucMyRoom_HomeBack(object sender, EventArgs e)
        {
            if (_currentUser != _ucMyHome)
            {
                _currentUser = _ucMyHome;
                gridMain.Children.RemoveAt(1);
                gridMain.Children.Add(_ucMyHome);
            }
        }

        private void _ucMyHome_NewRoomClicked(object sender, EventArgs e)
        {
            if (_currentUser != _ucMyRoom)
            {
                gridMain.Children.RemoveAt(1);
                _currentUser = _ucMyRoom;
                _ucMyRoom.ChangeRoomName(((TextBlock)sender).Text);
                gridMain.Children.Add(_ucMyRoom);
            }
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            if(_currentUser != _ucMyHome)
            {
                _currentUser = _ucMyHome;
                gridMain.Children.RemoveAt(1);
                gridMain.Children.Add(_ucMyHome);
            }
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser != _ucSettings)
            {
                gridMain.Children.RemoveAt(1);
                _currentUser = _ucSettings;
                gridMain.Children.Add(_ucSettings);
            }
        }

        private void btnSupport_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser != _ucSupport)
            {
                _currentUser = _ucSupport;
                gridMain.Children.RemoveAt(1);
                gridMain.Children.Add(_ucSupport);
            }
        }
    }
}
