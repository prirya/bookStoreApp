using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookStoreApp
{
    public class UserModel
    {
        public int Number { get; set; }
        public string userId { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public DateTime birthday { get; set; }
        public bool sex { get; set; }
        public bool typeAdmin { get; set; }
    }
    public class CustomerModel
    {
        public string CustomerId { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public DateTime birthday { get; set; }
        public bool sex { get; set; }
        public int phone { get; set; }
    }

}
