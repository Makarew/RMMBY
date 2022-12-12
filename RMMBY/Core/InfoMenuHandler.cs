using RMMBY.Editable;
using RMMBY.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace RMMBY
{
    internal class InfoMenuHandler : MonoBehaviour
    {
        public string sceneToLoad = "";
        public int sceneToLoadIndex;

        // 0 = Restart To Unload | 1 = Plugin Update | 2 = Message
        public int infoType;
        // 0 = None | 1 = String | 2 = Index
        public int sceneToLoadType;

        public bool setup;

        private Text textBox;
        private Text optionText;

        private InputHandler inputHandler;

        public void SetupRestart(string message)
        {
            infoType = 0;
            sceneToLoadType = 0;

            textBox = GameObject.Find("TextBox").GetComponent<Text>();
            optionText = GameObject.Find("SelectText").GetComponent<Text>();

            textBox.text = message;
            optionText.text = "Restart";

            inputHandler = FindObjectOfType<InputHandler>();

            SetResolution();

            setup = true;
        }

        public void RestartUpdate()
        {
            if (inputHandler.select)
            {
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
                Application.Quit();
            }
        }

        public void Update()
        {
            if (!setup) return;

            switch (infoType)
            {
                case 0:
                    RestartUpdate();
                    break;
            }
        }

        private void SetResolution()
        {
            float referenceResolution = float.Parse(DataReader.ReadData("ModMenuScreenResolutionReference"));
            Canvas modmenu = GameObject.Find("ModMenu").GetComponent<Canvas>();
            float screenHeight = Screen.height;
            float scaleFactor = screenHeight / referenceResolution;
            modmenu.scaleFactor = scaleFactor;
        }
    }
}
