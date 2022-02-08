using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace T2012E_UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Link_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var navigationViewItem = sender as NavigationViewItem;
            //Debug.WriteLine(navigationViewItem.Tag);
            switch (navigationViewItem.Tag)
            {
                case "TestPage1":
                    var myView = CoreApplication.CreateNewView();

                    int newViewId = 0;

                    await myView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, ()=> {
                        Frame newFrame = new Frame();
                        newFrame.Navigate(typeof(Demo.Page1),null);

                        Window.Current.Content = newFrame;
                        Window.Current.Activate();

                        newViewId = ApplicationView.GetForCurrentView().Id;
                    });
                    await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId,ViewSizePreference.UseMinimum);
                    break;
                case "TestPage2":
                    contentFrame.Navigate(typeof(Demo.Page2));
                    break;
                case "TestPage3":
                    contentFrame.Navigate(typeof(Demo.Page3));
                    break;
            }
        }
    }
}
