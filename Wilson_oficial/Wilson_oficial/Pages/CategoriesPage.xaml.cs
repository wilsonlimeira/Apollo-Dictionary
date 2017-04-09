using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Wilson_oficial.Pages
{
	public partial class CategoriesPage : ContentPage
	{
        ApolloDictionary app;
        private List<WordDefinition> _words;
        private bool isCategoriesOnScreen;
        ListView categories = new ListView();

        //Layout parameters
        ListView list_words;
        SearchBar search_field;
        StackLayout layout;

        public CategoriesPage ()
		{
			InitializeComponent ();

            app = new ApolloDictionary();
            app.List = ReadDictFiles.readAndBuildDictionary();
            _words = app.List;

            //Creating ListView
            list_words = ShowListCategories();
            isCategoriesOnScreen = true;
            /*
            list_words = new ListView
            {
                ItemTemplate = new DataTemplate(typeof(TextCell))
                {
                    Bindings = {
                            { TextCell.TextProperty, new Binding ("Name") }
                        }
                },

                GroupDisplayBinding = new Binding("Key"),
                GroupShortNameBinding = new Binding("Key"),
                IsGroupingEnabled = true,
                ItemsSource = Listing(),
            };*/

            //Creating SearchBar
            search_field = new SearchBar
            {
                Placeholder = "Search here..."
            };

            //Adding items to Content
            layout = new StackLayout
            {
                Padding = new Thickness(5, 5, 5, 5),
                Children = { search_field, list_words }
            };
            Content = layout;

            //Search when press the Search button
            search_field.SearchButtonPressed += Search_field_SearchButtonPressed;

            //When the person clicks to cancel the search or erase the word
            search_field.TextChanged += Search_field_TextChanged;

            //Click on the item
            list_words.ItemTapped += List_words_ItemTapped;

            
        }
        
        private ListView ShowListCategories()
        {
            
            SortedSet<string> catAux = new SortedSet<string>();

            foreach(WordDefinition def in _words)
            {
                if(!catAux.Contains(def.Category))
                {
                    catAux.Add(def.Category);
                }
            }
            categories.ItemsSource = catAux.ToList();

            return categories;
        }

        private void Search_field_TextChanged(object sender, TextChangedEventArgs e)
        {
            layout.Children.Remove(list_words);
            list_words.ItemsSource = Listing(e.NewTextValue);
            layout.Children.Add(list_words);
        }

        private void Search_field_SearchButtonPressed(object sender, EventArgs e)
        {
            list_words.ItemsSource = Listing(search_field.Text);
        }

        public IEnumerable<GroupingList<string, WordDefinition>> Listing(string filter = "")
        {
            IEnumerable<WordDefinition> filtered_words = _words;

            //check if filter is not null or empty string
            if (!string.IsNullOrEmpty(filter))
                filtered_words = _words.Where(l => l.Name.ToLower().Contains(filter.ToLower()));

            return from word in filtered_words
                   orderby word.Category
                   group word by word.Category into groups
                   select new GroupingList<string, WordDefinition>(groups.Key, groups);
        }

        public IEnumerable<GroupingList<string, WordDefinition>> ListingByCategory(string filter = "")
        {
            IEnumerable<WordDefinition> filtered_words = _words;

            //check if filter is not null or empty string
            if (!string.IsNullOrEmpty(filter))
                filtered_words = _words.Where(l => l.Category.ToLower().Contains(filter.ToLower())); //filter by Category

            return from word in filtered_words
                   orderby word.Category
                   group word by word.Category into groups
                   select new GroupingList<string, WordDefinition>(groups.Key, groups);
        }

        private void List_words_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            if (isCategoriesOnScreen)
            {
                var clickedItem = e.Item as string;

                //remove list on the screen
                layout.Children.Remove(list_words);

                list_words = new ListView
                {
                    ItemTemplate = new DataTemplate(typeof(TextCell))
                    {
                        Bindings = {
                            { TextCell.TextProperty, new Binding ("Name") }
                        }
                    },

                    GroupDisplayBinding = new Binding("Key"),
                    GroupShortNameBinding = new Binding("Key"),
                    IsGroupingEnabled = true,
                    ItemsSource = ListingByCategory(clickedItem), //Listing by Categories here
                };

                //add on screen after setup
                layout.Children.Add(list_words);
                //call this same method again, because now there is a new list on Screen, then a new method is required
                list_words.ItemTapped += List_words_ItemTapped;
                //prevent to get in this IF statement again
                isCategoriesOnScreen = false;
            }
            else
            {
                //showing an alert with the definition
                var clickedItem = e.Item as WordDefinition;
                var wordDef = app.getDefinitionsByName(clickedItem.Name);

                //in case of more than one definition, a text format will be made
                if (wordDef.Count > 1)
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
            }


            //unmarking item
            ((ListView)sender).SelectedItem = null;
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

        protected override bool OnBackButtonPressed()
        {
            //if list is on the screen and back button was pressed, then put Categories on the screen
            if(!isCategoriesOnScreen)
            {
                layout.Children.Remove(list_words);
                list_words = ShowListCategories();
                layout.Children.Add(list_words);
                isCategoriesOnScreen = true;

                return true;
            }

            return base.OnBackButtonPressed();
        }
    }
}
