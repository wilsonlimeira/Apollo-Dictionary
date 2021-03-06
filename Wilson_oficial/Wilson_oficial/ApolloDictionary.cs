﻿using PCLStorage;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wilson_oficial
{
    class ApolloDictionary
    {
        private static Dictionary<string, List<WordDefinition>> database = new Dictionary<string, List<WordDefinition>>();
        private static string new_words = "";

        public List<WordDefinition> List
        {
            //return the dictionary as List of Words to make possible to exhibit the list on the screen 
            get
            {
                List<WordDefinition> dict = new List<WordDefinition>();

                foreach(string name in database.Keys)
                {
                    string category = getDefinitionsByName(name)[0].Category;

                    dict.Add(new WordDefinition { Name = name, Category = category });
                }

                return dict;
            }

            set
            {
                foreach (WordDefinition definition in value)
                {
                    //if the Word is already there, so add a new Definition to the Name
                    if(database.ContainsKey(definition.Name))
                    {
                        database[definition.Name].Add(definition);
                    }
                    else
                    {
                        database.Add(definition.Name, new List<WordDefinition>() { definition });
                    }
                    
                }
            }
        }

        public List<WordDefinition> getDefinitionsByName(string name)
        {
            List<WordDefinition> def = null;

            if (database.ContainsKey(name))
            {
                def = database[name];
            }

            return def;
        }

        public WordDefinition AddSingleWord
        {
            set
            {
                //look for repeted words, avoid adding them
                if(!database.ContainsKey(value.Name))
                {
                    database.Add(value.Name, new List<WordDefinition>() { value });

                    //writing in the database to be added to a file later
                    //model used: word1 -> definition1 -> category1,category2,category3
                    new_words = new_words + value.Name + " -> " + value.Definition + " -> " + value.Category + Environment.NewLine;
                }
                else
                {
                    throw new Exception("This word is already in this dictionary");
                }
                
            }
        }

        public Dictionary<string, List<WordDefinition>> AddDictionary
        {
            set
            {
                database = value;
            }
        }

        public void writeNewWordsToStorage()
        {
            //writing in the file
            //model used: word1 -> definition1 -> category1,category2,category3
            PCLHelper.WriteTextAllAsync("MyDictionary.txt", new_words, App.folder);
        }

    }
}
