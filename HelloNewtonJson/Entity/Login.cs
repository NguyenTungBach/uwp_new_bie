using System;
using System.Collections.Generic;
using System.Text;

namespace HelloNewtonJson.Entity
{
    class Login
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string account_id { get; set; }
        public string expire_time { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string status { get; set; }
    }
}
