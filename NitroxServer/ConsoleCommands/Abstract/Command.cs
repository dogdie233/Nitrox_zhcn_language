using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NitroxModel.Core;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.Util;
using NitroxModel.Helper;
using NitroxModel.Logger;
using NitroxModel.Packets;
using NitroxServer.GameLogic;

namespace NitroxServer.ConsoleCommands.Abstract
{
    public abstract partial class Command
    {
        private int optional, required;

        public virtual IEnumerable<string> Aliases { get; } = Array.Empty<string>();

        public string Name { get; }
        public string Description { get; }
        public Perms RequiredPermLevel { get; }
        public bool AllowedArgOverflow { get; }
        public List<IParameter<object>> Parameters { get; }

        protected Command(string name, Perms perm, string description, bool allowedArgOveflow = false)
        {
            Validate.NotNull(name);

            Name = name;
            RequiredPermLevel = perm;
            Parameters = new List<IParameter<object>>();
            AllowedArgOverflow = allowedArgOveflow;
            Description = string.IsNullOrEmpty(description) ? "没有描述被提供" : description;
        }

        protected abstract void Execute(CallArgs args);

        public void TryExecute(Optional<Player> sender, string[] args)
        {
            if (args.Length < required)
            {
                SendMessage(sender, $"错误: 无效的参数\n用法: {ToHelpText(true)}");
                return;
            }

            if (!AllowedArgOverflow && args.Length > optional + required)
            {
                SendMessage(sender, $"错误: 参数过多\n用法: {ToHelpText(true)}");
                return;
            }

            try
            {
                Execute(new CallArgs(this, sender, args));
            }
            catch (ArgumentException ex)
            {
                SendMessage(sender, $"错误: {ex.Message}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "当执行命令时发生致命错误");
            }
        }

        public string ToHelpText(bool cropText = false)
        {
            StringBuilder cmd = new StringBuilder(Name);

            if (Aliases.Any())
            {
                cmd.AppendFormat("/{0}", string.Join("/", Aliases));
            }

            cmd.AppendFormat(" {0}", string.Join(" ", Parameters));
            int filled_space_number = 0;
            if (!cropText)
            {
                filled_space_number = 40 - Encoding.Default.GetBytes(cmd.ToString()).Length;
                cmd.Append(' ', filled_space_number);
            }
            
            return cropText ? $"{cmd}" : $"{cmd} - {Description}";
        }

        /// <summary>
        ///     Send a message to an existing player
        /// </summary>
        public void SendMessageToPlayer(Optional<Player> player, string message)
        {
            if (player.HasValue)
            {
                player.Value.SendPacket(new ChatMessage(ChatMessage.SERVER_ID, message));
            }
        }

        /// <summary>
        ///     Send a message to an existing player and logs it in the console
        /// </summary>
        public void SendMessage(Optional<Player> player, string message)
        {
            SendMessageToPlayer(player, message);
            Log.Info(message);
        }

        /// <summary>
        ///     Send a message to all connected players
        /// </summary>
        public void SendMessageToAllPlayers(string message)
        {
            PlayerManager playerManager = NitroxServiceLocator.LocateService<PlayerManager>();
            playerManager.SendPacketToAllPlayers(new ChatMessage(ChatMessage.SERVER_ID, message));
            Log.Info(message);
        }

        protected void AddParameter<T>(T param) where T : IParameter<object>
        {
            Validate.NotNull(param as object);
            Parameters.Add(param);

            if (param.IsRequired)
            {
                required++;
            }
            else
            {
                optional++;
            }
        }
    }
}
