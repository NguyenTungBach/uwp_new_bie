using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_Assignment.Entity;
using UWP_Assignment.Service;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
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
        int indexSong;
        MediaPlaybackList _mediaPlaybackList;
        public ListSongAllPage()
        {
            this.InitializeComponent();
            songService = new SongService();
            this.Loaded += ListSongAllPage_Loaded;
        }
       
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            MyMediaPlayer.MediaPlayer.Pause(); // Dừng bài hát khi chuyển trang
            base.OnNavigatingFrom(e);
        }

        private async void ListSongAllPage_Loaded(object sender, RoutedEventArgs e)
        {
            List<Song> listSongCheck = await songService.GetListSongAllAsync();

            _mediaPlaybackList = new MediaPlaybackList();

            // cập nhật vào _mediaPlaybackList và loại bỏ những thằng list Song bị lỗi
            for (int i = 0; i < listSongCheck.Count; i++)
            {
                try
                {
                    var mediaPlaybackItem = new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(listSongCheck[i].link)));
                    _mediaPlaybackList.Items.Add(mediaPlaybackItem);
                }
                catch (Exception ex)
                {
                    //listSongCheck.RemoveAt(i); // nếu nhạc đó lỗi thì xóa đi
                    var mediaPlaybackItemNull = new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri("about:blank")));
                    _mediaPlaybackList.Items.Add(mediaPlaybackItemNull);
                }
            }
            //// cập nhật lại danh sách nhạc vì RemoveAt chỉ xóa vị trí còn vị ở đâu thì nó vẫn như thế
            //List<Song> listSong = new List<Song>();
            //foreach(var song in listSongCheck)
            //{
            //    listSong.Add(song);
            //}
            ObservableCollection<Song> observableSongs = new ObservableCollection<Song>(listSongCheck);
            MyListSong.ItemsSource = observableSongs;

            //_mediaPlaybackList.MaxPlayedItemsToKeepOpen = 3;

            _mediaPlaybackList.CurrentItemChanged += MediaPlaybackList_CurrentItemChanged;

            var _mediaPlayer = new MediaPlayer();
            _mediaPlayer.Source = _mediaPlaybackList; // MediaPlayerElement < MediaPlayer < MediaPlaybackList < MediaPlaybackItem
            MyMediaPlayer.SetMediaPlayer(_mediaPlayer);
        }

        private async void MediaPlaybackList_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
        {
            indexSong = (int) sender.CurrentItemIndex; // Lấy vị trí
            Debug.WriteLine("Vị trí nhạc khi thay đổi: " + indexSong);
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                MyListSong.SelectedIndex = indexSong; // Mục đích cần có Dispatcher là vì vấn đề luồng, sử dụng chung tài nguyên
            });
        }

        private void MyListSong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            indexSong = MyListSong.SelectedIndex; // vị trí chọn
            //Song currentSong = MyListSong.SelectedItem as Song;
            Debug.WriteLine("Nhạc chọn: " + MyListSong.SelectedIndex);
            if(indexSong != -1)
            {
                _mediaPlaybackList.MoveTo(Convert.ToUInt32(indexSong)); // Chạy đến vị trí list cần tìm trong MediaPlayback
                MyMediaPlayer.MediaPlayer.Play();
            }
        }

    }
}
