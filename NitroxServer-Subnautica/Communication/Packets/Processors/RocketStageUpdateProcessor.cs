using NitroxModel.DataStructures.Util;
using NitroxModel.Logger;
using NitroxModel_Subnautica.DataStructures.GameLogic;
using NitroxModel_Subnautica.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Vehicles;

namespace NitroxServer_Subnautica.Communication.Packets.Processors
{
    public class RocketStageUpdateProcessor : AuthenticatedPacketProcessor<RocketStageUpdate>
    {
        private readonly VehicleManager vehicleManager;
        private readonly PlayerManager playerManager;

        public RocketStageUpdateProcessor(VehicleManager vehicleManager, PlayerManager playerManager)
        {
            this.vehicleManager = vehicleManager;
            this.playerManager = playerManager;
        }

        public override void Process(RocketStageUpdate packet, NitroxServer.Player player)
        {
            Optional<NeptuneRocketModel> opRocket = vehicleManager.GetVehicleModel<NeptuneRocketModel>(packet.Id);

            if (opRocket.HasValue)
            {
                opRocket.Value.CurrentStage = packet.NewStage;
            }
            else
            {
                Log.Error($"{nameof(RocketStageUpdateProcessor)}: 无法找到火箭Id为 {packet.Id} 的服务器模型");
            }

            playerManager.SendPacketToOtherPlayers(packet, player);
        }
    }
}
