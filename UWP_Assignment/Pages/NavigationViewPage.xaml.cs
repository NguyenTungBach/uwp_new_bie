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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP_Assignment.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NavigationViewPage : Page
    {
        public static Frame NavigationFrame;

        private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            ("listSongAll", typeof(Pages.ListSongAllPage)),
            ("listSongMine", typeof(Pages.ListSongMinePage)),
            ("createSongAll", typeof(Pages.CreateSongAllPage)),
            ("createSongMine", typeof(Pages.CreateSongMinePage)),
            ("info", typeof(Pages.InfoPage)),
        };
        public NavigationViewPage()
        {
            this.InitializeComponent();
            NavigationFrame = MainContent;
            this.Loaded += NavigationViewPage_Loaded;
        }

        private void NavigationViewPage_Loaded(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(typeof(Pages.InfoPage));
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                MainContent.Navigate(typeof(Pages.SettingPage));
            }
            else
            {
                var selectItem = sender.SelectedItem as NavigationViewItem;
                var item = _pages.FirstOrDefault(p => p.Tag.Equals(selectItem.Tag)); // Tìm ra 1 thằng của _page mà có key Tag trùng với tag mình click
                MainContent.Navigate(item.Page);
            }
        }
    }
}
