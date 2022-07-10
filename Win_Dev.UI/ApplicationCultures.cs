using System;
using System.Collections.Generic;
using System.Windows;

namespace Win_Dev.UI
{
    public class ApplicationCultures
    {
        public readonly List<string> Cultures = new List<string>() { "en-GB", "ru-RU" };

        public ResourceDictionary LocalisationDictionary { get; set; }

        public Uri MapCultureToResourceUri(string culture)
        {
            switch (culture)
            {
                case ("ru-RU"):
                    {
                        return new Uri("pack://application:,,,/Win_Dev.Assets;component/Language/Strings.ru-RU.xaml");
                        
                    } 
                    
                case ("en-US"):
                case ("en-GB"):
                default:
                    {
                        return new Uri("pack://application:,,,/Win_Dev.Assets;component/Language/Strings.xaml");
                        
                    }
            }
        }

    }
}
