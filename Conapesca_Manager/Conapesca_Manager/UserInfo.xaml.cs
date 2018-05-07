using Conapesca_Manager.Classes;
using Conapesca_Manager.Models;
using Rg.Plugins.Popup.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Conapesca_Manager
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserInfo : ContentPage
	{
        public Member member;


        public MemberDatabase memberDatabase;
        public UserInfo ()
		{
            InitializeComponent();
            memberDatabase = new MemberDatabase();
            var members = memberDatabase.GetMembers();
            listMembers.ItemsSource = members;
        }

        public async void OnSelected(object obj, ItemTappedEventArgs args)
        {
            var member = args.Item as Member;
            try
            {
                // await DisplayActionSheet("You selected", member.Name + " " + member.Roll,"ok");
                await userOptionsHandlerAsync(member.ID);
            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.ToString(), "");
                return;
            }
        }

        private void ShowP(object o, EventArgs e)
        {
            #pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            PopupNavigation.PushAsync(new MyPopupPage());
            #pragma warning restore CS0618 // El tipo o el miembro están obsoletos
        }



        private async Task userOptionsHandlerAsync(int ID)
        {

            var actionSheet = await DisplayActionSheet("Opciones de usuario " + ID, "Cancel", null, "Borrar");


            switch (actionSheet)
            {
                case "Borrar":

                    try
                    {
                        memberDatabase.DeleteMember(ID);
                       
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("", "" + ex.ToString(), "ok");
                    }

                    await Navigation.PopAsync();
                    Application.Current.MainPage = new NavigationPage(new AuthPage());

                    break;
            }

        }
        //listMembers Name id firma
        public void Checking() {
            if (Imgx.IsVisible)
            {
                btnImg.Text = "Mostrar firma";
                Imgx.IsVisible = false;
                Imgy.IsVisible = true;
            }
            else{
                ToImage();
            }
        }

        public void ToImage()
        {
            Cator.IsRunning = true;
            Imgx.IsVisible = true;
            Imgy.IsVisible = false;
    
                btnImg.Text = "Ocultar firma";
                var TmpList = (List<Member>)listMembers.ItemsSource;
                var img64 = (TmpList != null && TmpList.Count > 0) ? TmpList[0].Firma : "";
                    
                //handler de error en caso de que no exista firma
                if (img64 == null)
                {
                    DisplayAlert("Error con firma", 
                        "Al parecer no se encuntra ninguna firma y es imposible continuar, cerraremos sesión para vovler a empezar"
                        , "Aceptar y continuar");
                    memberDatabase.DropTbMember();
                    Navigation.PopAsync();
                    Application.Current.MainPage = new NavigationPage(new AuthPage());

            }
                else {
                    byte[] imageBytes = Convert.FromBase64String(img64);
                    Xamarin.Forms.Image image = new Xamarin.Forms.Image();
                    Imgx.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                }
                


            Cator.IsRunning = false;
        }
    }
}