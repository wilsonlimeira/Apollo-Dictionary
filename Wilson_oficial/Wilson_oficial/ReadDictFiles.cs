using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Wilson_oficial
{
    class ReadDictFiles
    {
        
        public static List<WordDefinition> readAndBuildDictionary()
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
            var dictionary = new List<WordDefinition>();
            dictionary.AddRange(processWords(stream1));
            dictionary.AddRange(processWords(stream2));

            return dictionary;
        }

        private static List<WordDefinition> processWords(Stream stream)
        {
            var reader = new System.IO.StreamReader(stream);

            List<WordDefinition> dict = new List<WordDefinition>();

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
                    string category = "Other";

                    //divide elements in Acronyms if they have only uppercase letters
                    string exp = @"^[A-Z]+[a-z\&\/]*[A-Z]+[a-z]*$"; //@"[A-Z][a-z]+\s*[a-zA-Z]*"
                    if (Regex.Match(name, exp).Success)
                    {
                        category = "Acronyms";
                    }

                    //add to the Dictionary
                    dict.Add(new WordDefinition { Name = name, Definition = definition, Category = category });
                }
            }

            return dict;
        }
    }
}
