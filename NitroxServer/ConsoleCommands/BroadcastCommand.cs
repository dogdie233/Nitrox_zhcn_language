using System.Collections.Generic;
using NitroxModel.DataStructures.GameLogic;
using NitroxServer.ConsoleCommands.Abstract;
using NitroxServer.ConsoleCommands.Abstract.Type;

namespace NitroxServer.ConsoleCommands
{
    internal class BroadcastCommand : Command
    {
        public override IEnumerable<string> Aliases { get; } = new[] { "say" };

        public BroadcastCommand() : base("broadcast", Perms.ADMIN, "广播一条消息到服务器", true)
        {
            AddParameter(new TypeString("消息", true));
        }

        protected override void Execute(CallArgs args)
        {
            SendMessageToAllPlayers(args.GetTillEnd());
        }
    }
}
