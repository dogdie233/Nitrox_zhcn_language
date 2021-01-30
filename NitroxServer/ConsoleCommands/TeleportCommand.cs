using System.Collections.Generic;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Helper;
using NitroxServer.ConsoleCommands.Abstract;
using NitroxServer.ConsoleCommands.Abstract.Type;

namespace NitroxServer.ConsoleCommands
{
    internal class TeleportCommand : Command
    {
        public override IEnumerable<string> Aliases { get; } = new[] { "teleport" };

        public TeleportCommand() : base("tp", Perms.ADMIN, "传送你到一个地方")
        {
            AddParameter(new TypeInt("x", true));
            AddParameter(new TypeInt("y", true));
            AddParameter(new TypeInt("z", true));
        }

        protected override void Execute(CallArgs args)
        {
            Validate.IsTrue(args.Sender.HasValue, "控制台不能使用该命令");

            NitroxVector3 position = new NitroxVector3(args.Get<int>(0), args.Get<int>(1), args.Get<int>(2));
            args.Sender.Value.Teleport(position);

            SendMessage(args.Sender, $"已传送到 {position}");
        }
    }
}
