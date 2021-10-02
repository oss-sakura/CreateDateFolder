using Microsoft.Win32;

namespace CreateDateFolder
{
    public static class CreateDateFolderRegistry
    {
        /// <summary>
        /// Set the key and value in the registry.
        /// </summary>
        /// <param name="regkey">Registry.[ClassesRoot|LocalMachine|CurrentUser|etc]</param>
        /// <param name="subkey">CreateSubKey(key)</param>
        /// <param name="name">SetValue(name,value)</param>
        /// <param name="value">SetValue(null,value)</param>
        public static void CreateSubKeyAndSetValue(RegistryKey regkey, string subkey, string name, object value)
        {
            using (RegistryKey regk = regkey.CreateSubKey(subkey))
            {
                regk.SetValue(name, value);
            }
        }

        /// <summary>
        /// Deletes a subkey and any child subkeys recursively.
        /// </summary>
        /// <param name="regkey">Registry.[ClassesRoot|LocalMachine|CurrentUser|etc]</param>
        /// <param name="subkey">DeleteSubKeyTree(subkey)</param>
        public static void DeleteSubKeyTree(RegistryKey regkey, string subkey)
        {
            regkey.DeleteSubKeyTree(subkey);
        }

        /// <summary>
        /// Registry key exists?
        /// </summary>
        /// <param name="regkey">Registry.[ClassesRoot|LocalMachine|CurrentUser|etc]</param>
        /// <param name="key"></param>
        /// <returns>true: found, false not found.</returns>
        public static bool RegistryExists(RegistryKey regkey, string key)
        {
            bool found = false;
            using (RegistryKey reg = regkey.OpenSubKey(key))
            {
                if (reg != null)
                {
                    found = true;
                }
            }
            return found;
        }

        /// <summary>
        /// Run as administrator?
        /// </summary>
        /// <returns>true: administrator, false: not administrator</returns>
        public static bool IsAdministrator()
        {
            System.Security.Principal.WindowsIdentity identity
                = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal
                = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }
    }
}
