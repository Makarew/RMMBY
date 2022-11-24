using UnityEngine;

// Game Specific - Replace For New Game
using RWBY;
using RWBY.UI.Title;

namespace RMMBY.Editable
{
    // Input Handler Used By RMMBY
    // Example Built For RWBY: Arrowfell
    // Modify This Script To Use New Games Input System
    internal class InputHandler : MonoBehaviour
    {
        // Required Variables - Do Not Delete
        public bool up;
        public bool down;
        public bool left;
        public bool right;
        public bool select;
        public bool back;
        public bool active;
        // End Of Required Variables

        // Game Specific Input System
        private PlayerInput playerInput;

        // Runs The First Frame After This Input Handler Is Created
        private void Start()
        {
            // Example Code For RWBY: Arrowfell
            // Replace Or Remove For New Game's Input System
            playerInput = Singleton<RWBY.InputHandler>.Instance.GetPlayerInput(0);
        }

        // Only Runs When The RMMBY Mod Menu Is Loaded
        public void OnSceneLoaded()
        {
            // Example Code For RWBY: Arrowfell
            // Replace Or Remove For New Game's Input System
            GameObject to = new GameObject();
            to.AddComponent<TitleHandler>();
            to.GetComponent<TitleHandler>().enabled = true;
        }

        // Will Run Every Frame
        private void Update()
        {
            // Active Should Only True While The Mod Menu Is Loaded - Handled By RMMBY
            // All Inputs Should Be Set To False When Not In The Mod Menu
            // Don't Modify This
            if (!active)
            {
                up = false;
                down = false;
                left = false;
                right = false;
                select = false;
                back = false;

                return;
            }

            // RMMBY's Mod Menu Will Only Use This Input Handler's Booleans
            // The Rest Of This Script Converts The Input System From RWBY: Arrowfell To RMMBY's Booleans
            // Modify Below For Your Input System

            // Moves Selection Up When Pressed
            if (playerInput.UI_Up)
            {
                up = true;
            } else
            {
                up = false;
            }

            // Moves Selection Down When Pressed
            if (playerInput.UI_Down)
            {
                down = true;
            } else
            {
                down = false;
            }

            // Moves Selection Left When Pressed
            // Currently Only Used In Individual Mod Settings
            // Options For Additional Horizontal Menus Will Be Added To RMMBY Later
            // This Will Also Work For Those Future Menus
            if (playerInput.UI_Left)
            {
                left = true;
            } else
            {
                left = false;
            }

            // Moves Selection Right When Pressed
            // Currently Only Used In Individual Mod Settings
            // Options For Additional Horizontal Menus Will Be Added To RMMBY Later
            // This Will Also Work For Those Future Menus
            if (playerInput.UI_Right)
            {
                right = true;
            } else
            {
                right = false;
            }

            // Returns To Mod List When In Individual Mod Settings, And Saves Mod Settings
            // Loads ListenToLoadMenu.sceneToReturnTo When In Mod List
            if (playerInput.UI_Cancel || playerInput.UI_CloseMenu)
            {
                back = true;
            } else
            {
                back = false;
            }

            // Loads Individual Mod Settings When In Mod List
            if (playerInput.UI_Submit)
            {
                select = true;
            } else
            {
                select = false;
            }
        }
    }
}
