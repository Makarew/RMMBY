using System.IO;
using MelonLoader;

namespace RMMBY.Helpers
{
    public class RegistryHelper
    {
        public enum RegistryKey
        {
            HKEY_CLASSES_ROOT,
            HKEY_CURRENT_USER,
            HKEY_LOCAL_MACHINE,
            HKEY_USERS,
            HKEY_CURRENT_CONFIG,
            HKEY_PERFORMANCE_DATA
        }

        public static string UrlProtocol;
        public static string RegistryKeyRoot;
        public static string RegistryKeyShell;
        public static string RegistryValueRootDefault;
        public static readonly RegistryKey Registry = RegistryKey.HKEY_CURRENT_USER;


        public static void ValidateRegistry()
        {
            if (DataReader.ReadData("GBOneClick") != "Enabled") return;

            string ShellCommand = "\"" + Path.Combine(MelonHandler.ModsDirectory, "RMMBY", DataReader.ReadData("GBOneClickExe")) + "\" \"%1\"";

            UrlProtocol = DataReader.ReadData("GBSchema");
            RegistryKeyRoot = string.Format("SOFTWARE\\Classes\\{0}", UrlProtocol);
            RegistryKeyShell = string.Format("{0}\\shell\\open\\command", RegistryKeyRoot);
            RegistryValueRootDefault = DataReader.ReadData("GBRegDefault");

            if (KeyExists(Registry, RegistryKeyShell) && ValueExists<string>(Registry, RegistryKeyShell, ShellCommand, true))
            {
                return;
            }

            Write(Registry, RegistryKeyShell);
            Write<string>(Registry, RegistryKeyRoot, RegistryValueRootDefault, true);
            Write<string>(Registry, Path.Combine(RegistryKeyRoot, "URL Protocol"), string.Empty, false);
            Write<string>(Registry, RegistryKeyShell, ShellCommand, true);
        }

        public static T Read<T>(RegistryKey key, string path, bool @default = false)
        {
            Microsoft.Win32.RegistryKey registryKey = GetRegistryByKey(key).OpenSubKey(@default ? path : Path.GetDirectoryName(path));
            T result = (T)((object)registryKey.GetValue(@default ? string.Empty : Path.GetFileName(path)));
            registryKey.Close();
            return result;
        }

        public static void Write(RegistryKey key, string path)
        {
            GetRegistryByKey(key).CreateSubKey(path).Close();
        }

        public static void Write<T>(RegistryKey key, string path, T value, bool @default = false)
        {
            Microsoft.Win32.RegistryKey registryKey = GetRegistryByKey(key).CreateSubKey(@default ? path : Path.GetDirectoryName(path));
            registryKey.SetValue(@default ? string.Empty : Path.GetFileName(path), value);
            registryKey.Close();
        }

        public static bool KeyExists(RegistryKey key, string path)
        {
            return GetRegistryByKey(key).OpenSubKey(path) != null;
        }

        public static bool ValueExists<T>(RegistryKey key, string path, T expectedValue, bool @default = false)
        {
            T t = Read<T>(key, path, @default);
            return t.Equals(expectedValue);
        }

        private static Microsoft.Win32.RegistryKey GetRegistryByKey(RegistryKey key)
        {
            switch (key)
            {
                case RegistryKey.HKEY_CLASSES_ROOT:
                    return Microsoft.Win32.Registry.ClassesRoot;
                case RegistryKey.HKEY_LOCAL_MACHINE:
                    return Microsoft.Win32.Registry.LocalMachine;
                case RegistryKey.HKEY_USERS:
                    return Microsoft.Win32.Registry.Users;
                case RegistryKey.HKEY_CURRENT_CONFIG:
                    return Microsoft.Win32.Registry.CurrentConfig;
                case RegistryKey.HKEY_PERFORMANCE_DATA:
                    return Microsoft.Win32.Registry.PerformanceData;
            }
            return Microsoft.Win32.Registry.CurrentUser;
        }
    }
}
