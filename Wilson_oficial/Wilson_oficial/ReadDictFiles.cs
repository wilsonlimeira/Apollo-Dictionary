using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Wilson_oficial
{
    class ReadDictFiles
    {
        
        public static string read()
        {
            #if __IOS__
                string resourcePrefix = "Wilson_oficial.iOS.";
            #elif __ANDROID__
                string resourcePrefix = "Wilson_oficial.Droid.";
            #endif

            var assembly = typeof(ReadDictFiles).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(resourcePrefix + "Dictionaries.Test.txt");

            return new System.IO.StreamReader(stream).ReadLine();
        }
    }
}
