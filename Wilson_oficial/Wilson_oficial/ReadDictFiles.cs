using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Wilson_oficial
{
    class ReadDictFiles
    {
        
        public static List<Word> readAndBuildDictionary()
        {
            string resourcePrefix = null;

            #if __IOS__
                resourcePrefix = "Wilson_oficial.iOS.";
            #elif __ANDROID__
                resourcePrefix = "Wilson_oficial.Droid.";
            #endif

            //Get the files from the source folder
            var assembly = typeof(ReadDictFiles).GetTypeInfo().Assembly;
            Stream stream1 = assembly.GetManifestResourceStream(resourcePrefix + "Dictionaries.Governor_Dictionary.txt");
            Stream stream2 = assembly.GetManifestResourceStream(resourcePrefix + "Dictionaries.nhs-acronym-dictionary.txt");

            //read each file from Stream and make it into usable data
            var dictionary = new List<Word>();
            dictionary.AddRange(processWords(stream1));
            dictionary.AddRange(processWords(stream2));

            return dictionary;
        }

        private static List<Word> processWords(Stream stream)
        {
            var reader = new System.IO.StreamReader(stream);

            List<Word> dict = new List<Word>();

            string line;
            while((line = reader.ReadLine()) != null)
            {
                if(line.Contains(" -> "))
                {
                    //split the line between name -> definition
                    string[] separator = { " -> " };
                    string[] splitLine = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    string name = splitLine[0];
                    string definition = splitLine[1];

                    //add to the Dictionary
                    dict.Add(new Word { Name = name, Definition = definition });
                }
            }

            return dict;
        }
    }
}
