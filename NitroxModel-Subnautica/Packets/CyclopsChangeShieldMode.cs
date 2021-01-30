using System;
using NitroxModel.Packets;
using NitroxModel.DataStructures;

namespace NitroxModel_Subnautica.Packets
{
    [Serializable]
    public class CyclopsChangeShieldMode : Packet
    {
        public NitroxId Id { get; }
        public bool IsOn { get; }

        public CyclopsChangeShieldMode(NitroxId id, bool isOn)
        {
            Id = id;
            IsOn = isOn;
        }

        public override string ToString()
        {
            return $"[独眼巨人号防护罩激活状态(CyclopsActivateShield) - Id: {Id}, 启用?: {IsOn}]";
        }
    }
}
