using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2012E_UWP.Entity
{
    public class Credential
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
