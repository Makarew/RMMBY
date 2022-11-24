using RMMBY.Editable;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections.Generic;
using MelonLoader;

namespace RMMBY
{
    internal class ModMenuHandler : MonoBehaviour
    {
        private bool down;
        private bool up;
        private bool right;
        private bool left;

        public void Start()
        {
            inputHandler = FindObjectOfType<Editable.InputHandler>();
            modDirectory = Path.Combine(MelonHandler.ModsDirectory, "RMMBY/Mods");
            LoadMods();
            currentY = buttonStart.y;
            buttonPrefab = GameObject.Find("ButtonPrefab");
            buttonHolder = GameObject.Find("Buttons");
            CreateButtons();
            GetMenus();
            ToggleMenu(0);
            currentMenu = 0;
            em = FindObjectOfType<EnabledMods>();
        }

        private void Update()
        {
            if (!modsLoaded) return;

            switch (currentMenu)
            {
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

            if (currentButtonSelect >= buttons.Count)
            {
                currentButtonSelect = 0;
            }
            else if (currentButtonSelect < 0)
            {
                currentButtonSelect = buttons.Count - 1;
            }

            SetSelected(buttons[currentButtonSelect]);

            if (inputHandler.back)
            {
                SceneManager.LoadScene(ListenToLoadMenu.sceneToReturnTo);
            }

            if (inputHandler.select && Metadata[currentButtonSelect].State == MetadataState.Success)
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
                if(settingsList[currentSettingSelect][i] == selectedObject.transform.Find("ChoiceText").GetComponent<Text>().text)
                {
                    currentChoice = i;
                }
            }

            if (inputHandler.back)
            {
                List<string> list = new List<string>();

                for (int i = 1; i < currentSettings.Count; i++)
                {
                    list.Add(currentSettings[i].ToString());
                }

                if(currentSettings[0] == 0)
                {
                    em.AddNewEnabledPath(Path.Combine(Metadata[currentButtonSelect].Location, Metadata[currentButtonSelect].Modules[0]));
                }
                else
                {
                    em.RemoveEnabledPath(Path.Combine(Metadata[currentButtonSelect].Location, Metadata[currentButtonSelect].Modules[0]));
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
            } else
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
            } else if (currentMenu == 1)
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
                button.transform.localPosition = new Vector3(buttonStart.x, currentY, 0);
                if (i != 0)
                {
                    button.transform.Find("Highlight").gameObject.SetActive(false);
                }
                button.transform.Find("Text (Legacy)").GetComponent<Text>().text = Metadata[i].Title;

                button.AddComponent<ModToggleButton>();

                buttons.Add(button);

                currentY -= buttonYDif;
            }

            buttonsCreated = true;
        }

        private void GetMenus()
        {
            menus.Add(GameObject.Find("ModSelectionMenu"));
            menus.Add(GameObject.Find("ModSettingsMenu"));
        }

        private void ToggleMenu(int menuID)
        {
            for (int i = 0; i < menus.Count; i++)
            {
                if(i != menuID)
                {
                    menus[i].SetActive(false);
                } else
                {
                    menus[i].SetActive(true);
                }
            }

            currentMenu = menuID;
        }

        private void CreateSettings(string settingsPath, string configPath)
        {
            ToggleMenu(1);

            settingHolder = GameObject.Find("Settings");
            settingPrefab = GameObject.Find("OptionPrefab");

            GameObject[] children = settingHolder.GetComponentsInChildren<GameObject>();
            foreach (GameObject child in children)
            {
                if(child != settingHolder)
                {
                    Destroy(child);
                }
            }

            currentSettings = new List<int>();

            currentSettings.Add(0);

            GameObject.Find("SettingsModName").GetComponent<Text>().text = Metadata[currentButtonSelect].Title;
            settingY = settingStart.y;
            settingsList.Clear();

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
        }

        private void LoadMods()
        {
            List<Text> texts = new List<Text>();
            texts.Add(GameObject.Find("ModName").GetComponent<Text>());
            texts.Add(GameObject.Find("Author").GetComponent<Text>());
            texts.Add(GameObject.Find("Description").GetComponent<Text>());
            texts.Add(GameObject.Find("Version").GetComponent<Text>());

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
                    catch (Exception ex)
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
            if(currentMenu == 0)
            {
                if (selectedObject.transform.position.y < 265)
                {
                    Vector3 pos = buttonHolder.transform.position;
                    pos.y += buttonYDif;
                    buttonHolder.transform.position = pos;
                } else if (selectedObject.transform.position.y > 1045)
                {
                    Vector3 pos = buttonHolder.transform.position;
                    pos.y -= buttonYDif;
                    buttonHolder.transform.position = pos;
                }
            }
        }

        private Vector2 buttonStart = new Vector2(-850, 320);
        private float buttonYDif = 110;
        private float currentY;

        private Vector2 settingStart = new Vector2(0, 195);
        private float settingYDif = 60;
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
        private bool buttonsCreated;

        private int currentButtonSelect = 0;
        private int currentSettingSelect = 0;

        public static List<MetadataBase> Metadata = new List<MetadataBase>();
        public Text[] modText;

        private Editable.InputHandler inputHandler;
        private string modDirectory;

        private List<GameObject> menus = new List<GameObject>();
        private int currentMenu;

        private EnabledMods em;
    }
}
