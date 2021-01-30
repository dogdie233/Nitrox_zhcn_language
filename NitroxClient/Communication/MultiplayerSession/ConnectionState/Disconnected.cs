using System;
using NitroxClient.Communication.Abstract;
using NitroxClient.Communication.Exceptions;
using NitroxModel.Helper;
using NitroxModel.Packets;

namespace NitroxClient.Communication.MultiplayerSession.ConnectionState
{
    public class Disconnected : IMultiplayerSessionConnectionState
    {
        private object stateLock = new object();
        public MultiplayerSessionConnectionStage CurrentStage => MultiplayerSessionConnectionStage.DISCONNECTED;

        public void NegotiateReservation(IMultiplayerSessionConnectionContext sessionConnectionContext)
        {
            lock (stateLock)
            {
                ValidateState(sessionConnectionContext);

                IClient client = sessionConnectionContext.Client;
                string ipAddress = sessionConnectionContext.IpAddress;
                int port = sessionConnectionContext.ServerPort;
                StartClient(ipAddress, client,port);
                EstablishSessionPolicy(sessionConnectionContext, client);
            }
        }

        private void ValidateState(IMultiplayerSessionConnectionContext sessionConnectionContext)
        {
            ValidateClient(sessionConnectionContext);

            try
            {
                Validate.NotNull(sessionConnectionContext.IpAddress);
            }
            catch (ArgumentNullException ex)
            {
                throw new InvalidOperationException("内容竟然不包含IP地址。", ex);
            }
        }

        private static void ValidateClient(IMultiplayerSessionConnectionContext sessionConnectionContext)
        {
            try
            {
                Validate.NotNull(sessionConnectionContext.Client);
            }
            catch (ArgumentNullException ex)
            {
                throw new InvalidOperationException("客户端在尝试通过一个会话预留之前应该设置连接内容。", ex);
            }
        }

        private static void StartClient(string ipAddress, IClient client, int port)
        {
            if (!client.IsConnected)
            {
                client.Start(ipAddress,port);

                if (!client.IsConnected)
                {
                    throw new ClientConnectionFailedException("客户端在没有任何提供错误的情况下连接失败了");
                }
            }
        }

        private static void EstablishSessionPolicy(IMultiplayerSessionConnectionContext sessionConnectionContext, IClient client)
        {
            string policyRequestCorrelationId = Guid.NewGuid().ToString();

            MultiplayerSessionPolicyRequest requestPacket = new MultiplayerSessionPolicyRequest(policyRequestCorrelationId);
            client.Send(requestPacket);

            EstablishingSessionPolicy nextState = new EstablishingSessionPolicy(policyRequestCorrelationId);
            sessionConnectionContext.UpdateConnectionState(nextState);
        }

        public void JoinSession(IMultiplayerSessionConnectionContext sessionConnectionContext)
        {
            throw new InvalidOperationException("无法加入一个会话直到服务器通过一个预留信息。");
        }

        public void Disconnect(IMultiplayerSessionConnectionContext sessionConnectionContext)
        {
            throw new InvalidOperationException("未连接到多人服务器。");
        }
    }
}
