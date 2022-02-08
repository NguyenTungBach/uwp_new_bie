using HelloNewtonJson.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HelloNewtonJson
{
    class Program
    {
        private static string ApiUrl = "https://music-i-like.herokuapp.com/api/v1/accounts";
        private static string ApiUrlLogin = "https://music-i-like.herokuapp.com/api/v1/accounts/authentication";
        private static string ApiUrlSongMine = "https://music-i-like.herokuapp.com/api/v1/songs/mine";
        public static string Login_user_access_token;

        static void Main(string[] args)
        {
            //var account = new Account()
            //{
            //    firstName = "Nguyễn Tùng",
            //    lastName = "Bách",
            //    password = "123",
            //    address = "Ha Noi",
            //    phone = "0912345678",
            //    avatar = "https://trumboss.com/wp-content/uploads/2018/10/xa-hoi-hoa-meo-con-696x530.jpg",
            //    gender = 1,
            //    email = "bach@gmail.com",
            //    birthday = "1998-06-02",
            //};
            //Process(account);

            //Login("bach@gmail.com", "123");

            Information("YIUvb37RjtHdBiqjoGLm9UsSvzb2LTU0riRFwAOuCbGyLLtJ7VSnJfh8m1mHgkZagTwyiDvF0rGLXw3N1642502770");

            //var songCreate = new Song()
            //{
            //    name = "Beggin",
            //    description = "Nhạc dancer chất lừ",
            //    singer = "Mneskin",
            //    author = "Mneskin (chắc thế)",
            //    thumbnail = "https://res.cloudinary.com/dark-faith/image/upload/v1620756838/kz0nezuwj31eis6woch5.jpg",
            //    link = "https://res.cloudinary.com/dark-faith/video/upload/v1643104551/Beggin-Mneskin-5305611_tjvvdh.mp3",
            //};

            //Create_mine_song("YIUvb37RjtHdBiqjoGLm9UsSvzb2LTU0riRFwAOuCbGyLLtJ7VSnJfh8m1mHgkZagTwyiDvF0rGLXw3N1642502770", songCreate);

            //GetList("YIUvb37RjtHdBiqjoGLm9UsSvzb2LTU0riRFwAOuCbGyLLtJ7VSnJfh8m1mHgkZagTwyiDvF0rGLXw3N1642502770");

            Console.ReadLine();
        }

        private static async void Information(string Access_token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Access_token);
            
            
             var result = await httpClient.GetAsync(ApiUrl);
            
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // good case
                var content = await result.Content.ReadAsStringAsync();
                //Login_user = JsonConvert.DeserializeObject<Login> (content);
                Console.WriteLine("Thong tin dang nhap");
                Console.WriteLine(content);
            }
            else
            {
                // bad case
                Console.WriteLine("Co loi xay ra");
            }
        }

        private static async void Login(string Email, string Password)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("email", Email);
            parameters.Add("password", Password);

            var encodedContent = new FormUrlEncodedContent(parameters);

            HttpClient httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HttpContent contentToSend = new StringContent(encodedContent, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync(ApiUrlLogin, encodedContent);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // good case
                var content = await result.Content.ReadAsStringAsync();
                var Login_user = JsonConvert.DeserializeObject<Login>(content);
                Login_user_access_token = Login_user.access_token;
                Console.WriteLine(content);
            }
            else
            {
                // bad case
                Console.WriteLine("Co loi xay ra");
            }
        }

        public static async void Process(Account account)
        {
            var jsonString = JsonConvert.SerializeObject(account);
            Console.WriteLine("Json Serialize Object");
            Console.WriteLine(jsonString);
            //Console.WriteLine("Json Deserialize Object");
            //Account obj = JsonConvert.DeserializeObject <Account>(jsonString);
            //Console.WriteLine(obj.email);
            HttpClient httpClient = new HttpClient();
            //httpClient.BaseAddress = new Uri(ApiUrl);

            HttpContent contentToSend = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync(ApiUrl, contentToSend);
            if(result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                // good case
                var content = await result.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            else
            {
                // bad case
                Console.WriteLine("Co loi xay ra hoac email da dang ky, Error 500");
            }
        }

        private async static void Create_mine_song(string Access_token, Song songCreate)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Access_token);


            var jsonString = JsonConvert.SerializeObject(songCreate);
            HttpContent contentToSend = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync(ApiUrlSongMine, contentToSend);
            if (result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                // good case
                var content = await result.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            else
            {
                // bad case
                Console.WriteLine("Co loi xay ra hoac email da dang ky, Error 500");
            }
        }

        public async static void GetList(string token)
        {
            List<Song> listSong = new List<Song>();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var result = await httpClient.GetAsync("https://music-i-like.herokuapp.com/api/v1/songs/mine");
                var content = await result.Content.ReadAsStringAsync();

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // good case
                    listSong = JsonConvert.DeserializeObject<List<Song>>(content);
                    Console.WriteLine(listSong[0].author);
                }
                else
                {
                    // bad case
                    Console.WriteLine("Error 500");
                    Console.WriteLine(listSong);
                }
            }
            
        }
    }
}
