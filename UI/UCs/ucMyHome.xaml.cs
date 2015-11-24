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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using UI.Utils;
using UI.Data;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UI.UCs
{
    public sealed partial class ucMyHome : UserControl
    {
        public event EventHandler<EventArgs> ViewRoomClicked;

        public ucMyHome()
        {
            this.InitializeComponent();
            AppData.Instance().Read();
            AppData.Instance().AddHouse(HouseData.tagName);
        }

        private void btnAddRoom_Click(object sender, RoutedEventArgs e)
        {
            //Thêm phòng
            Button btn = NewRoomButton(AppData.Instance().GetHouse(HouseData.tagName).AddNewRoom());
            AppData.Instance().Write();
            wpRooms.Children.Add(btn);
        }

        private void btnViewRoom_Click(object sender, RoutedEventArgs e)
        {
            if (ViewRoomClicked != null)
            {
                foreach (TextBlock tb in UIHelper.FindVisualChildren<TextBlock>((Button)sender))
                {
                    ViewRoomClicked(tb, EventArgs.Empty);
                    break;
                }
            }
        }

        private Button NewRoomButton(string _name)
        {
            Button bt = new Button();
            bt.Margin = new Thickness(20, 20, 20, 20);
            bt.Height = 200;
            bt.Width = 200;

            /*<Grid
                        <Grid.RowDefinitions>
                            <RowDefinition Height="8*"></RowDefinition>
                            <RowDefinition Height="2*"></RowDefinition>
                        </Grid.RowDefinitions>
                        <Image Grid.Row="0" Source="ms-appx:///Images/addicon.png" Stretch="Fill"></Image>
                        <TextBlock Grid.Row="1" HorizontalAlignment="Center" VerticalAlignment="Center" FontStyle="Italic">Add new room</TextBlock>
                    </Grid>*/

            Grid grid = new Grid();
            RowDefinition rowDef = new RowDefinition();
            rowDef.Height = new GridLength(8, GridUnitType.Star);
            grid.RowDefinitions.Add(rowDef);
            rowDef = new RowDefinition();
            rowDef.Height = new GridLength(2, GridUnitType.Star);
            grid.RowDefinitions.Add(rowDef);

            Image image = new Image();
            image.Stretch = Stretch.Fill;
            BitmapImage bitmapImage = new BitmapImage();
            Uri uri = new Uri("ms-appx:///Images/none.png");
            bitmapImage.UriSource = uri;
            image.Source = bitmapImage;

            TextBlock tb = new TextBlock();
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.FontStyle = Windows.UI.Text.FontStyle.Italic;
            tb.Text = _name != null? _name: "New Room " + (wpRooms.Children.Count - 1);

            grid.Children.Add(tb);
            Grid.SetRow(tb, 1);
            grid.Children.Add(image);
            Grid.SetRow(image, 0);

            bt.Content = grid;
            bt.Click += btnViewRoom_Click;

            return bt;
        }
    }
}
