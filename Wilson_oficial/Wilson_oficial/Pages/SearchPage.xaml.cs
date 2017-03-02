using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Wilson_oficial.Pages
{
	public partial class SearchPage : ContentPage
	{
        private List<Word> _word;

		public SearchPage ()
		{
			InitializeComponent ();

            _word = new List<Word>();
            _word.Add(new Word { Name = "NHS", Definition = "British health system" });
            _word.Add(new Word { Name = "Heart Attack", Definition = "Heart disease"});

            this.list_words.ItemsSource = this.Listing();

            /*var search_field = new SearchBar
            {
                Placeholder = "Search here..."
            };

            Content = new StackLayout //TODO maybe change this for GRID, people said it's faster
            {
                Children =
                {
                    search_field
                }
            };*/
        }

        public IEnumerable<GroupingList<char, Word>> Listing()
        {
            return from word in _word
                   orderby word.Name
                   group word by word.Name[0] into groups
                   select new GroupingList<char, Word>(groups.Key, groups);
        }
    }
}
