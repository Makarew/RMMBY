using MelonLoader;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RMMBY
{
    public class EnabledMods : MonoBehaviour
    {
        private string path;
        public List<string> enabledPaths = new List<string>();

        private void Start()
        {
            DontDestroyOnLoad(this);
            GetPath();
            GetEnabledPaths();
            LoadAllEnabled();
        }

        private void LoadAllEnabled()
        {
            for (int i = 0; i < enabledPaths.Count; i++)
            {
                RegisterMelon(enabledPaths[i]);
            }
        }

        public void GetPath()
        {
            path = Path.Combine(MelonHandler.ModsDirectory, "RMMBY/data");
        }

        public void GetEnabledPaths()
        {
            StreamReader r = new StreamReader(path);

            enabledPaths.Clear();

            string line;
            using (r)
            {
                do
                {
                    //Read a line
                    line = r.ReadLine();
                    if (line != null)
                    {
                        //Divide line into basic structure
                        string[] lineData = line.Split(';');
                        //Check the line type
                        if (lineData[0] == "enabledmod")
                        {
                            enabledPaths.Add(lineData[1]);
                        }
                    }
                }
                while (line != null);
                //Stop reading the file
                r.Close();
            }
        }

        internal void AddNewEnabledPath(string newpath)
        {
            if (CheckEnabled(newpath)) return;

            for (int i = 0; i < enabledPaths.Count; i++)
            {
                if (enabledPaths[i] == newpath) return;
            }

            WriteToFile.WriteFile(path, new string[1] { "enabledmod;" + newpath }, true);

            GetEnabledPaths();

            RegisterMelon(newpath);
        }

        internal void RemoveEnabledPath(string newpath)
        {
            if (!CheckEnabled(newpath)) return;

            WriteToFile.ReplaceLine(path, newpath, "", 1, true);

            GetEnabledPaths();

            UnregisterMelon(newpath);
        }

        public bool CheckEnabled(string newpath)
        {
            for (int i = 0; i < enabledPaths.Count; i++)
            {
                if (enabledPaths[i] == newpath)
                {
                    return true;
                }
            }

            return false;
        }

        public static void RegisterMelon(string newpath)
        {
            foreach (MelonBase melonBase in MelonAssembly.LoadMelonAssembly(newpath, true).LoadedMelons)
            {
                if (melonBase.Registered)
                {
                    Melon<Plugin>.Logger.Msg("Melon '" + melonBase.Info.Name + "' Already Registered");
                }
                else
                {
                    melonBase.Register();
                    Melon<Plugin>.Logger.Msg("Registered Melon: " + melonBase.Info.Name);
                }
            }
        }

        public static void UnregisterMelon(string newpath)
        {
            foreach (MelonAssembly melonAssembly in MelonAssembly.LoadedAssemblies)
            {
                bool flag = melonAssembly.Location == newpath;
                if (flag)
                {
                    foreach (MelonBase melonBase in melonAssembly.LoadedMelons)
                    {
                        melonBase.Unregister(null, false);

                        Melon<Plugin>.Logger.Msg("Unregistered Melon: " + melonBase.Info.Name);
                    }
                }
            }
        }
    }
}