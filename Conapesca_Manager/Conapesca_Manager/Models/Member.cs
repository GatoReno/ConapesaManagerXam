using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Conapesca_Manager.Models
{
    public class Member
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
        public string Firma { get; set; }
        public string Access_Token { set; get; }
        public string Token_Type { set; get; }
        public int AdminType { set; get; }
    }
}
