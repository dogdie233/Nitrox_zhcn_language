using System.Collections.Generic;
using NitroxModel.DataStructures.GameLogic;
using NitroxServer.ConsoleCommands.Abstract;
using NitroxServer.ConsoleCommands.Abstract.Type;

namespace NitroxServer.ConsoleCommands
{
    internal class WhisperCommand : Command
    {
        public override IEnumerable<string> Aliases { get; } = new[] { "m", "whisper", "w" };

        public WhisperCommand() : base("msg", Perms.PLAYER, "发送一条私信给某玩家", true)
        {
            AddParameter(new TypePlayer("玩家名", true));
            AddParameter(new TypeString("消息", true));
        }

        protected override void Execute(CallArgs args)
        {
            Player foundPlayer = args.Get<Player>(0);

            if (foundPlayer != null)
            {
                string message = $"[{args.SenderName} -> 你]: {args.GetTillEnd(1)}";
                SendMessageToPlayer(foundPlayer, message);
            }
            else
            {
                SendMessage(args.Sender, "发送失败，找不到玩家");
            }
        }
    }
}
