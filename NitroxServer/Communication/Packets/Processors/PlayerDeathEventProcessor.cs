using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Packets;
using NitroxModel.Server;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.Serialization;

namespace NitroxServer.Communication.Packets.Processors
{
    class PlayerDeathEventProcessor : AuthenticatedPacketProcessor<PlayerDeathEvent>
    {
        private readonly PlayerManager playerManager;
        private readonly ServerConfig serverConfig;

        public PlayerDeathEventProcessor(PlayerManager playerManager, ServerConfig config)
        {
            this.playerManager = playerManager;
            this.serverConfig = config;
        }

        public override void Process(PlayerDeathEvent packet, Player player)
        {
            if(serverConfig.IsHardcore)
            {
                player.IsPermaDeath = true;
                PlayerKicked playerKicked = new PlayerKicked("极限模式下永久死亡");
                player.SendPacket(playerKicked);
            }

            player.LastStoredPosition = packet.DeathPosition;

            if (player.Permissions > Perms.MODERATOR)
            {
                player.SendPacket(new ChatMessage(ChatMessage.SERVER_ID, "你可以使用/back回到死亡地点"));
            }

            playerManager.SendPacketToOtherPlayers(packet, player);
        }
    }
}
