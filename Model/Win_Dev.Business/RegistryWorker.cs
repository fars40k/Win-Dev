using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Win_Dev.Business
{
    public static class RegistryWorker
    {
        public static string DefaultValue { get; private set; } = "en-GB";
        public static List<string> AvalableCultures = new List<string>();

        public static string ReadLanguageRegistryEntry()
        {
            RegistryKey currentUserKey = Registry.CurrentUser;

            try
            {
                // If the value retrieved from the registry is an unapproved culture or does not exist,
                // writes an entry with the default value

                currentUserKey.OpenSubKey("WinTaskManager", true);
                RegistryKey winTaskKey = currentUserKey.CreateSubKey("WinTaskManager");
                string fromRegistry = winTaskKey.GetValue("Language").ToString();
                if (AvalableCultures.Contains(fromRegistry))
                {
                    return fromRegistry;
                }

                throw new Exception();

            }
            catch (Exception e)
            {
                UpdateLanguageRegistryEntry(DefaultValue);
                return DefaultValue;
            }
        }

        public static void UpdateLanguageRegistryEntry(string newValue)
        {
            RegistryKey currentUserKey = Registry.CurrentUser;

            RegistryKey winTaskKey = currentUserKey.CreateSubKey("WinTaskManager");
            winTaskKey.SetValue("Language", newValue);           
        }

       
    }
}
