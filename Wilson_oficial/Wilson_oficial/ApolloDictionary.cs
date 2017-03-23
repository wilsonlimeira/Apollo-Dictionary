﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Wilson_oficial
{
    class ApolloDictionary
    {
        private Dictionary<string, List<WordDefinition>> database = new Dictionary<string, List<WordDefinition>>();

        public List<WordDefinition> List
        {
            //return the dictionary as List of Words to make possible to exhibit the list on the screen 
            get
            {
                List<WordDefinition> dict = new List<WordDefinition>();

                foreach(string name in this.database.Keys)
                {
                    dict.Add(new WordDefinition { Name = name });
                }

                return dict;
            }

            set
            {
                foreach (WordDefinition definition in value)
                {
                    //if the Word is already there, so add a new Definition to the Name
                    if(this.database.ContainsKey(definition.Name))
                    {
                        this.database[definition.Name].Add(definition);
                    }
                    else
                    {
                        database.Add(definition.Name, new List<WordDefinition>() { definition });
                    }
                    
                }
            }
        }

        public WordDefinition AddSingleWord
        {
            set
            {
                database.Add(value.Name, new List<WordDefinition>() { value });
            }
        }

        public Dictionary<string, List<WordDefinition>> AddDictionary
        {
            set
            {
                this.database = value;
            }
        }

    }
}
