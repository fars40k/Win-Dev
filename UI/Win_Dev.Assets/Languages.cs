using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Win
{
    class LocalizedStrings
    {
        // ru-RU , en-GB

        public readonly Dictionary<string, string> AvalableLanguages = new Dictionary<string, string>
        {

            {"Русский (ru-RU)", "ru-RU" },
            {"English (en-GB)", "en-GB" }

        };

        public Uri GiveUriToLanguageDictionary(string culture)
        {
            switch (culture)
            {

                case "ru-RU":
                    return new Uri("pack://application:,,,/Win_Dev.Assets;component/Resources.ru-RU.xaml"
                        , UriKind.Relative);

                default:
                    return new Uri("pack://application:,,,/Win_Dev.Assets;component/Resources.xaml"
                        , UriKind.Relative);

            }
            // Caller should use something like this.Resources.MergedDictionaries.Add(dict.Source = returned);
        }

    }
}
