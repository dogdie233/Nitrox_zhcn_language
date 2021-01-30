using System;
using NitroxModel.Packets;
using NitroxModel.DataStructures;

namespace NitroxModel_Subnautica.Packets
{
    [Serializable]
    public class CyclopsChangeSilentRunning : Packet
    {
        public NitroxId Id { get; }
        public bool IsOn { get; }

        public CyclopsChangeSilentRunning(NitroxId id, bool isOn)
        {
            Id = id;
            IsOn = isOn;
        }

        public override string ToString()
        {
            return $"[独眼巨人号开始无声潜行(CyclopsBeginSilentRunning) - Id: {Id} , 启动?: {IsOn}]";
        }
    }
}
