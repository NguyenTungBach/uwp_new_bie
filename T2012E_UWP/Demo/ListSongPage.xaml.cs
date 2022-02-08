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
using Windows.Media.Core;
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
    public sealed partial class ListSongPage : Page
    {
        private SongService songService;
        public ListSongPage()
        {
            this.InitializeComponent();
            songService = new SongService();
            Loaded += ListSongPage_Loaded;
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

        private async void ListSongPage_Loaded(object sender, RoutedEventArgs e)
        {
            List<Song> listSong = await songService.GetListAsync();
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
