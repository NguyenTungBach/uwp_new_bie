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
using Windows.Media.Core;
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
    public sealed partial class ListSongAllPage : Page
    {
        private SongService songService;
        public ListSongAllPage()
        {
            this.InitializeComponent();
            songService = new SongService();
            this.Loaded += ListSongAllPage_Loaded;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            MyMediaPlayer.MediaPlayer.Pause();
            base.OnNavigatingFrom(e);
        }

        private async void ListSongAllPage_Loaded(object sender, RoutedEventArgs e)
        {
            List<Song> listSong = await songService.GetListSongAllAsync();
            ObservableCollection<Song> observableSongs = new ObservableCollection<Song>(listSong);
            //Debug.WriteLine(listSong.Count);
            MyListSong.ItemsSource = observableSongs;
        }

        private void MyListSong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Song currentSong = MyListSong.SelectedItem as Song;
            MyMediaPlayer.MediaPlayer.Source = MediaSource.CreateFromUri(new Uri(currentSong.link));
            MyMediaPlayer.MediaPlayer.Play();
        }
    }
}
