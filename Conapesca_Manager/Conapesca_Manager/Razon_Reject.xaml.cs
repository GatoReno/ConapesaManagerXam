using Conapesca_Manager.Classes;
using Conapesca_Manager.Models;
using Java.Sql;
using Newtonsoft.Json;
using Plugin.Messaging;
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
	public partial class Razon_Reject : ContentPage
	{


        public Member member;
        public SQLiteConnection conn;

        public Razon_Reject (int ID)
		{
			InitializeComponent ();
            lbl_ID.Text = ID.ToString();
           
		}

        protected async override void OnDisappearing()
        {
            base.OnDisappearing();
            //Desocupar orden
          
            // await Navigation.PopAsync();


        }

        private async void Send_Rejection() {
            MemberDatabase memberDatabase = new MemberDatabase();

            var members = memberDatabase.GetMembers();
            var member = members.FirstOrDefault();
            var tok_ty = member.Token_Type;
            var acc_tok = member.Access_Token;

            string ID = lbl_ID.Text;
            var body = Editx.Text;

            if (string.IsNullOrEmpty(Editx.Text))
            {
                lbl_ID.Text = "Especifica a detalle el porqué de tu rechazo a esta orden";
                Editx.Focus();
            }
            else {

                var emailMsn = CrossMessaging.Current.EmailMessenger;

                if (emailMsn.CanSendEmail)
                {

                    var uri = "http://aige.sytes.net/ApiRestSAM/api/conapesca/Mail?Id=" + ID;

                    var request = new HttpRequestMessage();
                    request.RequestUri = new Uri(uri);

                    request.Method = HttpMethod.Get;

                    var client2 = new HttpClient();
                    // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tok_ty, acc_tok);
                    client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response2 = await client2.SendAsync(request);


                    switch (response2.StatusCode)
                    {
                        //200
                        case (System.Net.HttpStatusCode.OK):
                            HttpContent content2 = response2.Content;

                           
                                var responseString = await response2.Content.ReadAsStringAsync();
                                RootObject xjson = JsonConvert.DeserializeObject<RootObject>(responseString);
                                var flag = Int32.Parse(xjson.bandera);




                            if (flag == -2)
                            {
                                await DisplayAlert("Error con datos de usuario"
                                        , "Error inesperado ,puede que el usuario no cuente con un mail"
                                        , "Continuar");
                                int IDX = Int32.Parse(ID);
                                Orden or = new Orden(IDX);
                                await or.UnCheckUso();
                                await Navigation.PushModalAsync(new SplashPage ());
                            }
                                else
                                {
                                
                                string mail = xjson.Tablas.Table[0].email;
                                res_Label.Text = "" + mail;
                                emailMsn.SendEmail("" + mail, "Conapesca Serivicios - Orden Rechazada", "Razón de rechazo: " + body);
                                int IDX = Int32.Parse(ID);
                                Orden or = new Orden(IDX);
                                await or.UnCheckUso();

                                // esto no está pasando

                                await Navigation.PushModalAsync(new SplashPage());

                            }
                               

                            break;
                        //404
                        case (System.Net.HttpStatusCode.BadRequest):
                            await DisplayAlert("Error con datos de usuario", "Es posible que el usuario no cuente con un mail", "Continuar");

                          

                            await Navigation.PushModalAsync(new Ordenes());
                            break;
                        //500
                        case (System.Net.HttpStatusCode.InternalServerError):
                            await DisplayAlert("Error de servidor", "Nuestros servidores estan en mantenimiento", "Continuar");

                            await Navigation.PushModalAsync(new SplashPage());
                            break;
                        //404
                        case (System.Net.HttpStatusCode.Forbidden):
                            try
                            {
                                await DisplayAlert("Su sesión ha caducado", "Reingrese sus datos", "ok");
                                // memberDatabase.DeleteMember(id);
                                await Navigation.PushModalAsync(new SplashPage());
                            }
                            catch (Exception ex)
                            {

                                lbl_ID.Text = ex.ToString();

                            }
                            break;

                    }


                    }
                }


            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tok_ty, acc_tok);
            client.DefaultRequestHeaders.Add("api-version", "1.0");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            DateTime dt = new DateTime();

            var values = new Dictionary<string, string>
                         {
                            { "idOrden",  ID },
                            { "razon", body}

                         };


            var content = new FormUrlEncodedContent(values);


            var response = await client.PostAsync("http://aige.sytes.net/ApiRestSAM/api/Conapesca/Rechazo",
           content);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

               // await Navigation.PopAsync();

                await DisplayAlert("Solicitud rechazada", "Ustedes rechazó una solicitud", "Entiendo");


                int IDX = Int32.Parse(ID);
                Orden or = new Orden(IDX);
                await or.UnCheckUso();
                goOrdenes();
            }
            else
            {
                //
                int IDX = Int32.Parse(ID);
                Orden or = new Orden(IDX);
                await or.UnCheckUso();
                await DisplayAlert("Error", "Hubo algún error", "Entiendo");
                goOrdenes();
            }
            
        
         


        }

        public async void goOrdenes() {

            await Navigation.PopAsync(); // esto no está pasando

           await Navigation.PushModalAsync( new SplashPage());
           // Application.Current.MainPage = new NavigationPage(new PageNav());

        }



    }
}