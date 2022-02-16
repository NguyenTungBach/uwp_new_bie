using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using UWP_Assignment.Entity;
using UWP_Assignment.Service;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
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
    public sealed partial class RegisterPage : Windows.UI.Xaml.Controls.Page
    {
        private static string publicIDAvatarCloudinary;
        private CloudinaryDotNet.Account accountCloudinary;
        private Cloudinary cloudinary;
        private int checkGenderInt;
        private string dateChanged;
        private int countValid = 0;
        private AccountService accountService = new AccountService();
        public RegisterPage()
        {
            this.InitializeComponent();
            this.Loaded += RegisterPage_Loaded;
        }

        private void RegisterPage_Loaded(object sender, RoutedEventArgs e)
        {
            accountCloudinary = new CloudinaryDotNet.Account(
            "dark-faith",
            "953928119328554",
            "V2Jg1mDPkFzDX3dSX8fYPn_zvXQ"
            );
            cloudinary = new Cloudinary(accountCloudinary);
            cloudinary.Api.Secure = true;
        }
        // Kiểm tra số điện thoại
        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(84|0[3|5|7|8|9])+([0-9]{8})$").Success;

        }

        // Kiểm tra email
        public static bool IsEmail(string email)
        {
            string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            return Regex.IsMatch(email, pattern);
        }

        // Tạo ảnh đại diện
        private async void btnCreateAvatar_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                BitmapImage bitmapImageAvatar = new BitmapImage();
                IRandomAccessStream fileStream = await file.OpenReadAsync();
                await bitmapImageAvatar.SetSourceAsync(fileStream);
                imgAvatar.Source = bitmapImageAvatar;
                RawUploadParams imageUploadParams = new RawUploadParams()
                {
                    File = new FileDescription(file.Name, await file.OpenStreamForReadAsync())
                };
                lblCheckAvatar.Visibility = Visibility.Visible;
                lblCheckAvatar.Text = "Xin hãy chờ để cập nhật ảnh";
                RawUploadResult result = await cloudinary.UploadAsync(imageUploadParams);
                lblCheckAvatar.Visibility = Visibility.Collapsed;
                lblCheckAvatar.Text = "Hãy chọn ảnh hoặc gán link ảnh";
                publicIDAvatarCloudinary = result.PublicId;
                txtAvatar.Text = result.Url.ToString();
                btnCreateAvatar.Visibility = Visibility.Collapsed;
                btnDeleteAvatar.Visibility = Visibility.Visible;
            }
            else
            {
                Debug.WriteLine("Tạo ảnh cho nhạc thất bại!");
            }
        }

        // Xóa đổi ảnh
        private async void btnDeleteAvatar_Click(object sender, RoutedEventArgs e)
        {
            List<string> listPublicIdCouldinary = new List<string>();
            listPublicIdCouldinary.Add(publicIDAvatarCloudinary);
            string[] arrayPublicIdCouldinary = listPublicIdCouldinary.ToArray();
            await cloudinary.DeleteResourcesAsync(arrayPublicIdCouldinary);
            btnDeleteAvatar.Visibility = Visibility.Collapsed;
            btnCreateAvatar.Visibility = Visibility.Visible;
            imgAvatar.Source = null;
            txtAvatar.Text = "";
        }

        private void HandleCheck(object sender, RoutedEventArgs e)
        {
            var check = sender as RadioButton;
            switch (check.Content)
            {
                case "Nam":
                    checkGenderInt = 1;
                    break;
                case "Nữ":
                    checkGenderInt = 2;
                    break;
                case "Khác":
                    checkGenderInt = 3;
                    break;
            }
        }

        // Lấy ngày sinh
        private void dtmBirthday_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var date = sender;
            dateChanged = date.Date.Value.ToString("yyyy-MM-dd");
        }

        private void checkValidate(string FirstName, string LastName, string Email, string Password, string Phone, string Address, int CheckGenderInt, string Avatar, string dateChanged, string Introduction)
        {
            countValid = 0;
            if (string.IsNullOrEmpty(FirstName))
            {
                lblCheckFirstName.Visibility = Visibility.Visible;
            }
            else
            {
                lblCheckFirstName.Visibility = Visibility.Collapsed;
                countValid++;
            }
            if (string.IsNullOrEmpty(LastName))
            {
                lblCheckLastName.Visibility = Visibility.Visible;
            }
            else
            {
                lblCheckLastName.Visibility = Visibility.Collapsed;
                countValid++;
            }
            if (string.IsNullOrEmpty(Email))
            {
                lblCheckEmail.Visibility = Visibility.Visible;
            }
            else
            {
                if (!IsEmail(Email))
                {
                    lblCheckEmail.Text = "Đây không phải là email";
                    lblCheckEmail.Visibility = Visibility.Visible;
                }
                else
                {
                    lblCheckEmail.Visibility = Visibility.Collapsed;
                    lblCheckEmail.Text = "Hãy nhập email";
                    countValid++;
                }
            }
            if (string.IsNullOrEmpty(Password))
            {
                lblCheckPassword.Visibility = Visibility.Visible;
            }
            else
            {
                lblCheckPassword.Visibility = Visibility.Collapsed;
                countValid++;
            }
            if (string.IsNullOrEmpty(Phone))
            {
                lblCheckPhone.Visibility = Visibility.Visible;
            }
            else
            {
                if (!IsPhoneNumber(Phone))
                {
                    lblCheckPhone.Text = "Đây không phải là số điện thoại";
                    lblCheckPhone.Visibility = Visibility.Visible;
                }
                else
                {
                    lblCheckPhone.Visibility = Visibility.Collapsed;
                    countValid++;
                }
            }
            if (string.IsNullOrEmpty(Avatar))
            {
                lblCheckAvatar.Visibility = Visibility.Visible;
            }
            else
            {
                lblCheckAvatar.Visibility = Visibility.Collapsed;
                countValid++;
            }
            if (CheckGenderInt == 0 || CheckGenderInt == null)
            {
                checkGender.Visibility = Visibility.Visible;
            }
            else
            {
                checkGender.Visibility = Visibility.Collapsed;
                countValid++;
            }
            if (string.IsNullOrEmpty(Address))
            {
               lblCheckAddress.Visibility = Visibility.Visible;
            }
            else
            {
                lblCheckAddress.Visibility = Visibility.Collapsed;
                countValid++;
            }
            if (string.IsNullOrEmpty(dateChanged))
            {
                lblCheckBirthday.Visibility = Visibility.Visible;
            }
            else
            {
                lblCheckBirthday.Visibility = Visibility.Collapsed;
                countValid++;
            }
            if (string.IsNullOrEmpty(Introduction))
            {
                lblCheckIntroduction.Visibility = Visibility.Visible;
            }
            else
            {
                lblCheckIntroduction.Visibility = Visibility.Collapsed;
                countValid++;
            }
        }

        // Đăng ký tài khoản
        private async void RegisterAccount_Click(object sender, RoutedEventArgs e)
        {
            checkValidate(txtFirstName.Text, txtLastName.Text, txtEmail.Text, txtPassword.Password.ToString(), txtPhone.Text, txtAddress.Text, checkGenderInt, txtAvatar.Text, dateChanged, txtIntroduction.Text);
            if (countValid < 10)
            {
                return;
            }
            waitingRespone.Visibility = Visibility.Visible;
            var account = new Entity.Account()
            {
                firstName = txtFirstName.Text,
                lastName = txtLastName.Text,
                email = txtEmail.Text,
                phone = txtPhone.Text,
                password = txtPassword.Password.ToString(),
                address = txtAddress.Text,
                gender = checkGenderInt,
                avatar = txtAvatar.Text,
                birthday = dateChanged,
                introduction = txtIntroduction.Text,
            };
            var result = await accountService.IsRegisterAsync(account);
            waitingRespone.Visibility = Visibility.Collapsed;
            ContentDialog contentDialog = new ContentDialog();
            if (result)
            {
                contentDialog.Title = "Acction success!";
                contentDialog.Content = "Tạo tài khoản thành công!";
            }
            else
            {
                contentDialog.Title = "Acction false!";
                contentDialog.Content = "Tạo tài khoản thất bại!";
            }
            contentDialog.CloseButtonText = "Oke";
            await contentDialog.ShowAsync();
        }

        private void ReturnLogin_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Pages.LoginPage));
        }
    }
}
