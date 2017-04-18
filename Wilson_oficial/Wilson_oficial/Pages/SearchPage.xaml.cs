using PCLStorage;
using Rg.Plugins.Popup;
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
        private IFile file;

        //Layout parameters
        ListView list_words;
        SearchBar search_field;
        StackLayout layout;
        Button addNewItemButton;

        public SearchPage ()
		{
			InitializeComponent ();
            
            app = new ApolloDictionary();

            //Creating ListView
            list_words = new ListView
            {
                ItemTemplate = new DataTemplate(typeof(TextCell))
                {
                    Bindings =
                    {
                        { TextCell.TextProperty, new Binding ("Name") }
                    }
                },

                GroupDisplayBinding = new Binding("Key"),
                GroupShortNameBinding = new Binding("Key"),
                IsGroupingEnabled = true,
                ItemsSource = Listing()
            };

            //Add New Item Button
            addNewItemButton = new Button
            {
                Text = "Add New Word",
            };

            //Creating SearchBar
            search_field = new SearchBar
            {
                Placeholder = "Search here...",
                WidthRequest = 225
            };

            //Adding items to Content
            layout = new StackLayout
            {
                Margin = new Thickness(0, 5, 0, 0),
                Spacing = 5,

                Children =
                {
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children = { search_field, addNewItemButton }
                    },
                    list_words
                }
            };
            Content = layout;

            //Search when press the Search button
            search_field.SearchButtonPressed += Search_field_SearchButtonPressed;

            //When the person clicks to cancel the search or erase the word
            search_field.TextChanged += Search_field_TextChanged;

            //Click on the item
            list_words.ItemTapped += List_words_ItemTapped;

            //Click on Add New Item Button
            addNewItemButton.Clicked += AddNewItemButton_Clicked;

            //When a new word is added the list is refreshed
            MessagingCenter.Subscribe<MyPopupPage>(this, "newWord", (sender) => {
                layout.Children.Remove(list_words);
                list_words.ItemsSource = Listing();
                layout.Children.Add(list_words);
            });

            //When reading the user words from file, updates the list
            MessagingCenter.Subscribe<ReadDictFiles>(this, "fileReadingDone", (sender) => {
                layout.Children.Remove(list_words);
                list_words.ItemsSource = Listing();
                layout.Children.Add(list_words);
            });

        }

        private async void AddNewItemButton_Clicked(object sender, EventArgs e)
        {
            var page = new MyPopupPage();

            await Navigation.PushModalAsync(page);
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

            //unmarking item
            ((ListView)sender).SelectedItem = null;

            //add the clicked item to history list if it's not duplicated
            if(!UserProperties.HistoryList.Contains(clickedItem.Name))
                UserProperties.HistoryList.Push(clickedItem.Name);

            //Send a message to HistoryPage to refresh their screen with this new word
            MessagingCenter.Send<SearchPage>(this, "newWordHistory");

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
            this.file = await folder.GetFileAsync("MyFile.txt");
        }

        private void Search_field_SearchButtonPressed(object sender, EventArgs e)
        {
            list_words.ItemsSource = Listing(search_field.Text);
        }

        private void Search_field_TextChanged(object sender, TextChangedEventArgs e)
        {
            layout.Children.Remove(list_words);
            list_words.ItemsSource = Listing(e.NewTextValue);
            layout.Children.Add(list_words);
        }

        public IEnumerable<GroupingList<char, WordDefinition>> Listing(string filter = "")
        {
            List<WordDefinition> _words = app.List;
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
