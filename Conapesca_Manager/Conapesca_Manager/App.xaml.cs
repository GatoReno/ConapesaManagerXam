using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Conapesca_Manager
{
	public partial class App : Application
	{


		public App ()
		{
			InitializeComponent();

            MainPage = // new Ordenes();
                 new SplashPage();
               //new Signature();
            //new AuthPage();
            //new Conapesca_Manager.MainPage();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
