using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2012E_UWP.Config
{
    class ApiConfig
    {
        public static string ApiDoman = "https://music-i-like.herokuapp.com";
        public static string AccountPath = "/api/v1/accounts";
        public static string SongPath = "/api/v1/songs";
        public static string SongMinePath = "/api/v1/songs/mine";
        public static string AccountPathLogin = "/api/v1/accounts/authentication";
        public static string MediaType = "applicaion/json";
    }
}
