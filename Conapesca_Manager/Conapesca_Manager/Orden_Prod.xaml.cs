
using Conapesca_Manager.Classes;
using Conapesca_Manager.Models;
using Newtonsoft.Json;
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
	public partial class Orden_Prod : ContentPage
	{

  
		public Orden_Prod (int ID)
		{
			InitializeComponent ();
            Id_lbl.Text = ID.ToString();
            CheckButtonInit();

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await GetData();
        }

       
        //instancia cambio de estado de boton
        private void CheckButtonInit() {
            checkBoxGenero.CheckedChanged += Accepted;

        }
        //funcion de cambio de estado
        private void Accepted(object sender, XLabs.EventArgs<bool> e)
        {
            if (checkBoxGenero.Checked)
            {
                Env.IsEnabled = true;
                Rech.IsEnabled = false;
            }
            else {
                Env.IsEnabled = false;
                Rech.IsEnabled = true;
            }

        }
        //rechazar
        public async Task Reject() {
            int ID = Int32.Parse(Id_lbl.Text);
           
            
            await Navigation.PushAsync(new Razon_Reject(ID));
        }

        public async void goOrdenes()
        {

            await Navigation.PopAsync(); // esto no está pasando


            Application.Current.MainPage = new NavigationPage(new PageNav());

        }
        //Aceptar y firmar
        public async Task SendAccepted() {

            MemberDatabase memberDatabase = new MemberDatabase();
            int ID = Int32.Parse(Id_lbl.Text);
            var members = memberDatabase.GetMembers();
            var member = members.FirstOrDefault();
            var tok_ty = member.Token_Type;
            var acc_tok = member.Access_Token;
            var idmember = member.ID;
            var firma = member.Firma;
            
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tok_ty, acc_tok);
            client.DefaultRequestHeaders.Add("api-version", "1.0");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var values = new Dictionary<string, string>
                         {
                            { "firma",  firma },
                            { "id", idmember.ToString() },
                            { "idOrden", ID.ToString() }

                         };


            var content = new FormUrlEncodedContent(values);


            var response = await client.PostAsync("http://aige.sytes.net/ApiRestSAM/api/Conapesca/AceptarOrden",
           content);

            switch (response.StatusCode)
            {
                case (System.Net.HttpStatusCode.OK):
                    lbl_res.Text = "Exito";


                    var responseString = await response.Content.ReadAsStringAsync();
                    RootObject xjson = JsonConvert.DeserializeObject<RootObject>(responseString);

                    int flag = Int32.Parse(xjson.bandera);

                    if (flag == -2)
                    {
                        await DisplayAlert("Error en la solicitud ", "Ha existido algun error", "Entiendo");
                        //Desocupar orden
                        Orden or = new Orden(ID);
                        await or.UnCheckUso();
                        goOrdenes();
                    }
                    if (flag == 0) {
                        await DisplayAlert("Solicitud aceptada", "Usted aceptó una solicitud", "Entiendo");
                        Orden or = new Orden(ID);
                        await or.UnCheckUso();
                        goOrdenes();
                    }


                    break;

                case (System.Net.HttpStatusCode.BadRequest):
                    lbl_res.Text = "Exisitió un problema con la consulta";
                    break;

                case (System.Net.HttpStatusCode.Forbidden):
                    lbl_res.Text = "Usted no tiene acceso";
                    break;
                //500
                case (System.Net.HttpStatusCode.InternalServerError):
                    string status = "Nuestros servidores estan en mantenimiento";
                    lbl_res.Text = status;
                    break;
            }

            //await Navigation.PopAsync();

        }

        //Obtener datos de destino
        public async Task GetData() {

            lbl_res.Text = "Solicitando datos ... por favor espera";
            Cator.IsRunning = true;
            Cator.IsVisible = true;

            int x = Int32.Parse(Id_lbl.Text);

            //try http GET
            var uri = "http://aige.sytes.net/ApiRestSAM/api/conapesca/Orden?IdOrden=" + x;

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

                        RootObject myobject = JsonConvert.DeserializeObject<RootObject>(xjson);
                        if (myobject.Tablas == null)
                        {
                            lbl_txt.Text = "Pudo haber un error";
                            lbl_res.Text = "No hay datos que mostrar";
                            checkBoxGenero.IsEnabled = false;
                            Rech.IsEnabled = false;
                        }
                        else {
                            ListOrden.ItemsSource = myobject.Tablas.Table1;
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
                    await DisplayAlert("Error de servicio", "Nuestros servidores estan en mantenimiento, intentelo en otro momento", "Continuar");
                    lbl_res.Text = "Error - 500";

                    //  await Navigation.PushModalAsync(new AuthPage());
                    break;
                //404
                case (System.Net.HttpStatusCode.Forbidden):
                    try
                    {
                        lbl_res.Text = "Error - 404";

                        await DisplayAlert("Su sesión ha caducado", "Reingrese sus datos", "ok");
                         
                        await Navigation.PushModalAsync(new SplashPage());
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
