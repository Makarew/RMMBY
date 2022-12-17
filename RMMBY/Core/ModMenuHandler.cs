using RMMBY.Editable;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections.Generic;
using MelonLoader;
using RMMBY.Helpers;

namespace RMMBY
{
    internal class ModMenuHandler : MonoBehaviour
    {
        private bool down;
        private bool up;
        private bool right;
        private bool left;
        private bool select;
        private bool selectLock = true;
        private bool back;
        private bool backLock = true;

        Canvas modmenu;
        private float scaleFactor = 1;

        public void Start()
        {
            inputHandler = FindObjectOfType<Editable.InputHandler>();
            modDirectory = Path.Combine(MelonHandler.ModsDirectory, "RMMBY/Mods");
            SetVariables();

            float referenceResolution = 0;
            try
            {
                referenceResolution = float.Parse(DataReader.ReadData("ModMenuScreenResolutionReference"));
            }
            catch { DataError(); }

            if (error) return;

            LoadMods();
            currentY = buttonStart.y;
            currentX = buttonStart.x;
            buttonPrefab = GameObject.Find("ButtonPrefab");
            buttonHolder = GameObject.Find("Buttons");
            CreateButtons();
            GetMenus();
            ToggleMenu(0);
            currentMenu = 0;
            em = FindObjectOfType<EnabledMods>();

            modmenu = GameObject.Find("ModMenu").GetComponent<Canvas>();
            float screenHeight = Screen.height;
            scaleFactor = screenHeight / referenceResolution;
            modmenu.scaleFactor = scaleFactor;
        }

        private void Update()
        {
            if(currentMenu == -1 && back)
                SceneManager.LoadScene(ListenToLoadMenu.sceneToReturnTo);

            if (!modsLoaded) return;

            if (inputHandler.select)
            {
                if (!selectLock)
                {
                    select = true;
                    selectLock = true;
                } else
                {
                    select = false;
                }
            } else
            {
                selectLock = false;
                select = false;
            }

            if (inputHandler.back)
            {
                if (!backLock)
                {
                    back = true;
                    backLock = true;
                }
                else
                {
                    back = false;
                }
            }
            else
            {
                backLock = false;
                back = false;
            }

            switch (currentMenu)
            {
                case -1:
                    if (back || select)
                    {
                        SceneManager.LoadScene(ListenToLoadMenu.sceneToReturnTo);
                    }
                    break;
                case 0:
                    CMMods();
                    break;
                case 1:
                    CMSettings();
                    break;
            }

            SetMenuItemsPosition();
        }

        private void CMMods()
        {
            switch (buttonDir)
            {
                case 0:
                    if (inputHandler.down)
                    {
                        if (!down)
                        {
                            currentButtonSelect++;
                            down = true;
                        }
                    }
                    else
                    {
                        down = false;
                    }

                    if (inputHandler.up)
                    {
                        if (!up)
                        {
                            currentButtonSelect--;
                            up = true;
                        }
                    }
                    else
                    {
                        up = false;
                    }
                    break;
                case 2:
                    if (inputHandler.down || inputHandler.right)
                    {
                        if (!down)
                        {
                            currentButtonSelect++;
                            down = true;
                        }
                    }
                    else
                    {
                        down = false;
                    }

                    if (inputHandler.up || inputHandler.left)
                    {
                        if (!up)
                        {
                            currentButtonSelect--;
                            up = true;
                        }
                    }
                    else
                    {
                        up = false;
                    }
                    break;
                case 3:
                    if (inputHandler.down || inputHandler.left)
                    {
                        if (!down)
                        {
                            currentButtonSelect++;
                            down = true;
                        }
                    }
                    else
                    {
                        down = false;
                    }

                    if (inputHandler.up || inputHandler.right)
                    {
                        if (!up)
                        {
                            currentButtonSelect--;
                            up = true;
                        }
                    }
                    else
                    {
                        up = false;
                    }
                    break;
                case 1:
                    if (inputHandler.right)
                    {
                        if (!down)
                        {
                            currentButtonSelect++;
                            down = true;
                        }
                    }
                    else
                    {
                        down = false;
                    }

                    if (inputHandler.left)
                    {
                        if (!up)
                        {
                            currentButtonSelect--;
                            up = true;
                        }
                    }
                    else
                    {
                        up = false;
                    }
                    break;
                case 4:
                    if (inputHandler.right || inputHandler.down)
                    {
                        if (!down)
                        {
                            currentButtonSelect++;
                            down = true;
                        }
                    }
                    else
                    {
                        down = false;
                    }

                    if (inputHandler.left || inputHandler.up)
                    {
                        if (!up)
                        {
                            currentButtonSelect--;
                            up = true;
                        }
                    }
                    else
                    {
                        up = false;
                    }
                    break;
                case 5:
                    if (inputHandler.right || inputHandler.up)
                    {
                        if (!down)
                        {
                            currentButtonSelect++;
                            down = true;
                        }
                    }
                    else
                    {
                        down = false;
                    }

                    if (inputHandler.left || inputHandler.down)
                    {
                        if (!up)
                        {
                            currentButtonSelect--;
                            up = true;
                        }
                    }
                    else
                    {
                        up = false;
                    }
                    break;
            }

            if (currentButtonSelect >= buttons.Count)
            {
                currentButtonSelect = 0;
            }
            else if (currentButtonSelect < 0)
            {
                currentButtonSelect = buttons.Count - 1;
            }

            SetSelected(buttons[currentButtonSelect]);

            if (back)
            {
                if (restartRequired) {
                    Melon<Plugin>.Instance.LoadInfo(restartMessage, 0);
                }
                else
                {
                    SceneManager.LoadScene(ListenToLoadMenu.sceneToReturnTo);
                }
            }

            if (select && Metadata[currentButtonSelect].State == MetadataState.Success)
            {
                CreateSettings(Path.Combine(Metadata[currentButtonSelect].Location, Metadata[currentButtonSelect].SettingsFile), Path.Combine(Metadata[currentButtonSelect].Location, Metadata[currentButtonSelect].ConfigFile));
            }
        }

