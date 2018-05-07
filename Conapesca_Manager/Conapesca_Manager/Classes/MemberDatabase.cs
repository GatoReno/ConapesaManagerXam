using Conapesca_Manager.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Conapesca_Manager.Classes
{
    public class MemberDatabase
    {
        private SQLiteConnection conn;


        public MemberDatabase()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<Member>();
        }

        public IEnumerable<Member> GetMembers()
        {
            var members = (from mem in conn.Table<Member>() select mem);
            return members.ToList();
        }

        public string AddMember(Member member)
        {
            try
            {
                conn.Insert(member);
                return "success baby bluye ;*";
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
           
        }

        public void DeleteMember(int ID)
        {
            conn.Delete<Member>(ID);
        }

        public void DropTbMember() {
            conn.DropTable<Member>();
        }
    }
}
