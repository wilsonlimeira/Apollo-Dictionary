﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Wilson_oficial.Pages
{
	public partial class SearchPage : ContentPage
	{
        private List<Word> _words;

		public SearchPage ()
		{
			InitializeComponent ();

            _words = new List<Word>();
            _words.Add(new Word { Name = "NHS", Definition = "British health system" });
            _words.Add(new Word { Name = "Heart Attack", Definition = "Heart disease"});

           
            search_field.TextChanged += Search_field_TextChanged;
            

            //Add the list to the XAML
            list_words.ItemsSource = Listing();

            //Click on the item
            list_words.ItemTapped += List_words_ItemTapped;
        }

        public void ShowWordSection()
        {
            Content = new StackLayout();
            //TODO IMPLEMENT HERE
        }

        private void List_words_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            list_words.IsVisible = false;

            ShowWordSection();
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
