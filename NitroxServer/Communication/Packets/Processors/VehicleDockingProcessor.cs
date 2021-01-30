using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.Util;
using NitroxModel.Logger;
using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Vehicles;

namespace NitroxServer.Communication.Packets.Processors
{
    class VehicleDockingProcessor : AuthenticatedPacketProcessor<VehicleDocking>
    {
        private readonly PlayerManager playerManager;
        private readonly VehicleManager vehicleManager;

        public VehicleDockingProcessor(PlayerManager playerManager, VehicleManager vehicleManager)
        {
            this.playerManager = playerManager;
            this.vehicleManager = vehicleManager;
        }

        public override void Process(VehicleDocking packet, Player player)
        {
            Optional<VehicleModel> vehicle = vehicleManager.GetVehicleModel(packet.VehicleId);

            if (!vehicle.HasValue)
            {
                Log.Error($"载具停靠处理接收到不存在的载具id {packet.VehicleId}");
                return;
            }

            VehicleModel vehicleModel = vehicle.Value;
            vehicleModel.DockingBayId = Optional.Of(packet.DockId);

            playerManager.SendPacketToOtherPlayers(packet, player);
        }
    }
}
