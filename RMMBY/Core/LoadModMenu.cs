using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using MelonLoader;

namespace RMMBY
{
    internal class LoadModMenu
    {
        private static AssetBundle bundle;
        public static string scene;

        public static void CheckForBundle(string sceneName)
        {
            if (bundle == null)
            {
                string path = Path.Combine(MelonHandler.ModsDirectory, "RMMBY/rmmby");
                bundle = AssetBundle.LoadFromFile(path);
            }

            SceneManager.LoadScene(sceneName);
        }
    }
}