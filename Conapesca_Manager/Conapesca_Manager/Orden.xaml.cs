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
	public partial class Orden : ContentPage
	{
		public Orden (int ID)
		{
			InitializeComponent ();
            Id_lbl.Text = ID.ToString();
            lbl_res.Text = "Orden Número "+ ID.ToString();
        }


        public async void PasAe_Pro()
        {
            int ID = Int32.Parse(Id_lbl.Text);
            await Navigation.PushAsync(new PasAe_Pro(ID));
        }

        public async void Orden_Pro() {
            int ID = Int32.Parse(Id_lbl.Text);
            await Navigation.PushAsync(new Orden_Prod(ID));
        }
        protected async override void OnDisappearing()
        {
            base.OnDisappearing();
           await UnCheckUso();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await CheckUso();
            await GetData_Vuelo();
        }
        public async Task UnCheckUso() {

            int x = Int32.Parse(Id_lbl.Text);
            //try http GET
            var uri = "http://aige.sytes.net/ApiRestSAM/api/conapesca/NoEnUso?idOrden=" + x;

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




                    RootObject myobject = JsonConvert.DeserializeObject<RootObject>(xjson);
                    var flag = Int32.Parse(myobject.bandera);
                 

                    if (flag == -2)
                    {
                        await DisplayAlert("Error Inesperado", "Hubo un error con la información de esta orden, regresaremos a la vista anterior", "Entiendo");
                        await Navigation.PopAsync();
                        await Navigation.PushAsync(new Ordenes());

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
                  
                        lbl_res.Text = "Error - 404";

                        await DisplayAlert("Su sesión ha caducado", "Reingrese sus datos", "ok");

                        await Navigation.PushModalAsync(new SplashPage());
                   
                    break;

            }
        }
        public async Task CheckUso() {

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

                
                        

                        RootObject myobject = JsonConvert.DeserializeObject<RootObject>(xjson);
                        var flag = Int32.Parse(myobject.bandera);

                    if (flag == 3)
                    {
                        await DisplayAlert("Información en uso", "Lo sentimos pero alguien más esta ocupando esta información, intente nuevamente más tarde.", "Entiendo");
                        await Navigation.PopAsync();
                        await Navigation.PushAsync(new Ordenes());
                        
                    }


                    if (flag == -2)
                    {
                        await DisplayAlert("Error Inesperado", "Hubo un error con la información de esta orden, regresaremos a la vista anterior", "Entiendo");
                        await Navigation.PopAsync();
                        await Navigation.PushAsync(new Ordenes());

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
        }

        public async Task GetData_Vuelo() {

            int x = Int32.Parse(Id_lbl.Text);

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

                  //List<Table_Loc> loc_list = JsonConvert.DeserializeObject<List<Table_Loc>>(xjson);
                        lbl_res.Text = "Ordenes Pendientes";
                        Root myobject = JsonConvert.DeserializeObject<Root>(xjson);

                    //int check = Newtonsoft.Json.Linq.JArray.Parse(xjson).Count;

                    //var T1count = myobject.tablas.Table1.Count;
                    if (myobject.tablas == null )
                    {
                        PasajeAereo.Text = "PASAJE AEREO - NO DISPOBILE";
                        PasajeAereo.IsEnabled = false;
                    }
                    else {
                        if (myobject.tablas.Table1.Count > 0 )
                        {
                            PasajeAereo.IsEnabled = true;
                        }
                        else {
                            PasajeAereo.Text = "PASAJE AEREO - NO DISPOBILE";
                            PasajeAereo.IsEnabled = false;
                        }
                    }
                    
                    break;
                //500
                case (System.Net.HttpStatusCode.InternalServerError):
                    await DisplayAlert("No existe registro de usuario", "Nuestros servidores estan en mantenimiento", "Continuar");
                    lbl_res.Text = "Error - 500";

                    break;
                //404
                case (System.Net.HttpStatusCode.Forbidden):
                    try
                    {
                        lbl_res.Text = "Error - 404";

                        await DisplayAlert("Su sesión ha caducado", "Reingrese sus datos", "ok");
                        // memberDatabase.DeleteMember(id);
                        //await Navigation.PushModalAsync(new AuthPage());
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