using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using T2012E_UWP.Service;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class SettingPage : Page
    {
        
        public SettingPage()
        {
            this.InitializeComponent();
        }

        private async void Button_LogOut(object sender, RoutedEventArgs e)
        {
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile manifestFile = await storageFolder.GetFileAsync(AccountService.TokenFileName);
                await manifestFile.DeleteAsync();
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(Login));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Có lỗi xảy ra khi logout" + ex);
            }
        }
    }
}
