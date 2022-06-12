using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Win_Dev.UI.Interfaces;

namespace Win_Dev.UI
{
    class RegistryWorker : IRegistryData
    {
        public string DefaultValue { get; private set; } = "en-GB";

        public RegistryWorker()
        {
            
        }

        public string ReadLanguageRegistryEntry()
        {
            RegistryKey currentUserKey = Registry.CurrentUser;

            try
            {
                // If the value retrieved from the registry is an unapproved culture or does not exist,
                // writes an entry with the default value

                currentUserKey.OpenSubKey("WinTaskManager", true);
                RegistryKey winTaskKey = currentUserKey.CreateSubKey("WinTaskManager");
                string fromRegistry = winTaskKey.GetValue("Language").ToString();
                if (ApplicationCultures.Cultures.Contains(fromRegistry))
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

        public void UpdateLanguageRegistryEntry(string newValue)
        {
            RegistryKey currentUserKey = Registry.CurrentUser;

            RegistryKey winTaskKey = currentUserKey.CreateSubKey("WinTaskManager");
            winTaskKey.SetValue("Language", newValue);           
        }

       
    }
}
