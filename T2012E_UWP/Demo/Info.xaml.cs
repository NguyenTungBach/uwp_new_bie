using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using T2012E_UWP.Entity;
using T2012E_UWP.Service;
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
    public sealed partial class Info : Page
    {
        public Info()
        {
            this.InitializeComponent();
            Loaded += Info_Loaded;
        }

        private void Info_Loaded(object sender, RoutedEventArgs e)
        {
            List<Account> accountList = new List<Account>();
            accountList.Add(App.accountUser);
            ObservableCollection<Account> observableAccount = new ObservableCollection<Account>(accountList);
            
            MyInfo.ItemsSource = observableAccount;
        }
    }
}
