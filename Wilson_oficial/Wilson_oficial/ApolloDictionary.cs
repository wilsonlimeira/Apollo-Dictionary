using System;
using System.Collections.Generic;
using System.Text;

namespace Wilson_oficial
{
    class ApolloDictionary
    {
        private Dictionary<string, List<Word>> database = new Dictionary<string, List<Word>>();

        public List<Word> List
        {
            //return the dictionary as List of Words to make possible to exhibit the list on the screen 
            get
            {
                List<Word> dict = new List<Word>();

                foreach(string name in this.database.Keys)
                {
                    dict.Add(new Word { Name = name });
                }

                return dict;
            }

            set
            {
                foreach (Word word in value)
                {
                    //if the Word is already there, so add a new "Definition" (defined as Word class) to the Name
                    if(this.database.ContainsKey(word.Name))
                    {
                        this.database[word.Name].Add(word);
                    }
                    else
                    {
                        database.Add(word.Name, new List<Word>() { word });
                    }
                    
                }
            }
        }

        public Word AddSingleWord
        {
            set
            {
                database.Add(value.Name, new List<Word>() { value });
            }
        }

        public Dictionary<string, List<Word>> AddDictionary
        {
            set
            {
                this.database = value;
            }
        }

    }
}
