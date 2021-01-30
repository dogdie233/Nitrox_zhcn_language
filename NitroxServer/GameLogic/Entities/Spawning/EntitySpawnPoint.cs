using System.Collections.Generic;
using NitroxModel.DataStructures.GameLogic;

namespace NitroxServer.GameLogic.Entities.Spawning
{
    public class EntitySpawnPoint
    {
        public readonly List<EntitySpawnPoint> Children = new List<EntitySpawnPoint>();

        public NitroxVector3 LocalPosition;

        public NitroxQuaternion LocalRotation;
        public AbsoluteEntityCell AbsoluteEntityCell { get; }
        public NitroxVector3 Scale { get; }
        public string ClassId { get; }
        public string BiomeType { get; }
        public float Density { get; }
        public bool CanSpawnCreature { get; private set; }
        public List<string> AllowedTypes { get; }

        public EntitySpawnPoint Parent { get; set; }

        public EntitySpawnPoint(AbsoluteEntityCell absoluteEntityCell, NitroxVector3 localPosition, NitroxQuaternion localRotation, List<string> allowedTypes, float density, string biomeType)
        {
            AbsoluteEntityCell = absoluteEntityCell;
            LocalPosition = localPosition;
            LocalRotation = localRotation;
            BiomeType = biomeType;
            Density = density;
            AllowedTypes = allowedTypes;
        }

        public EntitySpawnPoint(AbsoluteEntityCell absoluteEntityCell, NitroxVector3 localPosition, NitroxQuaternion localRotation, NitroxVector3 scale, string classId)
        {
            AbsoluteEntityCell = absoluteEntityCell;
            ClassId = classId;
            Density = 1;
            LocalPosition = localPosition;
            Scale = scale;
            LocalRotation = localRotation;
        }

        public override string ToString() => $"[实体生成点 - {AbsoluteEntityCell}, 本地位置: {LocalPosition}, 本地旋转: {LocalRotation}, 缩放: {Scale}, 类Id: {ClassId}, 生物群系类型: {BiomeType}, 密度: {Density}, 允许产生生物: {CanSpawnCreature}]";
    }
}
