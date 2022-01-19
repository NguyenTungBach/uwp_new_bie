using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Lab2
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

        private async void RedFile_Click(object sender, RoutedEventArgs e)
        {
            HanderRedFile_Click();
        }

        private async void HanderRedFile_Click()
        {
            // thư mục cần lưu
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Downloads;
            picker.FileTypeFilter.Add(".txt");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                 var stringContent = await FileIO.ReadTextAsync(file);
                editor.Text = stringContent;

                // Application now has read/write access to the picked file
                //Debug.WriteLine("Picker file: " + file.Name);
            }
            else
            {
                Debug.WriteLine("Operation cancelled.");

            }
        }

        private async void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            HanderSaveFile_Click();
        }

        private async void HanderSaveFile_Click()
        {
            if(editor.Text == "")
            {
                checkEditor.Visibility = Visibility.Visible;
                return;
            }
            checkEditor.Visibility = Visibility.Collapsed;

            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Downloads;

            // Kiểu file cần luu
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            // Default file name
            savePicker.SuggestedFileName = "New Document";

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            ContentDialog contentDialog = new ContentDialog();
            if (file != null)
            {
                await FileIO.WriteTextAsync(file, editor.Text);
                contentDialog.Title = "Acction success";
                contentDialog.Content = "Save file success";
                
                // Application now has read/write access to the picked file
                //Debug.WriteLine("Picker file: " + file.Name);
            }
            else
            {
                contentDialog.Title = "Acction false";
                contentDialog.Content = "Save file false, please chose file save";
                
                //Debug.WriteLine("Operation cancelled.");
            }
            contentDialog.CloseButtonText = "OK";
            await contentDialog.ShowAsync();
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuFlyoutItem;
            switch (menuItem.Tag)
            {
                case "open":
                    HanderRedFile_Click();
                    break;
                case "save":
                    HanderSaveFile_Click();
                    break;
            }
        }

        private void SaveFileLocalFolder_Click(object sender, RoutedEventArgs e)
        {
            HanderSaveFileLocalFolder_Click();
        }

        private async void HanderSaveFileLocalFolder_Click()
        {
            if (editor.Text == "")
            {
                checkEditor.Visibility = Visibility.Visible;
                return;
            }
            checkEditor.Visibility = Visibility.Collapsed;

            // Create sample file; replace if exists.
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.CreateFileAsync("test.txt", CreationCollisionOption.GenerateUniqueName);

            ContentDialog contentDialog = new ContentDialog();
            if (sampleFile != null)
            {
                await FileIO.WriteTextAsync(sampleFile, editor.Text);
                contentDialog.Title = "Acction success";
                contentDialog.Content = "Save file Local Folder success";

                // Application now has read/write access to the picked file
                //Debug.WriteLine("Picker file: " + file.Name);
            }
            else
            {
                //contentDialog.Title = "Acction false";
                //contentDialog.Content = "Save file Local Folder false, please chose file save";

                Debug.WriteLine("Operation cancelled.");
            }
            contentDialog.CloseButtonText = "OK";
            await contentDialog.ShowAsync();
        }
    }
}
