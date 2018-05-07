using Conapesca_Manager.Classes;
using Conapesca_Manager.Models;
using Rg.Plugins.Popup.Services;
using SQLite;
using System;
using System.Linq;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Conapesca_Manager
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyPopupPage 
	{
        public Member member;
        public SQLiteConnection conn;
        public MemberDatabase memberDatabase;

        public MyPopupPage ()
		{
			InitializeComponent ();
		}


        public async void BtnClouse()
        {
            memberDatabase = new MemberDatabase();

            var mx = memberDatabase.GetMembers();
            var mx_first = mx.FirstOrDefault();

            var id = mx_first.ID;

            try
            {
                memberDatabase.DeleteMember(id);


            }
            catch (Exception ex)
            {

                txt_lb.Text = "" + ex.ToString();
            }
            await Navigation.PushModalAsync(new SplashPage());
            await PopupNavigation.PopAsync();



        }
    }
}