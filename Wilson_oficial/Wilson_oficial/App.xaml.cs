using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using PCLStorage;
using System.Threading.Tasks;

namespace Wilson_oficial
{
	public partial class App : Application
	{
        ApolloDictionary app;
        UserProperties userProp;

        public static IFile userFile;
        public static IFolder folder;

        public App ()
		{
			InitializeComponent();

            //open file and get the file pointer
            OpenFile();

            //initialize the dictionary
            app = new ApolloDictionary();
            app.List = ReadDictFiles.readAndBuildDictionary();

            //initializing the History methods to get user stored words
            userProp = new UserProperties();

			MainPage = new Wilson_oficial.Pages.HomePage();
		}

        private async void OpenFile()
        {
            // get hold of the file system
            //IFolder rootFolder = FileSystem.Current.LocalStorage;

            // create a folder, if one does not exist already
            folder = await PCLHelper.CreateFolder("userData");

            // create a file, overwriting any existing file
            //userFile = await PCLHelper.CreateFile("MyDictionary.txt", folder);
            //userFile = await folder.GetFileAsync("MyDictionary.txt");

            // populate the file with some text
            //await file.WriteAllTextAsync("Sample Text...");
        }

        protected override void OnStart ()
		{
            // Handle when your app starts

            //get list which is saved in the Storage
            userProp.ObtainList();
		}

		protected override void OnSleep ()
		{
            // Handle when your app sleeps

            //save list in the Storage
            userProp.SaveList();
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
