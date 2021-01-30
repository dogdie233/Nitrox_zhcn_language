using NitroxModel.DataStructures.GameLogic;
using NitroxServer.ConsoleCommands.Abstract;
using NitroxServer.ConsoleCommands.Abstract.Type;

namespace NitroxServer.ConsoleCommands
{
    internal class DeopCommand : Command
    {
        public DeopCommand() : base("deop", Perms.ADMIN, "移除玩家的管理员权限")
        {
            AddParameter(new TypePlayer("玩家名", true));
        }

        protected override void Execute(CallArgs args)
        {
            Player targetPlayer = args.Get<Player>(0);
            string playerName = args.Get(0);

            targetPlayer.Permissions = Perms.PLAYER;

            SendMessage(args.Sender, $"已更新玩家 {playerName} 的权限");
        }
    }
}
