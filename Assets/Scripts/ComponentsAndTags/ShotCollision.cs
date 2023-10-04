using Unity.Entities;
using Unity.Mathematics;

namespace TMG.Shooter
{
    public struct ShotCollision : IComponentData
    {
        public float3 Position;
        public Entity HitEntity;
    }
}