        private void CMSettings()
        {
            if (inputHandler.down)
            {
                if (!down)
                {
                    currentSettingSelect++;
                    down = true;
                }
            }
            else
            {
                down = false;
            }

            if (inputHandler.up)
            {
                if (!up)
                {
                    currentSettingSelect--;
                    up = true;
                }
            }
            else
            {
                up = false;
            }

            if (currentSettingSelect >= settings.Count)
            {
                currentSettingSelect = 0;
            }
            else if (currentSettingSelect < 0)
            {
                currentSettingSelect = settings.Count - 1;
            }

            SetSelected(settings[currentSettingSelect]);

            for (int i = 0; i < settingsList[currentSettingSelect].Count; i++)
            {
                if (settingsList[currentSettingSelect][i] == selectedObject.transform.Find("ChoiceText").GetComponent<Text>().text)
                {
                    currentChoice = i;
                }
            }

            if (back)
            {
                List<string> list = new List<string>();

                for (int i = 1; i < currentSettings.Count; i++)
                {
                    list.Add(currentSettings[i].ToString());
                }

                if (currentSettings[0] == 0)
                {
                    em.AddNewEnabledPath(Path.Combine(Metadata[currentButtonSelect].Location, Metadata[currentButtonSelect].Modules[0]));
                }
                else
                {
                    em.RemoveEnabledPath(Path.Combine(Metadata[currentButtonSelect].Location, Metadata[currentButtonSelect].Modules[0]));
                    if (Metadata[currentButtonSelect].RequiresRestartToUnload)
                    {
                        restartRequired = true;
                        restartMessage = "Restart required to unload " + Metadata[currentButtonSelect].Title;
                    }
                }

                if (Metadata[currentButtonSelect].ConfigFile != "N/A" && currentSettings.Count > 1)
                {
                    WriteToFile.WriteFile(Path.Combine(Metadata[currentButtonSelect].Location, Metadata[currentButtonSelect].ConfigFile), list.ToArray(), false);
                }

                ToggleMenu(0);
            }

            if (inputHandler.right)
            {
                if (!right)
                {
                    currentChoice++;
                    right = true;
                }
            }
            else
            {
                right = false;
            }
            if (inputHandler.left)
            {
                if (!left)
                {
                    currentChoice--;
                    left = true;
                }
            }
            else
            {
                left = false;
            }

            if (currentChoice >= settingsList[currentSettingSelect].Count)
            {
                currentChoice = 0;
            }
            else if (currentChoice < 0)
            {
                currentChoice = settingsList[currentSettingSelect].Count - 1;
            }

            currentSettings[currentSettingSelect] = currentChoice;
            selectedObject.transform.Find("ChoiceText").GetComponent<Text>().text = settingsList[currentSettingSelect][currentChoice];
        }

