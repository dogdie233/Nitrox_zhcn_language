using System;
using NitroxModel.Packets;
using NitroxModel_Subnautica.DataStructures.GameLogic.Creatures.Actions;
using NitroxModel.DataStructures;

namespace NitroxModel_Subnautica.Packets
{
    [Serializable]
    public class CreatureActionChanged : Packet
    {
        public NitroxId Id { get; }
        public SerializableCreatureAction NewAction { get; }

        public CreatureActionChanged(NitroxId id, SerializableCreatureAction newAction)
        {
            Id = id;
            NewAction = newAction;
        }

        public override string ToString()
        {
            return $"[生物行为改变(CreatureActionChanged) - Id: {Id}, 新动作: {NewAction}]";
        }
    }
}
