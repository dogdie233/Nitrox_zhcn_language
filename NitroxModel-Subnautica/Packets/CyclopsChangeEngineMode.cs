using System;
using NitroxModel.Packets;
using NitroxModel.DataStructures;

namespace NitroxModel_Subnautica.Packets
{
    [Serializable]
    public class CyclopsChangeEngineMode : Packet
    {
        public NitroxId Id { get; }
        public CyclopsMotorMode.CyclopsMotorModes Mode { get; }

        public CyclopsChangeEngineMode(NitroxId id, CyclopsMotorMode.CyclopsMotorModes mode)
        {
            Id = id;
            Mode = mode;
        }

        public override string ToString()
        {
            return $"[独眼巨人号改变引擎模式(CyclopsChangeEngineMode) - Id: {Id}, Mode: {Mode}]";
        }
    }
}
