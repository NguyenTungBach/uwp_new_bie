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

namespace T2012E_UWP.Demo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NavigationViewDemo : Page
    {
        public static Frame NavigationFrame;

        private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            ("listSong", typeof(Demo.Page1)),
            ("register", typeof(Demo.RegisterPage)),
            ("login", typeof(Demo.Login)),
           
        };
        public NavigationViewDemo()
        {
            this.InitializeComponent();
            this.Loaded += NavigationViewDemo_Loaded; // DOM Content Loaded
            NavigationFrame = MainContent;
        }

        private void NavigationViewDemo_Loaded(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(typeof(Demo.Page1));
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                MainContent.Navigate(typeof(Demo.SettingPage));
            }
            else
            {
                var selectItem = sender.SelectedItem as NavigationViewItem;
                var item = _pages.FirstOrDefault(p => p.Tag.Equals(selectItem.Tag)); // Tìm ra 1 thằng của _page mà có key Tag trùng với tag mình click
                MainContent.Navigate(item.Page);
            }

            //switch (selectItem.Tag)
            //{
            //    case "listSong":
            //        MainContent.Navigate(typeof(Demo.RegisterPage));
            //        break;
            //    case "register":
            //        MainContent.Navigate(typeof(Demo.Page2));
            //        break;
            //    case "login":
            //        MainContent.Navigate(typeof(Demo.Page3));
            //        break;
            //}
        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            NavView.MenuItems.Add(new NavigationViewItemSeparator());
            NavView.MenuItems.Add(new NavigationViewItem
            {
                Content = "My content",
                Icon = new SymbolIcon((Symbol)0xF1AD),
                Tag = "content"
            });
            _pages.Add(("content", typeof(Demo.Page1)));
        }
    }
}
