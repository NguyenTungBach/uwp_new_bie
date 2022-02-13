using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class CreateSongAllPage : Windows.UI.Xaml.Controls.Page
    {
        private static string publicIDCloudinary;
        private CloudinaryDotNet.Account accountCloudinary;
        private Cloudinary cloudinary;
        private int checkValid = 0;
        public CreateSongAllPage()
        {
            this.InitializeComponent();
            this.Loaded += CreateSongAllPage_Loaded;
        }

        private void CreateSongAllPage_Loaded(object sender, RoutedEventArgs e)
        {
            accountCloudinary = new CloudinaryDotNet.Account(
            "dark-faith",
            "953928119328554",
            "V2Jg1mDPkFzDX3dSX8fYPn_zvXQ"
            );
            cloudinary = new Cloudinary(accountCloudinary);
            cloudinary.Api.Secure = true;
        }

        private void checkValidate(string Name, string Description, string Singer, string Author, string Thumbnail, string Link)
        {
            if (string.IsNullOrEmpty(Name))
            {
                lblCheckName.Visibility = Visibility.Visible;
            }
            else
            {
                lblCheckName.Visibility = Visibility.Collapsed;
                checkValid++;
            }
            if (string.IsNullOrEmpty(Description))
            {
                lblCheckDescription.Visibility = Visibility.Visible;
            }
            else
            {
                lblCheckDescription.Visibility = Visibility.Collapsed;
                checkValid++;
            }
            if (string.IsNullOrEmpty(Singer))
            {
                lblCheckSinger.Visibility = Visibility.Visible;
            }
            else
            {
                lblCheckSinger.Visibility = Visibility.Collapsed;
                checkValid++;
            }
            if (string.IsNullOrEmpty(Author))
            {
                lblCheckAuthor.Visibility = Visibility.Visible;
            }
            else
            {
                lblCheckAuthor.Visibility = Visibility.Collapsed;
                checkValid++;
            }
            if (string.IsNullOrEmpty(Thumbnail))
            {
                lblCheckThumbnail.Visibility = Visibility.Visible;
            }
            else
            {
                lblCheckThumbnail.Visibility = Visibility.Collapsed;
                checkValid++;
            }
            if (string.IsNullOrEmpty(Link))
            {
                lblCheckLink.Visibility = Visibility.Visible;
            }
            else
            {
                lblCheckLink.Visibility = Visibility.Collapsed;
                checkValid++;
            }
        }

        private async void Button_CreateSong(object sender, RoutedEventArgs e)
        {
            checkValidate(txtName.Text, txtDescription.Text, txtSinger.Text, txtAuthor.Text, txtThumbnail.Text, txtLink.Text);
            if (checkValid < 6)
            {
                return;
            }
            waitingRespone.Visibility = Visibility.Visible;
            var songCreate = new Song()
            {
                name = txtName.Text,
                description = txtDescription.Text,
                singer = txtSinger.Text,
                author = txtAuthor.Text,
                thumbnail = txtThumbnail.Text,
                link = txtLink.Text,
            };

            bool result = await SongService.IsCreateSongAllAsync(songCreate);

            ContentDialog contentDialog = new ContentDialog();
            waitingRespone.Visibility = Visibility.Collapsed;
            if (result)
            {
                contentDialog.Title = "Login success, this is your access_token!";
                contentDialog.Content = "Tạo nhạc mới thành công";
            }
            else
            {
                contentDialog.Title = "Login false!";
                contentDialog.Content = "Tạo nhạc mới thất bại!";
            }
            contentDialog.CloseButtonText = "Oke";
            await contentDialog.ShowAsync();
        }

        private async void Button_CreateThumbnail(object sender, RoutedEventArgs e)
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
                BitmapImage bitmapImage = new BitmapImage();
                IRandomAccessStream fileStream = await file.OpenReadAsync();
                await bitmapImage.SetSourceAsync(fileStream);
                imgThumbnail.Source = bitmapImage;
                RawUploadParams imageUploadParams = new RawUploadParams()
                {
                    File = new FileDescription(file.Name, await file.OpenStreamForReadAsync())
                };
                RawUploadResult result = await cloudinary.UploadAsync(imageUploadParams);
                publicIDCloudinary = result.PublicId;
                txtThumbnail.Text = result.Url.ToString();
                btnCreateThumbnail.Visibility = Visibility.Collapsed;
                btnDeleteThumbnail.Visibility = Visibility.Visible;
            }
            else
            {

                Debug.WriteLine("Tạo ảnh cho nhạc thất bại!");
            }
        }

        private async void Button_DeleteThumbnail(object sender, RoutedEventArgs e)
        {
            List<string> listPublicIdCouldinary = new List<string>();
            listPublicIdCouldinary.Add(publicIDCloudinary);
            string[] arrayPublicIdCouldinary = listPublicIdCouldinary.ToArray();
            await cloudinary.DeleteResourcesAsync(arrayPublicIdCouldinary);
            btnDeleteThumbnail.Visibility = Visibility.Collapsed;
            btnCreateThumbnail.Visibility = Visibility.Visible;
            imgThumbnail.Source = null;
            txtThumbnail.Text = "";
        }
    }
}
