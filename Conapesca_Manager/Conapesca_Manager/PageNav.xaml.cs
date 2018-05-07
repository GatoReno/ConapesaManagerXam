using Conapesca_Manager.Classes;
using Conapesca_Manager.Models;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Conapesca_Manager
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageNav : ContentPage
    {
        public Member member;
        public SQLiteConnection conn;

        public MemberDatabase memberDatabase;
        public PageNav ()
		{
			InitializeComponent ();
            memberDatabase = new MemberDatabase();
            var members = memberDatabase.GetMembers();
            var member = members.FirstOrDefault();

   

        }

       

       
       
        private void ToolbarItem_Instrucciones(object sender, EventArgs e)
        {
            //navigation a una page llamada QrPage()
            Navigation.PushAsync(new How());
        }
        private void ToolbarItem_InfoUser(object sender, EventArgs e)
        {
            //navigation a una page llamada QrPage()
            Navigation.PushAsync(new UserInfo());
        }

        private void ToolbarItem_Solicitudes(object sender, EventArgs e)
        {
            //navigation a una page llamada QrPage()
            Navigation.PushAsync(new Ordenes());
        }

        //private void ToolbarItem_Sign() {
        //    Navigation.PushAsync(new Signature());
        //}



    }
}
