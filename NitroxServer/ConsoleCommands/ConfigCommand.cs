using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NitroxModel.Core;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Logger;
using NitroxModel.OS;
using NitroxModel.Serialization;
using NitroxServer.ConsoleCommands.Abstract;
using NitroxServer.Serialization;

namespace NitroxServer.ConsoleCommands
{
    internal sealed class ConfigCommand : Command
    {
        private readonly SemaphoreSlim configOpenLock = new SemaphoreSlim(1);

        public ConfigCommand() : base("config", Perms.CONSOLE, "打开服务器配置文件")
        {
        }

        protected override void Execute(CallArgs args)
        {
            if (!configOpenLock.Wait(0))
            {
                Log.Warn("等待上一个config命令关闭配置文件");
                return;
            }

            ServerConfig currentActiveConfig = NitroxServiceLocator.LocateService<ServerConfig>();
            string configFile = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? "", currentActiveConfig.FileName);
            if (!File.Exists(configFile))
            {
                Log.Error($"无法找到配置文件: {configFile}");
                return;
            }

            Task.Run(async () =>
                {
                    try
                    {
                        await StartWithDefaultProgram(configFile);
                    }
                    finally
                    {
                        configOpenLock.Release();
                    }
                    NitroxConfig.Deserialize<ServerConfig>(); // Notifies user if deserialization failed.
                    Log.Info("如果你修改了内容，你需要重启使修改生效");
                })
                .ContinueWith(t =>
                {
#if DEBUG
                    if (t.Exception != null)
                    {
                        throw t.Exception;
                    }
#endif
                });
        }

        private async Task StartWithDefaultProgram(string fileToOpen)
        {
            using Process process = FileSystem.Instance.OpenOrExecuteFile(fileToOpen);
            process.WaitForExit();
            try
            {
                while (!process.HasExited)
                {
                    await Task.Delay(100);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
