using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.Util;
using NitroxModel.Logger;
using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Entities;

namespace NitroxServer.Communication.Packets.Processors
{
    public class PingRenamedPacketProcessor : AuthenticatedPacketProcessor<PingRenamed>
    {
        private readonly EntityManager entities;
        private readonly PlayerManager playerManager;

        public PingRenamedPacketProcessor(PlayerManager playerManager, EntityManager entities)
        {
            this.playerManager = playerManager;
            this.entities = entities;
        }

        public override void Process(PingRenamed packet, Player player)
        {
            Optional<Entity> beaconEntity = entities.GetEntityById(packet.Id);
            if (!beaconEntity.HasValue)
            {
                Log.Error($"服务器无法找到nitrox id为 '{packet.Id}' 的实体");
                return;
            }

            Log.Info($"接受到ping重命名: {packet} 来自玩家: {player}");
            playerManager.SendPacketToOtherPlayers(packet, player);

            // Persist label change on server for future players
            beaconEntity.Value.SerializedGameObject = packet.BeaconGameObjectSerialized;
        }
    }
}
