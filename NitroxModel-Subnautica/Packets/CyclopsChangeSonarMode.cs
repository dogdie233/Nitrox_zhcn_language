using System;
using NitroxModel.Packets;
using NitroxModel.DataStructures;

namespace NitroxModel_Subnautica.Packets
{
    [Serializable]
    public class CyclopsChangeSonarMode : Packet
    {
        public NitroxId Id { get; }
        public bool IsOn { get; }
        
        public CyclopsChangeSonarMode(NitroxId id, bool isOn)
        {
            Id = id;
            IsOn = isOn;
        }

        public override string ToString()
        {
            return $"[独眼巨人号声呐激活状态(CyclopsActivateSonar) - Id: {Id}, 启用?: {IsOn}]";
        }
    }
}
