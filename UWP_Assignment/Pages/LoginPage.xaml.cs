using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP_Assignment.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private AccountService accountService = new AccountService();
        private int countValid = 0;
        public LoginPage()
        {
            this.InitializeComponent();
        }
        private void checkValidate(string Email, string Password)
        {
            if (string.IsNullOrEmpty(Email))
            {
                checkEmail.Visibility = Visibility.Visible;
            }
            else
            {
                checkEmail.Visibility = Visibility.Collapsed;
                countValid++;
            }
            if (string.IsNullOrEmpty(Password))
            {
                checkPassword.Visibility = Visibility.Visible;
            }
            else
            {
                checkPassword.Visibility = Visibility.Collapsed;
                countValid++;
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Pages.RegisterPage));
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            checkValidate(email.Text, password.Password.ToString());
            if (countValid < 2)
            {
                return;
            }
            waitingRespone.Visibility = Visibility.Visible;
            Credential result = await accountService.LoginAsync(email.Text, password.Password.ToString());
            ContentDialog contentDialog = new ContentDialog();
            waitingRespone.Visibility = Visibility.Collapsed;
            if (result != null)
            {
                //contentDialog.Title = "Login success, this is your access_token!";
                //contentDialog.Content = result.access_token;
                Frame.Navigate(typeof(Pages.NavigationViewPage));
            }
            else
            {
                contentDialog.Title = "Login false!";
                contentDialog.Content = "Đăng nhập thất bại!";
            }
            contentDialog.CloseButtonText = "Oke";
            await contentDialog.ShowAsync();
        }
    }
}
