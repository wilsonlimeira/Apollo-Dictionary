using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Wilson_oficial.Pages
{
	public partial class SearchPage : ContentPage
	{
        private List<Word> _words;
        private IFile file;

		public SearchPage ()
		{
			InitializeComponent ();

            //_words = new List<Word>();
            //_words.Add(new Word { Name = "NHS", Definition = "British health system" });
            //_words.Add(new Word { Name = "Heart Attack", Definition = "Heart disease"});
            ApolloDictionary app = new ApolloDictionary();
            app.List = ReadDictFiles.readAndBuildDictionary();
            _words = app.List;


            search_field.TextChanged += Search_field_TextChanged;
            

            //Add the list to the XAML
            list_words.ItemsSource = Listing();

            //Click on the item
            list_words.ItemTapped += List_words_ItemTapped;
        }

        public void ShowWordSection(Word item)
        {
            //WORKING var textFromFile = await file.ReadAllTextAsync();
            ReadDictFiles.readAndBuildDictionary();
            var textFromFile = "DELETAR";

            var text = new Label()
            {
                Text = textFromFile,
                FontSize = 18
            };

            Content = new StackLayout
            {
                Children =
                {
                    text
                }
            };
            //TODO IMPLEMENT HERE
        }

        private async void List_words_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            list_words.IsVisible = false; //hide the list of the View

            var item = e.Item as Word;

            //Testing how PCL works now:
            await CreateRealFileAsync();

            ShowWordSection(item);

            Application.Current.Properties["a"] = item.Name; //Add searched word into Properties
            //Application.Current.Properties.Keys.
            //TODO see the best way to use it

            //TODO save searched word http://stackoverflow.com/questions/31655327/how-can-i-save-some-user-data-locally-on-my-xamarin-forms-app
            
            
        }

        public async Task CreateRealFileAsync()
        {
            // get hold of the file system
            IFolder rootFolder = FileSystem.Current.LocalStorage;

            // create a folder, if one does not exist already
            IFolder folder = await rootFolder.CreateFolderAsync("MySubFolder", CreationCollisionOption.OpenIfExists);

            // create a file, overwriting any existing file
            //IFile file = await folder.CreateFileAsync("MyFile.txt", CreationCollisionOption.ReplaceExisting);
            this.file = await folder.GetFileAsync("MyFile.txt");

            // populate the file with some text
            //await file.WriteAllTextAsync("Sample Text...");
        }

        private void Search_field_TextChanged(object sender, TextChangedEventArgs e)
        {
            list_words.ItemsSource = Listing(search_field.Text);
        }

        public IEnumerable<GroupingList<char, Word>> Listing(string filter = "")
        {
            //TODO ajeitar o erro da busca OU buscar somente quando clicarem busca
            IEnumerable<Word> filtered_words = _words;

            if(!string.IsNullOrEmpty(filter))
                filtered_words = _words.Where(l => l.Name.ToLower().Contains(filter.ToLower()));

            return from word in filtered_words
                   orderby word.Name
                   group word by word.Name[0] into groups
                   select new GroupingList<char, Word>(groups.Key, groups);
        }
    }
}
