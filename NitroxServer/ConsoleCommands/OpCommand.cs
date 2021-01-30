using NitroxModel.DataStructures.GameLogic;
using NitroxServer.ConsoleCommands.Abstract;
using NitroxServer.ConsoleCommands.Abstract.Type;

namespace NitroxServer.ConsoleCommands
{
    internal class OpCommand : Command
    {
        public OpCommand() : base("op", Perms.ADMIN, "设置一位玩家为管理员")
        {
            AddParameter(new TypePlayer("玩家名", true));
        }

        protected override void Execute(CallArgs args)
        {
            Player receivingPlayer = args.Get<Player>(0);
            string playerName = receivingPlayer.Name;

            receivingPlayer.Permissions = Perms.ADMIN;
            SendMessage(args.Sender, $"已将玩家 {playerName} 的权限设置为管理员");
        }
    }
}
