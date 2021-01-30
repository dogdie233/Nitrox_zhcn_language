using NitroxModel.Logger;
using NitroxModel.MultiplayerSession;
using NitroxModel.Packets;
using NitroxServer.Communication.NetworkingLayer;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;

namespace NitroxServer.Communication.Packets.Processors
{
    public class MultiplayerSessionReservationRequestProcessor : UnauthenticatedPacketProcessor<MultiplayerSessionReservationRequest>
    {
        private readonly PlayerManager playerManager;

        public MultiplayerSessionReservationRequestProcessor(PlayerManager playerManager)
        {
            this.playerManager = playerManager;
        }

        public override void Process(MultiplayerSessionReservationRequest packet, NitroxConnection connection)
        {
            Log.Info($"正在处理来自 {packet.AuthenticationContext.Username} 的请求");

            string correlationId = packet.CorrelationId;
            PlayerSettings playerSettings = packet.PlayerSettings;
            AuthenticationContext authenticationContext = packet.AuthenticationContext;
            MultiplayerSessionReservation reservation = playerManager.ReservePlayerContext(
                connection,
                playerSettings,
                authenticationContext,
                correlationId);

            Log.Info($"处理请求成功: 用户名: {packet.AuthenticationContext.Username} - {reservation}");

            connection.SendPacket(reservation);
        }
    }
}
