using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Wilson_oficial.Pages
{
	public partial class HistoryPage : ContentPage
	{
        ApolloDictionary app;
        ListView historyList;

        StackLayout layout;

		public HistoryPage ()
		{
			InitializeComponent ();

            app = new ApolloDictionary();

            //Creating ListView
            historyList = new ListView();
            historyList.ItemsSource = UserProperties.HistoryList;

            layout = new StackLayout
            {
                Padding = new Thickness(5, 5, 5, 5),
                Children = { historyList }
            };
            Content = layout;


            historyList.ItemTapped += HistoryList_ItemTapped;
        }

        

        private void HistoryList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //showing an alert with the definition
            var clickedItem = e.Item as string;
            var wordDef = app.getDefinitionsByName(clickedItem);

            //in case of more than one definition, a text format will be made
            if (wordDef.Count > 1)
            {
                string defText = textFormat(wordDef);

                DisplayAlert(wordDef.First().Name, defText, "OK"); //show alert on screen
            }
            else
            {
                WordDefinition def = wordDef.First();

                DisplayAlert(def.Name, def.Definition, "OK"); //show alert on screen
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
    }
}
