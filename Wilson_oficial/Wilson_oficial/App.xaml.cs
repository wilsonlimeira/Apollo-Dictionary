using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Wilson_oficial
{
	public partial class App : Application
	{
        UserProperties userProp;

		public App ()
		{
			InitializeComponent();

            userProp = new UserProperties();

			MainPage = new Wilson_oficial.Pages.HomePage();
		}

		protected override void OnStart ()
		{
            // Handle when your app starts
            userProp.ObtainList();
		}

		protected override void OnSleep ()
		{
            // Handle when your app sleeps
            userProp.SaveList();
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
