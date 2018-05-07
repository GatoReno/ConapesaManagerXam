using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Conapesca_Manager
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Signature : ContentPage
	{
		public Signature ()
		{
			InitializeComponent ();
           
        }
    

        public void DropSignature() {
            Sign.Clear();
            Imgx.IsVisible = false;
            Sign.IsVisible = true;
            Cator.IsVisible = false;
            Cator.IsRunning = false;
        }


        public async void SaveSigature() {

            if (Sign.IsBlank)
            {
                Sign.Focus();
                Savebtn.Focus();
                res1_lbl.IsVisible = true;
                res1_lbl.Text = "Por favor firme para continuar";

            }
            else {
                res1_lbdl.Text = "";
                res1_lbl.Text = "Gracias, le estamos atendiendo";
                Cator.IsRunning = true;
                Cator.IsVisible = true;
                res_lbl.Text = "Por favor espere";
                Stream image = await Sign.GetImageStreamAsync
                    (SignaturePad.Forms.SignatureImageFormat.Png);
                Sign.Clear();
                Sign.IsVisible = false;

                
                BinaryReader br = new BinaryReader(image);
                Byte[] bytes = br.ReadBytes((Int32)image.Length);
                string base64Str = Convert.ToBase64String(bytes, 0, bytes.Length);
                bytex.IsVisible = true;
                bytex.Text = base64Str;
                Cator.IsRunning = false;
            }
            
           
        }

        public  void ToImage()
        {
            Cator.IsRunning = true;
            bytex.IsVisible = false;
            Imgx.IsVisible = true;
            var img64 = bytex.Text;
            //MemoryStream ms = new MemoryStream(Convert.FromBase64String(img64));
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(img64);
            // Convert byte[] to Image
            Image image = new Image();
          
        

            Imgx.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
            Cator.IsRunning = false;
        }
    }
}