using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UWP_Assignment.Config;
using UWP_Assignment.Entity;
using Windows.Storage;

namespace UWP_Assignment.Service
{
    public class AccountService
    {
        public static string tokenUserFile = "dataUserLogin.txt";

        // Đăng ký
        public async Task<bool> IsRegisterAsync(Account account)
        {
            var jsonString = JsonConvert.SerializeObject(account);
            HttpClient httpClient = new HttpClient();
            HttpContent contentToSend = new StringContent(jsonString, Encoding.UTF8, ApiConfig.mediaType);
            var result = await httpClient.PostAsync($"{ ApiConfig.apiDoman }{ApiConfig.accountPathRegisterAndInfo}", contentToSend);
            if (result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                // good case
                return true;
            }
            else
            {
                // bad case
                return false;
            }
        }

        // Đăng nhập
        public async Task<Credential> LoginAsync(string Email, string Password)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("email", Email);
            parameters.Add("password", Password);
            var encodedContent = new FormUrlEncodedContent(parameters);
            using (HttpClient httpClient = new HttpClient())
            {
                var result = await httpClient.PostAsync($"{ ApiConfig.apiDoman }{ApiConfig.accountPathLogin}", encodedContent);
                var content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // good case
                    SaveToken(content);
                    Credential obj = JsonConvert.DeserializeObject<Credential>(content);
                    await GetAccountInformation(obj.access_token);
                    Debug.WriteLine("tokenkey la: " + content);
                    return obj;
                }
                else
                {
                    // bad case
                    Debug.WriteLine("Error 500");
                    return null;
                }
            }
        }

        // Lưu token
        private async void SaveToken(string content)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.CreateFileAsync(tokenUserFile, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, content);
        }


        // Kiểm tra và lấy token
        private async Task<Credential> LoadToken()
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

        // lấy thông tin tài khoản
        public async Task<Account> GetAccountInformation(string token)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var result = await httpClient.GetAsync($"{ ApiConfig.apiDoman }{ApiConfig.accountPathRegisterAndInfo}");
                var content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // good case
                    Account account = JsonConvert.DeserializeObject<Account>(content);
                    App.accountUser = account;
                    return account;
                }
                else
                {
                    // bad case
                    return null;
                }
            }
        }

        // lấy tài khoản đăng nhập
        public async Task<Account> GetLoggedAccount()
        {
            Account account;
            Credential credential = await LoadToken();
            if (credential == null) // không tồn tại file token
            {
                return null;
            }
            account = await GetAccountInformation(credential.access_token);
            return account;
        }
    }
}
