using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UWE;

namespace NitroxClient.MonoBehaviours.Gui.InGame
{
    // TODO: Prevent closing of window when clicking outside of it.
    // TODO: Refactor to a standalone modal so that it can be used in the main menu.
    /// <summary>
    ///     Extends the IngameMenu with a disconnect popup.
    /// </summary>
    public class LostConnectionModal : MonoBehaviour
    {
        public const string SUB_WINDOW_NAME = "连接丢失";
        private static GameObject lostConnectionSubWindow;
        public static LostConnectionModal Instance { get; private set; }

        public void Show()
        {
            FreezeTime.Begin("NitroxDisconnected");
            StartCoroutine(Show_Impl());
        }

        private static void InitSubWindow()
        {
            if (!IngameMenu.main)
            {
                throw new NotSupportedException($"无法显示游戏内子窗口 {SUB_WINDOW_NAME} 因为这个游戏内窗口不存在。");
            }

            if (!lostConnectionSubWindow)
            {
                GameObject derivedSubWindow = IngameMenu.main.transform.Find("QuitConfirmation").gameObject;
                lostConnectionSubWindow = Instantiate(derivedSubWindow, IngameMenu.main.transform, false);
                lostConnectionSubWindow.name = SUB_WINDOW_NAME;

                // Styling.
                RectTransform main = lostConnectionSubWindow.GetComponent<RectTransform>();
                main.sizeDelta = new Vector2(700, 195);

                DestroyImmediate(lostConnectionSubWindow.FindChild("ButtonNo")); // Delete Button No

                GameObject header = lostConnectionSubWindow.FindChild("Header"); //Message Object

                Text messageText = header.GetComponent<Text>();
                messageText.text = "丢失和游戏服务器的连接";

                RectTransform messageTransform = header.GetComponent<RectTransform>();
                messageTransform.sizeDelta = new Vector2(700, 195);

                GameObject buttonYes = lostConnectionSubWindow.FindChild("ButtonYes"); //Button Yes Object
                buttonYes.transform.position = new Vector3(lostConnectionSubWindow.transform.position.x / 2, buttonYes.transform.position.y, buttonYes.transform.position.z); // Center Button

                Text messageTextbutton = buttonYes.GetComponentInChildren<Text>(); //Get Button Text Component
                messageTextbutton.text = "OK";
            }
        }

        private void Start()
        {
            if (Instance)
            {
                throw new NotSupportedException($"只有一个 {nameof(LostConnectionModal)} 可以被激活");
            }

            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private IEnumerator Show_Impl()
        {
            // Execute frame-by-frame to allow UI scripts to initialize.
            InitSubWindow();
            yield return null;
            IngameMenu.main.Open();
            yield return null;
            IngameMenu.main.ChangeSubscreen(SUB_WINDOW_NAME);
        }
    }
}