        private void SetSelected(GameObject toSelect)
        {
            selectedObject = toSelect;

            if (currentMenu == 0)
            {
                if (Metadata[currentButtonSelect].State == MetadataState.Success)
                {
                    modText[0].text = Metadata[currentButtonSelect].Title;
                    modText[1].text = "Created By: " + Metadata[currentButtonSelect].Author;
                    modText[2].text = "Description: " + Metadata[currentButtonSelect].Description;
                    modText[3].text = "Version: " + Metadata[currentButtonSelect].Version;

                    for (int i = 0; i < modText.Length; i++)
                    {
                        modText[i].color = Color.black;
                    }
                }
                else if (Metadata[currentButtonSelect].State == MetadataState.NoModule)
                {
                    modText[0].text = Metadata[currentButtonSelect].Title + " (Missing Module)";
                    modText[1].text = "Created By: " + Metadata[currentButtonSelect].Author;
                    modText[2].text = "Description: " + Metadata[currentButtonSelect].Description;
                    modText[3].text = "Version: " + Metadata[currentButtonSelect].Version;

                    for (int i = 0; i < modText.Length; i++)
                    {
                        modText[i].color = Color.red;
                    }
                }
                else if (Metadata[currentButtonSelect].State == MetadataState.BadJson)
                {
                    modText[0].text = "Bad Metadata";
                    modText[1].text = "Created By: ";
                    modText[2].text = "Description: ";
                    modText[3].text = "Version: ";

                    for (int i = 0; i < modText.Length; i++)
                    {
                        modText[i].color = Color.red;
                    }
                }
            }
            else if (currentMenu == 1)
            {
                for (int i = 0; i < settingsList[currentSettingSelect].Count; i++)
                {
                    if (settingsList[currentSettingSelect][i] == selectedObject.transform.Find("ChoiceText").GetComponent<Text>().text)
                    {
                        currentChoice = i;
                    }
                }
            }
        }

        private void CreateButtons()
        {
            for (int i = 0; i < Metadata.Count; i++)
            {
                GameObject button = GameObject.Instantiate(buttonPrefab);
                button.transform.SetParent(buttonHolder.transform);
                button.transform.SetSiblingIndex(i);
                button.transform.localPosition = new Vector3(currentX, currentY, 0);
                if (i != 0)
                {
                    button.transform.Find("Highlight").gameObject.SetActive(false);
                }
                button.transform.Find("Text (Legacy)").GetComponent<Text>().text = Metadata[i].Title;

                button.AddComponent<ModToggleButton>();

                buttons.Add(button);

                currentY -= buttonYDif;
                currentX += buttonXDif;
            }
        }

        private void GetMenus()
        {
            menus.Clear();
            menus.Add(GameObject.Find("ModSelectionMenu"));
            menus.Add(GameObject.Find("ModSettingsMenu"));
        }

        private void ToggleMenu(int menuID)
        {
            for (int i = 0; i < menus.Count; i++)
            {
                if (i != menuID)
                {
                    menus[i].SetActive(false);
                }
                else
                {
                    menus[i].SetActive(true);
                }
            }

            currentMenu = menuID;
        }

        private void CreateSettings(string settingsPath, string configPath)
        {
            modmenu.scaleFactor = 1;

            ToggleMenu(1);

            settingHolder = GameObject.Find("Settings");
            settingPrefab = GameObject.Find("OptionPrefab");

            GameObject[] children = ObjectFinders.FindAllObjectsWithName("OptionPrefab(Clone)");
            foreach (GameObject child in children)
            {
                if (child != settingHolder)
                {
                    Destroy(child);
                }
            }

            currentSettings = new List<int>
            {
                0
            };

            GameObject.Find("SettingsModName").GetComponent<Text>().text = Metadata[currentButtonSelect].Title;
            settingY = settingStart.y;
            settingsList.Clear();
            settings.Clear();

            GameObject gameObject = Instantiate(settingPrefab);
            gameObject.transform.SetParent(settingHolder.transform);
            gameObject.transform.SetSiblingIndex(0);
            gameObject.transform.localPosition = new Vector3(settingStart.x, settingY, 0);
            gameObject.transform.Find("OptionName").GetComponent<Text>().text = "Enabled";
            gameObject.transform.Find("ChoiceText").GetComponent<Text>().text = em.CheckEnabled(Path.Combine(Metadata[currentButtonSelect].Location, Metadata[currentButtonSelect].Modules[0])) ? "True" : "False";
            currentSettings[0] = em.CheckEnabled(Path.Combine(Metadata[currentButtonSelect].Location, Metadata[currentButtonSelect].Modules[0])) ? 0 : 1;
            gameObject.AddComponent<ModToggleButton>();

            settingsList.Add(new List<string>());
            settingsList[0].Add("True");
            settingsList[0].Add("False");
            settings.Add(gameObject);
            settingY -= settingYDif;

            if (Metadata[currentButtonSelect].SettingsFile != "N/A")
            {
                StreamReader r = new StreamReader(configPath);
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
                            currentSettings.Add(int.Parse(lineData[0]));
                        }
                    }
                    while (line != null);
                    //Stop reading the file
                    r.Close();
                }


