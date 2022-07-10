using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Win_Dev.Business
{
    public class RegistryWorker
    {
        public string DefaultValue { get; private set; } = "en-GB";
        public List<string> AvalableCultures = new List<string>();

        public string ReadLanguageRegistryEntry()
        {
            RegistryKey currentUserKey = Registry.CurrentUser;

            try
            {
                // If the value retrieved from the registry is an unapproved culture or does not exist,
                // writes an entry with the default value

                currentUserKey.OpenSubKey("WinTaskManager", true);
                RegistryKey winTaskKey = currentUserKey.CreateSubKey("WinTaskManager");

                object fromRegistry = winTaskKey.GetValue("Language");
                if ((fromRegistry != null) && (AvalableCultures.Contains(fromRegistry.ToString())))
                {
                    return fromRegistry.ToString();
                }

                throw new Exception();

            }
            catch 
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
