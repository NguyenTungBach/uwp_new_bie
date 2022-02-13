using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UWP_Assignment.Config;
using UWP_Assignment.Entity;
using Windows.Storage;

namespace UWP_Assignment.Service
{
    public class SongService
    {
        private const string tokenUserFile = "dataUserLogin.txt"; // lấy token
        // Kiểm tra và lấy token
        public static async Task<Credential> LoadToken()
        {
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await storageFolder.GetFileAsync(tokenUserFile);
                string token = await FileIO.ReadTextAsync(sampleFile);
                Credential credential = JsonConvert.DeserializeObject<Credential>(token);
                return credential;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // 1. Mọi người
        // 1.1 Tạo bài hát chung
        public static async Task<bool> IsCreateSongAllAsync(Song song)
        {
            var jsonString = JsonConvert.SerializeObject(song);

            HttpClient httpClient = new HttpClient();

            HttpContent contentToSend = new StringContent(jsonString, Encoding.UTF8, ApiConfig.mediaType);
            var result = await httpClient.PostAsync($"{ ApiConfig.apiDoman }{ApiConfig.songPathCreateAndListInfoAll}", contentToSend);
            if (result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // 1.2 Lấy danh sách bài hát chung
        public async Task<List<Song>> GetListSongAllAsync()
        {
            List<Song> listSong = new List<Song>();
            Credential credential = await LoadToken();
            if (credential == null) // không tồn tại file token
            {
                return listSong;
            }
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credential.access_token);
                var result = await httpClient.GetAsync($"{ ApiConfig.apiDoman }{ApiConfig.songPathCreateAndListInfoAll}");
                var content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // good case
                    listSong = JsonConvert.DeserializeObject<List<Song>>(content);
                }
                else
                {
                    // bad case
                    Debug.WriteLine("Error 500");
                }
            }
            return listSong;
        }

        // 2. Mọi người
        // 2.1 Tạo bài hát riêng
        public static async Task<bool> IsCreateSongMineAsync(Song song)
        {
            Credential credential = await LoadToken();
            var jsonString = JsonConvert.SerializeObject(song);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credential.access_token);
            HttpContent contentToSend = new StringContent(jsonString, Encoding.UTF8, ApiConfig.mediaType);
            var result = await httpClient.PostAsync($"{ ApiConfig.apiDoman }{ApiConfig.songPathCreateAndListInfoMine}", contentToSend);
            if (result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // 2.2 Lấy danh sách bài hát riêng
        public async Task<List<Song>> GetListSongMineAsync()
        {
            List<Song> listSong = new List<Song>();
            Credential credential = await LoadToken();
            if (credential == null) // không tồn tại file token
            {
                return listSong;
            }
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credential.access_token);
                var result = await httpClient.GetAsync($"{ ApiConfig.apiDoman }{ApiConfig.songPathCreateAndListInfoMine}");
                var content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // good case
                    listSong = JsonConvert.DeserializeObject<List<Song>>(content);
                }
                else
                {
                    // bad case
                    Debug.WriteLine("Error 500");
                }
            }
            return listSong;
        }
    }
}
