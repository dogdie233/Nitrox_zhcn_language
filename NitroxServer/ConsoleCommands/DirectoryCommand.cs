using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Logger;
using NitroxServer.ConsoleCommands.Abstract;

namespace NitroxServer.ConsoleCommands
{
    internal sealed class DirectoryCommand : Command
    {
        public override IEnumerable<string> Aliases { get; } = new[] { "dir" };

        public DirectoryCommand() : base("directory", Perms.CONSOLE, "打开此程序文件夹")
        {
        }

        protected override void Execute(CallArgs args)
        {
            string dir = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            if (!Directory.Exists(dir))
            {
                Log.Error($"无法打开文件夹 '{dir}' 因为它不存在");
                return;
            }

            Process.Start(dir);
        }
    }
}
