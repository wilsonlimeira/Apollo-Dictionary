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
        ApolloDictionary app;
        private List<WordDefinition> _words;
        private IFile file;

		public SearchPage ()
		{
			InitializeComponent ();

            //_words = new List<Word>();
            //_words.Add(new Word { Name = "NHS", Definition = "British health system" });
            //_words.Add(new Word { Name = "Heart Attack", Definition = "Heart disease"});
            app = new ApolloDictionary();
            app.List = ReadDictFiles.readAndBuildDictionary();
            _words = app.List;

            //TODO: Em um futuro distante, fazer a busca funcionar ao modificar o texto
            //search_field.TextChanged += Search_field_TextChanged;

            //Search when press the Search button
            search_field.SearchButtonPressed += Search_field_SearchButtonPressed;

            //When the person clicks to cancel the search or erase the word
            search_field.TextChanged += Search_field_TextChanged;

            //Add the list to the XAML
            list_words.ItemsSource = Listing();

            //Click on the item
            list_words.ItemTapped += List_words_ItemTapped;

            
            
        }


        public void ShowWordSection(WordDefinition item)
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

        private void List_words_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //showing an alert with the definition
            var clickedItem = e.Item as WordDefinition;
            var wordDef = app.getDefinitionsByName(clickedItem.Name);

            //in case of more than one definition, a text format will be made
            if(wordDef.Count > 1)
            {
                string defText = textFormat(wordDef);

                DisplayAlert(wordDef.First().Name, defText, "OK");
            }
            else
            {
                WordDefinition def = wordDef.First();
                DisplayAlert(def.Name, def.Definition, "OK");
            }
            //DisplayAlert(item.Name, item.Definition, "OK");

            //unmarking item
            ((ListView)sender).SelectedItem = null;

            /*
            list_words.IsVisible = false; //hide the list of the View

            var item = e.Item as WordDefinition;

            //Testing how PCL works now:
            await CreateRealFileAsync();

            ShowWordSection(item);

            Application.Current.Properties["a"] = item.Name; //Add searched word into Properties
            //Application.Current.Properties.Keys.
            //TODO see the best way to use it

            //TODO save searched word http://stackoverflow.com/questions/31655327/how-can-i-save-some-user-data-locally-on-my-xamarin-forms-app
            */

        }

        private static string textFormat(List<WordDefinition> wordDef)
        {
            string defText = "";
            int i = 1;

            foreach (WordDefinition def in wordDef)
            {
                //case the definition is already written there, to avoid duplicates
                if (!defText.ToLower().Contains(def.Definition.ToLower()))
                {
                    defText = defText + i + ") " + def.Definition + System.Environment.NewLine + System.Environment.NewLine;
                    i++;
                }
            }

            return defText;
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

        private void Search_field_SearchButtonPressed(object sender, EventArgs e)
        {
            list_words.ItemsSource = Listing(search_field.Text);
        }

        private void Search_field_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                list_words.ItemsSource = Listing();
            }
            //list_words.ItemsSource = Listing(e.NewTextValue);
        }

        public IEnumerable<GroupingList<char, WordDefinition>> Listing(string filter = "")
        {
            //TODO ajeitar o erro da busca OU buscar somente quando clicarem busca
            IEnumerable<WordDefinition> filtered_words = _words;

            if(!string.IsNullOrEmpty(filter))
                filtered_words = _words.Where(l => l.Name.ToLower().Contains(filter.ToLower()));

            return from word in filtered_words
                   orderby word.Name
                   group word by word.Name[0] into groups
                   select new GroupingList<char, WordDefinition>(groups.Key, groups);
        }
    }
}
