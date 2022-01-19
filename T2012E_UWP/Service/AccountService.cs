using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using T2012E_UWP.Config;
using T2012E_UWP.Entity;

namespace T2012E_UWP.Service
{
    class AccountService
    {
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

        public async Task<Login> LoginAsync(string Email, string Password)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("email", Email);
            parameters.Add("password", Password);

            var encodedContent = new FormUrlEncodedContent(parameters);

            HttpClient httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HttpContent contentToSend = new StringContent(encodedContent, Encoding.UTF8, "applicaion/json");
            var result = await httpClient.PostAsync($"{ ApiConfig.ApiDoman }{ApiConfig.AccountPathAuthentication}", encodedContent);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // good case
                var content = await result.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<Login>(content);
                return obj;
            }
            else
            {
                // bad case
                //Console.WriteLine("Co loi xay ra");
                return null;
            }
        }
    }
}
