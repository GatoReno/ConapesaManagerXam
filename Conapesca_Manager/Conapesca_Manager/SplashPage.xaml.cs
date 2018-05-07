using Conapesca_Manager.Classes;
using Conapesca_Manager.Models;
using Plugin.Connectivity;
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
	public partial class SplashPage : ContentPage
	{
        //Validar aquí todo lo validable
		


        public MemberDatabase memberDatabase;
        public Member member;
        public SQLiteConnection conn;

        Image splashImage;
        public SplashPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            var sub = new AbsoluteLayout();
            splashImage = new Image
            {
                Source = "iko.png",
                WidthRequest = 100,
                HeightRequest = 100
            };
            AbsoluteLayout.SetLayoutFlags(splashImage,
               AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(splashImage,
             new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            sub.Children.Add(splashImage);

            this.BackgroundColor = Color.FromHex("#FFFFFF");
            this.Content = sub;
        }



        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await splashImage.ScaleTo(1, 2000); //Time-consuming processes such as initialization
            await splashImage.ScaleTo(0.6, 1500, Easing.Linear);
            await splashImage.ScaleTo(150, 1200, Easing.Linear);

            InitializeComponent();

            
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Advertencia ! ", "ACTIVAR  CONSUMO DE DATOS O WIFI  PARA PODER CONTINUAR ", "Ok");
                OnAppearing();
            }
            else
            {
               
                TryAccess();
            }

            //end OnAppering
        }

        public void TryAccess()
        {
            memberDatabase = new MemberDatabase();
            var members = memberDatabase.GetMembers();
            int RowCount = 0;
            int membcount = members.Count();
            RowCount = Convert.ToInt32(membcount);
            if (RowCount > 0)
            {

                Application.Current.MainPage = new NavigationPage(new PageNav());

            }
            else
            {
                Application.Current.MainPage = new NavigationPage(new AuthPage());

            }
        }



    }
}