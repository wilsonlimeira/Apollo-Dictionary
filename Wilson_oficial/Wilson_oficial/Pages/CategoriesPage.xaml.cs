﻿using System;
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

        public CategoriesPage ()
		{
			InitializeComponent ();

            app = new ApolloDictionary();
            app.List = ReadDictFiles.readAndBuildDictionary();
            _words = app.List;

            //Search when press the Search button
            search_field.SearchButtonPressed += Search_field_SearchButtonPressed;

            //When the person clicks to cancel the search or erase the word
            search_field.TextChanged += Search_field_TextChanged;

            //Add the list to the XAML
            list_words.ItemsSource = Listing();

            //Click on the item
            list_words.ItemTapped += List_words_ItemTapped;
        }

        private void Search_field_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                list_words.ItemsSource = Listing();
            }
        }

        private void Search_field_Unfocused(object sender, FocusEventArgs e)
        {
            list_words.ItemsSource = Listing();
        }

        private void Search_field_SearchButtonPressed(object sender, EventArgs e)
        {
            list_words.ItemsSource = Listing(search_field.Text);
        }

        public IEnumerable<GroupingList<string, WordDefinition>> Listing(string filter = "")
        {
            IEnumerable<WordDefinition> filtered_words = _words;

            if (!string.IsNullOrEmpty(filter))
                filtered_words = _words.Where(l => l.Name.ToLower().Contains(filter.ToLower()));

            return from word in filtered_words
                   orderby word.Category
                   group word by word.Category into groups
                   select new GroupingList<string, WordDefinition>(groups.Key, groups);
        }

        private void List_words_ItemTapped(object sender, ItemTappedEventArgs e)
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
    }
}
