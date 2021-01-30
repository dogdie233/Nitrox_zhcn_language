using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Logger;
using NitroxModel.Server;
using NitroxServer.ConsoleCommands.Abstract;
using NitroxServer.ConsoleCommands.Abstract.Type;
using NitroxServer.Serialization;

namespace NitroxServer.ConsoleCommands
{
    internal class ChangeServerPasswordCommand : Command
    {
        private readonly ServerConfig serverConfig;

        public ChangeServerPasswordCommand(ServerConfig serverConfig) : base("changeserverpassword", Perms.ADMIN, "修改服务器密码。没有参数则清除密码")
        {
            this.serverConfig = serverConfig;
            AddParameter(new TypeString("密码", false));
        }

        protected override void Execute(CallArgs args)
        {
            string password = args.Get(0) ?? string.Empty;

            serverConfig.ServerPassword = password;

            Log.InfoSensitive("服务器密码已被 {playername} 修改为 {password}", password, args.SenderName);
            SendMessageToPlayer(args.Sender, "Server password changed");
        }
    }
}
