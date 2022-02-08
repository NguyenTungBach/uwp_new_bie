using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using T2012E_UWP.Entity;
using T2012E_UWP.Service;
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

namespace T2012E_UWP.Demo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateSong : Windows.UI.Xaml.Controls.Page
    {
        private static string publicIDCloudinary;
        private CloudinaryDotNet.Account accountCloudinary;
        private Cloudinary cloudinary;
        private int checkValid = 0;
        public CreateSong()
        {
            this.InitializeComponent();
            this.Loaded += CreateSong_Loaded;
        }

        private void CreateSong_Loaded(object sender, RoutedEventArgs e)
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
                checkName.Visibility = Visibility.Visible;
            }
            else
            {
                checkName.Visibility = Visibility.Collapsed;
                checkValid++;
            }
            if (string.IsNullOrEmpty(Description))
            {
                checkDescription.Visibility = Visibility.Visible;
            }
            else
            {
                checkDescription.Visibility = Visibility.Collapsed;
                checkValid++;
            }
            if (string.IsNullOrEmpty(Singer))
            {
                checkSinger.Visibility = Visibility.Visible;
            }
            else
            {
                checkSinger.Visibility = Visibility.Collapsed;
                checkValid++;
            }
            if (string.IsNullOrEmpty(Author))
            {
                checkAuthor.Visibility = Visibility.Visible;
            }
            else
            {
                checkAuthor.Visibility = Visibility.Collapsed;
                checkValid++;
            }
            if (string.IsNullOrEmpty(Thumbnail))
            {
                checkThumbnail.Visibility = Visibility.Visible;
            }
            else
            {
                checkThumbnail.Visibility = Visibility.Collapsed;
                checkValid++;
            }
            if (string.IsNullOrEmpty(Link))
            {
                checkLink.Visibility = Visibility.Visible;
            }
            else
            {
                checkLink.Visibility = Visibility.Collapsed;
                checkValid++;
            }
        }

        private async void Button_CreateSong(object sender, RoutedEventArgs e)
        {
            checkValidate(name.Text, description.Text, singer.Text, author.Text, thumbnail.Text, link.Text);
            if (checkValid < 6)
            {
                return;
            }
            waitingRespone.Visibility = Visibility.Visible;
            var songCreate = new Song()
            {
                name = name.Text,
                description = description.Text,
                singer = singer.Text,
                author = author.Text,
                thumbnail = thumbnail.Text,
                link = link.Text,
            };
            
            bool result = await SongService.CreateAsync(songCreate);

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
                imageThumbnail.Source = bitmapImage;
                RawUploadParams imageUploadParams = new RawUploadParams(){ 
                    File = new FileDescription(file.Name, await file.OpenStreamForReadAsync())
                };
                RawUploadResult result = await cloudinary.UploadAsync(imageUploadParams);
                publicIDCloudinary = result.PublicId;
                thumbnail.Text = result.Url.ToString();
                createThumbnail.Visibility = Visibility.Collapsed;
                deleteThumbnail.Visibility = Visibility.Visible;
            }
            else
            {
                //ContentDialog contentDialog = new ContentDialog();
                //contentDialog.Title = "Thumbnail false!";
                //contentDialog.Content = "Tạo ảnh cho nhạc thất bại!";
                //contentDialog.CloseButtonText = "Oke";
                //await contentDialog.ShowAsync();
                Debug.WriteLine("Tạo ảnh cho nhạc thất bại!");
            }
        }

        private async void Button_DeleteThumbnail(object sender, RoutedEventArgs e)
        {
            List<string> listPublicIdCouldinary = new List<string>();
            listPublicIdCouldinary.Add(publicIDCloudinary);
            string[] arrayPublicIdCouldinary = listPublicIdCouldinary.ToArray();
            await cloudinary.DeleteResourcesAsync(arrayPublicIdCouldinary);
            deleteThumbnail.Visibility = Visibility.Collapsed;
            createThumbnail.Visibility = Visibility.Visible;
            imageThumbnail.Source = null;
            thumbnail.Text = "";
        }
    }
}
