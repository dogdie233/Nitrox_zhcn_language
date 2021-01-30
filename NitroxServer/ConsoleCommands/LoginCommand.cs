using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Helper;
using NitroxModel.Server;
using NitroxServer.ConsoleCommands.Abstract;
using NitroxServer.ConsoleCommands.Abstract.Type;
using NitroxServer.Serialization;

namespace NitroxServer.ConsoleCommands
{
    internal class LoginCommand : Command
    {
        private readonly ServerConfig serverConfig;

        public LoginCommand(ServerConfig serverConfig) : base("login", Perms.PLAYER, "作为管理员登录 (需要密码)")
        {
            this.serverConfig = serverConfig;
            AddParameter(new TypeString("密码", true));
        }

        protected override void Execute(CallArgs args)
        {
            Validate.IsTrue(args.Sender.HasValue, "控制台不能执行此命令");

            if (args.Get(0) == serverConfig.AdminPassword)
            {
                args.Sender.Value.Permissions = Perms.ADMIN;
                SendMessage(args.Sender, $"已将 {args.SenderName} 的权限提升为管理员");
            }
            else
            {
                SendMessage(args.Sender, "密码错误");
            }
        }
    }
}
