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
using Conapesca_Manager.Models;
using Conapesca_Manager.Classes;
using SQLite;

namespace Conapesca_Manager
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Ordenes : ContentPage
	{

        public MemberDatabase memberDatabase;
        public Member member;
        public SQLiteConnection conn;

        public Ordenes ()
		{
			InitializeComponent ();
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();

          

                memberDatabase = new MemberDatabase();
                var members = memberDatabase.GetMembers();
                var member = members.FirstOrDefault();

                var mx_first = member.AdminType;
                res_y.Text = mx_first.ToString();


            res_x.Text = "Solicitando datos ... por favor espera";
            Cator.IsRunning = true;
            Cator.IsVisible = true;

            switch (mx_first)
            {
                case(1):
                case (2):


                    //desde aquí
                    //try http GET
                    var uri = "http://aige.sytes.net/ApiRestSAM/api/conapesca/orden";

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
                                //List<Table_Loc> loc_list = JsonConvert.DeserializeObject<List<Table_Loc>>(xjson);

                                Root myobject = JsonConvert.DeserializeObject<Root>(xjson);

                                res_x.Text = "Ordenes Pendientes";
                                ListOrdenes.IsVisible = true;
                                ListV.IsVisible = false;
                                ListOrdenes.ItemsSource = myobject.tablas.Table1;

                                if (myobject.tablas.Table1.Count == 0)
                                {
                                    ListV.IsVisible = true;
                                    res_x.Text = "No Existen Ordenes Pendientes";
                                }
                                // ListLoc
                            }
                            catch (Exception ex)
                            {
                                await DisplayAlert("", "" + ex.ToString(), "ok");
                                return;
                            }

                            break;
                        //500
                        case (System.Net.HttpStatusCode.InternalServerError):
                            await DisplayAlert("No existe registro de usuario", "Nuestros servidores estan en mantenimiento", "Continuar");

                            await Navigation.PushModalAsync(new AuthPage());
                            break;
                        //404
                        case (System.Net.HttpStatusCode.Forbidden):
                            try
                            {
                                await DisplayAlert("Su sesión ha caducado", "Reingrese sus datos", "ok");
                                // memberDatabase.DeleteMember(id);
                                await Navigation.PushModalAsync(new AuthPage());
                            }
                            catch (Exception ex)
                            {

                                res_x.Text = ex.ToString();

                            }
                            break;

                    }
                    //hasta acá para llamar info segun admin type

                    break;
            }


            //var at = 







           


            Cator.IsRunning = false;
            Cator.IsVisible = false;

            //res_lbl

            //await DisplayAlert("","fack user "+id+" we R good 2 Go! yei :D / :3 ","ok");
        }


        public async void OnSelected(object obj, ItemTappedEventArgs args)
        {
            var orden = args.Item as Table1;
            try
            {
                //await DisplayAlert("You selected", orden.orden + " " + orden.idOrden,"ok");
                int ID = orden.idOrden;
                
                await Navigation.PushAsync(new Orden(ID));

            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.ToString(), "");
                return;
            }
        }
    }
}