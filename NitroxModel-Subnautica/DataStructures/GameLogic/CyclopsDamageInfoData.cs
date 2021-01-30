﻿using System;
using NitroxModel.DataStructures;
using ProtoBufNet;
using UnityEngine;

namespace NitroxModel_Subnautica.DataStructures.GameLogic
{
    [Serializable]
    [ProtoContract]
    public class CyclopsDamageInfoData
    {
        [ProtoMember(1)]
        public NitroxId ReceiverId { get; set; }

        [ProtoMember(2)]
        public NitroxId DealerId { get; set; }

        [ProtoMember(3)]
        public float OriginalDamage { get; set; }

        [ProtoMember(4)]
        public float Damage { get; set; }

        [ProtoMember(5)]
        public Vector3 Position { get; set; }

        [ProtoMember(6)]
        public DamageType Type { get; set; }

        protected CyclopsDamageInfoData()
        {
            // Constructor for serialization. Has to be "protected" for json serialization.
        }

        public CyclopsDamageInfoData(NitroxId receiverId, NitroxId dealerId, float originalDamage, float damage, Vector3 position, DamageType type)
        {
            ReceiverId = receiverId;
            DealerId = dealerId;
            OriginalDamage = originalDamage;
            Damage = damage;
            Position = position;
            Type = type;
        }

        public override string ToString()
        {
            return $"[独眼巨人号损坏信息数据(CyclopsDamageInfoData) - 接收者Id: {ReceiverId} DealerId:{DealerId} 原损坏: {OriginalDamage} 损坏: {Damage} 位置: {Position} 类型: {Type}}}]";
        }
    }
}
