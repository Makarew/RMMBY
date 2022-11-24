using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using MelonLoader;

namespace RMMBY
{
    internal class LoadModMenu
    {
        private static AssetBundle bundle;
        public static string scene;

        private static void GetModMenu()
        {
            SceneManager.LoadScene(scene);
        }

        public static void CheckForBundle()
        {
            if (bundle == null)
            {
                string path = Path.Combine(MelonHandler.ModsDirectory, "RMMBY/rmmby");
                bundle = AssetBundle.LoadFromFile(path);
                string[] scenePaths = bundle.GetAllScenePaths();
                scene = Path.GetFileNameWithoutExtension(scenePaths[0]);
            }

            GetModMenu();
        }
    }
}