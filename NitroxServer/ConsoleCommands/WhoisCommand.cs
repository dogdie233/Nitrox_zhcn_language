using System.Text;
using NitroxModel.DataStructures.GameLogic;
using NitroxServer.ConsoleCommands.Abstract;
using NitroxServer.ConsoleCommands.Abstract.Type;

namespace NitroxServer.ConsoleCommands
{
    internal class WhoisCommand : Command
    {
        public WhoisCommand() : base("whois", Perms.PLAYER, "显示玩家信息")
        {
            AddParameter(new TypePlayer("玩家名", true));
        }

        protected override void Execute(CallArgs args)
        {
            Player player = args.Get<Player>(0);

            StringBuilder info = new StringBuilder($"==== {player.Name} ====\n");
            info.AppendLine($"ID: {player.Id}");
            info.AppendLine($"权限组: {player.Permissions}");
            info.AppendLine($"位置: {player.Position.X} {player.Position.Y} {player.Position.Z} (狗带: 为了方便复制用作 /warp 我把逗号去了)");
            info.AppendLine($"氧气: {player.Stats.Oxygen}/{player.Stats.MaxOxygen}");
            info.AppendLine($"食物: {player.Stats.Food}");
            info.AppendLine($"水分: {player.Stats.Water}");
            info.AppendLine($"感染程度: {player.Stats.InfectionAmount}");

            SendMessage(args.Sender, info.ToString());
        }
    }
}
