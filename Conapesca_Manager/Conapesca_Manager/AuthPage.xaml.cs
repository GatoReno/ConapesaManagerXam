using Android.Renderscripts;
using Conapesca_Manager.Classes;
using Conapesca_Manager.Models;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
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
	public partial class AuthPage : ContentPage
    {
        public Member member;
        public MemberDatabase memberdatabase;
        public SQLiteConnection conn;
        private MemberDatabase memberDatabase;

        public AuthPage ()
		{
			InitializeComponent ();
		}

        //protected override void OnAppearing() {

        //    base.OnAppearing();

        //}

        public async Task Access_Login()
        {

            if (string.IsNullOrEmpty(userTxt.Text))
            {
                await DisplayAlert("Error", "Por favor ingrese su nombre de usuario", "ok");
                userTxt.Focus();
                return;

            }
            if (string.IsNullOrEmpty(passTxt.Text))
            {
                await DisplayAlert("Error", "Por favor ingrese una contraseña", "ok");
                passTxt.Focus();
                return;
            }
            else
            {
                


                Sign.IsVisible = true;
                Savebtn.IsVisible = true;
                userTxt.IsVisible = false;
                passTxt.IsVisible = false;
               
                res_Label.IsVisible = false;
                btnAuth.IsVisible = false;

                res_Label_api.Text = "Firme para coninuar";

         
            }
        }


        public async void SaveSigature()
        {

            if (Sign.IsBlank)
            {
                Sign.Focus();
                Savebtn.Focus();
                res_Label.IsVisible = true;
                res_Label_api.Text = "Por favor firme para continuar";

            }
            else
            {
                res_Label.IsVisible = true;
                Savebtn.IsVisible = false;
                waitActIndicator.IsRunning = true;
                waitActIndicator.IsVisible = true;
                res_Label.Text = "Por favor espere";
                Stream image = await Sign.GetImageStreamAsync
                    (SignaturePad.Forms.SignatureImageFormat.Png);
                Sign.Clear();
                Sign.IsVisible = false;


                BinaryReader br = new BinaryReader(image);
                Byte[] bytes = br.ReadBytes((Int32)image.Length);
                string base64Str = Convert.ToBase64String(bytes, 0, bytes.Length);

                try
                {
                   

                  var firma = base64Str;
                    await Access_Token(firma);
                   

                    res_Label.Text = "Éxito , fimra guardada.";
                    btnAuth.IsVisible = false;

                }
                catch (Exception ex)
                {

                    await DisplayAlert("", "" + ex.ToString(), "ok");
                }
            }


        }

        public void TryAgain() {
            Application.Current.MainPage = new NavigationPage(new AuthPage());
        }

        public async Task Access_Token(string firma) {
            HttpClient client = new HttpClient();


            var values = new Dictionary<string, string>
                         {
                            { "UserName",  "conapesca_sam" },
                            { "Password", "2{'9At)nuH$2&SzK" },
                            { "grant_type" , "password" }
                         };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("http://aige.sytes.net/ApiRestSAM/TOKEN",
                content);
            string status;

            switch (response.StatusCode)
            {


                // 200
                case (System.Net.HttpStatusCode.OK):


                    var responseString = await response.Content.ReadAsStringAsync();

                    var xjson = JsonConvert.DeserializeObject<TokenRequest>(responseString);

                    status = xjson.access_token + " " + xjson.expires_in;
                    var xjson_token = xjson.access_token;
                    var xjson_type = xjson.token_type;
                    var xjson_exp = xjson.expires_in;

                    res_Label.Text = "Éxito, Token Request";

                    string tok_ty = xjson_type;
                    string acc_tok = xjson_token;

                    var userName = userTxt.Text;
                    var pass = passTxt.Text;
                    
                        Login_Api(userName, pass, tok_ty, acc_tok, firma);                                     

                    break;

                // 400
                case (System.Net.HttpStatusCode.BadRequest):
                    status = "Usuario o contraseña invalidos -error 400";
                    res_Label.Text = status;
                    break;

                //500
                case (System.Net.HttpStatusCode.InternalServerError):
                    status = "Nuestros servidores estan en mantenimiento";
                    res_Label.Text = status;
                    break;

                // 502
                case (System.Net.HttpStatusCode.BadGateway):
                    status = "Usuario o contraseña invalidos - error 502";
                    res_Label.Text = status;
                    break;

                // 403 required

                case (System.Net.HttpStatusCode.Forbidden):
                    status = "Acceso rechazado";
                    res_Label.Text = status;
                    await DisplayAlert("Error de acceso", "Es probable que tu sesión haya caducado. Ingresa tus datos de acceso nuevamente", "Continuar");
                    break;

                // 404
                case (System.Net.HttpStatusCode.NotFound):
                    status = "Error - 404 Servidor no encontrado";
                    res_Label.Text = status;
                    await DisplayAlert("Error de acceso", "Es probable que tu sesión haya caducado. Ingresa tus datos de acceso nuevamente", "Continuar");
                    break;


            }

        }


        public async void Login_Api(string userName, string pass, string tok_ty, string acc_tok, string firma)
        {



            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tok_ty, acc_tok);
            client.DefaultRequestHeaders.Add("api-version", "1.0");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var values = new Dictionary<string, string>
                         {
                            { "usuario",  userName },
                            { "contraseña", pass }

                         };


            var content = new FormUrlEncodedContent(values);

           
                var response = await client.PostAsync("http://aige.sytes.net/ApiRestSAM/api/Conapesca/Acceso",
               content);

                switch (response.StatusCode)
                {
                    case (System.Net.HttpStatusCode.OK):
                        res_Label_api.Text = "Acceso aprovado";

                        var responseString = await response.Content.ReadAsStringAsync();


                        
                        var xjson = JsonConvert.DeserializeObject<RootObject>(responseString);
                        int x = Int32.Parse( xjson.bandera);
                        var usr = xjson.DatosEnvioJson;
                        var id = xjson.DatosEnvio.IdUsuario;

                        //res_Label_api.Text = xjson.DatosEnvio.AdminType.ToString();

                        if (x == -2)
                        {
                            res_Label.Text = "Lo sentimos";
                            res_Label_api.Text = "Usuario o contraseña no validos";
                            TryAgainbtn.IsVisible = true;
                           // Application.Current.MainPage = new AuthPage();
                         }
                        else
                        {

                            member = new Member();
                            memberdatabase = new MemberDatabase();
                            member.Name = userName;
                            member.Pass = pass;
                            member.Firma = firma;
                            member.Token_Type = tok_ty;
                            member.Access_Token = acc_tok;
                            member.ID = xjson.DatosEnvio.IdUsuario;
                            member.AdminType = xjson.DatosEnvio.AdminType;

                     
                            memberdatabase.AddMember(member);
                        var xs = member.ID.ToString();
                        CheckDelegado(xs);



                        //Application.Current.MainPage = new NavigationPage(new PageNav());
                            //await DisplayAlert("", "..."+" "+xjson.DatosEnvio.Usuario+" " + xjson.DatosEnvio.AdminType + " " + xjson.DatosEnvio.IdUsuario, "ok");

                        }
                   
                        break;

                    case (System.Net.HttpStatusCode.BadRequest):
                        res_Label_api.Text = "Error 400";
                    TryAgainbtn.IsVisible = true;
                    break;

                    case (System.Net.HttpStatusCode.Forbidden):
                        res_Label_api.Text = "Error 404";
                    TryAgainbtn.IsVisible = true;
                    break;
                    //500
                    case (System.Net.HttpStatusCode.InternalServerError):
                        string status = "Nuestros servidores estan en mantenimiento";
                        res_Label_api.Text = status;
                    TryAgainbtn.IsVisible = true;
                    break;
                }
            

            waitActIndicator.IsRunning = false;
            btnAuth.IsEnabled = true;
            return;
        }


        public async void CheckDelegado(string x)
        {

            //try http GET
            var uri = "http://aige.sytes.net/ApiRestSAM/api/conapesca/Delegar?Id=" + x;

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

                    Root myobject = JsonConvert.DeserializeObject<Root>(xjson);



                    var delega = myobject.tablas.Table1[0].delega;
                    if (delega != true)
                    {
                        await DisplayAlert("Usuario sin permisos",
                            "Sus datos son correctos, pero no cuenta con permisos para usar la aplicación, Contacte a su directivo", "ok");
                        memberDatabase = new MemberDatabase();
                        memberDatabase.DropTbMember();
                        Application.Current.MainPage = new NavigationPage(new SplashPage());
                    }
                    else
                    {
                        Application.Current.MainPage = new NavigationPage(new PageNav());
                    }



                    break;
                //500
                case (System.Net.HttpStatusCode.InternalServerError):
                    await DisplayAlert("No existe registro de usuario", "Nuestros servidores estan en mantenimiento", "Continuar");
                    res_Label_api.Text = "Error - 500";

                    break;
                //404
                case (System.Net.HttpStatusCode.Forbidden):
                    try
                    {
                        res_Label_api.Text = "Error - 404";

                        await DisplayAlert("Su sesión ha caducado", "Reingrese sus datos", "ok");
                        // memberDatabase.DeleteMember(id);
                        //await Navigation.PushModalAsync(new AuthPage());
                    }
                    catch (Exception ex)
                    {
                        res_Label_api.Text = ex.ToString();
                    }
                    break;
            }
        }


    }
 }
