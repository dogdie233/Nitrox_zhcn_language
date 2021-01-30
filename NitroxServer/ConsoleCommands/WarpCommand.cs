using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Helper;
using NitroxServer.ConsoleCommands.Abstract;
using NitroxServer.ConsoleCommands.Abstract.Type;

namespace NitroxServer.ConsoleCommands
{
    internal class WarpCommand : Command
    {
        public WarpCommand() : base("warp", Perms.ADMIN, "传送到玩家")
        {
            AddParameter(new TypePlayer("玩家名", true));
            AddParameter(new TypePlayer("玩家名", false));
        }

        protected override void Execute(CallArgs args)
        {
            Player destination;
            Player sender;

            if (args.IsValid(1))
            {
                destination = args.Get<Player>(1);
                sender = args.Get<Player>(0);
            }
            else
            {
                Validate.IsTrue(args.Sender.HasValue, "控制台不能使用该命令");
                destination = args.Get<Player>(0);
                sender = args.Sender.Value;
            }

            sender.Teleport(destination.Position);
            SendMessage(sender, $"传送到 {destination.Name}");
        }
    }
}
