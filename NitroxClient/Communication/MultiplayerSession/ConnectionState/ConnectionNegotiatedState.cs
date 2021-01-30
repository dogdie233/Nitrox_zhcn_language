using System;
using NitroxClient.Communication.Abstract;

namespace NitroxClient.Communication.MultiplayerSession.ConnectionState
{
    public abstract class ConnectionNegotiatedState : CommunicatingState
    {
        public override void NegotiateReservation(IMultiplayerSessionConnectionContext sessionConnectionContext)
        {
            throw new InvalidOperationException("无法通过当前状态下的会话连接。");
        }
    }
}
