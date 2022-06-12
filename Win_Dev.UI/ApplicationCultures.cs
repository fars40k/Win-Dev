using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Win_Dev.UI
{
    internal static class ApplicationCultures
    {
        public static readonly List<string> Cultures = new List<string>() { "en-GB", "ru-RU" };

        public static ResourceDictionary LocalisationDictionary { get; set; }

        public static Uri MapCultureToResourceUri(string culture)
        {
            switch (culture)
            {
                case ("ru-RU"):
                    {
                        return new Uri("pack://application:,,,/Win_Dev.Assets;component/Language/Strings.ru-RU.xaml");
                        break;
                    } 
                    
                case ("en-US"):
                case ("en-GB"):
                default:
                    {
                        return new Uri("pack://application:,,,/Win_Dev.Assets;component/Language/Strings.xaml");
                        break;
                    }
            }
        }

    }
}
