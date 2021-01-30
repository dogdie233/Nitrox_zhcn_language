using NitroxModel.DataStructures.GameLogic;
using NitroxServer.ConsoleCommands.Abstract;
using NitroxServer.ConsoleCommands.Abstract.Type;
using NitroxServer.GameLogic;

namespace NitroxServer.ConsoleCommands
{
    internal class TimeCommand : Command
    {
        private readonly TimeKeeper timeKeeper;

        public TimeCommand(TimeKeeper timeKeeper) : base("time", Perms.ADMIN, "改变地图时间")
        {
            this.timeKeeper = timeKeeper;
            AddParameter(new TypeString("(day/night) / (早上/晚上)", false));
        }

        protected override void Execute(CallArgs args)
        {
            string time = args.Get(0);

            switch (time?.ToLower())
            {
                case "早上":
                case "day":
                    timeKeeper.SetDay();
                    SendMessageToAllPlayers("时间设置到了早上");
                    break;

                case "晚上":
                case "night":
                    timeKeeper.SetNight();
                    SendMessageToAllPlayers("时间设置到了晚上");
                    break;

                default:
                    timeKeeper.SkipTime();
                    SendMessageToAllPlayers("时间被跳过了");
                    break;
            }
        }
    }
}
