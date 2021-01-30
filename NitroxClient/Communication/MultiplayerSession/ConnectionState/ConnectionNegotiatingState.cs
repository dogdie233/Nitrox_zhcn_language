using System;
using NitroxClient.Communication.Abstract;

namespace NitroxClient.Communication.MultiplayerSession.ConnectionState
{
    public abstract class ConnectionNegotiatingState : CommunicatingState
    {
        public override void JoinSession(IMultiplayerSessionConnectionContext sessionConnectionContext)
        {
            throw new InvalidOperationException("无法加入一个会话直到服务器通过一个预留信息");
        }
    }
}
