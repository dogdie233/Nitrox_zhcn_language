using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Helper;
using NitroxServer.ConsoleCommands.Abstract;

namespace NitroxServer.ConsoleCommands
{
    internal class BackCommand : Command
    {
        public BackCommand() : base("back", Perms.ADMIN, "传送到你最后的位置")
        {
        }

        protected override void Execute(CallArgs args)
        {
            Validate.IsTrue(args.Sender.HasValue, "控制台无法执行此命令");
            Player player = args.Sender.Value;

            if (player.LastStoredPosition == null)
            {
                SendMessage(args.Sender, "无法找到上次的位置...");
                return;
            }

            player.Teleport(player.LastStoredPosition.Value);
            SendMessage(args.Sender, $"传送回到 {player.LastStoredPosition.Value}");
        }
    }
}
