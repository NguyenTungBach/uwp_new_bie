using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
    public sealed partial class RegisterPage : Page
    {
        private int checkGenderInt;
        private string dateChanged;
        private int checkValid = 0;
        private AccountService accountService = new AccountService();
        public RegisterPage()
        {
            this.InitializeComponent();
        }

        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(84|0[3|5|7|8|9])+([0-9]{8})$").Success;

        }

        public static bool IsEmail(string email)
        {
            string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            return Regex.IsMatch(email, pattern);
        }

        private void HandleCheck(object sender, RoutedEventArgs e)
        {
            var check = sender as RadioButton;
            switch (check.Content)
            {
                case "Male":
                    checkGenderInt = 1;
                    break;
                case "Fermale":
                    checkGenderInt = 2;
                    break;
                case "Other":
                    checkGenderInt = 3;
                    break;
            }
        }

        private void Birthday_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var date = sender;
            dateChanged = date.Date.Value.ToString("yyyy-MM-dd");
        }

        private void checkValidate(string FirstName, string LastName, string Email, string Password, string Phone, string Address, int CheckGenderInt, string Avatar, string dateChanged, string Introduction)
        {
            checkValid = 0;
            if (string.IsNullOrEmpty(FirstName))
            {
                checkFirstName.Visibility = Visibility.Visible;
            }
            else
            {
                checkFirstName.Visibility = Visibility.Collapsed;
                checkValid++;
            }
            if (string.IsNullOrEmpty(LastName))
            {
                checkLastName.Visibility = Visibility.Visible;
            }
            else
            {
                checkLastName.Visibility = Visibility.Collapsed;
                checkValid++;
            }
            if (string.IsNullOrEmpty(Email))
            {
                checkEmail.Visibility = Visibility.Visible;
            }
            else
            {
                if (!IsEmail(Email))
                {
                    checkEmail.Text = "Đây không phải là email";
                    checkEmail.Visibility = Visibility.Visible;
                }
                else
                {
                    checkEmail.Visibility = Visibility.Collapsed;
                    checkValid++;
                }
            }
            if (string.IsNullOrEmpty(Password))
            {
                checkPassword.Visibility = Visibility.Visible;
            }
            else
            {
                checkPassword.Visibility = Visibility.Collapsed;
                checkValid++;
            }
            if (string.IsNullOrEmpty(Phone))
            {
                checkPhone.Visibility = Visibility.Visible;
            }
            else
            {
                if (!IsPhoneNumber(Phone))
                {
                    checkPhone.Text = "Đây không phải là số điện thoại";
                    checkPhone.Visibility = Visibility.Visible;
                }
                else
                {
                    checkPhone.Visibility = Visibility.Collapsed;
                    checkValid++;
                }
            }
            if (string.IsNullOrEmpty(Avatar))
            {
                checkAvatar.Visibility = Visibility.Visible;
            }
            else
            {
                checkAvatar.Visibility = Visibility.Collapsed;
                checkValid++;
            }
            if (CheckGenderInt == 0 || CheckGenderInt == null)
            {
                checkGender.Visibility = Visibility.Visible;
            }
            else
            {
                checkGender.Visibility = Visibility.Collapsed;
                checkValid++;
            }
            if (string.IsNullOrEmpty(Address))
            {
                checkAddress.Visibility = Visibility.Visible;
            }
            else
            {
                checkAddress.Visibility = Visibility.Collapsed;
                checkValid++;
            }
            if (string.IsNullOrEmpty(dateChanged))
            {
                checkBirthday.Visibility = Visibility.Visible;
            }
            else
            {
                checkBirthday.Visibility = Visibility.Collapsed;
                checkValid++;
            }
            if (string.IsNullOrEmpty(Introduction))
            {
                checkIntroduction.Visibility = Visibility.Visible;
            }
            else
            {
                checkIntroduction.Visibility = Visibility.Collapsed;
                checkValid++;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine(checkGenderInt);
            checkValidate(firstName.Text, lastName.Text, email.Text, password.Password.ToString(), phone.Text, address.Text, checkGenderInt, avatar.Text, dateChanged, introduction.Text);
            if(checkValid < 10)
            {
                return;
            }
            waitingRespone.Visibility = Visibility.Visible;
            var account = new Account()
            {
                firstName = firstName.Text,
                lastName = lastName.Text,
                email = email.Text,
                phone = phone.Text,
                password = password.Password,
                address = address.Text,
                gender = checkGenderInt,
                avatar = avatar.Text,
                birthday = dateChanged,
                introduction= introduction.Text,
            };
            var result = await accountService.RegisterAsync(account);
            waitingRespone.Visibility = Visibility.Collapsed;
            ContentDialog contentDialog = new ContentDialog();
            if (result){
                contentDialog.Title = "Acction success!";
                contentDialog.Content = "Tạo tài khoản thành công!";
            }
            else{
                contentDialog.Title = "Acction false!";
                contentDialog.Content = "Tạo tài khoản thất bại!";
            }
            contentDialog.CloseButtonText = "Oke";
            await contentDialog.ShowAsync();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Demo.Login));
        }
    }
}
