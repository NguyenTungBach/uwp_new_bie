using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_SQLite.Entity
{
    public class PersonalTransaction
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public double Money { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Category { get; set; }
    }
}
