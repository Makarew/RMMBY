using MelonLoader;
using UnityEngine;
using RMMBY.Editable;
using RMMBY.Helpers;
using System;
using System.Runtime.InteropServices;

namespace RMMBY
{
    public class Plugin : MelonMod
    {
        private bool inScene;
        private EnabledMods em;
        private InputHandler inputHandler;

        private bool holdConsoleToggle;
        private bool consoleHidden;
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private string infoText = "";
        private int infoType;
        private bool inInfo;

        public void LoadInfo(string message, int type)
        {
            infoText = message;
            infoType = type;

            LoadModMenu.CheckForBundle("RMMBYInfo");
        }

        public void LoadInfo(int type)
        {
            switch (type)
            {
                case 0:
                    LoadModMenu.CheckForBundle("RMMBYInfo");
                    break;
            }
        }

        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();

            RegistryHelper.ValidateRegistry();

            ShowWindow(GetConsoleWindow(), 0);
            consoleHidden = true;
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);

            if (em == null)
            {
                em = new GameObject().AddComponent<EnabledMods>();
                em.name = "RMMBY";
                em.gameObject.AddComponent<InputHandler>();
                inputHandler = em.gameObject.GetComponent<InputHandler>();
            }

            if (sceneName == ListenToLoadMenu.sceneToListen)
            {
                inScene = true;
                ListenToLoadMenu.OnSceneStart();
            }
            else
            {
                inScene = false;
            }

            if (sceneName == "RMMBYModMenu")
            {
                GameObject go = new GameObject();
                go.AddComponent<ModMenuHandler>();

                inputHandler.OnSceneLoaded();
                inputHandler.active = true;
            } else if (sceneName == "RMMBYInfo")
            {
                GameObject go = new GameObject();
                go.AddComponent<InfoMenuHandler>();

                inputHandler.OnSceneLoaded();
                inputHandler.active = true;

                inInfo = true;
            }
            else
            {
                inputHandler.active = false;
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (inInfo) SetInfo();

            if (inScene && ListenToLoadMenu.runOnUpdate)
            {
                ListenToLoadMenu.OnSceneUpdate();
            }

            if(!inputHandler.toggleConsole) {
                holdConsoleToggle = false;
            }

            if (inputHandler.toggleConsole && !holdConsoleToggle)
            {
                switch (consoleHidden)
                {
                    case true:
                        ShowWindow(GetConsoleWindow(), 5);
                        consoleHidden = false;
                        break;
                    case false:
                        ShowWindow(GetConsoleWindow(), 0);
                        consoleHidden = true;
                        break;
                }
                
                holdConsoleToggle = true;
            }
        }

        public void SetInfo()
        {
            if (!GameObject.FindObjectOfType<InfoMenuHandler>()) return;

            switch (infoType)
            {
                case 0:
                    GameObject.FindObjectOfType<InfoMenuHandler>().SetupRestart(infoText);
                    break;
            }

            inInfo = false;
        }
    }
}