                r = new StreamReader(settingsPath);

                int i = 1;
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
                            GameObject go = Instantiate(settingPrefab);

                            go.transform.SetParent(settingHolder.transform);
                            go.transform.SetSiblingIndex(i);
                            go.transform.localPosition = new Vector3(settingStart.x, settingY, 0);
                            go.transform.Find("Highlight").gameObject.SetActive(false);
                            go.transform.Find("OptionName").GetComponent<Text>().text = lineData[0];
                            go.transform.Find("ChoiceText").GetComponent<Text>().text = lineData[1 + currentSettings[i]];

                            go.AddComponent<ModToggleButton>();

                            settingsList.Add(new List<string>());

                            for (int j = 0; j < lineData.Length; j++)
                            {
                                if (j != 0)
                                    settingsList[i].Add(lineData[j]);
                            }

                            settings.Add(go);

                            settingY -= settingYDif;

                            i++;
                        }
                    }
                    while (line != null);
                    //Stop reading the file
                    r.Close();
                }
            }

            currentSettingSelect = 0;

            modmenu.scaleFactor = scaleFactor;
        }

        private void LoadMods()
        {
            List<Text> texts = new List<Text>
            {
                GameObject.Find("ModName").GetComponent<Text>(),
                GameObject.Find("Author").GetComponent<Text>(),
                GameObject.Find("Description").GetComponent<Text>(),
                GameObject.Find("Version").GetComponent<Text>()
            };

            modText = texts.ToArray();

            Metadata.Clear();

            for (int i = 0; i < modText.Length; i++)
            {
                modText[i].text = String.Empty;
                modText[i].color = Color.grey;
            }
            string[] files = Directory.GetFiles(modDirectory, "mod.json", SearchOption.AllDirectories);
            bool flag = files.Length == 0;
            if (flag)
            {
                modText[0].text = "No mods available";
                modsLoaded = false;
            }
            else
            {
                int j = 0;
                while (j < files.Length)
                {
                    string text = files[j];
                    try
                    {
                        bool flag2 = text.Remove(0, modDirectory.Length + 1) == "mod.json";
                        if (!flag2)
                        {
                            Metadata.Add(MetadataBase.Load<MetadataBase>(text));
                        }
                    }
                    catch
                    {

                    }
                    j++;
                    continue;
                }

                for (int i = 0; i < modText.Length; i++)
                {
                    modText[i].text = String.Empty;
                    modText[i].color = Color.black;
                }

                modsLoaded = true;
            }
        }

        private void SetMenuItemsPosition()
        {
            if (currentMenu == 0)
            {
                float currentPos = 0;
                float dirMod = 1;

                switch (buttonDir) {
                    case 1:
                        currentPos = selectedObject.transform.position.x;
                        dirMod = -1;
                        break;
                    case 4:
                        currentPos = selectedObject.transform.position.x;
                        dirMod = -1;
                        break;
                    case 5:
                        currentPos = selectedObject.transform.position.x;
                        dirMod = -1;
                        break;
                    default:
                        currentPos = selectedObject.transform.position.y;
                        break;
                }

                if (currentPos < buttonValley * scaleFactor)
                {
                    Vector3 pos = buttonHolder.transform.position;
                    pos.y += (buttonYDif * scaleFactor * dirMod);
                    pos.x -= (buttonXDif * scaleFactor * dirMod);
                    buttonHolder.transform.position = pos;
                }
                else if (currentPos > buttonPeak * scaleFactor)
                {
                    Vector3 pos = buttonHolder.transform.position;
                    pos.y -= (buttonYDif * scaleFactor * dirMod);
                    pos.x += (buttonXDif * scaleFactor * dirMod);
                    buttonHolder.transform.position = pos;
                }
            }
        }

        private void SetVariables()
        {
            try
            {
                string[] dataMulti = DataReader.ReadDataMulti("ModListStartPosition");
                string data = DataReader.ReadData("ModListYDistance");

                buttonStart.x = float.Parse(dataMulti[0]);
                buttonStart.y = float.Parse(dataMulti[1]);
                buttonYDif = float.Parse(data);

                data = DataReader.ReadData("ModListXDistance");

                buttonXDif = float.Parse(data);

                dataMulti = DataReader.ReadDataMulti("ModSettingStartPosition");
                data = DataReader.ReadData("ModSettingYDistance");

                settingStart.x = float.Parse(dataMulti[0]);
                settingStart.y = float.Parse(dataMulti[1]);
                settingYDif = float.Parse(data);

                data = DataReader.ReadData("ModListSelectionPeak");
                buttonPeak = float.Parse(data);

                data = DataReader.ReadData("ModListSelectionValley");
                buttonValley = float.Parse(data);

                data = DataReader.ReadData("ModListDirection");
                switch (data)
                {
                    case "Vertical":
                        buttonDir = 0;
                        break;
                    case "Horizontal":
                        buttonDir = 1;
                        break;
                    default:
                        float.Parse("Force Error");
                        break;
                }

                data = DataReader.ReadData("ModListAltInput");
                switch (data)
                {
                    case "Right":
                        if(buttonDir == 0)
                            buttonDir = 2;
                        else if (buttonDir == 1)
                            buttonDir = 4;
                        break;
                    case "Left":
                        if (buttonDir == 0)
                            buttonDir = 3;
                        else if (buttonDir == 1)
                            buttonDir = 5;
                        break;
                    default:
                        break;
                }
            } catch {
                DataError();
            }
        }

        private void DataError()
        {
            GetMenus();
            ToggleMenu(0);

            error = true;

            string data = "";

            try
            {
                data = DataReader.ReadData("ModMenuScreenResolutionReference");
                float.Parse(data);
            }
            catch
            {
                data = "2160";
            }
            modmenu = GameObject.Find("ModMenu").GetComponent<Canvas>();
            float screenHeight = Screen.height;
            scaleFactor = screenHeight / float.Parse(data);
            modmenu.scaleFactor = scaleFactor;

            currentMenu = -1;

            List<Text> texts = new List<Text>
            {
                GameObject.Find("ModName").GetComponent<Text>(),
                GameObject.Find("Author").GetComponent<Text>(),
                GameObject.Find("Description").GetComponent<Text>(),
                GameObject.Find("Version").GetComponent<Text>()
            };

            modText = texts.ToArray();

            modText[0].text = "";
            modText[1].text = "";
            modText[2].text = "Error Reading Data File." +
                "\n\nIf you made this version of RMMBY, please check the RMMBY wiki to make sure your data file is filled out correctly." +
                "\n\nIf you downloaded RMMBY, please reinstall RMMBY. If it still doesn't work, let the creator of this version of RMMBY know that there's an error in the data file.";
            modText[3].text = "";
        }

        private bool error;

        private int buttonDir;
        private Vector2 buttonStart;
        private float buttonYDif;
        private float buttonXDif;
        private float currentY;
        private float currentX;
        private float buttonPeak;
        private float buttonValley;

        private Vector2 settingStart;
        private float settingYDif;
        private float settingY;

        private GameObject settingHolder;
        private List<GameObject> settings = new List<GameObject>();
        private List<int> currentSettings = new List<int>();
        private List<List<string>> settingsList = new List<List<string>>();
        private int currentChoice = 0;

        private GameObject buttonHolder;
        private List<GameObject> buttons = new List<GameObject>();
        public GameObject selectedObject;

        public GameObject buttonPrefab;
        public GameObject settingPrefab;

        private bool modsLoaded = false;

        private int currentButtonSelect = 0;
        private int currentSettingSelect = 0;

        public static List<MetadataBase> Metadata = new List<MetadataBase>();
        public Text[] modText;

        private Editable.InputHandler inputHandler;
        private string modDirectory;

        private List<GameObject> menus = new List<GameObject>();
        private int currentMenu = -1;

        private EnabledMods em;

        private bool restartRequired = false;
        private string restartMessage = "";
    }
}