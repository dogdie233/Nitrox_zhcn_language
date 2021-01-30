using NitroxModel.DataStructures.GameLogic;
using NitroxServer.ConsoleCommands.Abstract;

namespace NitroxServer.ConsoleCommands
{
    internal class SaveCommand : Command
    {
        public SaveCommand() : base("save", Perms.ADMIN, "保存地图")
        {
        }

        protected override void Execute(CallArgs args)
        {
            Server.Instance.Save();
            SendMessageToPlayer(args.Sender, "保存完成");
        }
    }
}
