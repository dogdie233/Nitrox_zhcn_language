using System.Timers;
using NitroxModel.Logger;
using NitroxServer.Serialization.World;
using System.IO;
using System.Text;
using System.Linq;
using NitroxServer.Serialization;
using NitroxModel.Serialization;

namespace NitroxServer
{
    public class Server
    {
        private readonly Communication.NetworkingLayer.NitroxServer server;
        private readonly WorldPersistence worldPersistence;
        private readonly ServerConfig serverConfig;
        private readonly Timer saveTimer;
        private readonly World world;

        public static Server Instance { get; private set; }

        public bool IsRunning { get; private set; }
        public bool IsSaving { get; private set; }

        public int Port => serverConfig?.ServerPort ?? -1;

        public Server(WorldPersistence worldPersistence, World world, ServerConfig serverConfig, Communication.NetworkingLayer.NitroxServer server)
        {
            this.worldPersistence = worldPersistence;
            this.serverConfig = serverConfig;
            this.server = server;
            this.world = world;

            Instance = this;

            saveTimer = new Timer();
            saveTimer.Interval = serverConfig.SaveInterval;
            saveTimer.AutoReset = true;
            saveTimer.Elapsed += delegate { Save(); };
        }

        public string SaveSummary
        {
            get
            {
                // TODO: Extend summary with more useful save file data
                StringBuilder builder = new StringBuilder("\n");
                builder.AppendLine($" - 存档位置: {Path.GetFullPath(serverConfig.SaveName)}");
                builder.AppendLine($" - 无线电信息储存: {world.GameData.StoryGoals.RadioQueue.Count}");
                builder.AppendLine($" - 完成故事目标: {world.GameData.StoryGoals.CompletedGoals.Count}");
                builder.AppendLine($" - 解锁故事目标: {world.GameData.StoryGoals.GoalUnlocks.Count}");
                builder.AppendLine($" - 百科全书条目: {world.GameData.PDAState.EncyclopediaEntries.Count}");
                builder.AppendLine($" - 物品栏物品: {world.InventoryManager.GetAllStorageSlotItems().Count}");
                builder.AppendLine($" - 背包物品: {world.InventoryManager.GetAllInventoryItems().Count}");
                builder.AppendLine($" - 已解锁科技: {world.GameData.PDAState.KnownTechTypes.Count}");
                builder.AppendLine($" - 载具: {world.VehicleManager.GetVehicles().Count()}");

                return builder.ToString();
            }
        }

        public void Save()
        {
            if (IsSaving)
            {
                return;
            }

            // Don't overwrite config changes that users made to file
            if (!File.Exists(serverConfig.FileName))
            {
                NitroxConfig.Serialize(serverConfig);
            }
            IsSaving = true;
            worldPersistence.Save(world, serverConfig.SaveName);
            IsSaving = false;
        }

        public bool Start()
        {
            if (!server.Start())
            {
                return false;
            }

            Log.Info($"服务器在端口 {Port} UDP 开始监听");
            Log.Info($"使用 {serverConfig.SerializerMode} 作为存档文件序列化器");
            Log.InfoSensitive("服务器密码: {password}", string.IsNullOrEmpty(serverConfig.ServerPassword) ? "无。公开服务器。" : serverConfig.ServerPassword);
            Log.InfoSensitive("管理员密码: {password}", serverConfig.AdminPassword);
            Log.Info($"自动保存: {(serverConfig.DisableAutoSave ? "禁用" : $"启用 (每 {serverConfig.SaveInterval / 60000} 分钟)")}");
            Log.Info($"世界游戏模式: {serverConfig.GameMode}");
            Log.Info($"加载存档\n{SaveSummary}");

            PauseServer();

            IsRunning = true;
#if RELEASE
            IpLogger.PrintServerIps();
#endif
            return true;
        }

        public void Stop()
        {
            Log.Info("Nitrox 服务器关闭中...");
            DisablePeriodicSaving();
            Save();
            server.Stop();
            Log.Info("Nitrox 服务器已关闭。");
            IsRunning = false;
        }

        public void EnablePeriodicSaving()
        {
            saveTimer.Start();
        }

        public void DisablePeriodicSaving()
        {
            saveTimer.Stop();
        }

        public void PauseServer()
        {
            DisablePeriodicSaving();
            world.EventTriggerer.PauseWorldTime();
            world.EventTriggerer.PauseEventTimers();
            Log.Info("服务器已暂停");
        }

        public void ResumeServer()
        {
            if (!serverConfig.DisableAutoSave)
            {
                EnablePeriodicSaving();
            }
            world.EventTriggerer.StartWorldTime();
            world.EventTriggerer.StartEventTimers();
            Log.Info("服务器已恢复");
        }
    }
}
