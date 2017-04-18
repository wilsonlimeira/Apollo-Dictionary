using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Wilson_oficial
{
    class UserProperties
    {
        //TODO: eliminar adicao de elementos repetidos e colocar a lista functionando como uma pilha
        public static List<string> HistoryList { get; set; }

        public UserProperties()
        {
            HistoryList = new List<string>();
        }

        //save the current list into storage
        public void SaveList()
        {
            if(HistoryList != null && HistoryList.Count > 0)
            {
                var list = serializeWords();

                Application.Current.Properties["historyList"] = list;
            }
        }

        //get list from storage
        public void ObtainList()
        {
            if (Application.Current.Properties.ContainsKey("historyList"))
            {
                var list = Application.Current.Properties["historyList"] as string;

                unserializeWords(list);
            }
        }

        //transform the whole list into a single string (e.g: {AHE, ATP, Good Morning} becomes "AHE|ATP|Good Morning")
        private string serializeWords()
        {
            string serialString = string.Empty;

            foreach(var word in HistoryList)
            {
                serialString = serialString + "|" + word;
            }

            return serialString;
        }

        //transform the single string into a List<string>
        private void unserializeWords(string list)
        {
            string[] separator = { "|" };
            string[] splitLine = list.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in splitLine)
            {
                if(!HistoryList.Contains(word)) //check for duplicates
                    HistoryList.Add(word);
            }
        }
    }
}
