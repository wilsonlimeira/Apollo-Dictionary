using PCLStorage;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Wilson_oficial
{
    class ReadDictFiles
    {

        public static void readAndBuildDictionary(ApolloDictionary app)
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

            //get words from the personalized filesystem, which contains all the new words the user had added
            ReadUserWordsFile(app);

            //update main Dictionary
            app.List = dictionary;
        }

        private static async void ReadUserWordsFile(ApolloDictionary app)
        {
            var readText = await PCLHelper.ReadAllTextAsync("MyDictionary.txt", App.folder);
            
            if (!string.IsNullOrEmpty(readText))
            {
                //split the string into sequence of words
                string[] separator = { Environment.NewLine };
                string[] wordsAndDef = readText.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                //take each word and split it into Name, Definition and Category
                //from the file, which is in this model: word1 -> definition1 -> category1,category2,category3
                string[] separator2 = { " -> " };
                foreach (string wordDef in wordsAndDef)
                {
                    string[] word = wordDef.Split(separator2, StringSplitOptions.RemoveEmptyEntries);

                    WordDefinition newWord = new WordDefinition
                    {
                        Name = word[0],
                        Definition = word[1],
                        Category = word[2]
                    };

                    try
                    {
                        //word is added to the Dictionary
                        app.AddSingleWord = newWord;
                    }
                    catch (Exception e)
                    {
                        //don't do anything
                    }
                    
                }

                //Send a new message when the file was read
                MessagingCenter.Send(new ReadDictFiles(), "fileReadingDone");
            }
            else
            {
                //call function again until it gets a result from the file
                ReadUserWordsFile(app);
            }

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
