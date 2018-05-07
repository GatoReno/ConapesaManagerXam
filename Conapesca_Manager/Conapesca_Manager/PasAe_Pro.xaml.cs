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
	public partial class PasAe_Pro : ContentPage
	{
         public Member member;
        public MemberDatabase memberdatabase;
        public SQLiteConnection conn;

		public PasAe_Pro (int ID)
		{
			InitializeComponent ();
            Id_lbl.Text = ID.ToString();
          
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await GetData_Vuelo();
        }

       
        //funcion de cambio de estado
      
        //rechazar
        public async Task Reject()
        {
            int ID = Int32.Parse(Id_lbl.Text);
            await Navigation.PushAsync(new Razon_Reject(ID));
        }

        
        public async Task GetData_Vuelo()
        {

            int x = Int32.Parse(Id_lbl.Text);
            Cator.IsRunning = true;
            Cator.IsVisible = true;
            //try http GET
            var uri = "http://aige.sytes.net/ApiRestSAM/api/conapesca/Vuelo?IdOrden=" + x;

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(uri);

            request.Method = HttpMethod.Get;

            var client = new HttpClient();
            // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tok_ty, acc_tok);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.SendAsync(request);


            switch (response.StatusCode)
            {
                //200
                case (System.Net.HttpStatusCode.OK):

                    HttpContent content = response.Content;
                    string xjson = await content.ReadAsStringAsync();

                   

                    try
                    {
                        lbl_res.Text = "Ordenes Pendientes";
                        Root myobject = JsonConvert.DeserializeObject<Root>(xjson);

                        if (myobject.tablas == null)
                        {
                            lbl_txt.Text = "Pudo haber un error";
                            lbl_res.Text = "No hay datos que mostrar";
                       
                           
                        }
                        else
                        {
                            ListOrden.ItemsSource = myobject.tablas.Table1;
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("", "" + ex.ToString(), "ok");
                        return;
                    }


                    break;
                //500
                case (System.Net.HttpStatusCode.InternalServerError):
                    await DisplayAlert("Error de servidor", "Nuestros servidores estan en mantenimiento, INTENTE EN OTRO MOMENTO", "Continuar");
                    lbl_res.Text = "Error - 500";

                    break;
                //404
                case (System.Net.HttpStatusCode.Forbidden):
                    try
                    {
                        lbl_res.Text = "Error - 404";

                        await DisplayAlert("Su sesión ha caducado", "Reingrese sus datos", "ok");
                        memberdatabase.DropTbMember();
                        await Navigation.PushModalAsync(new AuthPage());
                    }
                    catch (Exception ex)
                    {
                        lbl_res.Text = ex.ToString();
                    }
                    break;
            }

            Cator.IsRunning = false;
            Cator.IsVisible = false;
        }


    }
}