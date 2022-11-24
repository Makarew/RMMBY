using HarmonyLib;
using UnityEngine;
using System;

// Game Specific - Replace For New Game
using RWBY.UI.Title;
using RTLTMPro;

namespace RMMBY.Editable
{
    // Use This Script To Load The Mod Menu
    internal class ListenToLoadMenu
    {
        // Do Not Delete Variables - Replace Values As Required
        // sceneToListen Is The Name Of The Scene That Must Be Loaded For OnSceneStart And OnSceneUpdate To Run
        public static string sceneToListen = "Title";
        // Set To True If You Need Code To Run Every Frame While sceneToListen Is Loaded
        public static bool runOnUpdate = true;
        // Scene RMMBY Will Go To When Exiting The Mod Menu
        public static string sceneToReturnTo = "Title";

        // Will Run Once When sceneToListen Is Loaded
        public static void OnSceneStart()
        {

        }

        // If runOnUpdate, This Will Run Every Frame While sceneToListen Is Loaded
        public static void OnSceneUpdate()
        {
            // Example Used For RWBY: Arrowfell
            // RWBY: Arrowfell Replaces ControllerConfigButton To Load The Mod Menu
            // This Code Must Run Every Frame Because The Game Will Automatically Change The Text The Button Has
            if (GameObject.Find("ControllerConfigButton"))
                GameObject.Find("ControllerConfigButton").transform.Find("DescriptionText").GetComponent<RTLTextMeshPro>().text = "Mods";
        }

        // Example Harmony Patch Used For RWBY: Arrowfell
        // Run LoadModMenu.CheckForBundle() To Load RMMBYs Mod Menu
        [HarmonyPatch(typeof(TitleOptionsHandler), "ShowKeybindScreen", new Type[] { typeof(bool) })]
        private static class Patch
        {
            private static bool Prefix(ref bool enable)
            {
                LoadModMenu.CheckForBundle();

                return false;
            }
        }
    }
}