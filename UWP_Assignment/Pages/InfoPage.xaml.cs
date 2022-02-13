using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_Assignment.Entity;
using UWP_Assignment.Service;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP_Assignment.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InfoPage : Page
    {
        
        public InfoPage()
        {
            this.InitializeComponent();
            this.Loaded += InfoPage_Loaded;
            
        }

        private void InfoPage_Loaded(object sender, RoutedEventArgs e)
        {
            Account account = App.accountUser;
            txtFirstName.Text = account.firstName;
            txtLastName.Text = account.lastName.ToString();
            txtEmail.Text = account.email.ToString();
            txtAddress.Text = account.address.ToString();
            txtPhone.Text = account.phone.ToString();
            BitmapImage bitmapImageAvatar = new BitmapImage(new Uri(account.avatar.ToString()));
            txtAvatar.Source = bitmapImageAvatar;
            txtGender.Text = account.gender.ToString();
            txtBirthday.Text = account.birthday.ToString();
            txtStatus.Text = account.status.ToString();
            txtIntroduction.Text = account.introduction ?? "";
        }
    }
}
