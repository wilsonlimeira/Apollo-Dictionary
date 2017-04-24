using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace Wilson_oficial.Pages
{
	public class HomePage : TabbedPage
    {
		public HomePage ()
		{
            Title = "Apollo";

            Children.Add(new SearchPage()
            {
                Title = "Search",
                Icon = "Search_50.png"
            });

            Children.Add(new CategoriesPage()
            {
                Title = "Categories",
                Icon = "Category_50.png"
            });

            Children.Add(new HistoryPage()
            {
                Title = "History",
                Icon = "Past_50.png"
            });
        }
	}
}
