using System.Collections.Generic;
using NitroxModel.DataStructures.GameLogic;
using NitroxServer.ConsoleCommands.Abstract;

namespace NitroxServer.ConsoleCommands
{
    internal class ExitCommand : Command
    {
        public override IEnumerable<string> Aliases { get; } = new[] { "exit", "halt", "quit" };

        public ExitCommand() : base("stop", Perms.ADMIN, "关闭服务器")
        {
        }

        protected override void Execute(CallArgs args)
        {
            Server.Instance.Stop();
        }
    }
}
