using System.Diagnostics;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Logger;
using NitroxServer.ConsoleCommands.Abstract;

namespace NitroxServer.ConsoleCommands
{
    internal sealed class RestartCommand : Command
    {
        private readonly Server server;

        public RestartCommand(Server server) : base("restart", Perms.CONSOLE, "重启服务器")
        {
            this.server = server;
        }

        protected override void Execute(CallArgs args)
        {
            if (Debugger.IsAttached)
            {
                Log.Error("附加调试器时无法重启");
                return;
            }
            string program = Process.GetCurrentProcess().MainModule?.FileName;
            if (program == null)
            {
                Log.Error("无法获取服务器的位置");
                return;
            }

            using Process proc = Process.Start(program);
            server.Stop();
        }
    }
}
