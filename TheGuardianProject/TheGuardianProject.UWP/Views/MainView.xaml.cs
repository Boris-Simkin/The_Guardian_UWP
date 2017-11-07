using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TheGuardian.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView
    {
        public MainView()
        {
            this.InitializeComponent();
        }

        #region  Anumation stuff
        private void HeaderImage_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Image img = (Image)e.OriginalSource;
            Storyboard s = (Storyboard)img.Resources["Scale"];
            s.Begin();
        }

        private async void Header_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            StackPanel stackPanel = (StackPanel)sender;
            Storyboard storyboar = (Storyboard)stackPanel.Resources["Scale"];
            storyboar.Begin();

            var grid = stackPanel.Children.OfType<Grid>().FirstOrDefault();
            if (grid != null)
            {
                var headerImage = grid.Children.OfType<Image>().FirstOrDefault();
                if (headerImage != null)
                {
                    var imageBrush = new ImageBrush { ImageSource = headerImage.Source, Stretch = Stretch.Fill, Opacity = 0 };
                    MainGrid.Background = imageBrush;
                    await Task.Delay(100);
                    for (int i = 0; i < 20; i++)
                    {
                        await Task.Delay(10);
                        imageBrush.Opacity = (i/250.1);
                    }
                }
            }
        }
        private void Header_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            StackPanel stackPanel = (StackPanel)sender;
            Storyboard storyboard = (Storyboard)stackPanel.Resources["UnScale"];

            try
            {
                storyboard.Begin();
            }
            //When a header is pressed the another view loaded and access the element is impossible
            catch (Exception) { }
        }
        #endregion
    }
}
