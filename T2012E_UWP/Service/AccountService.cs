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
    public class AccountService
    {
        public static string TokenFileName = "dataUserLogin.txt";
        public async Task<bool> RegisterAsync(Account account)
        {
            var jsonString = JsonConvert.SerializeObject(account);
            Console.WriteLine("Json Serialize Object");
            Console.WriteLine(jsonString);
            //Console.WriteLine("Json Deserialize Object");
            //Account obj = JsonConvert.DeserializeObject <Account>(jsonString);
            //Console.WriteLine(obj.email);
            HttpClient httpClient = new HttpClient();
            //httpClient.BaseAddress = new Uri(ApiUrl);

            HttpContent contentToSend = new StringContent(jsonString, Encoding.UTF8, ApiConfig.MediaType);
            var result = await httpClient.PostAsync($"{ ApiConfig.ApiDoman }{ApiConfig.AccountPath}", contentToSend);
            if (result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                // good case
                //var content = await result.Content.ReadAsStringAsync();
                //Console.WriteLine(content);
                return true;
            }
            else
            {
                // bad case
                //Console.WriteLine("Co loi xay ra hoac email da dang ky, Error 500");
                return false;
            }
        }

        public async Task<Credential> LoginAsync(string Email, string Password)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("email", Email);
            parameters.Add("password", Password);

            var encodedContent = new FormUrlEncodedContent(parameters);
            using (HttpClient httpClient = new HttpClient())
            {
                var result = await httpClient.PostAsync($"{ ApiConfig.ApiDoman }{ApiConfig.AccountPathLogin}", encodedContent);
                var content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // good case
                    SaveToken(content);
                    Credential obj = JsonConvert.DeserializeObject<Credential>(content);
                    return obj;
                }
                else
                {
                    // bad case
                    //Console.WriteLine("Co loi xay ra");
                    Debug.WriteLine("Error 500");
                    return null;
                }
            }
            return null;
        }

        // Lưu token
        private async void SaveToken(string content)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.CreateFileAsync(TokenFileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, content);
        }


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

        // lấy thông tin tài khoản
        private async Task<Account> GetAccountInformation(string token)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var result = await httpClient.GetAsync($"{ ApiConfig.ApiDoman }{ApiConfig.AccountPath}");
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
