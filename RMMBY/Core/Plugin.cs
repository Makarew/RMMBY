using MelonLoader;
using UnityEngine;
using RMMBY.Editable;

namespace RMMBY
{
    public class Plugin : MelonMod
    {
        private bool inScene;
        private EnabledMods em;
        private InputHandler inputHandler;

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

            if (sceneName == "ModMenu")
            {
                GameObject go = new GameObject();
                go.AddComponent<ModMenuHandler>();

                inputHandler.OnSceneLoaded();
                inputHandler.active = true;
            }
            else
            {
                inputHandler.active = false;
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (inScene && ListenToLoadMenu.runOnUpdate)
            {
                ListenToLoadMenu.OnSceneUpdate();
            }
        }
    }
}
