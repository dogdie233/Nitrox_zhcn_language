using NitroxModel.DataStructures.GameLogic;
using NitroxServer.ConsoleCommands.Abstract;

namespace NitroxServer.ConsoleCommands
{
    internal class SummaryCommand : Command
    {
        private readonly Server server;

        public SummaryCommand(Server server) : base("summary", Perms.PLAYER, "显示持久化数据", true)
        {
            this.server = server;
        }

        protected override void Execute(CallArgs args)
        {
            SendMessage(args.Sender, server.SaveSummary);
        }
    }
}
