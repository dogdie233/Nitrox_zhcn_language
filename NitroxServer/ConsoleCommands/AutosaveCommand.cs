using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Server;
using NitroxServer.ConsoleCommands.Abstract;
using NitroxServer.ConsoleCommands.Abstract.Type;
using NitroxServer.Serialization;

namespace NitroxServer.ConsoleCommands
{
    internal class AutoSaveCommand : Command
    {
        private readonly ServerConfig serverConfig;

        public AutoSaveCommand(ServerConfig serverConfig) : base("autosave", Perms.ADMIN, "启用/关闭 定时保存")
        {
            this.serverConfig = serverConfig;
            AddParameter(new TypeBoolean("是/否", true));
        }

        protected override void Execute(CallArgs args)
        {
            bool toggle = args.Get<bool>(0);

            if (toggle)
            {
                serverConfig.DisableAutoSave = false;
                Server.Instance.EnablePeriodicSaving();
                SendMessage(args.Sender, "启用定时保存");
            }
            else
            {
                serverConfig.DisableAutoSave = true;
                Server.Instance.DisablePeriodicSaving();
                SendMessage(args.Sender, "关闭定时保存");
            }
        }
    }
}
