using System;
using System.Collections.Generic;
using System.Linq;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.Util;
using NitroxModel.Logger;
using NitroxModel.Packets;
using NitroxServer.ConsoleCommands.Abstract;
using NitroxServer.Exceptions;

namespace NitroxServer.ConsoleCommands.Processor
{
    public class ConsoleCommandProcessor
    {
        private readonly Dictionary<string, Command> commands = new Dictionary<string, Command>();
        private readonly char[] splitChar = { ' ' };

        public ConsoleCommandProcessor(IEnumerable<Command> cmds)
        {
            foreach (Command cmd in cmds)
            {
                if (commands.ContainsKey(cmd.Name))
                {
                    throw new DuplicateRegistrationException($"命令 {cmd.Name} 被多次注册。");
                }

                commands[cmd.Name] = cmd;
                foreach (string alias in cmd.Aliases)
                {
                    if (commands.ContainsKey(alias))
                    {
                        throw new DuplicateRegistrationException($"命令 {alias} 被多次注册。");
                    }

                    commands[alias] = cmd;
                }
            }
        }

        public void ProcessCommand(string msg, Optional<Player> player, Perms perms)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }

            string[] parts = msg.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
            if (!commands.TryGetValue(parts[0], out Command cmd))
            {
                string errorMessage = "无法找到命令: " + parts[0];
                Log.Info(errorMessage);

                if (player.HasValue)
                {
                    player.Value.SendPacket(new ChatMessage(ChatMessage.SERVER_ID, errorMessage));
                }

                return;
            }

            if (perms >= cmd.RequiredPermLevel)
            {
                RunCommand(cmd, player, parts);
            }
            else
            {
                cmd.SendMessageToPlayer(player, "你没有权限执行此命令!");
            }
        }

        private void RunCommand(Command command, Optional<Player> player, string[] parts)
        {
            command.TryExecute(player, parts.Skip(1).ToArray());
        }
    }
}
