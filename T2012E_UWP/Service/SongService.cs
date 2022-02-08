using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using T2012E_UWP.Config;
using T2012E_UWP.Entity;
using Windows.Storage;

namespace T2012E_UWP.Service
{
    class SongService
    {
        private const string TokenFileName = "dataUserLogin.txt";
        // Kiểm tra và lấy token
        private async Task<Credential> LoadToken()
        {
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await storageFolder.GetFileAsync(TokenFileName);

                string token = await FileIO.ReadTextAsync(sampleFile);
                Credential credential = JsonConvert.DeserializeObject<Credential>(token);
                return credential;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<bool> CreateAsync(Song song)
        {
            var jsonString = JsonConvert.SerializeObject(song);
            
            HttpClient httpClient = new HttpClient();

            HttpContent contentToSend = new StringContent(jsonString, Encoding.UTF8, ApiConfig.MediaType);
            var result = await httpClient.PostAsync($"{ ApiConfig.ApiDoman }{ApiConfig.SongMinePath}", contentToSend);
            if (result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Song>> GetListAsync()
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

                var result = await httpClient.GetAsync($"{ ApiConfig.ApiDoman }{ApiConfig.SongMinePath}");
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
