using NitroxClient.MonoBehaviours.Gui.MainMenu;
using NitroxModel.Logger;
using UnityEngine;

namespace NitroxClient.MonoBehaviours
{
    public class NitroxBootstrapper : MonoBehaviour
    {
        internal static NitroxBootstrapper Instance;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            gameObject.AddComponent<SceneCleanerPreserve>();
            gameObject.AddComponent<MainMenuMods>();

#if DEBUG
            EnableDeveloperFeatures();
#endif

            CreateDebugger();
        }

        private void EnableDeveloperFeatures()
        {
            Log.Info("正在启用开发者控制台。");
            DevConsole.disableConsole = false;
            Application.runInBackground = true;
            Log.Info($"Unity 后台运行被设置为：\"{Application.runInBackground}\"");
        }

        private void CreateDebugger()
        {
            GameObject debugger = new GameObject();
            debugger.name = "Debug manager";
            debugger.AddComponent<NitroxDebugManager>();
            debugger.transform.SetParent(transform);
        }
    }
}
