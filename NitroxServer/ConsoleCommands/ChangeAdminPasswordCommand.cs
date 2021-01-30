using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Logger;
using NitroxModel.Server;
using NitroxServer.ConsoleCommands.Abstract;
using NitroxServer.ConsoleCommands.Abstract.Type;
using NitroxServer.Serialization;

namespace NitroxServer.ConsoleCommands
{
    internal class ChangeAdminPasswordCommand : Command
    {
        private readonly ServerConfig serverConfig;

        public ChangeAdminPasswordCommand(ServerConfig serverConfig) : base("changeadminpassword", Perms.ADMIN, "修改管理员密码")
        {
            this.serverConfig = serverConfig;
            AddParameter(new TypeString("密码", true));
        }

        protected override void Execute(CallArgs args)
        {
            string newPassword = args.Get(0);

            serverConfig.AdminPassword = newPassword;

            Log.InfoSensitive("管理员密码已被 {playername} 修改为 {password}", newPassword, args.SenderName);
            SendMessageToPlayer(args.Sender, "管理员密码已修改");
        }
    }
}
