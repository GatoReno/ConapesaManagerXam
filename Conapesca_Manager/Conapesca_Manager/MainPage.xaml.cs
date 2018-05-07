using Conapesca_Manager.Classes;
using Conapesca_Manager.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Conapesca_Manager
{
	public partial class MainPage : ContentPage
    {
        public MemberDatabase memberDatabase;
        public Member member;
        public SQLiteConnection conn;
        private MemberDatabase memberdatabase;

        public MainPage()
		{
			InitializeComponent();
            memberDatabase = new MemberDatabase();
            var members = memberDatabase.GetMembers();
            listMembers.ItemsSource = members;
        }


        private void ToolbarItem_Solicitudes(object sender, EventArgs e)
        {
            //navigation a una page llamada QrPage()
            Navigation.PushAsync(new Ordenes());
        }

        private void ToolbarItem_InfoUser(object sender, EventArgs e)
        {
            //navigation a una page llamada QrPage()
            Navigation.PushAsync(new UserInfo());
        }
       
    }
}
