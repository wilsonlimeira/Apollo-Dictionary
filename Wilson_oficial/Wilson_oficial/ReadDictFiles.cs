using PCLStorage;
using System.Threading.Tasks;
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
        private static IFile userFile;
        private static List<WordDefinition> userList;

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

            //get words from the personalized filesystem, which contains all the new words the user had added
            var aux = ReadUserWordsFile();
            dictionary.AddRange(userList);

            return dictionary;
        }

        private static async Task ReadUserWordsFile()
        {
            var readText = await userFile.ReadAllTextAsync();

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

                userList.Add(newWord);
            }
        }

        private async Task CreateRealFileAsync()
        {
            // get hold of the file system
            IFolder rootFolder = FileSystem.Current.LocalStorage;

            // create a folder, if one does not exist already
            IFolder folder = await rootFolder.CreateFolderAsync("MySubFolder", CreationCollisionOption.OpenIfExists);

            // create a file, overwriting any existing file
            //IFile file = await folder.CreateFileAsync("MyFile.txt", CreationCollisionOption.ReplaceExisting);
            userFile = await folder.GetFileAsync("MyWords.txt");

            // populate the file with some text
            //await file.WriteAllTextAsync("Sample Text...");
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
