using NitroxClient.MonoBehaviours.Gui.MainMenu;
using NitroxModel.Helper;
using NitroxModel.Logger;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NitroxClient.MonoBehaviours.DiscordRP
{
    public class DiscordRPController : MonoBehaviour
    {
        public DiscordRpc.RichPresence Presence = new DiscordRpc.RichPresence();
        public bool ShowingWindow;

        private const string APPLICATION_ID = "405122994348752896";
        private DiscordRpc.EventHandlers handlers;

        private static DiscordRPController main;
        public static DiscordRPController Main
        {
            get
            {
                if (main == null)
                {
                    main = new GameObject("DiscordController").AddComponent<DiscordRPController>();
                }
                return main;
            }
        }

        public void ReadyCallback(ref DiscordRpc.DiscordUser connectedUser) => Log.Info("[Discord] Ready");

        public void DisconnectedCallback(int errorCode, string message) => Log.Info($"[Discord] Disconnected: {errorCode} <=> {message}");

        public void ErrorCallback(int errorCode, string message) => Log.Error($"[Discord] Error: {errorCode} <=> {message}");

        public void JoinCallback(string secret)
        {
            Log.Info("[Discord] 正在加入服务器");
            if (SceneManager.GetActiveScene().name == "StartScreen" && MainMenuMultiplayerPanel.Main != null)
            {
                string[] splitSecret = secret.Split(':');
                string ip = splitSecret[0];
                string port = splitSecret[1];
                MainMenuMultiplayerPanel.Main.OpenJoinServerMenu(ip,port);
            }
            else
            {
                Log.InGame("如果要加入会话，请输入 multiplayer-main-menu");
                Log.Warn("[Discord] 警告: 无法加入一个服务器在 main-menu 外");
            }
        }

        public void RequestCallback(ref DiscordRpc.DiscordUser request)
        {
            if (!ShowingWindow)
            {
                Log.Info($"[Discord] 加入请求: 名字:{request.username}#{request.discriminator} 用户ID:{request.userId}");
                DiscordJoinRequestGui acceptRequest = gameObject.AddComponent<DiscordJoinRequestGui>();
                acceptRequest.Request = request;
                ShowingWindow = true;
            }
            else
            {
                Log.Debug("[Discord] Request window is already active.");
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            DiscordRpc.RunCallbacks();
        }

        private void OnEnable()
        {
            Log.Info("[Discord] 初始化");
            handlers = new DiscordRpc.EventHandlers
            {
                readyCallback = ReadyCallback
            };
            handlers.disconnectedCallback += DisconnectedCallback;
            handlers.errorCallback += ErrorCallback;
            handlers.joinCallback += JoinCallback;
            handlers.requestCallback += RequestCallback;
            DiscordRpc.Initialize(APPLICATION_ID, ref handlers, true, "");
        }

        private void OnDisable()
        {
            Log.Info("[Discord] 关闭");
            DiscordRpc.Shutdown();
        }

        public void InitializeInGame(string username, int playerCount, int maxConnections, string ipAddressPort)
        {
            Presence.state = "在游戏中";
            Presence.details = "Playing as " + username;
            Presence.startTimestamp = 0;
            Presence.partyId = "PartyID:" + CheckIP(ipAddressPort);
            Presence.partySize = playerCount;
            Presence.partyMax = maxConnections;
            Presence.joinSecret = CheckIP(ipAddressPort);
            SendRP();
        }

        public void InitializeMenu()
        {
            Presence.state = "在菜单中";
            SendRP();
        }

        public void UpdatePlayerCount(int playerCount)
        {
            Presence.partySize = playerCount;
            SendRP();
        }

        private void SendRP()
        {
            Presence.largeImageKey = "icon";
            Presence.instance = false;
            DiscordRpc.UpdatePresence(Presence);
        }

        public void RespondJoinRequest(string userID, DiscordRpc.Reply reply)
        {
            Log.Info($"[Discord] 回复加入请求与: {userID} 回应 {reply:g}");
            ShowingWindow = false;
            DiscordRpc.Respond(userID, reply);
        }

        private static string CheckIP(string ipPort)
        {
            string ip = ipPort.Split(':')[0];
            string port = ipPort.Split(':')[1];

            if (ip == "127.0.0.1")
            {
                return WebHelper.GetPublicIP() + ":" + port;
            }
            else
            {
                return ipPort;
            }
        }
    }
}
