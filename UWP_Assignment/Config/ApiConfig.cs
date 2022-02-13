using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Assignment.Config
{
    public class ApiConfig
    {
        public static string mediaType = "applicaion/json";
        public static string apiDoman = "https://music-i-like.herokuapp.com";
        // Account
        public static string accountPathRegisterAndInfo = "/api/v1/accounts";
        public static string accountPathLogin = "/api/v1/accounts/authentication";
        // Song
        public static string songPathCreateAndListInfoAll = "/api/v1/songs";
        public static string songPathCreateAndListInfoMine = "/api/v1/songs/mine";
        
    }
}
