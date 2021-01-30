using System;
using NitroxClient.Communication.Abstract;
using NitroxModel.Packets;

namespace NitroxClient.Communication.MultiplayerSession.ConnectionState
{
    public class SessionReserved : ConnectionNegotiatedState
    {
        public override MultiplayerSessionConnectionStage CurrentStage => MultiplayerSessionConnectionStage.SESSION_RESERVED;

        public override void JoinSession(IMultiplayerSessionConnectionContext sessionConnectionContext)
        {
            try
            {
                ValidateState(sessionConnectionContext);
                EnterMultiplayerSession(sessionConnectionContext);
                ChangeState(sessionConnectionContext);
            }
            catch (Exception)
            {
                Disconnect(sessionConnectionContext);
                throw;
            }
        }

        private static void ValidateState(IMultiplayerSessionConnectionContext sessionConnectionContext)
        {
            if (!sessionConnectionContext.Client.IsConnected)
            {
                throw new InvalidOperationException("客户端未连接");
            }
        }

        private void EnterMultiplayerSession(IMultiplayerSessionConnectionContext sessionConnectionContext)
        {
            IClient client = sessionConnectionContext.Client;
            MultiplayerSessionReservation reservation = sessionConnectionContext.Reservation;
            string correlationId = reservation.CorrelationId;
            string reservationKey = reservation.ReservationKey;

            PlayerJoiningMultiplayerSession packet = new PlayerJoiningMultiplayerSession(correlationId, reservationKey);
            client.Send(packet);
        }

        private void ChangeState(IMultiplayerSessionConnectionContext sessionConnectionContext)
        {
            SessionJoined nextState = new SessionJoined();
            sessionConnectionContext.UpdateConnectionState(nextState);
        }
    }
}
